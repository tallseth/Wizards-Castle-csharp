using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.Situations
{
    internal class TeleportSituation : ISituation
    {
        public ISituation PlayThrough(GameData data, GameTools tools)
        {
            var location = tools.TeleportUI.GetTeleportationTarget();
            return tools.SituationBuilder.EnterRoom(location);
        }
    }
}