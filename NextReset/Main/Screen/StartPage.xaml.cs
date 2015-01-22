using Main.Screens.Game;
using Network;
using Network.Levels;
using Network.Singleton;
using Network.ThrowableException;
using Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main.Screens
{
    public partial class StartPage : Page
    {
        private DispatcherTimer _clearTimer;
        private Dictionary<string, ILevel> _LevelsToPlay = null;
        private Dictionary<string, bool> _LevelCompleted = null;
        public StartPage()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            #region Timer
            _clearTimer = new DispatcherTimer();
            _clearTimer.Interval = new TimeSpan(0, 0, 2);
            _clearTimer.Tick += Timer_Tick;
            #endregion

            _LevelCompleted = new Dictionary<string, bool>();
            LoadLevels();
            UpdateLevelButtons();
        }
        #region Update
        private void UpdateLevelButtons()
        {
            List<StackPanel> panels = new List<StackPanel>();
            int index = 0;
            try
            {
                foreach (string key in _LevelsToPlay.Keys)
                {
                    if (index % 5 == 0)
                    {
                        panels.Add(CreateSelectionStackpanel());
                    }
                    Button button = new Button();
                    button.Content = key;
                    button.Margin = new Thickness(10, 10, 10, 10);
                    button.Click += button_Click;
                    Debug.WriteLine("Level compleeted: " + _LevelCompleted[key]);
                    if (_LevelCompleted[key] == true)
                    { Debug.WriteLine("Level Completed: " + key); button.Background = AppSettings.ButtonLevelColor._Compleet; }
                    else
                    { Debug.WriteLine("Level Not Completed: " + key); button.Background = AppSettings.ButtonLevelColor._NotCompleet; }
                    panels[index / 5].Children.Add(button);
                    index++;
                }
                _Buttons_Stackpanel.Children.Clear();
                foreach (StackPanel sp in panels)
                { _Buttons_Stackpanel.Children.Add(sp); }
            }
            catch (NullReferenceException) { Debug.WriteLine("UpdateLevelButtons - Nullreference"); }
        }
        #endregion

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this._Feedback_tx.Text = AppSettings.Messages.Feedback.Loading;
            string[] array = sender.ToString().Split(' ');
            string level = array[array.Length - 2] + " " + array[array.Length - 1];
            GoToSingleGame(level);
        }
        private StackPanel CreateSelectionStackpanel()
        {
            StackPanel sp = new StackPanel();
            sp.Width = 100;
            return sp;
        }
        private void GoToSingleGame(string level)
        {
            if (!SetCorrectGameData(level))
            {
                this._Feedback_tx.Text = AppSettings.Messages.Feedback.UnableToLoad;
                _clearTimer.Start();
                return;
            }
            try
            {
                SingleGamePage page = new SingleGamePage();
                page.Return += new System.Windows.Navigation.ReturnEventHandler<String>(OnSingleGamePageReturned);
                this.NavigationService.Navigate(page);
            }
            catch (LevelUnstartubleException) { this._Feedback_tx.Text = AppSettings.Messages.Feedback.UnableToLoad; }
        }
        private void OnSingleGamePageReturned(object sender, ReturnEventArgs<String> e)
        {
            _Feedback_tx.Text = AppSettings.Messages.Feedback.Saving;
            string message = e.Result.Trim();
            string[] array = message.Split(',');
            SaveLevel(array[0].Trim(), array[1].Trim());
            Save_State();
            _Feedback_tx.Text = AppSettings.Messages.Feedback.None;
        }
        private bool SetCorrectGameData(string level)
        {
            SingleGameData data = SingleGameData.Get;
            ILevel ilevel = GetLevel(level);
            try
            {
                data.SetLandscape(ilevel.Landscape);
                data.SetAvailableMethods(ilevel.Methods);
                data.SetLevelName(ilevel.Name);
            }
            catch (NullReferenceException) { return false; }
            catch (ArgumentNullException) { return false; }
            return true;
        }
        private ILevel GetLevel(string level)
        {
            try
            { return _LevelsToPlay[level]; }
            catch (KeyNotFoundException)
            { return null; }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            this._Feedback_tx.Text = AppSettings.Messages.Feedback.None;
            this._clearTimer.Stop();
        }
        private void LoadLevels()
        {
            Debug.WriteLine("Load levels");
            GameController controller = new GameController();
            if (controller.HasLoaded())
            {
                _LevelsToPlay = controller.GetLevels(); // TODO: Load from leveldata.reset
            }
            foreach (string key in _LevelsToPlay.Keys)
            { _LevelCompleted.Add(key, false); }
            Load_State();
        }

        #region Save

        #region Save Level

        private void SaveLevel(string state, string level_name)
        {
            if (state.Equals("Compleet")) // TODO: Magic cookie
            {
                level_name.Trim();
                _LevelCompleted[level_name] = true;
                Debug.WriteLine("Level: [" + level_name + "] - Compleet");
                Debug.WriteLine("Print: " + _LevelCompleted[level_name]);
            } // TODO: Implementation
            UpdateLevelButtons();
        }

        #endregion

        #region Save State
        private void Save_State()
        {
            string location = AppSettings.SaveOrLoad._State_Location;
            string filename = AppSettings.SaveOrLoad._State_Filename;
            if (!Directory.Exists(location)) // TODO: Abstractie / Move
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + filename))
            {
                foreach (string key in _LevelCompleted.Keys)
                { Save(writer, key, _LevelCompleted[key]); }
            }
            // TODO: Implementation
        }
        #endregion

        #endregion

        #region Load
        #region Load State
        private void Load_State()
        {
            string location = AppSettings.SaveOrLoad._State_Location;
            string filename = AppSettings.SaveOrLoad._State_Filename;
            if (Directory.Exists(location))
            {
                if (File.Exists(location + "/" + filename))
                {
                    using (StreamReader reader = new StreamReader(location + "/" + filename))
                    {
                        while (reader.Peek() >= 0)
                        {
                            try
                            {
                                string[] array = Load(reader);
                                string level_name = array[0].Trim();
                                if (_LevelCompleted.ContainsKey(level_name))
                                { _LevelCompleted[level_name] = bool.Parse(array[1].Trim()); }
                                else { Debug.WriteLine("Does not contain key: [" + level_name + "]"); }
                            }
                            catch (FormatException) { Debug.WriteLine("Loading Format exception"); }
                            catch (IndexOutOfRangeException) { Debug.WriteLine("Loading index exception"); }
                        }
                    }
                }
            }
            // TODO: Implementation
        }
        #endregion
        #endregion
        #region Help Functions
        private void Save(StreamWriter writer, string levelname, bool state)
        { writer.WriteLine(levelname + ", " + state); }
        private string[] Load(StreamReader reader)
        { return reader.ReadLine().Split(','); }
        #endregion
    }
}
