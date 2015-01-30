using Main.Screen.Tutorial;
using Main.Screens;
using Network;
using Network.Singleton;
using Settings;
using Settings.Network;
using Settings.Network.Handlers.Client;
using Settings.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        private int _LoginWidth = 200, _LoginFontSize = 20, _StartOffset = 1;
        private NetworkClient client;
        private Thread _NetworkReader = null;
        private Dictionary<int, CHandler> _Handlers = null;
        private bool StopReading = true;
        public ClientPage()
        {
            InitializeComponent();
            if (SingleGameData.Get.client != null)
            { client = SingleGameData.Get.client; }
            if (!SingleGameData.Get.LoggedIn)
            { InitBeforeLogin(); }
            else
            { InitAfterLogin(); }
            AppSettings.PageSettings(this);
        }
        #region Befor Login
        #region Init
        private void InitBeforeLogin()
        {
            #region Username
            Label Username_lb = new Label();
            TextBox Username_tx = new TextBox();
            #endregion
            #region Password
            Label Password_lb = new Label();
            PasswordBox Password_tx = new PasswordBox();
            #endregion
            #region Feedback
            TextBlock Feedback_tx = new TextBlock();
            #endregion
            _Button_Stackpanel.Children.Add(Username_lb);
            _Button_Stackpanel.Children.Add(Username_tx);
            _Button_Stackpanel.Children.Add(Password_lb);
            _Button_Stackpanel.Children.Add(Password_tx);
            _Button_Stackpanel.Children.Add(Feedback_tx);
            InitFields();
            AddButton("Confirm", Confirm);
        }
        private void InitUsername_lb()
        {
            Label label = _Button_Stackpanel.Children[0 + _StartOffset] as Label;
            label.Width = _LoginWidth;
            label.FontSize = _LoginFontSize;
            label.Content = AppSettings.Login._Username_tx;
            label.Foreground = AppSettings.Login._Username_label_brush;
        }
        private void InitUsername_tx()
        {
            TextBox tb = _Button_Stackpanel.Children[1 + _StartOffset] as TextBox;
            tb.Width = _LoginWidth;
            tb.FontSize = _LoginFontSize;
            tb.Foreground = AppSettings.Login._Username_tb_brush;
            tb.IsEnabled = true;
        }
        private void InitPassword_lb()
        {
            Label label = _Button_Stackpanel.Children[2 + _StartOffset] as Label;
            label.Width = _LoginWidth;
            label.FontSize = _LoginFontSize;
            label.Content = AppSettings.Login._Password_tx;
            label.Foreground = AppSettings.Login._Password_label_brush;
        }
        private void InitPassword_tx()
        {
            PasswordBox pb = _Button_Stackpanel.Children[3 + _StartOffset] as PasswordBox;
            pb.Width = _LoginWidth;
            pb.FontSize = _LoginFontSize;
            pb.Foreground = AppSettings.Login._Password_tb_brush;
            pb.IsEnabled = true;
        }
        private void InitFeedback_tx()
        {
            TextBlock tb = _Button_Stackpanel.Children[4 + _StartOffset] as TextBlock;
            tb.Width = _LoginWidth;
            tb.FontSize = _LoginFontSize;
            tb.Foreground = AppSettings.Login._Feedback_tb_brush;
        }
        private void InitFields()
        {
            InitUsername_lb();
            InitUsername_tx();
            InitPassword_lb();
            InitPassword_tx();
            InitFeedback_tx();
        }
        #endregion
        #region Buttons
        #endregion
        #region Functions
        private void Confirm(object sender, RoutedEventArgs e)
        {
            int before = 1;
            InitFields();
            if ((_Button_Stackpanel.Children[1 + before] as TextBox).Text == null || (_Button_Stackpanel.Children[1 + before] as TextBox).Text.Equals(""))
            {
                (_Button_Stackpanel.Children[0 + before] as Label).Content = "(Fill in) Username:";
                (_Button_Stackpanel.Children[0 + before] as Label).Foreground = new SolidColorBrush(Colors.Red);
            }
            else if ((_Button_Stackpanel.Children[3 + before] as PasswordBox).Password == null || (_Button_Stackpanel.Children[3 + before] as PasswordBox).Password.Equals(""))
            {
                (_Button_Stackpanel.Children[2 + before] as Label).Content = "(Fill in) Password:";
                (_Button_Stackpanel.Children[2 + before] as Label).Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                TextBox Username_tb = _Button_Stackpanel.Children[1 + _StartOffset] as TextBox;
                PasswordBox Password_tb = _Button_Stackpanel.Children[3 + _StartOffset] as PasswordBox;
                Username_tb.IsEnabled = false;
                Password_tb.IsEnabled = false;
                string username = Username_tb.Text;
                string password = Password_tb.Password;
                string passFix = "";
                for (int i = 0; i < password.Length; i++)
                { passFix += (i == 0 || i == password.Length - 1 ? "" + password[i] : "*"); }
                string user = AppSettings.MessageBox.Confirmation._User + "[" + username + "]";
                string pass = AppSettings.MessageBox.Confirmation._Pass + "[" + passFix + "]";
                string title = AppSettings.MessageBox.Confirmation._Confirm;
                MessageBoxResult result = MessageBox.Show(user + ", " + pass, title, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                { SendConfirmation(username, password); }
                else
                { InitFields(); }
            }
        }
        private void SendConfirmation(string username, string password)
        {
            if (client == null)
                client = SetupConnection(username);
            NetworkPackage package = new NetworkPackage((int)NetworkSettings.ExecuteCode.login_request);
            List<string> list = new List<string>();
            list.Add(username);
            list.Add(password);
            package.Data.Add(new Tuple<List<string>, string>(list, AppSettings.Login._LoginData));
            client.Send(package);
            NetworkPackage ReturnPackage = client.Receive();
            ConfirmationHandler(ReturnPackage);
        }
        private void ConfirmationHandler(NetworkPackage ReturnPackage)
        {
            if (ReturnPackage.Value == 0)
            { LoginWrong(); }
            else if (ReturnPackage.Value == 1)
            { Succes(); }
            else if (ReturnPackage.Value == 2)
            { AdminSucces(); }
            else { LoginFailed(); }
        }
        private void LoginWrong()
        {
            (_Button_Stackpanel.Children[4 + _StartOffset] as TextBlock).Text = AppSettings.Login._LoginWrong;
            FeedbackTimer();
        }
        private void LoginFailed()
        {
            (_Button_Stackpanel.Children[4 + _StartOffset] as TextBlock).Text = AppSettings.Login._LoginFailed;
            ServerData.Get.AddOutputLine(AppSettings.Login._LoginFailed);
            FeedbackTimer();
        }
        #region Timer
        private void FeedbackTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Tick += FeedbackTimer_Tick;
            timer.Start();
        }
        private void FeedbackTimer_Tick(object sender, EventArgs e)
        {
            (_Button_Stackpanel.Children[4 + _StartOffset] as TextBlock).Text = "";
            InitFields();
            (sender as DispatcherTimer).Stop();
        }
        #endregion
        private void AdminSucces()
        {
            Succes();
        }
        private void Succes()
        {
            SingleGameData.Get.client = client;
            SingleGameData.Get.LoggedIn = true;
            _Button_Stackpanel.Children.Clear();
            InitAfterLogin();
        }
        #endregion
        #endregion

        #region After Login
        private void InitAfterLogin()
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
            int UpdateCheckValue = UpdateAvailableCheck(client, GameController.LoadVersion());
            if (UpdateCheckValue == -1)
            {
                MessageBoxResult result = MessageBox.Show(AppSettings.MessageBox.UpdateCheck._Line, AppSettings.MessageBox.UpdateCheck._Title, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Debug.WriteLine("Get next update");
                    NetworkPackage sendPackage = new NetworkPackage((int)NetworkSettings.ExecuteCode.update_request);
                    client.Send(sendPackage);
                    WaitForResponseUpdate(client);
                    Debug.WriteLine("Not waiting anymore");
                }
            }
            else if (UpdateCheckValue == 1)
            { MessageBox.Show("Newer version then on the server"); }
            else if (UpdateCheckValue == 0)
            { MessageBox.Show(AppSettings.MessageBox.UpdateCheck._No_Update); }
            else
            { Debug.WriteLine("Update Available Check, Wrong package send [" + UpdateCheckValue + "]"); }
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
        public int UpdateAvailableCheck(NetworkClient client, string version)
        {
            int available = 0;
            NetworkPackage sendPackage = new NetworkPackage((int)NetworkSettings.ExecuteCode.update_available_check);
            sendPackage.Message = version;
            client.Send(sendPackage);
            NetworkPackage returnPackage = client.Receive();
            available = returnPackage.Value;
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
                HandleUpdate(returnPackage);
            }
            else { Debug.WriteLine("Server send wrong package [" + returnPackage.ExecuteCode + "]"); }
        }
        #endregion
        #region Handle Update
        public void HandleUpdate(NetworkPackage package)
        {
            for (int i = 0; i < package.Data.Count; i++)
            {
                Debug.WriteLine("--- " + package.Data[i].Item2 + " ---");
                foreach (string line in package.Data[i].Item1)
                    Debug.WriteLine("Received: " + line);
            }
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
            string file1 = AppSettings.SaveOrLoad._Level_Source_Filename;
            using (StreamWriter writer = new StreamWriter(location + "/" + AppSettings.SaveOrLoad._Version_Filename))
            {
                writer.WriteLine(package.Message);
            }
            using (StreamWriter writer = new StreamWriter(location + "/" + file1))
            {
                for (int i = 0; i < package.Data.Count; i++)
                {
                    writer.WriteLine(package.Data[i].Item2);
                }
            }
            foreach (Tuple<List<string>, string> datapack in package.Data)
            {
                using (StreamWriter writer2 = new StreamWriter(location + "/" + datapack.Item2))
                {
                    foreach (string line in datapack.Item1)
                    {
                        writer2.WriteLine(line);
                    }
                }
            }
        }
        #endregion
        private void SaveSettings() { }
        private void SaveAppData() { }
        #endregion
        #endregion
    }
}
