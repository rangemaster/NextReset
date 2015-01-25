using Settings.Network;
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
using System.Windows.Threading;

namespace NextResetServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerPage : Page
    {
        private List<string> _NewOutputLines = null;
        private List<string> _OldOutputLines = null;
        private DispatcherTimer _OutputTimer = null;
        private NetworkListener server;
        private Thread ReceiveThread = null;
        public ServerPage()
        {
            InitializeComponent();
            _Output_tx.Text = "";
            server = new NetworkListener();
        }
        private void Output(string message)
        {
            Debug.WriteLine("Output: " + message);
            _Output_tx.Text += message + "\n";
        }
        private int Peek()
        { return _NewOutputLines.Count > 0 ? 1 : -1; }
        private void StartOutputTimer()
        {
            if (_OutputTimer == null)
            {
                _OutputTimer = new DispatcherTimer();
                _OutputTimer.Interval = new TimeSpan(0, 0, 5);
                _OutputTimer.Tick += OutputTimer_Tick;
                _OutputTimer.Start();
            }
        }
        private void OutputTimer_Tick(object sender, EventArgs e)
        {
            while (Peek() >= 0)
            {
                string line = _NewOutputLines[0];
                _NewOutputLines.RemoveAt(0);
                Output(line);
                _OldOutputLines.Add(line);
            }
        }
        private void _Start_bn_Click(object sender, RoutedEventArgs e)
        {
            server.Start();
            StartReceiving();
        }
        private void _Stop_bn_Click(object sender, RoutedEventArgs e)
        {
            server.Stop();
            StopReceiving();
        }

        #region Reading Thread
        private void InitReceiveThread()
        {
            this.ReceiveThread = new Thread(() =>
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    string name = NetworkListener.RecievePackage(client).message;
                    Debug.WriteLine("Message: " + name);
                    server.AddTcpClient(name, client);
                    new Thread(() =>
                    {
                        ClientHandler handler = new ClientHandler(server, name, client);
                        while (true)
                        {
                            handler.handle();
                        }
                    }).Start();
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
    }
}
