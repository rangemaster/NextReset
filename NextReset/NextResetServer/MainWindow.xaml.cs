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
    public partial class MainWindow : Window
    {
        private NetworkListener server;
        private Thread ReceiveThread = null;
        private Dictionary<string, Socket> clients = null;
        public MainWindow()
        {
            InitializeComponent();
            _Output_tx.Text = "";
            for (int i = 1; i <= 20; i++)
            { Output("Test: " + i); }
            server = new NetworkListener();
        }
        private void Output(string message)
        {
            Debug.WriteLine("Output: " + message);
            //this.Dispatcher.BeginInvoke(Output(message), this);
            _Output_tx.Text += message + "\n";
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
        private void InitReceiveThread()
        {
            this.ReceiveThread = new Thread(() =>
            {
                while (true)
                {
                    Debug.WriteLine("Waiting for client");
                    TcpClient client = server.AcceptTcpClient();
                    Debug.WriteLine("Server: Accept");
                    NetworkPackage package = server.RecievePackage(client);

                    Output(package.message);
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
    }
}
