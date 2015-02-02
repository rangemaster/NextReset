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
        private string forbidden_list = "Forbidden.reset";
        private List<string> _OutputLines;
        public bool IsUpdatable { get; set; }
        private Dictionary<string, string> _Accounts { get; set; }
        private Dictionary<string, string> _Registrations { get; set; }
        public List<string> _WhiteList { get; private set; }
        public List<string> _BlackList { get; private set; }
        public List<string> _ForbiddenList { get; private set; }
        private ServerData()
        {
            _Registrations = new Dictionary<string, string>();
            _Accounts = new Dictionary<string, string>();
            _OutputLines = new List<string>();
            _WhiteList = new List<string>();
            _BlackList = new List<string>();
            _ForbiddenList = new List<string>();
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
            Debug.WriteLine("Reload Data");
            DirectoryExists();
            AccountsExists();
            LoadAccounts();
            BlackListExists();
            LoadBlackList();
            WhiteListExists();
            LoadWhiteList();
            ForbiddenListExists();
            LoadForbiddenList();
            return true;
        }
        public bool Save()
        {
            // TODO: implementation
            return false;
        }
        #region Save or Load
        #region Save
        private void SaveAccounts()
        {
            // TODO: implementation
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
        private bool ForbiddenListExists()
        {
            if (!File.Exists(location + "/" + forbidden_list))
            { CreateForbiddenList(); return false; }
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
        private void CreateForbiddenList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + forbidden_list)) { writer.WriteLine(""); }
            AddOutputLine("Forbiddenlist [" + location + "/" + forbidden_list + "] has been created");
        }
        public bool CreateRegistration(string username, string password)
        {
            if (_Accounts.ContainsKey(username))
            { return false; }
            if (_Registrations.ContainsKey(username))
            { return false; }
            try
            { _Registrations.Add(username, password); }
            catch (ArgumentException) { return false; }
            AddOutputLine("Registration [" + username + "] [" + password + "] was Added");
            return true;
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
            { while (reader.Peek() >= 0) { string line = reader.ReadLine(); _WhiteList.Add(line.Trim()); } }
        }
        private void LoadBlackList()
        {
            _BlackList.Clear();
            using (StreamReader reader = new StreamReader(location + "/" + black_list))
            { while (reader.Peek() >= 0) { string line = reader.ReadLine(); _BlackList.Add(line.Trim()); } }
        }
        private void LoadForbiddenList()
        {
            _ForbiddenList.Clear();
            using (StreamReader reader = new StreamReader(location + "/" + forbidden_list))
            { while (reader.Peek() >= 0) { string line = reader.ReadLine(); _ForbiddenList.Add(line.Trim()); } }
        }
        public void AddOutputLine(string line)
        { _OutputLines.Add(Time() + " " + line); }
        public void AddAccount(string username, string password)
        {
            if (!_Accounts.ContainsKey(username))
            {
                _Accounts.Add(username, password);
                AddOutputLine("[" + username + "] [" + password + "] was added to the accounts");
            }
        }
        public List<string> GetOutputLines()
        { return _OutputLines; }
        public Dictionary<string, string> GetAccounts()
        { return _Accounts; }
        public Dictionary<string, string> GetRegistrations()
        { return _Registrations; }
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
            _OutputLines.Add("Login try: " + username + " - " + password + " = " + succes);
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
        public int OnForbiddenList(string word)
        {
            LoadForbiddenList();
            Debug.WriteLine("Forbidden word check [" + word + "]");
            for (int i = 0; i < _ForbiddenList.Count; i++)
            {
                string forbiddenWord = _ForbiddenList[i];
                Debug.WriteLine("Forbidden word: " + forbiddenWord);
                //for (int j = 0; j < forbiddenWord.Length; j++)
                //{
                if (!forbidden_list.Equals("") && !word.Equals(""))
                    if (word.Contains(forbiddenWord))
                    { Debug.WriteLine("!!Forbidden!!"); AddOutputLine("Busted!! --- [" + forbiddenWord + "[" + word + "]] forbidden"); return 1; }
                //}
            }
            return -1;
        }
        #endregion
    }
}
