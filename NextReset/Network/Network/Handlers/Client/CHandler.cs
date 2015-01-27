using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Handlers.Client
{
    public interface CHandler
    {
        void Handle(NetworkClient client, NetworkPackage package);
    }
}
