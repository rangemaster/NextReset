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
        private string account_list = "AccountData.reset";
        private string black_list = "BlackData.reset";
        private string white_list = "WhiteData.reset";
        private List<string> OutputLines;
        public bool IsUpdatable { get; set; }
        public Dictionary<string, string> _Accounts { get; set; }
        public List<string> _WhiteList { get; private set; }
        public List<string> _BlackList { get; private set; }
        private ServerData()
        {
            OutputLines = new List<string>();
            _WhiteList = new List<string>();
            _BlackList = new List<string>();
            Load();
        }
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
            DirectoryExists();
            AccountsExists();
            LoadAccounts();
            BlackListExists();
            LoadBlackList();
            WhiteListExists();
            LoadWhiteList();
            return true;
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
            { CreateSubDictionary(); }
            if (!Directory.Exists(location))
            { CreateDictionary(); return false; }
            return true;
        }
        private bool AccountsExists()
        {
            if (!File.Exists(location + "/" + account_list))
            { CreateAccountList(); return false; }
            return true;
        }
        private bool BlackListExists()
        {
            if (!File.Exists(location + "/" + black_list))
            { CreateBlackList(); return false; }
            return true;
        }
        private bool WhiteListExists()
        {
            if (!File.Exists(location + "/" + white_list))
            { CreateWhiteList(); return false; }
            return true;
        }
        #region Create
        private void CreateSubDictionary()
        {
            Directory.CreateDirectory(AppSettings.SaveOrLoad._Level_Source_Location);
            AddOutputLine("Directory [" + AppSettings.SaveOrLoad._Level_Source_Location + "] has been created");
        }
        private void CreateDictionary()
        {
            Directory.CreateDirectory(location);
            AddOutputLine("Directory [" + location + "] has been created");
        }
        private void CreateAccountList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + account_list))
            { writer.WriteLine("admin - admin"); }
            AddOutputLine("File [" + location + "/" + account_list + "] has been created");
        }
        private void CreateBlackList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + black_list)) { writer.WriteLine(""); }
            AddOutputLine("Whitelist [" + location + "/" + black_list + "] has been created");
        }
        private void CreateWhiteList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + white_list)) { writer.WriteLine("admin"); }
            AddOutputLine("Blacklist [" + location + "/" + white_list + "] has been created");
        }
        #endregion
        private void LoadAccounts()
        {
            _Accounts.Clear();
            int row = 0;
            using (StreamReader reader = new StreamReader(location + "/" + account_list))
            {
                while (reader.Peek() >= 0)
                {
                    try
                    {
                        string line = reader.ReadLine();
                        string[] array = line.Split('-');
                        _Accounts.Add(array[0].Trim(), array[1].Trim());
                    }
                    catch (FormatException) { AddOutputLine("Account loading Exception row(" + row + ")"); }
                    row++;
                }
            }
        }
        private void LoadWhiteList()
        {
            _WhiteList.Clear();
            using (StreamReader reader = new StreamReader(location + "/" + white_list))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    _WhiteList.Add(line.Trim());
                }
            }
        }
        private void LoadBlackList()
        {
            _BlackList.Clear();
            using (StreamReader reader = new StreamReader(location + "/" + black_list))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    _BlackList.Add(line.Trim());
                }
            }
        }
        public void AddOutputLine(string line)
        { OutputLines.Add(Time() + " line"); }
        public List<string> GetOutputLines()
        { return OutputLines; }
        public static string Time()
        { return DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]"); }
        #endregion
        #endregion
        #region Contains Account
        public int ContainsAccount(string username, string password)
        {
            int succes = 0;
            if (_Accounts.ContainsKey(username))
            { if (_Accounts[username].Equals(password)) { succes = 1; } }
            succes += OnBlackList(username);
            succes += OnWhiteList(username);
            OutputLines.Add("Login try: " + username + " - " + password + " = " + succes);
            return succes;
        }
        private int OnBlackList(string username)
        {
            if (_BlackList.Contains(username))
                return 10;
            return 0;
        }
        private int OnWhiteList(string username)
        {
            if (_WhiteList.Contains(username))
                return 1;
            return 0;
        }
        #endregion
    }
}
