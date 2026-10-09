using MelonLoader;

[assembly: MelonInfo(typeof(FellAndSell.Mod.Plugin), "Fell & Sell Korean Patch", "0.4.0", "myso-kr")]
[assembly: MelonGame("Art Games Studio SA", "Fell & Sell")]

namespace FellAndSell.Mod;

public sealed class Plugin : MelonMod
{
    public override void OnInitializeMelon()
    {
        try
        {
            I18n.Catalog.Load();
            I18n.Patches.Apply(HarmonyInstance);
        }
        catch (Exception error)
        {
            LoggerInstance.Error($"Translation initialization failed: {error}");
        }
        Log.Guard("config", Config.Load);
        Log.Guard("panel-text", Panel.Text.Load);
        Log.Guard("hooks", () => Autoplay.Patches.Apply(HarmonyInstance));
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        I18n.FontFallback.Register();
    }

    private long _nextFontCheck;

    public override void OnUpdate()
    {
        Autoplay.Supervisor.Tick();
        // Addressables may load fonts after the scene callback.
        if (Environment.TickCount64 < _nextFontCheck) return;
        _nextFontCheck = Environment.TickCount64 + 3000;
        I18n.FontFallback.Register();
    }

    public override void OnGUI() => Autoplay.Supervisor.Draw();
    public override void OnSceneWasUnloaded(int buildIndex, string sceneName) => Autoplay.Supervisor.Reset();
    public override void OnDeinitializeMelon() => Autoplay.Supervisor.Reset();
}
