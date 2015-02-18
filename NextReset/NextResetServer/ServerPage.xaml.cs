using Network;
using Settings.Network;
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

namespace NextResetServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerPage : Page
    {
        private static List<string> _NewOutputLines = null;
        private static List<string> _NewLogLines = null;
        private List<string> _OldOutputLines = null;
        private List<string> _OldLogLines = null;
        private DispatcherTimer _OutputTimer = null;
        private NetworkListener server;
        private Thread ReceiveThread = null;
        private bool LogSelected = false;
        public ServerPage()
        {
            InitializeComponent();
            Init();
            StartOutputTimer();
            _Start_bn_Click(new object(), new RoutedEventArgs());
        }
        private void Init()
        {
            _Output_tx.Text = "";
            server = new NetworkListener();
            _NewOutputLines = new List<string>();
            _OldOutputLines = new List<string>();
            _NewLogLines = new List<string>();
            _OldLogLines = new List<string>();
        }
        #region Output
        private void Output(string message)
        { _Output_tx.Text += message + "\n"; }
        private void Log(string message)
        { _Output_tx.Text += message + "\n"; }
        public void AddError(int code)
        { AddError("", code); }
        public void AddError(string location, int code)
        { _NewLogLines.Add(ServerData.Time() + " --- " + "Error [" + location + "][" + code + "]"); }
        public void AddOutput(string output)
        { _NewOutputLines.Add(ServerData.Time() + " --- " + output); }
        #endregion
        #region Peek
        private int OutputPeek
        { get { return _NewOutputLines.Count > 0 ? 1 : -1; } }
        private int LogPeek
        { get { return _NewLogLines.Count > 0 ? 1 : -1; } }
        private int RegistrationPeek
        { get { return ServerData.Get.GetRegistrations().Count > 0 ? 1 : -1; } }
        #endregion
        #region Timer
        private void StartOutputTimer()
        {
            if (_OutputTimer == null)
            {
                _OutputTimer = new DispatcherTimer();
                _OutputTimer.Interval = new TimeSpan(0, 0, 2);
                _OutputTimer.Tick += OutputTimer_Tick;
                _OutputTimer.Start();
            }
        }
        private void OutputTimer_Tick(object sender, EventArgs e)
        {
            OutputLineHandling();
            LogLineHandling();
            RegistrationHandling();
            UpdateOnlineCounter();
        }
        private void OutputLineHandling()
        {
            for (int i = 0; i < ServerData.Get.GetOutputLines().Count; i++)
            { _NewOutputLines.Add(ServerData.Get.GetOutputLines()[i]); }
            ServerData.Get.GetOutputLines().Clear();
            while (OutputPeek >= 0)
            {
                string line = _NewOutputLines[0];
                Debug.WriteLine("Activity Peek: " + line);
                _NewOutputLines.RemoveAt(0);
                _OldOutputLines.Add(line);
                if (!LogSelected)
                { Output(line); }
            }
        }
        private void LogLineHandling()
        {
            for (int i = 0; i < ServerData.Get.GetLogLines().Count; i++)
            { _NewLogLines.Add(ServerData.Get.GetLogLines()[i]); }
            ServerData.Get.GetLogLines().Clear();
            while (LogPeek >= 0)
            {
                string line = _NewLogLines[0];
                Debug.WriteLine("Log peek: " + line);
                _NewLogLines.RemoveAt(0);
                _OldLogLines.Add(line);
                if (LogSelected)
                { Output(line); }
            }
        }
        private void RegistrationHandling()
        {
            if (RegistrationPeek >= 0)
            {
                Debug.WriteLine("Peek");
                foreach (string username in ServerData.Get.GetRegistrations().Keys)
                {
                    if (ServerData.Get.OnForbiddenList(username) < 0)
                    { ServerData.Get.AddAccount(username, ServerData.Get.GetRegistrations()[username]); }
                    else { Debug.WriteLine("On Forbiddenlist: " + username); }
                }
                ServerData.Get.GetRegistrations().Clear();
            }
        }
        #endregion
        #region Buttons
        private void _Start_bn_Click(object sender, RoutedEventArgs e)
        {
            server.Start();
            StartReceiving();
            _Start_bn.IsEnabled = false;
            _Stop_bn.IsEnabled = true;
            ServerData.Get.IsUpdatable = false;
            _Updatable_bn_Click(sender, e);
            _NewOutputLines.Add("Server Started");
        }
        private void _Stop_bn_Click(object sender, RoutedEventArgs e)
        {
            server.Stop();
            StopReceiving();
            _Stop_bn.IsEnabled = false;
            _Start_bn.IsEnabled = true;
            ServerData.Get.IsUpdatable = false;
            _NewOutputLines.Add("Server Stopped");
        }
        private void _Updatable_bn_Click(object sender, RoutedEventArgs e)
        {
            if (ServerData.Get.IsUpdatable)
            {
                _Updatable_bn.Background = AppSettings.ServerSettings.Buttons._InActive;
                ServerData.Get.IsUpdatable = false;
            }
            else
            {
                _Updatable_bn.Background = AppSettings.ServerSettings.Buttons._Active;
                ServerData.Get.IsUpdatable = true;
            }
            Debug.WriteLine("Updatable: " + ServerData.Get.IsUpdatable);
        }
        private void _Save_bn_Click(object sender, RoutedEventArgs e)
        { ServerData.Get.Save(); }
        private void _Load_bn_Click(object sender, RoutedEventArgs e)
        { ServerData.Get.Reload(); }
        private void _Online_bn_Click(object sender, RoutedEventArgs e)
        { PrintOnlineUsers(); }
        #endregion
        #region Reading Thread
        private void InitReceiveThread()
        {
            this.ReceiveThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient();
                        string name = NetworkListener.RecievePackage(client).Message;
                        Debug.WriteLine("Server: Username: " + name);
                        server.AddTcpClient(name, client);
                        new Thread(() =>
                        {
                            ClientHandler handler = new ClientHandler(this, server, name, client);
                            while (true)
                            {
                                if (!handler.handle())
                                    break;
                            }
                        }).Start();
                    }
                    catch (SocketException) { AddError(NetworkSettings.Error.SocketExeption); }
                }
            });
        }
        private void StartReceiving()
        {
            if (ReceiveThread == null)
            {
                InitReceiveThread();
                this.ReceiveThread.Start();
                Debug.WriteLine("Started Receiving");
            }
        }
        private void StopReceiving()
        {
            if (ReceiveThread != null)
            {
                server.Stop();
                this.ReceiveThread.Abort();
                this.ReceiveThread = null;
                Debug.WriteLine("Stopped Receiving");
            }
        }
        #endregion
        #region AccountData
        public bool LogOff(string name)
        {
            try { server.RemoveTcpClient(name); }
            catch (IOException) { return false; }
            catch (ArgumentNullException) { return false; }
            catch (NullReferenceException) { return false; }
            AddOutput(name + " --> Goes offline");
            return true;
        }
        public void UpdateOnlineCounter()
        {
            int amount = server.CountTcpClients();
            _Online_Amount_tx.Content = amount;
        }
        private void PrintOnlineUsers()
        {
            AddOutput("=========== Online ===========");
            foreach (string key in server.GetTcpClientKeys())
            { AddOutput(" * " + key); }
            AddOutput("==============================");
        }
        #endregion
        public static void AddNewOutputLine(string line)
        { _NewOutputLines.Add(line); }
        private void Log_bn_Click(object sender, RoutedEventArgs e)
        {
            LogSelected = true;
            this._Output_tx.Text = "";
            foreach (string line in _OldLogLines)
            { Output(line); }
        }
        private void Activity_bn_Click(object sender, RoutedEventArgs e)
        {
            LogSelected = false;
            this._Output_tx.Text = "";
            foreach (string line in _OldOutputLines)
            { Output(line); }
        }
    }
}
