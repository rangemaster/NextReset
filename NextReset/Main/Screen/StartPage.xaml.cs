using Main.Screens.Game;
using Network;
using Network.Levels;
using Network.Singleton;
using Network.ThrowableException;
using Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main.Screens
{
    public partial class StartPage : Page
    {
        private DispatcherTimer _clearTimer;
        private Dictionary<string, ILevel> _LevelsToPlay = null;
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
            LoadLevels();
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
                    panels[index / 5].Children.Add(button);
                    index++;
                }
                foreach (StackPanel sp in panels)
                { _Buttons_Stackpanel.Children.Add(sp); }
            }
            catch (NullReferenceException) { }
        }

        void button_Click(object sender, RoutedEventArgs e)
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
                this.NavigationService.Navigate(page);
            }
            catch (LevelUnstartubleException) { this._Feedback_tx.Text = AppSettings.Messages.Feedback.UnableToLoad; }
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
            this._Feedback_tx.Text = "";
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
        }
    }
}
