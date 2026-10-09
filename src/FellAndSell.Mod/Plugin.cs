using MelonLoader;

[assembly: MelonInfo(typeof(FellAndSell.Mod.Plugin), "Fell & Sell Korean Patch", "0.2.0", "myso-kr")]
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
    }

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
    {
        I18n.FontFallback.Register();
    }

    private long _nextFontCheck;

    public override void OnUpdate()
    {
        // Addressables may load fonts after the scene callback.
        if (Environment.TickCount64 < _nextFontCheck) return;
        _nextFontCheck = Environment.TickCount64 + 3000;
        I18n.FontFallback.Register();
    }
}
