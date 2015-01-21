using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Settings.Network
{
    public class NetworkListener
    {
        private TcpListener listener = null;
        public NetworkListener()
        {
        }
        public void Start()
        {
            if (listener == null)
            {
                this.listener = new TcpListener(7999); // TODO: Magic cookie
                this.listener.Start();
            }
        }
        public void Stop()
        {
            if (listener != null)
            {
                this.listener.Stop();
                this.listener = null;
            }
        }
        public TcpClient AcceptTcpClient()
        {
            return listener.AcceptTcpClient();
        }
        public Socket AcceptSocket()
        {
            return listener.AcceptSocket();
        }
        public NetworkPackage RecievePackage(TcpClient client)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkPackage package = (NetworkPackage)formatter.Deserialize(client.GetStream());
            Debug.WriteLine("Receiving: " + package.message);
            return package;
        }
        public void SendPackage(TcpClient client, NetworkPackage package)
        {
            Debug.WriteLine("Sending : " + package.message);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(client.GetStream(), package);
        }
    }
}
