using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Handlers.Client
{
    public class RequestResponse : CHandler
    {
        public void Handle(NetworkClient client, NetworkPackage package)
        {
            Debug.WriteLine("Response");
        }
    }
}
