using Settings.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NextResetServer
{
    public class ClientHandler
    {
        private string _name = null;
        private TcpClient _client = null;
        public ClientHandler(NetworkListener server, string name, TcpClient client)
        {
            _name = name;
            _client = client;
        }
        public void handle()
        {
            Debug.WriteLine("Waiting for networkpackage");
            NetworkPackage package = NetworkListener.RecievePackage(_client);
            Debug.WriteLine("Clienthandler, Output: " + package.message);
        }
    }
}
