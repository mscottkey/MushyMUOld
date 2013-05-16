using System;
using MushyExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MushyExtensionMethods
{
    /// <summary>
    /// Interaction logic for NewGameView.xaml
    /// </summary>
    public partial class NewGameView : UserControl
    {
        public NewGameView(GameOrchestrator newGameView)
        {
            InitializeComponent();
        }


        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            
                string gameName = tbGameName.Text.ToString();
                string address = tbGameAddress.Text.ToString();
                string portString = tbGamePort.Text.ToString();
                bool successfulConnection;
                int port;
                if (!int.TryParse(portString, out port))
                    port = 23;

                successfulConnection = true;
                var connectionInfo = new ConnectionInfo
                {
                    Host = address,
                    Port = port,
                    Name = gameName
                };
                var game = GameFactory.NewGame(connectionInfo);  // pass quick connect info
                _games.Add(game);
                MushyTabs.Items.Add(game.View);
                game.View.Header = connectionInfo.Name;
                game.View.IsSelected = true;
             
                game.Controller.Connect();

        }
    }
}
