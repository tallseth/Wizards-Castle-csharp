using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal interface ISituation
    {
        ISituation PlayThrough(GameData data, GameTools tools);
    }
}