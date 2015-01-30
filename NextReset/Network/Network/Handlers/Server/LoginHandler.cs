using Settings.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Settings.Network.Handlers.Server
{
    public class LoginHandler : SHandler
    {
        public void Handle(NetworkListener server, TcpClient client, NetworkPackage package)
        {
            int Value = -1;
            if (ServerData.Get._Accounts == null)
            { ServerData.Get.Reload(); }
            if (package.Data[0].Item2.Equals("Login Data")) // Magic cookie
            {
                string username = package.Data[0].Item1[0];
                string password = package.Data[0].Item1[1];
                Value = ServerData.Get.ContainsAccount(username, password);
            }
            NetworkPackage ReturnPackage = new NetworkPackage(); // TODO: NetworkPackage Constructor (int executecode)
            ReturnPackage.ExecuteCode = (int)NetworkSettings.ExecuteCode.login_response;
            ReturnPackage.Value = Value;
            NetworkListener.SendPackage(client, ReturnPackage);
            Debug.WriteLine("Server: Login Package Send");
        }
    }
}
