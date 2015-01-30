using Network;
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
            // TODO: Update available questions set off button. So there will be no confict when changing the files
            List<Tuple<List<string>, string>> data = new List<Tuple<List<string>, string>>();
            List<string> patches = LoadPatches();
            foreach (string patch in patches)
            {
                try
                {
                    List<string> list = LoadPatch(patch);
                    data.Add(CreateLevel(list, patch));
                }
                catch (FileNotFoundException) { Debug.WriteLine("Patch not fount [" + patch + "]"); }
            }
            string version = null;
            using (StreamReader reader = new StreamReader(AppSettings.SaveOrLoad._Level_Source_Location + "/" + "Version.reset"))
            { version = reader.ReadLine(); }

            Debug.WriteLine("Sending Update Data");
            NetworkPackage SendPackage = new NetworkPackage();
            SendPackage.ExecuteCode = (int)(NetworkSettings.ExecuteCode.update_response);
            SendPackage.Message = version;
            SendPackage.Data = data;
            Debug.WriteLine("Debugging list");
            for (int i = 0; i < SendPackage.Data.Count; i++)
            {
                for (int j = 0; j < SendPackage.Data[i].Item1.Count; j++)
                {
                    Debug.WriteLine("Sending: " + SendPackage.Data[i].Item1[j]);
                }
            }
            NetworkListener.SendPackage(client, SendPackage);
        }
        //private List<string> CreateLevel(List<string> lines, string patchname)
        //{ return lines; }
        private Tuple<List<string>, string> CreateLevel(List<string> lines, string patchname)
        { return new Tuple<List<string>, string>(lines, patchname); }
        private List<string> LoadPatches()
        {
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
            string file = AppSettings.SaveOrLoad._Level_Source_Filename;
            if (!Directory.Exists(location))
            { Directory.CreateDirectory(location); }
            if (!File.Exists(location + "/" + file))
            { using (StreamWriter writer = new StreamWriter(location + "/" + file)) { writer.WriteLine("Starters.reset"); } }

            List<string> list_patches = new List<string>();
            using (StreamReader reader = new StreamReader(location + "/" + file))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    list_patches.Add(line);
                }
            }
            return list_patches;
        }
        private List<string> LoadPatch(string patch)
        {
            GameController.CreateExample();
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
            List<string> list_patch = new List<string>();
            using (StreamReader reader = new StreamReader(location + "/" + patch))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    list_patch.Add(line);
                }
            }
            return list_patch;
        }
    }
}
