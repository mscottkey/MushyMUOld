using MushyExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Game> _games;
        private List<TabItem> _tabItems;
        private TabItem _tabAdd;    

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                _games = new List<Game>();
   
                // initialize tabItem array
                _tabItems = new List<TabItem>();

                // add a tabItem with + in header 
                TabItem tabAdd = new TabItem();
                tabAdd.Header = "+";

                _tabItems.Add(tabAdd);

                // add first tab
                this.AddTabItem("New Game");

                // bind tab control
                MushyTabs.DataContext = _tabItems;

                MushyTabs.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }  
    

        private TabItem AddTabItem(string header)
        {
            int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("{0}. " + header, count);
            tab.Name = string.Format("game{0}", count);
            tab.HeaderTemplate = MushyTabs.FindResource("TabHeader") as DataTemplate;
 
            // add new game view to the tab item.
            var newGame = new GameOrchestrator();
            tab.Content = newGame;
            
            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count - 1, tab);
            return tab; 
        } 

        private void AddressBookClick()
        {
            var connectionInfo = new ConnectionInfo
            {
                Host = "blah blah",
                Port = 123,
                Name = "Sweetwater"
            };
            var game = GameFactory.NewGame(connectionInfo);  // pass selected address book connection info
            _games.Add(game);
            MushyTabs.Items.Add(game.View);
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {

            var d = new MushyQuickConnect();
            d.ShowDialog();

            if (d.DialogResult == true)
            {
                string gameName = d.tbGameName.Text.ToString();
                string address = d.tbServerAddress.Text.ToString();
                string portString = d.tbPort.Text.ToString();
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = MushyTabs.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to close your connection to'{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = MushyTabs.SelectedItem as TabItem;

                    // clear tab control binding
                    MushyTabs.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    MushyTabs.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    MushyTabs.SelectedItem = selectedTab;
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //MushyTabs.SelectedItem._output.Find();
        }

        private void MushyTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = MushyTabs.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Header.Equals("+"))
                {
                    // clear tab control binding
                    MushyTabs.DataContext = null;

                    // add new tab
                    TabItem newTab = this.AddTabItem("New Game");

                    // bind tab control
                    MushyTabs.DataContext = _tabItems;

                    // select newly added tab item
                    MushyTabs.SelectedItem = newTab;
                }
                else
                {
                    // your code here...
                }
            }
        }

        //public static getController
        //{
        //    foreach (var g in _games) 
        //        { if (g.View == MushyTabs.SelectedItem) 
        //                {return g.Controller }
        //}

        
    }
}
