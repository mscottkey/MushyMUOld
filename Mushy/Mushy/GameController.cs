using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MushyExtensionMethods
{
    public class GameController
    {
        GameView _view;
        GameModel _model;
        ConnectionInfo _connectInfo;

        public GameController(ConnectionInfo connectInfo, GameView view, GameModel model)
        {
            _view = view;
            _model = model;
            _connectInfo = connectInfo;
        }

        public void Connect()
        {
            _view.UpdateText("Connecting....");
            _model.Connect();
        }

        public void HandleInput(string input)
        {
            _model.Send(input);
        }

        public void Disconnect()
        {
            _view.UpdateText("Bye!");
            _model.Disconnect();
        }

        public void HandleDataReceived(string lineOfData)
        {
            _view.UpdateText(lineOfData);
            System.Diagnostics.Debug.WriteLine(lineOfData);
        }
    }
}
