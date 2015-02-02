using Network;
using Settings.Singleton;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Handlers.Server
{
    public class CreateAccountHandler : SHandler
    {
        public void Handle(NetworkListener server, TcpClient client, NetworkPackage package)
        {
            // TODO: Implementation Create Account
            bool succes = false;
            if (package.Data[0].Item2.Equals(AppSettings.Login._CreateData))
            {
                string username = package.Data[0].Item1[0];
                string password = package.Data[0].Item1[1];
                succes = ServerData.Get.CreateRegistration(username, password);
                Debug.WriteLine("Create Account Request [" + username + "] [" + password + "] = " + succes);
            }
            NetworkPackage ReturnPackage = new NetworkPackage((int)NetworkSettings.ExecuteCode.create_account_response);
            ReturnPackage.Value = succes ? 1 : 0;
            NetworkListener.SendPackage(client, ReturnPackage);
        }
    }
}
