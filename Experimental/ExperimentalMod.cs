using MelonLoader;

namespace Bnfour.MoeJigsawMods.Experimental;

public class ExperimentalMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg("hello 32bit legacy world");
    }
}
