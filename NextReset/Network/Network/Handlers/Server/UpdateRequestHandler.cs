using Network;
using Settings.Network.Server.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Network.Handlers.Server
{
    public class UpdateRequestHandler : SHandler
    {
        public void Handle(NetworkListener server, TcpClient client, NetworkPackage package)
        {
            List<Tuple<List<string>, string>> levels = new List<Tuple<List<string>, string>>();

            List<string> patches = LoadPatches();
            foreach (string patch in patches)
            {
                levels.Add(CreateLevel(LoadPatch(patch), patch));
            }


            Debug.WriteLine("Sending Update Data");
            NetworkPackage SendPackage = new NetworkPackage();
            SendPackage.ExecuteCode = (int)(NetworkSettings.ExecuteCode.update_response);
            LevelsContainer container = new LevelsContainer();
            container.Levels = levels;
            NetworkListener.SendPackage(client, SendPackage);
        }
        private Tuple<List<string>, string> CreateLevel(List<string> lines, string patchname)
        { return new Tuple<List<string>, string>(lines, patchname); }
        private List<string> LoadPatches()
        {
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
            string file = AppSettings.SaveOrLoad._Level_Source_Filename;
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (!File.Exists(file))
            { using (StreamWriter writer = new StreamWriter(location + "/" + file)) { writer.WriteLine("Starters.reset"); }}

            List<string> patches = new List<string>();
            using (StreamReader reader = new StreamReader(location + "/" + file))
            {
                while (reader.Peek() >= 0)
                { patches.Add(reader.ReadLine()); }
            }
            return patches;
        }
        private List<string> LoadPatch(string patch)
        {
            GameController.CreateExample();
            return null;
        }
    }
}
