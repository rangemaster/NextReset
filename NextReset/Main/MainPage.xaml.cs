using Main.Screen.Tutorial;
using Main.Screens;
using Network;
using Settings.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private NetworkClient client;
        public MainPage()
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
            AddButton("Check for updates", Unknown);
            AddButton("//Test Connection", TestConnection);
            AddButton("//Send", SendMessage);

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
        private void TestConnection(object sender, RoutedEventArgs e)
        {
            client = new NetworkClient();
            client.Connect();
        }
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            if (client != null)
                client.SendMessage("Test message from client");
        }
        #endregion

        #region Functions
        private void SaveSettings() { }
        private void SaveAppData() { }
        #endregion
    }
}
