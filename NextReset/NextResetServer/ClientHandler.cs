﻿using Settings.Network;
using Settings.Network.Handlers;
using Settings.Network.Handlers.Client;
using Settings.Network.Handlers.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private NetworkListener _server = null;
        private ServerPage _page = null;
        private Dictionary<int, SHandler> Handlers = null;
        public ClientHandler(ServerPage page, NetworkListener server, string name, TcpClient client)
        {
            _name = name;
            _client = client;
            _server = server;
            _page = page;
            Register();
        }
        public bool handle()
        {
            Debug.WriteLine("Waiting for networkpackage");
            try
            {
                NetworkPackage package = NetworkListener.RecievePackage(_client);
                try
                { Handlers[package.ExecuteCode].Handle(_server, _client, package); }
                catch (NullReferenceException) { if (!_page.LogOff(_name)) { _page.AddOutput("Client [" + _name + "] Left"); }; return false; }
                catch (KeyNotFoundException) { _page.AddOutput("ExecuteCode [" + package.ExecuteCode + "] was not found"); return false; }
            }
            catch (IOException) { if (!_page.LogOff(_name)) { _page.AddError("handle", NetworkSettings.Error.IOException); }; return false; }
            return true;
        }
        private void Register()
        {
            if (Handlers == null)
            {
                Handlers = new Dictionary<int, SHandler>();
                Handlers.Add((int)NetworkSettings.ExecuteCode.login_request, new LoginHandler());
                Handlers.Add((int)NetworkSettings.ExecuteCode.update_available_check, new CheckVersionHandler());
                Handlers.Add((int)NetworkSettings.ExecuteCode.update_request, new UpdateRequestHandler());
                Handlers.Add((int)NetworkSettings.ExecuteCode.create_account_request, new CreateAccountHandler());
            }
        }
    }
}
