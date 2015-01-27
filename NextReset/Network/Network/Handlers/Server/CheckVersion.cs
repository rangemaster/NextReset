using Settings.Network.Server.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Handlers.Server
{
    public class CheckVersion : SHandler
    {
        public void Handle(NetworkListener server, TcpClient client, NetworkPackage package)
        {
            Debug.WriteLine("Check Version");
            string ClientVersion = package.Message;
            string ServerVersion = GameController.LoadVersion();
            int dif = GameController.CompareVersion(ServerVersion, ClientVersion);
            NetworkPackage returnPackage = new NetworkPackage();
            returnPackage.ExecuteCode = (int)NetworkSettings.ExecuteCode.update_available_response;
            returnPackage.Value = dif;
            NetworkListener.SendPackage(client, returnPackage);
        }
    }
}
