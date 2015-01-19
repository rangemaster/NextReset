using Network;
using Network.Commando;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Main.Screens.Game
{
    /// <summary>
    /// Interaction logic for SingleGamePage.xaml
    /// </summary>
    public partial class SingleGamePage : Page
    {
        private List<Tuple<string, AvailableCommand>> _posibleCommands;
        private List<Tuple<string, AvailableCommand>> _commandos;
        private DispatcherTimer commandoTimer = null;
        private int commandoInteger = 0;
        private int _amountOfAvailableCommands = 0;
        private Point _yourPosition, _finnish;
        private int[][] _landscape;
        private bool[] _available_methods;
        public SingleGamePage()
        {
            InitializeComponent();
            _posibleCommands = new List<Tuple<string, AvailableCommand>>();
            _commandos = new List<Tuple<string, AvailableCommand>>();

            #region InitTimer
            commandoTimer = new DispatcherTimer();
            commandoTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            commandoTimer.Tick += commandoTimer_Tick;
            #endregion

            #region InitData
            _landscape = SingleGameData.Get.GetLandscape;
            _available_methods = SingleGameData.Get.GetAvailableMethods;
            SetDefaultYourPosition();

            #region InitField
            int rows = SingleGameData.Get.GetLandscape.Length;
            int colums = SingleGameData.Get.GetLandscape[0].Length;
            double width = this.Width - 100;
            double height = this.Height - 100;
            for (int i = 0; i < rows; i++)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                for (int j = 0; j < colums; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Margin = new Thickness(1, 1, 1, 1);
                    rect.Width = (width - rows) / rows;
                    rect.Height = (height - rows) / rows;
                    rect.Fill = new SolidColorBrush(Colors.Gray);
                    sp.Children.Add(rect);
                }
                _Field_Panel.Children.Add(sp);
            }
            SetDefaultFinnishPosition();

            #endregion

            #region InitAvailableFunctions
            AddAvailable("Right", AddRight, Use(0));
            AddAvailable("Left", AddLeft, Use(1));
            AddAvailable("Up", AddUp, Use(2));
            AddAvailable("Down", AddDown, Use(3));
            AddAvailable("Attack", Unknown, Use(4));
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            AddAvailable("", Unknown);
            #endregion
            #endregion
            try
            {
                UpdateAvailableList();
                UpdateCommandoList();
                UpdateView();
                CheckPosition();
            }
            catch (ReachedFinnishException) { ShowMessage("Finnish Reached", "Building Error"); }
        }
        private void SetDefaultYourPosition()
        { _yourPosition = new Point(0, 0); }
        private void SetDefaultFinnishPosition()
        {
            int lastRow = _Field_Panel.Children.Count - 1;
            int lastColumn = (_Field_Panel.Children[lastRow] as StackPanel).Children.Count - 1;
        }
        private bool Use(int index)
        {
            try
            { return _available_methods[index]; }
            catch (IndexOutOfRangeException)
            { return false; }
        }


        private void AddCommand(string text, Action<object, RoutedEventArgs> method)
        { _commandos.Add(new Tuple<string, AvailableCommand>(text, new AvailableCommand(method))); UpdateCommandoList(); }
        private void AddAvailable(string text, Action<object, RoutedEventArgs> method)
        { AddAvailable(text, method, false); }
        private void AddAvailable(string text, Action<object, RoutedEventArgs> method, bool available)
        { _posibleCommands.Add(new Tuple<string, AvailableCommand>(text, new AvailableCommand(method, available))); UpdateCommandoList(); }

        #region Functions

        private void AddRight(object sender, RoutedEventArgs e)
        { AddCommand("Right", MoveRight); }
        private void AddLeft(object sender, RoutedEventArgs e)
        { AddCommand("Left", MoveLeft); }
        private void AddUp(object sender, RoutedEventArgs e)
        { AddCommand("Up", MoveUp); }
        private void AddDown(object sender, RoutedEventArgs e)
        { AddCommand("Down", MoveDown); }
        private void MoveRight(object sender, RoutedEventArgs e)
        { Step(1, 0); }
        private void MoveLeft(object sender, RoutedEventArgs e)
        { Step(-1, 0); }
        private void MoveUp(object sender, RoutedEventArgs e)
        { Step(0, -1); }
        private void MoveDown(object sender, RoutedEventArgs e)
        { Step(0, 1); }
        private void Unknown(object sender, RoutedEventArgs e)
        { }

        #endregion

        #region Update
        private void UpdateAvailableList()
        {
            StackPanel sp = null;
            foreach (Tuple<string, AvailableCommand> action in _posibleCommands)
            {
                if (_amountOfAvailableCommands % 5 == 0)
                {
                    if (sp != null)
                        _AvailableCommandos_panel.Children.Add(sp);
                    sp = new StackPanel();
                    sp.Width = 100;
                    sp.Children.Add(GetAvailableCommandsTitle());
                    this._amountOfAvailableCommands++;
                }
                Debug.WriteLine(action.Item1);
                Button button = new Button();
                button.Content = action.Item1;
                button.Click += new RoutedEventHandler(action.Item2.Execute);
                button.IsEnabled = action.Item2.IsAvailable;
                sp.Children.Add(button);


                this._amountOfAvailableCommands++;
            }
            _AvailableCommandos_panel.Children.Add(sp);
        }
        private void UpdateCommandoList()
        {
            int index = 1;
            _Commando_List.Children.Clear();
            foreach (Tuple<string, AvailableCommand> commando in _commandos)
            {
                TextBlock text = new TextBlock();
                text.Foreground = new SolidColorBrush(Colors.Gray);
                text.Text = "[" + (index > 1000 ? "0" : (index > 100 ? "00" : (index >= 10 ? "000" : (index < 10 ? "0000" : "")))) + index + "] " + commando.Item1;
                _Commando_List.Children.Add(text);
                index++;
            }
        }
        private void UpdateView()
        {
            for (int y = 0; y < _Field_Panel.Children.Count; y++)
            {
                for (int x = 0; x < (_Field_Panel.Children[y] as StackPanel).Children.Count; x++)
                {
                    if (_yourPosition.X == x && _yourPosition.Y == y)
                    { ((_Field_Panel.Children[y] as StackPanel).Children[x] as Rectangle).Fill = AppSettings.Color._You; }
                    else
                    { ((_Field_Panel.Children[y] as StackPanel).Children[x] as Rectangle).Fill = AppSettings.Color._Path; }
                }
            }

            ((_Field_Panel.Children[(int)_finnish.Y] as StackPanel).Children[(int)_finnish.X] as Rectangle).Fill = AppSettings.Color._Finnish;
        }

        #region Help functions
        private TextBlock GetAvailableCommandsTitle()
        {
            TextBlock tb = new TextBlock();
            tb.Text = GetTitle();
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.Foreground = new SolidColorBrush(Colors.LightGray);
            return tb;
        }
        private string GetTitle()
        {
            switch (_amountOfAvailableCommands)
            {
                case 0: return "Movement";
                case 5: return "Normal Skills";
                default: return "Unknown";
            }
        }
        #endregion
        #endregion

        #region Buttons
        private void Start_bn(object sender, RoutedEventArgs e)
        { StartCommandoList(); }
        private void Stop_bn(object sender, RoutedEventArgs e)
        { StopCommandList(); }
        private void Clear_bn(object sender, RoutedEventArgs e)
        { ClearCommandList(); }
        #endregion
        #region Button Functions
        #region Start
        private void StartCommandoList()
        {
            this.commandoTimer.Start();
            Debug.WriteLine("Timer Started");
        }
        private void commandoTimer_Tick(object sender, EventArgs e)
        {
            Tuple<string, AvailableCommand> action = null;
            try
            { action = _commandos[commandoInteger]; }
            catch (ArgumentOutOfRangeException) { StopCommandList(); return; }
            if (action != null)
            {
                try
                {
                    action.Item2.Execute(this, new RoutedEventArgs());
                    UpdateView();
                    CheckPosition();
                    (_Commando_List.Children[commandoInteger] as TextBlock).Foreground = new SolidColorBrush(Colors.Red);
                    commandoInteger++;
                }
                catch (OutOfTheGameException) { StopCommandList(); ErrorMessage("Out Of The Game Error"); }
                catch (ReachedFinnishException) { StopCommandList(); CongratzMessage("You have reached the finnish"); }
            }
        }
        private void CheckPosition()
        {
            int columnCount = _Field_Panel.Children.Count;
            int rowCount = (_Field_Panel.Children[0] as StackPanel).Children.Count;
            if (_yourPosition.X < 0 || _yourPosition.X >= rowCount)
            { throw new OutOfTheGameException(); }
            if (_yourPosition.Y < 0 || _yourPosition.Y >= columnCount)
            { throw new OutOfTheGameException(); }
            if (_yourPosition.X == _finnish.X && _yourPosition.Y == _finnish.Y)
            { throw new ReachedFinnishException(); }
        }
        private void ErrorMessage(string message)
        { ShowMessage(message, "Error"); }
        private void CongratzMessage(string message)
        { ShowMessage(message, "Congratulations!"); }
        private void ShowMessage(string message, string header)
        { MessageBoxResult result = MessageBox.Show(message, header); }
        #endregion
        #region Stop
        private void StopCommandList()
        {
            this.commandoTimer.Stop();
            commandoInteger = 0;
            SetDefaultYourPosition();
            UpdateCommandoList();
            UpdateView();
            Debug.WriteLine("Timer Stopped");
        }
        #endregion
        #region Clear
        private void ClearCommandList()
        {
            _commandos.Clear();
            SetDefaultYourPosition();
            UpdateCommandoList();
            UpdateView();
        }
        #endregion
        #endregion

        #region Game Logic

        #region Step
        private void Step(int x, int y)
        { _yourPosition = new Point(_yourPosition.X + x, _yourPosition.Y + y); }
        #endregion

        #endregion
    }
}
