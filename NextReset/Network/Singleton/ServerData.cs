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
        private List<string> _LogLines;
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
            _LogLines = new List<string>();
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
            bool succes = true;
            succes = DirectoryExists();
            succes = AccountsExists();
            succes = BlackListExists();
            succes = WhiteListExists();
            succes = ForbiddenListExists();
            succes = Reload();
            return succes;
        }
        public bool Reload()
        {
            bool succes = true;
            Debug.WriteLine("Reload Data");
            succes = LoadAccounts();
            succes = LoadBlackList();
            succes = LoadWhiteList();
            succes = LoadForbiddenList();
            return succes;
        }
        public bool Save()
        {
            SaveAccounts();
            SaveWhiteList();
            SaveBlackList();
            SaveForbiddenList();
            return false;
        }
        #region Save or Load
        #region Save
        #region Save Accounts
        private void SaveAccounts()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + account_list))
            {
                AddLogLine("--- Save Accounts ---");
                foreach (string username in _Accounts.Keys)
                {
                    writer.WriteLine(username + " - " + _Accounts[username]);
                    AddLogLine(" * " + username + "[" + ToPasswordConverter(_Accounts[username] + "]"));
                }
                Debug.WriteLine("Account list saved (" + _Accounts.Count + ")");
            }
        }
        private string ToPasswordConverter(string pass)
        {
            string password = "";
            for (int i = 0; i < pass.Length; i++)
            {
                if (i == 0 || i == pass.Length - 1)
                { password += pass[i]; }
                else { password += "*"; }
            }
            return password;
        }
        #endregion
        #region Save White List
        private void SaveWhiteList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + white_list))
            {
                AddLogLine("--- Save White List ---");
                foreach (string username in _WhiteList)
                {
                    writer.WriteLine(username);
                    AddLogLine(" * " + username);
                }
                Debug.WriteLine("White list saved (" + _WhiteList.Count + ")");
            }
        }
        #endregion
        #region Save Black List
        private void SaveBlackList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + black_list))
            {
                AddLogLine("--- Save Black List ---");
                foreach (string username in _BlackList)
                {
                    writer.WriteLine(username);
                    AddLogLine(" * " + username);
                }
                Debug.WriteLine("Black list saved (" + _BlackList.Count + ")");
            }
        }
        #endregion
        #region Save Forbidden List
        private void SaveForbiddenList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + forbidden_list))
            {
                AddLogLine("--- Save Forbidden List ---");
                foreach (string word in _ForbiddenList)
                {
                    writer.WriteLine(word);
                    AddLogLine(" * " + word);
                }
                Debug.WriteLine("Forbidden list saved (" + _ForbiddenList.Count + ")");
            }
        }
        #endregion
        #endregion
        #region Exists
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
        #endregion
        #region Create
        private void CreateSubDictionary()
        {
            Directory.CreateDirectory(AppSettings.SaveOrLoad._Level_Source_Location);
            AddLogLine("Directory [" + AppSettings.SaveOrLoad._Level_Source_Location + "] has been created");
        }
        private void CreateDictionary()
        {
            Directory.CreateDirectory(location);
            AddLogLine("Directory [" + location + "] has been created");
        }
        private void CreateAccountList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + account_list))
            { writer.WriteLine("admin - admin"); }
            AddLogLine("File [" + location + "/" + account_list + "] has been created");
        }
        private void CreateBlackList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + black_list)) { writer.WriteLine(""); }
            AddLogLine("Whitelist [" + location + "/" + black_list + "] has been created");
        }
        private void CreateWhiteList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + white_list)) { writer.WriteLine("admin"); }
            AddLogLine("Blacklist [" + location + "/" + white_list + "] has been created");
        }
        private void CreateForbiddenList()
        {
            using (StreamWriter writer = new StreamWriter(location + "/" + forbidden_list)) { writer.WriteLine(""); }
            AddLogLine("Forbiddenlist [" + location + "/" + forbidden_list + "] has been created");
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
            string line = "Registration [" + username + "] [" + password + "] was Added";
            AddOutputLine(line);
            AddLogLine(line);
            return true;
        }
        #endregion
        #region Load
        #region Load Accounts
        private bool LoadAccounts()
        {
            _Accounts.Clear();
            int row = 0;
            try
            {
                using (StreamReader reader = new StreamReader(location + "/" + account_list))
                {
                    AddLogLine("--- Load Accounts ---");
                    while (reader.Peek() >= 0)
                    {
                        try
                        {
                            string line = reader.ReadLine();
                            string[] array = line.Split('-');
                            _Accounts.Add(array[0].Trim(), array[1].Trim());
                            AddLogLine(" * " + array[0].Trim());
                        }
                        catch (FormatException) { AddLogLine("Account loading Exception row(" + row + ")"); }
                        row++;
                    }
                }
            }
            catch (IOException) { return false; }
            return true;
        }
        #endregion
        #region Load White List
        private bool LoadWhiteList()
        {
            _WhiteList.Clear();
            try
            {
                using (StreamReader reader = new StreamReader(location + "/" + white_list))
                {
                    AddLogLine("--- Load White List ---");
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        _WhiteList.Add(line.Trim());
                        AddLogLine(" * " + line.Trim());
                    }
                }
            }
            catch (IOException) { return false; }
            return true;
        }
        #endregion
        #region Load Black List
        private bool LoadBlackList()
        {
            _BlackList.Clear();
            try
            {
                using (StreamReader reader = new StreamReader(location + "/" + black_list))
                {
                    AddLogLine("--- Load Black List ---");
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        _BlackList.Add(line.Trim());
                        AddLogLine(" * " + line.Trim());
                    }
                }
            }
            catch (IOException) { return false; }
            return true;
        }
        #endregion
        #region Load Forbidden List
        private bool LoadForbiddenList()
        {
            _ForbiddenList.Clear();
            try
            {
                using (StreamReader reader = new StreamReader(location + "/" + forbidden_list))
                {
                    AddLogLine("--- Load Forbidden List ---");
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        _ForbiddenList.Add(line.Trim());
                        AddLogLine(" * " + line.Trim());
                    }
                }
            }
            catch (IOException) { return false; }
            return true;
        }
        #endregion
        #endregion
        #region Add
        public void AddOutputLine(string line)
        { _OutputLines.Add(Time() + " " + line); }
        public void AddLogLine(string line)
        { _LogLines.Add(Time() + " " + line); }
        public void AddAccount(string username, string password)
        {
            if (!_Accounts.ContainsKey(username))
            {
                _Accounts.Add(username, password);
                AddLogLine("[" + username + "] [" + password + "] was added to the accounts");
            }
            else { Debug.WriteLine("_Accouts does allready contain [" + username + "]"); }
        }
        #endregion
        #region Get
        public List<string> GetOutputLines()
        { return _OutputLines; }
        public List<string> GetLogLines()
        { return _LogLines; }
        public Dictionary<string, string> GetAccounts()
        { return _Accounts; }
        public Dictionary<string, string> GetRegistrations()
        { return _Registrations; }
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
                if (!forbidden_list.Equals("") && !word.Equals(""))
                    if (word.Contains(forbiddenWord))
                    { Debug.WriteLine("!!Forbidden!!"); AddLogLine("Busted!! --- [" + forbiddenWord + "[" + word + "]] forbidden"); return 1; }
            }
            return -10;
        }
        #endregion
        public static string Time()
        { return DateTime.Now.ToString("[yyyy-MM-dd hh:mm:ss]"); }
    }
}
