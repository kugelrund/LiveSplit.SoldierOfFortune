using System;
using System.Reflection;
using LiveSplit.ComponentAutosplitter;
using LiveSplit.Model;
using LiveSplit.UI.Components;

namespace LiveSplit.SoldierOfFortune
{
    public class Factory : IComponentFactory
    {
        private SoldierOfFortuneGame game = new SoldierOfFortuneGame();
        public string ComponentName => game.Name + " Auto Splitter";
        public string Description => "Automates splitting for " + game.Name + " and allows to remove loadtimes.";
        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/kugelrund/LiveSplit.SoldierOfFortune/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => UpdateURL + "Components/update.LiveSplit.SoldierOfFortune.xml";

        public IComponent Create(LiveSplitState state)
        {
            return new Component(game, state);
        }
    }
}
