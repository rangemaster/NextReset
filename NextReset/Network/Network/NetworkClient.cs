using Settings.ThrowableException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network
{
    public class NetworkClient
    {
        private TcpClient client = null;
        public NetworkClient()
        {

        }
        public void Connect()
        { Connect("127.0.0.1", 7999); }
        public void Connect(string hostname, int port)
        { client = new TcpClient(hostname, port); }
        public bool IsConnected { get { return client.Connected; } }
        public void SendMessage(string message)
        {
            NetworkPackage package = new NetworkPackage();
            package.Message = message;
            Send(package);
        }
        public string ResieveMessage() { return Receive().Message; }
        public void Send(NetworkPackage package)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(client.GetStream(), package);
        }
        public NetworkPackage Receive()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkPackage package = (NetworkPackage)formatter.Deserialize(client.GetStream());
            return package;
        }
    }
}
