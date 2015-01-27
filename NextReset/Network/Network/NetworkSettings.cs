﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network
{
    public class NetworkSettings
    {
        public class Address
        {
            public const string IP_Address = "127.0.0.1"; // TODO: Implement in code
            public const int Server_Port = 7999; // TODO: Implement in code
        }
        public enum ExecuteCode
        {
            execute_request,
            execute_response,
            accept,
            denied,
            update_available_check,
            update_available_response,
            update_request,
            update_response,

        }
        public class Error
        {
            public const int SocketExeption = 202;
            public const int IOException = 203;
            public const int TcpException = 204;
        }
    }
}
