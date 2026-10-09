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
        PickupReady = true;
        // A transient predicate, never a write to the game's persistent block flag.
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.FirstPersonController"), "get_IsPlayerBlocked"),
            postfix: new HarmonyMethod(typeof(Patches), nameof(PanelBlocked)));
        harmony.Patch(Reflect.Method(Anchors.Type("Il2Cpp.PlayerInputHandler"), "IsPlayerActionBlocked"),
            postfix: new HarmonyMethod(typeof(Patches), nameof(PanelBlocked)));
        MelonLoader.MelonLogger.Msg("automation: native pickup and movement hooks installed; achievements unchanged");
    }
    private static bool BeforeMove(object __instance)
    {
        if (Panel.Widget.Visible) return false;
        if (!Equals(__instance, State.Player) || !Guide.Move.Active) return true;
        if (!Log.Guard("movement", () => Exec.BeginMovement(Guide.Move.Direction)))
        {
            Guide.Move.Stop();
            Log.Guard("movement-restore", Exec.EndMovement);
        }
        return true;
    }
    private static void PanelBlocked(ref bool __result) { if (Panel.Widget.Visible) __result = true; }
    private static void AfterMove() => Log.Guard("movement-restore", Exec.EndMovement);
    private static Exception? FinalMove(Exception? __exception) { AfterMove(); return __exception; }
    private static bool BeforePickup(object __0)
    {
        if (!Config.Ready || !Config.Pickup) return true;
        if (!Supervisor.Current.CanPickup || !Log.Available("pickup")) return false;
        var allowed = false;
        Log.Guard("pickup-sight", () => allowed = Sight.Clear(__0));
        return allowed;
    }
}
