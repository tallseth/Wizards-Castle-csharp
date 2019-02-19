using System;
using System.Collections.Generic;
using System.Linq;
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
            _tools.UI.ClearActionLog();
            _tools.UI.DisplayMessage("You use the runestaff to teleport to a location of your choice.");

            var x = GetLocationPart("X", _config.FloorWidth);
            var y = GetLocationPart("Y", _config.FloorHeight);
            var z = GetLocationPart("Z", _config.Floors);
            var target = new Location(x, y, z);

            _tools.UI.DisplayMessage($"You will teleport to {target}");

            return target;
        }

        private byte GetLocationPart(string partName, byte max)
        {
            _tools.UI.DisplayMessage($"{partName}: ");
            return _tools.UI.PromptUserChoice(LocationPartOptions().Take(max), false).GetData<byte>();
        }

        private IEnumerable<UserOption> LocationPartOptions()
        {
            yield return new UserOption("0", ConsoleKey.D0, (byte) 0);
            yield return new UserOption("1", ConsoleKey.D1, (byte) 1);
            yield return new UserOption("2", ConsoleKey.D2, (byte) 2);
            yield return new UserOption("3", ConsoleKey.D3, (byte) 3);
            yield return new UserOption("4", ConsoleKey.D4, (byte) 4);
            yield return new UserOption("5", ConsoleKey.D5, (byte) 5);
            yield return new UserOption("6", ConsoleKey.D6, (byte) 6);
            yield return new UserOption("7", ConsoleKey.D7, (byte) 7);
            yield return new UserOption("8", ConsoleKey.D8, (byte) 8);
            yield return new UserOption("9", ConsoleKey.D9, (byte) 9);
        }
    }
}