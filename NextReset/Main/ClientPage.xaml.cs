using Main.Screen.Tutorial;
using Main.Screens;
using Network;
using Settings;
using Settings.Network;
using Settings.Network.Handlers.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private NetworkClient client;
        private Thread _NetworkReader = null;
        private Dictionary<int, CHandler> _Handlers = null;
        private bool StopReading = true;
        public ClientPage()
        {
            InitializeComponent();
            Init();
            AppSettings.PageSettings(this);
        }
        private void Init()
        {
            AddButton("Start", StartPage);
            AddButton("Tutorial", TutorialPage);
            AddButton("Settings", SettingsPage);
            AddButton("Help", HelpPage);
            AddButton("Exit", Exit);
            AddButton("Check for updates", CheckForUpdates);

            Hyperlink link = new Hyperlink();
            link.Inlines.Add("about");
            link.Click += AboutPage;
            _About_tx.Inlines.Add("The game is made by Roel Suntjens. To learn more ");
            _About_tx.Inlines.Add(link);
            _About_tx.Inlines.Add(" me, see my about page");
        }

        private void AddButton(string text, Action<object, RoutedEventArgs> SelectionPage)
        {
            Button button = new Button();
            button.Content = text;
            button.FontSize = 32;
            button.FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS");
            button.Background = new SolidColorBrush(Colors.Black);
            button.Foreground = new SolidColorBrush(Colors.Brown);
            button.Click += new RoutedEventHandler(SelectionPage);
            _Button_Stackpanel.Children.Add(button);
        }

        #region Buttons
        private void StartPage(object sender, RoutedEventArgs e)
        {
            StartPage page = new StartPage();
            this.NavigationService.Navigate(page);
        }
        private void TutorialPage(object sender, RoutedEventArgs e)
        {
            TutorialPage page = new TutorialPage();
            this.NavigationService.Navigate(page);
        }
        private void SettingsPage(object sender, RoutedEventArgs e)
        {
            SettingsPage page = new SettingsPage();
            this.NavigationService.Navigate(page);
        }
        private void HelpPage(object sender, RoutedEventArgs e)
        { } // TODO: Implementatie
        private void AboutPage(object sender, RoutedEventArgs e)
        {
            AboutPage page = new AboutPage();
            this.NavigationService.Navigate(page); // TODO: Implementatie, Create Page
        }
        private void Unknown(object sender, RoutedEventArgs e)
        { Debug.WriteLine("Unknown"); }
        private void Exit(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            SaveAppData();
            Application.Current.Shutdown(0);
        }
        private void CheckForUpdates(object sender, RoutedEventArgs e)
        { UpdateCheck(); }
        #endregion

        #region Functions
        private void UpdateCheck()
        {
            if (client == null)
                client = SetupConnection("Roel"); // TODO: Inlog screen
            if (UpdateAvailableCheck(client, GameController.LoadVersion()))
            {
                MessageBoxResult result = MessageBox.Show("You want to get the update?", "Up - To - Date", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Debug.WriteLine("Get next update");
                    NetworkPackage sendPackage = new NetworkPackage();
                    sendPackage.ExecuteCode = (int)NetworkSettings.ExecuteCode.update_request;
                    client.Send(sendPackage);
                    Debug.WriteLine("Not waiting anymore");
                    WaitForResponseUpdate(client);
                }
            }
            else
            { MessageBox.Show("No Updates Available"); }
        }
        #region Setup connection
        public NetworkClient SetupConnection(string name)
        {
            NetworkClient client = new NetworkClient();
            client.Connect(NetworkSettings.Address.IP_Address, NetworkSettings.Address.Server_Port);
            client.SendMessage(name);
            return client;
        }
        #endregion
        #region Update Available Check
        /// <summary>
        /// '0' = same version - '1' = newer version - '-1' = older version.
        /// </summary>
        public bool UpdateAvailableCheck(NetworkClient client, string version)
        {
            bool available = false;
            NetworkPackage sendPackage = new NetworkPackage();
            sendPackage.ExecuteCode = (int)NetworkSettings.ExecuteCode.update_available_check;
            sendPackage.Message = version;
            client.Send(sendPackage);
            NetworkPackage returnPackage = client.Receive();
            if (returnPackage.ExecuteCode == (int)NetworkSettings.ExecuteCode.update_available_response)
            {
                Debug.WriteLine("Update Available Check: " + returnPackage.Message);
                if (returnPackage.Value == 0)
                { available = false; }
                else if (returnPackage.Value == -1) // meaning: you got lower version
                { available = true; }
                else
                {
                    MessageBox.Show("Newer version then on the server");
                    available = false;
                }
            }
            else { Debug.WriteLine("Update Available Check, Wrong package send [" + returnPackage.ExecuteCode + "]"); return false; }
            return available;
        }
        #endregion
        #region Wait for response update
        private void WaitForResponseUpdate(NetworkClient client)
        {
            Debug.WriteLine("Waiting for return package");
            NetworkPackage returnPackage = client.Receive();
            if (returnPackage.ExecuteCode == (int)NetworkSettings.ExecuteCode.update_response)
            {
                Debug.WriteLine("Received levels from server");
                HandleUpdate(returnPackage); // TODO: Implementation (Ended)
            }
            else { Debug.WriteLine("Server send wrong package [" + returnPackage.ExecuteCode + "]"); }
        }
        #endregion
        #region Handle Update
        public void HandleUpdate(NetworkPackage package)
        {
            // TODO: Implementation
            MessageBox.Show("Waiting for finishing update");
        }
        #endregion
        private void SaveSettings() { }
        private void SaveAppData() { }
        #endregion
    }
}
