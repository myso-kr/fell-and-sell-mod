using HarmonyLib;

namespace FellAndSell.Mod.Autoplay;

internal static class Patches
{
    internal static bool MovementReady, PickupReady;
    internal static void Apply(HarmonyLib.Harmony harmony)
    {
        var movement = Reflect.Method(Anchors.Type("Il2Cpp.FirstPersonController"), "HandleMovement");
        harmony.Patch(movement, prefix: new HarmonyMethod(typeof(Patches), nameof(BeforeMove)),
            postfix: new HarmonyMethod(typeof(Patches), nameof(AfterMove)),
            finalizer: new HarmonyMethod(typeof(Patches), nameof(FinalMove)));
        MovementReady = true;
        var collider = Anchors.Type("UnityEngine.Collider");
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.PlayerAutoPickup"), "TryPickup", collider),
            prefix: new HarmonyMethod(typeof(Patches), nameof(BeforePickup)));
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.PlayerAutoPickup"), "PerformProximityScan"),
            prefix: new HarmonyMethod(typeof(Patches), nameof(BeforeScan)),
            finalizer: new HarmonyMethod(typeof(Patches), nameof(FinalScan)));
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.OptionsManager"), "get_IsAutoPickupEnabled"),
            postfix: new HarmonyMethod(typeof(Patches), nameof(PickupOption)));
        PickupReady = true;
        foreach (var name in new[] { "BeginGeneration", "ClearDungeon" })
            harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.CustomDungeonGenerator"), name),
                prefix: new HarmonyMethod(typeof(Patches), nameof(BeforeGeneration)));
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.DungeonMinimapManager"), "InitializeMap",
            Anchors.Type("Il2CppFellSell.Minimap.DungeonMapLayoutData")),
            postfix: new HarmonyMethod(typeof(Patches), nameof(AfterMap)));
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.DungeonMinimapManager"), "RedrawFullMapBuffer"),
            postfix: new HarmonyMethod(typeof(Patches), nameof(AfterMapBuffer)));
        // Keep native input predicates and key-release processing truthful. Only
        // suppress individual actions while the owned panel is visible.
        foreach (var entry in new[] {
            ("FirstPersonController", new[] { "HandleRotation", "OnInteract", "OnInteractHold", "OnDashPressed", "HandleJumping", "HandleCrouch" }),
            ("PlayerCombat", new[] { "HandlePrimaryAttack", "HandleSecondaryAttack" }),
            ("PlayerInteraction", new[] { "HandleInteraction", "HandleSecondaryInteraction" }),
            ("PlayerConsumableController", new[] { "StartConsume" }) })
        foreach (var name in entry.Item2)
        {
            var methods = Anchors.Type("Il2Cpp." + entry.Item1).GetMethods().Where(method => method.Name == name).ToArray();
            if (methods.Length == 0) throw new MissingMethodException(entry.Item1, name);
            foreach (var method in methods) harmony.Patch(method, prefix: new HarmonyMethod(typeof(Patches), nameof(PanelAction)));
        }
        MelonLoader.MelonLogger.Msg("automation: native pickup and movement hooks installed; achievements unchanged");
    }
    private static bool BeforeMove(object __instance)
    {
        if (!Equals(__instance, State.Player)) return true;
        if (!Panel.Widget.Visible && !Guide.Move.Active) return true;
        if (!Log.Guard("movement", () => Exec.BeginMovement(Panel.Widget.Visible ? default : Guide.Move.Direction)))
        {
            Guide.Move.Stop();
            Log.Guard("movement-restore", Exec.EndMovement);
        }
        return true;
    }
    private static bool PanelAction() => !Panel.Widget.Visible;
    private static void BeforeGeneration() => Lifecycle.Invalidate("dungeon generation");
    private static void AfterMap() => Lifecycle.Invalidate("native map initialized");
    private static void AfterMapBuffer() => Map.Read.RefreshTexture();
    private static void PickupOption(ref bool __result)
    {
        // Honour the mod toggle inside its own native scan without changing or
        // saving the player's global option. Native eligibility still applies.
        __result = PickupScope.Option(__result, Config.Ready && Config.Pickup, Supervisor.Current.CanPickup);
    }
    private static void AfterMove()
    {
        if (!Log.Guard("movement-restore", Exec.EndMovement)) { Guide.Move.Stop(); MovementReady = false; }
    }
    private static Exception? FinalMove(Exception? __exception) { AfterMove(); return __exception; }
    private static bool BeforePickup(object __0)
    {
        if (!Config.Ready || !Config.Pickup) return true;
        if (!Supervisor.Current.CanPickup || !Log.Available("pickup")) return false;
        var allowed = false;
        Log.Guard("pickup-sight", () => allowed = Sight.Clear(__0));
        Pickup.Record(allowed);
        return allowed;
    }
    private static bool BeforeScan(object __instance, out bool __state)
    {
        __state = false;
        if (!Config.Ready || !Config.Pickup) return true;
        var allowed = false;
        Log.Guard("pickup", () => allowed = Pickup.BeforeScan(__instance));
        if (allowed) { PickupScope.Enter(); __state = true; }
        return allowed;
    }
    private static Exception? FinalScan(bool __state, Exception? __exception)
    {
        if (__state) PickupScope.Exit();
        return __exception;
    }
}
