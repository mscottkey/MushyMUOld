using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MushyExtensionMethods
{
    class GameOrchestrator
    {
        TabControl _tabs;
        List<Game> _games;
        public GameOrchestrator(TabControl tabs)
        {
            // He gets the tabs from mainwindow
            _tabs = tabs;
            _games = new List<Game>();
        }

        public void AddGame()
        {
            // Games are initially not connected
            // Give the game connector view a reference to ourself for when it's ready to connect.
            var newGame = new NewGameView(this);
            // A new blank game window always goes at the end
            _tabs.Items.Add(newGame);
        }

        // Note - using the parent class UserControl lets you pass either type of view
        public void CloseGame(TabItem view)
        {
            _tabs.Items.Remove(view);
            var game = FindGameForView(view);
            if (game != null)
            {
                game.Model.Disconnect();
            }
        }

        public void ConnectGame(NewGameView view)
        {
            int index = FindTabIndex(view);

            // Remove the old connector view
            _tabs.Items.RemoveAt(index);

            // Make up a new game and add it to the games list
            var game = GameFactory.NewGame(connectInfo);
            _games.Add(game);

            // Add the game view in the same position on the tab control
            _tabs.Items.Insert(index, game.View);
        }

        private int FindTabIndex(TabItem view)
        {
            for (int i = i; i < _tabs.Items.Count - 1; i++)
            {
                if (_tabs.Items[i] == view)
                {
                    return i;
                }
            }
            return 0;
        }
        private Game FindGameForView(TabItem view)
        {
            foreach (var g in _games)
            {
                if (g.View == view)
                {
                    return g;
                }
            }
            return null;
        }
    }
}
