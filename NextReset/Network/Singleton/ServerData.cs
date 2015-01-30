using Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Singleton
{
    public class ServerData
    {
        private static ServerData _Instance = null;
        private static object pathlock = new object();
        private string location = AppSettings.SaveOrLoad._Level_Source_Location + "/" + "Accounts";
        private string filename = "AccountData.reset";
        public List<string> OutputLines { get; private set; }
        public Dictionary<string, string> _Accounts { get; set; }
        private ServerData() { OutputLines = new List<string>(); Load(); }
        public static ServerData Get
        {
            get
            {
                lock (pathlock)
                {
                    if (_Instance == null)
                        _Instance = new ServerData();
                    return _Instance;
                }
            }
        }

        private bool Load()
        {
            Reload();
            return false;
        }
        public bool Reload()
        {
            _Accounts = new Dictionary<string, string>();
            Debug.WriteLine("Reload Data");
            if (DirectoryExists())
            { }
            if (FileExists())
            {

            }

            return false;
        }
        public bool Save()
        {
            return false;
        }
        #region Save or Load
        #region Save
        private void SaveData()
        {

        }
        #endregion
        #region Load
        private bool DirectoryExists()
        {
            if (!Directory.Exists(AppSettings.SaveOrLoad._Level_Source_Location))
            {
                Directory.CreateDirectory(AppSettings.SaveOrLoad._Level_Source_Location);
                OutputLines.Add(Time() + " Directory [" + AppSettings.SaveOrLoad._Level_Source_Location + "] has been created");
            }
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
                OutputLines.Add(Time() + " Directory [" + location + "] has been created");
                return false;
            }
            return true;
        }
        private bool FileExists()
        {
            if (!File.Exists(location + "/" + filename))
            {
                using (StreamWriter writer = new StreamWriter(location + "/" + filename))
                { writer.WriteLine("admin - admin"); }
                OutputLines.Add(Time() + " File [" + location + "/" + filename + "] has been created");
                return false;
            }
            return true;
        }
        public static string Time()
        { return DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]"); }
        #endregion
        #endregion
        public bool ContainsAccount(string username, string password)
        { return false; } // TODO: Implementation
    }
}
