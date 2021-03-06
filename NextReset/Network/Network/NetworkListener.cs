﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private Dictionary<string, TcpClient> clients = null;
        private TcpListener listener = null;
        public NetworkListener()
        {
            clients = new Dictionary<string, TcpClient>();
        }
        public void Start()
        {
            if (listener == null)
            {
                this.listener = new TcpListener(NetworkSettings.Address.Server_Port);
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
        public List<string> GetTcpClientKeys()
        {
            List<string> list = new List<string>();
            foreach (string key in clients.Keys)
            { list.Add(key); }
            return list;
        }
        public void AddTcpClient(string Key, TcpClient Client)
        { this.clients.Add(Key, Client); }
        public TcpClient GetTcpClient(string Key)
        { return this.clients[Key]; }
        public void RemoveTcpClient(string key)
        { this.clients.Remove(key); }
        public int CountTcpClients()
        { return clients.Count; }
        public static NetworkPackage RecievePackage(TcpClient client)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                NetworkPackage package = (NetworkPackage)formatter.Deserialize(client.GetStream());
                return package;
            }
            catch (IOException) { throw new IOException(); }
            catch (InvalidOperationException) { return null; }
        }
        public static void SendPackage(TcpClient client, NetworkPackage package)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(client.GetStream(), package);
            }
            catch (IOException) { throw new IOException(); }
        }
    }
}
