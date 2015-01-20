using Main.Screens.Game;
using Network.Levels;
using Network.Singleton;
using Network.ThrowableException;
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
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private DispatcherTimer _clearTimer;
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
            List<StackPanel> panels = new List<StackPanel>();
            for (int i = 0; i < 20; i++)
            {
                if (i % 5 == 0)
                {
                    panels.Add(CreateSelectionStackpanel());
                }
                Button button = new Button();
                button.Content = "Level " + (i + 1);
                button.Margin = new Thickness(10, 10, 10, 10);
                button.Click += button_Click;
                panels[i / 5].Children.Add(button);
            }
            foreach (StackPanel sp in panels)
            { _Buttons_Stackpanel.Children.Add(sp); }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            this._Feedback_tx.Text = "Loading game...";
            string button_name = sender.ToString();
            string[] array = button_name.Split(' ');
            int level = int.Parse(array[array.Length - 1]);
            GoToSingleGame(level);
        }
        private StackPanel CreateSelectionStackpanel()
        {
            StackPanel sp = new StackPanel();
            sp.Width = 100;
            return sp;
        }
        private void GoToSingleGame(int level)
        {
            if (!SetCorrectGameData(level))
            {
                this._Feedback_tx.Text = "Could not load level!";
                _clearTimer.Start();
                return;
            }
            try
            {
                SingleGamePage page = new SingleGamePage();
                this.NavigationService.Navigate(page);
            }
            catch (LevelUnstartubleException) { this._Feedback_tx.Text = "Could not load level!"; }
        }
        private bool SetCorrectGameData(int level)
        {
            Debug.WriteLine("Settings GameData: Level " + level);
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
        private ILevel GetLevel(int level)
        {
            switch (level)
            {
                case 1: return new Level1();
                case 2: return new Level2();
                case 3: return new Level3();
                case 4: return new Level4();
                case 5: return new Level5();
            }
            return null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            this._Feedback_tx.Text = "";
            this._clearTimer.Stop();
        }
    }
}
