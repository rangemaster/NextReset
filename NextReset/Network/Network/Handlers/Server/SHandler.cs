﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Server.Handlers
{
    public interface SHandler
    {
        void Handle(NetworkListener server, TcpClient client, NetworkPackage package);
    }
}