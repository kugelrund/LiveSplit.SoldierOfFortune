
using System;

namespace LiveSplit.SoldierOfFortune
{
    using ComponentAutosplitter;

    class SoldierOfFortuneGame : Game
    {
        private static readonly Type[] eventTypes = new Type[] { typeof(LoadedMapEvent) };
        public override Type[] EventTypes => eventTypes;

        public override string Name => "Soldier of Fortune";
        public override string[] ProcessNames => new string[] {"SoF"};
        public override bool GameTimeExists => false;
        public override bool LoadRemovalExists => true;

        public SoldierOfFortuneGame() : base(new CustomSettingBool[] {})
        {
        }
    }

    class LoadedMapEvent : MapEvent
    {
        public override string Description => "A certain map was loaded.";

        public LoadedMapEvent() : base()
        {
        }

        public LoadedMapEvent(string map) : base(map)
        {
        }

        public override bool HasOccured(GameInfo info)
        {
            return (info.PreviousGameState != SoldierOfFortuneState.InGame) && info.InGame && (info.CurrentMap == map);
        }

        public override string ToString()
        {
            return "Map '" + map + "' was loaded";
        }
    }

    public enum SoldierOfFortuneState
    {
        MainMenu = 1, InGame = 8
    }
}

namespace LiveSplit.ComponentAutosplitter
{
    using System.Text;
    using ComponentUtil;
    using SoldierOfFortune;

    partial class GameInfo
    {
        // 1 - main menu
        // 8 - in game
        private Int32 gameStateAddress = 0x1C1F00;
        // current map
        private Int32 mapAddress = 0x1E94CC;

        public SoldierOfFortuneState PreviousGameState { get; private set; }
        public SoldierOfFortuneState CurrentGameState { get; private set; }
        public string PreviousMap { get; private set; }
        public string CurrentMap { get; private set; }
        public bool MapChanged { get; private set; }

        partial void UpdateInfo()
        {
            if (gameProcess.ReadValue(baseAddress + gameStateAddress, out int currGameState))
            {
                PreviousGameState = CurrentGameState;
                CurrentGameState = (SoldierOfFortuneState)currGameState;
            }

            if (PreviousGameState != CurrentGameState)
            {
                UpdateMap();
            }
            else
            {
                MapChanged = false;
            }

            InGame = (CurrentGameState == SoldierOfFortuneState.InGame);
        }

        private void UpdateMap()
        {
            StringBuilder map_string_builder = new StringBuilder(32);
            if (gameProcess.ReadString(baseAddress + mapAddress, map_string_builder))
            {
                string new_map = map_string_builder.ToString();
                if (new_map != CurrentMap)
                {
                    PreviousMap = CurrentMap;
                    CurrentMap = new_map;
                    MapChanged = true;
                }
            }
        }
    }
}
