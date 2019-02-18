using WizardsCastle.Logic.Data;

namespace WizardsCastle.Logic.UI
{
    internal interface ITeleportUI
    {
        Location GetTeleportationTarget();
    }

    internal class TeleportUI : ITeleportUI
    {
        private readonly GameConfig _config;
        private readonly GameTools _tools;

        public TeleportUI(GameConfig config, GameTools tools)
        {
            _config = config;
            _tools = tools;
        }

        public Location GetTeleportationTarget()
        {
            _tools.UI.DisplayMessage("Imagine teleport options...");

            var target = new Location(0, 0, 0);
            _tools.UI.DisplayMessage($"Teleporting to {target}");
            return target;
        }
    }
}