using MelonLoader;

[assembly: MelonInfo(typeof(FellAndSell.Mod.Plugin), "Fell & Sell Korean Patch", "0.1.0", "myso-kr")]
[assembly: MelonGame("Art Games Studio SA", "Fell & Sell")]

namespace FellAndSell.Mod;

public sealed class Plugin : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("Scaffold initialized. Translation hooks and Korean font support are not implemented yet.");
    }
}
