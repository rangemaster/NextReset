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
        private static string version = null;
        private static List<string> _NewOutputLines = null;
        private List<string> _OldOutputLines = null;
        private DispatcherTimer _OutputTimer = null;
        private NetworkListener server;
        private Thread ReceiveThread = null;
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
        }
        private void Output(string message)
        { _Output_tx.Text += message + "\n"; }
        public void AddError(int code)
        { AddError("", code); }
        public void AddError(string location, int code)
        {
            _NewOutputLines.Add(ServerData.Time() + " --- " + "Error [" + location + "][" + code + "]");
        }
        public void AddOutput(string output)
        { _NewOutputLines.Add(ServerData.Time() + " --- " + output); }
        private int OutputPeek
        { get { return _NewOutputLines.Count > 0 ? 1 : -1; } }
        private int RegistrationPeek
        { get { return ServerData.Get.GetRegistrations().Count > 0 ? 1 : -1; } }
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
            RegistrationHandling();
        }
        private void OutputLineHandling()
        {
            Debug.WriteLine("Check Outputlines");
            for (int i = 0; i < ServerData.Get.GetOutputLines().Count; i++)
            {
                Debug.WriteLine("Output Line: " + ServerData.Get.GetOutputLines()[i]);
                _NewOutputLines.Add(ServerData.Get.GetOutputLines()[i]);
            }
            ServerData.Get.GetOutputLines().Clear();
            if (OutputPeek >= 0)
            {
                Debug.WriteLine("Peek");
                string line = _NewOutputLines[0];
                _NewOutputLines.RemoveAt(0);
                Output(line);
                _OldOutputLines.Add(line);
            }
        }
        private void RegistrationHandling()
        {
            Debug.WriteLine("Check Registrations");
            if (RegistrationPeek >= 0)
            {
                Debug.WriteLine("Peek");
                foreach (string username in ServerData.Get.GetRegistrations().Keys)
                {
                    Debug.WriteLine("Check username: " + username);
                    if (ServerData.Get.OnForbiddenList(username) < 0)
                    { ServerData.Get.AddAccount(username, ServerData.Get.GetRegistrations()[username]); }
                    else { Debug.WriteLine("On Forbiddenlist: " + username); }
                }// TODO:}
                ServerData.Get.GetRegistrations().Clear();
            }
        }
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
            // TODO: 31-01-2015
            return false;
        }
        #endregion
        public static void AddNewOutputLine(string line)
        { _NewOutputLines.Add(line); }
    }
}
