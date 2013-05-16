using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushyExtensionMethods
{
    class Game
    {

        public GameView View { get; set; }
        public GameController Controller { get; set; }
        public GameModel Model { get; set; }
    }

    class GameFactory
    {
        public static Game NewGame(ConnectionInfo connectInfo)
        {
            var view = new GameView();
            var model = new GameModel(connectInfo);
            var controller = new GameController(connectInfo, view, model);

            view.Controller = controller;
            model.Controller = controller;

            return new Game { View = view, Model = model, Controller = controller };
        }
    }
}
