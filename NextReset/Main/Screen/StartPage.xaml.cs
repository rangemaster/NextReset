using Main.Screen.Admin;
using Main.Screen.Game;
using Network;
using Network.Levels;
using Network.Singleton;
using Network.ThrowableException;
using Settings;
using Settings.Singleton;
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
using Main.Screen.Tutorial;

namespace Main.Screens
{
    public partial class StartPage : Page
    {
        // TODO: Message First time?
        private DispatcherTimer _clearTimer;
        public StartPage()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            #region Asktimer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += AskFirstTime;
            timer.Start();
            #endregion
            #region Timer
            _clearTimer = new DispatcherTimer();
            _clearTimer.Interval = new TimeSpan(0, 0, 2);
            _clearTimer.Tick += Timer_Tick;
            #endregion

            LevelData data = LevelData.Get;
            LoadLevels();
            UpdateLevelButtons();
        }
        private void AskFirstTime(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Is this your first time?", "Question", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            { FirstTime(); }
            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Stop();
        }

        #region Update
        private void UpdateLevelButtons()
        {
            List<StackPanel> panels = new List<StackPanel>();
            int index = 0;
            try
            {
                foreach (string key in LevelData.Get.LevelsToPlay.Keys)
                {
                    if (index % 5 == 0)
                    {
                        panels.Add(CreateSelectionStackpanel());
                    }
                    Button button = new Button();
                    button.Content = key;
                    button.Margin = new Thickness(10, 10, 10, 10);
                    button.Click += button_Click;
                    if (LevelData.Get.LevelCompleted[key] == true)
                    { button.Background = AppSettings.ButtonLevelColor._Compleet; }
                    else
                    { button.Background = AppSettings.ButtonLevelColor._NotCompleet; }
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
            this._Feedback_tx.Text = AppSettings.Messages.UserFeedback.Loading;
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
                this._Feedback_tx.Text = AppSettings.Messages.UserFeedback.UnableToLoad;
                _clearTimer.Start();
                return;
            }
            try
            {
                SingleGamePage page = new SingleGamePage();
                page.Return += new System.Windows.Navigation.ReturnEventHandler<String>(OnSingleGamePageReturned);
                this.NavigationService.Navigate(page);
            }
            catch (LevelUnstartubleException) { this._Feedback_tx.Text = AppSettings.Messages.UserFeedback.UnableToLoad; }
            catch (IndexOutOfRangeException) { this._Feedback_tx.Text = AppSettings.Messages.UserFeedback.UnableToLoad; }
        }
        private void OnSingleGamePageReturned(object sender, ReturnEventArgs<String> e)
        {
            _Feedback_tx.Text = AppSettings.Messages.UserFeedback.Saving;
            string message = e.Result.Trim();
            string[] array = message.Split(',');
            LevelData.Get.SaveLevel(array[0].Trim(), array[1].Trim());
            LevelData.Get.Save_State();
            _Feedback_tx.Text = AppSettings.Messages.UserFeedback.None;
            UpdateLevelButtons();
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
            catch (FormatException) { return false; }
            return true;
        }
        private ILevel GetLevel(string level)
        {
            try
            { return LevelData.Get.LevelsToPlay[level]; }
            catch (KeyNotFoundException)
            { return null; }
        }

        #region Timer Tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            this._Feedback_tx.Text = AppSettings.Messages.UserFeedback.None;
            this._clearTimer.Stop();
        }
        #endregion

        #region Load Levels
        private void LoadLevels()
        {
            Debug.WriteLine("Load levels");
            GameController controller = new GameController();
            if (controller.HasLoaded())
            {
                LevelData.Get.SetLevelsToPlay(controller.GetLevels());
            }
            Dictionary<string, bool> completed = new Dictionary<string, bool>();
            foreach (string key in LevelData.Get.LevelsToPlay.Keys)
            { completed.Add(key, false); }
            LevelData.Get.SetLevelsCompleted(completed);
            LevelData.Get.Load_State();
        }
        #endregion

        #region Easter Eggs
        #region Easter Egg Top Rect
        #region Rectangles
        private int _rect_counter = 0;
        private void _Title_rect1_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (_rect_counter)
            {
                case 2: SetRect1(Colors.Beige); break;
                case 6: SetRect1(Colors.Green); break;
                default: _rect_counter = 0; return;
            }
            _rect_counter++;
        }
        private void _Title_rect2_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (_rect_counter)
            {
                case 0: SetRect2(Colors.Aqua); break;
                case 4: SetRect2(Colors.Aquamarine); break;
                default: _rect_counter = 0; return;
            }
            _rect_counter++;
        }
        private void _Title_rect3_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (_rect_counter)
            {
                case 1: SetRect3(Colors.Wheat); break;
                case 5: SetRect3(Colors.Gold); break;
                default: _rect_counter = 0; return;
            }
            _rect_counter++;
        }
        private void _Title_rect4_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (_rect_counter)
            {
                case 3: SetRect4(Colors.Gainsboro); break;
                case 7: ExecuteTopRect(); break;
                default: _rect_counter = 0; return;
            }
            _rect_counter++;
        }
        #endregion
        #region SetColor
        private void SetRect1(Color color) { _Title_rect1.Fill = new SolidColorBrush(color); }
        private void SetRect2(Color color) { _Title_rect2.Fill = new SolidColorBrush(color); }
        private void SetRect3(Color color) { _Title_rect3.Fill = new SolidColorBrush(color); }
        private void SetRect4(Color color) { _Title_rect4.Fill = new SolidColorBrush(color); }
        #endregion
        #region navigation handling
        private void ExecuteTopRect()
        {
            MessageBoxResult result = MessageBox.Show("You want to continue", "Admin", MessageBoxButton.YesNo); // TODO: Magic cookie
            if (result == MessageBoxResult.Yes)
            {
                AdminPage page = new AdminPage();
                page.Return += page_Return;
                this.NavigationService.Navigate(page);
            }
            else { ResetRectEasterEgg(); }
        }

        private void page_Return(object sender, ReturnEventArgs<string> e)
        {
            if (e.Result.Equals(AppSettings.Return.Succes))
            { MessageBox.Show(AppSettings.Messages.UserFeedback.AdminChanges); }
            else if (e.Result.Equals(AppSettings.Return.NoSucces))
            { }
            ResetRectEasterEgg();
            UpdateLevelButtons();
        }
        #endregion
        #region reset
        private void ResetRectEasterEgg()
        {
            _rect_counter = 0;
            SetRect1(Colors.White);
            SetRect2(Colors.White);
            SetRect3(Colors.White);
            SetRect4(Colors.White);
        }
        #endregion
        #endregion

        #endregion

        private void FirstTime()
        {
            Debug.WriteLine("First time");
            TutorialPage page = new TutorialPage();
            page.Return += Tutorial_Return;
            //AppSettings.PageSettings(page);
            this.NavigationService.Navigate(page);
        }
        private void Tutorial_Return(object sender, ReturnEventArgs<string> e)
        {
            string result = e.Result;
            if (result.Equals(AppSettings.Return.Succes))
            { }
            else if (result.Equals(AppSettings.Return.NoSucces))
            { }
            else { throw new NotSupportedException(); }
        }
    }
}
