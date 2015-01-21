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
    public partial class SingleGamePage : Page
    {
        // TODO END: Add Easter Eggs
        private List<Tuple<string, AvailableCommand>> _AvailableCommands;
        private List<Tuple<string, AvailableCommand>> _commandos;
        private DispatcherTimer commandoTimer = null;
        private int _commandoInteger = 0;
        private int _amountOfAvailableCommands = 0;
        private Point _yourPosition = new Point(-2, -2), _finnish = new Point(-2, -2);
        private int[][] _landscape;
        private int[] _available_methods;
        public SingleGamePage()
        {
            InitializeComponent();
            _AvailableCommands = new List<Tuple<string, AvailableCommand>>();
            _commandos = new List<Tuple<string, AvailableCommand>>();

            #region InitTimer
            commandoTimer = new DispatcherTimer();
            commandoTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            commandoTimer.Tick += commandoTimer_Tick;
            #endregion
            Init();
        }

        #region Init

        private void Init()
        {
            InitData();
            InitField();
            InitAvailableFunctions();
            InitUpdate();
        }

        #region InitData
        private void InitData()
        {
            #region Landscape
            _landscape = null;
            int ylength = SingleGameData.Get.GetLandscape.Length;
            int xlength = SingleGameData.Get.GetLandscape[0].Length;
            _landscape = new int[ylength][];
            for (int y = 0; y < SingleGameData.Get.GetLandscape.Length; y++)
            {
                int[] row = new int[xlength];
                for (int x = 0; x < SingleGameData.Get.GetLandscape[y].Length; x++)
                    row[x] = SingleGameData.Get.GetLandscape[y][x];
                _landscape[y] = row;
            }

            #endregion
            #region methods
            _available_methods = null;
            _available_methods = SingleGameData.Get.GetAvailableMethods;
            #endregion
        }
        #endregion

        #region InitField
        private void InitField()
        {
            _Field_Panel.Children.Clear();
            int rows = _landscape.Length;
            int columns = _landscape[0].Length;
            double width = this.Width - 100;
            double height = this.Height - 100;
            for (int i = 0; i < rows; i++)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                for (int j = 0; j < columns; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Margin = new Thickness(1, 1, 1, 1);
                    rect.Width = (width - columns) / columns;
                    rect.Height = (height - rows) / rows;
                    sp.Children.Add(rect);
                }
                _Field_Panel.Children.Add(sp);
            }
            SetDefaultPositions();
        }
        #endregion

        #region InitAvailableFunctions
        private void InitAvailableFunctions()
        {
            _AvailableCommands.Clear();
            AddAvailable("Right", AddRight, Use(0));
            AddAvailable("Left", AddLeft, Use(1));
            AddAvailable("Up", AddUp, Use(2));
            AddAvailable("Down", AddDown, Use(3));
            AddAvailable("Attack", AddAttack, Use(4));
            AddAvailable("Bomb", AddBomb, Use(5));
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
            UpdateCommandoList();
        }
        #endregion

        #region InitUpdate
        private void InitUpdate()
        {
            try
            {
                UpdateAvailableList();
                UpdateCommandoList();
                UpdateView();
                CheckPosition();
            }
            catch (ReachedFinnishException) { ShowMessage("Finnish Reached", "Building Error"); throw new LevelUnstartubleException(); }
            catch (OutOfTheGameException) { ShowMessage("Out of the game area", "Building Error"); throw new LevelUnstartubleException(); }
            catch (LocationUnknownException) { ShowMessage("Your location or finnish location Unknown", "Building Error"); throw new LevelUnstartubleException(); }
        }
        #endregion
        private void SetDefaultPositions()
        {
            Debug.WriteLine("Set Default Positions");
            for (int y = 0; y < _landscape.Length; y++)
                for (int x = 0; x < _landscape[y].Length; x++)
                    if (_landscape[y][x] == AppSettings.Field._You)
                    { Debug.WriteLine("xy(" + x + ", " + y + ") You"); _yourPosition = new Point(x, y); }
                    else if (_landscape[y][x] == AppSettings.Field._Finnish)
                    { Debug.WriteLine("xy(" + x + ", " + y + ") Finnish"); _finnish = new Point(x, y); }
        }
        private int Use(int index)
        {
            try
            { return _available_methods[index]; }
            catch (IndexOutOfRangeException)
            { return 0; }
        }

        #endregion Init

        #region Functions
        #region AddFunctions
        private void AddCommand(string text, Action<object, RoutedEventArgs> method)
        { _commandos.Add(new Tuple<string, AvailableCommand>(text, new AvailableCommand(method))); UpdateCommandoList(); }
        private void AddAvailable(string text, Action<object, RoutedEventArgs> method)
        { AddAvailable(text, method, 0); }
        private void AddAvailable(string text, Action<object, RoutedEventArgs> method, int available)
        { _AvailableCommands.Add(new Tuple<string, AvailableCommand>(text, new AvailableCommand(method, available))); }
        private void AddRight(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[0].Item2.Decrees()) { AddCommand("Right", MoveRight); } UpdateAvailableList(); }
        private void AddLeft(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[1].Item2.Decrees()) { AddCommand("Left", MoveLeft); } UpdateAvailableList(); }
        private void AddUp(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[2].Item2.Decrees()) { AddCommand("Up", MoveUp); } UpdateAvailableList(); }
        private void AddDown(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[3].Item2.Decrees()) { AddCommand("Down", MoveDown); } UpdateAvailableList(); }
        private void AddAttack(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[4].Item2.Decrees()) { AddCommand("Attack", Attack); } UpdateAvailableList(); }
        private void AddBomb(object sender, RoutedEventArgs e)
        { if (_AvailableCommands[5].Item2.Decrees()) { AddCommand("Bomb", Bomb); } UpdateAvailableList(); }
        #endregion

        #region ActionFunctions
        private void MoveRight(object sender, RoutedEventArgs e)
        { Step(1, 0); }
        private void MoveLeft(object sender, RoutedEventArgs e)
        { Step(-1, 0); }
        private void MoveUp(object sender, RoutedEventArgs e)
        { Step(0, -1); }
        private void MoveDown(object sender, RoutedEventArgs e)
        { Step(0, 1); }
        private void Attack(object sender, RoutedEventArgs e)
        { AttackLogic(); }
        private void Bomb(object sender, RoutedEventArgs e)
        { BombLogic(); }
        private void Unknown(object sender, RoutedEventArgs e)
        { }
        #endregion

        #endregion

        #region Update
        private void UpdateAvailableList()
        {
            _amountOfAvailableCommands = 0;
            _AvailableCommandos_panel.Children.Clear();
            StackPanel sp = null;
            foreach (Tuple<string, AvailableCommand> action in _AvailableCommands)
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
                //Debug.WriteLine(action.Item1);
                Button button = new Button();
                button.Content = action.Item1 + " " + action.Item2.GetAvailable + "x";
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
                    ((_Field_Panel.Children[y] as StackPanel).Children[x] as Rectangle).Fill = GetColor(x, y);
                }
            }
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
        private Brush GetColor(int x, int y)
        {
            switch (_landscape[y][x])
            {
                case AppSettings.Field._Rock: return AppSettings.Color._Rock;
                case AppSettings.Field._Wall: return AppSettings.Color._Wall;
                case AppSettings.Field._Enemy1: return AppSettings.Color._Enemy1;
                case AppSettings.Field._You: return AppSettings.Color._You;
                case AppSettings.Field._Finnish: return AppSettings.Color._Finnish;
                case AppSettings.Field._Path: return AppSettings.Color._Path;
                default: return AppSettings.Color._Unknown;
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
        private void Reset_bn(object sender, RoutedEventArgs e)
        { ResetAll(); }
        #endregion
        #region Button Functions
        #region Start
        private void StartCommandoList()
        {
            ResetField();
            this.commandoTimer.Start();
            Debug.WriteLine("Timer Started");
        }
        private void commandoTimer_Tick(object sender, EventArgs e)
        {
            Tuple<string, AvailableCommand> action = null;
            try
            { action = _commandos[_commandoInteger]; }
            catch (ArgumentOutOfRangeException) { StopCommandList(); return; }
            if (action != null)
            {
                try
                {
                    action.Item2.Execute(this, new RoutedEventArgs());
                    (_Commando_List.Children[_commandoInteger] as TextBlock).Foreground = new SolidColorBrush(Colors.Red);
                    _commandoInteger++;
                    UpdateView();
                    CheckPosition();
                }
                catch (OutOfTheGameException) { StopCommandList(); ErrorMessage("Out Of The Game Error"); }
                catch (ReachedFinnishException)
                {
                    if (_commandos.Count == _commandoInteger)
                    {
                        StopCommandList();
                        Debug.WriteLine("You have reached the finnish");
                        CongratzMessage("You have reached the finnish");
                    }
                    else Debug.WriteLine("You have reached your finnish");
                }
            }
        }
        private void CheckPosition()
        {
            if (_yourPosition.X == -2 || _yourPosition.Y == -2)
                throw new LocationUnknownException();
            if (_finnish.X == -2 || _finnish.Y == -2)
                throw new LocationUnknownException();
            int columnCount = _Field_Panel.Children.Count;
            int rowCount = (_Field_Panel.Children[0] as StackPanel).Children.Count;
            if (_yourPosition.X == _finnish.X && _yourPosition.Y == _finnish.Y)
            { throw new ReachedFinnishException(); }
            else
            {
                if (_yourPosition.X < 0 || _yourPosition.X >= rowCount)
                { throw new OutOfTheGameException(); }
                if (_yourPosition.Y < 0 || _yourPosition.Y >= columnCount)
                { throw new OutOfTheGameException(); }
            }
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
            _commandoInteger = 0;
            DefaultSettingsAfterRun();
            Debug.WriteLine("Timer Stopped");
        }
        #endregion
        #region Clear
        private void ClearCommandList()
        {
            StopCommandList();
            _commandos.Clear();
            InitAvailableFunctions();
            UpdateAvailableList();
        }
        #endregion
        #region Reset
        private void ResetAll()
        {
            StopCommandList();
            ClearCommandList();
            Init();
        }
        private void ResetField()
        {
            InitData();
            InitField();
            UpdateCommandoList();
            UpdateView();
        }
        #endregion
        private void DefaultSettingsAfterRun()
        {
            InitData();
            InitField();
            InitUpdate();
        }
        #endregion

        #region Game Logic

        #region Step
        private void Step(int xas, int yas)
        {
            try
            {
                int x = (int)_yourPosition.X;
                int y = (int)_yourPosition.Y;
                int nextX = x + xas;
                int nextY = y + yas;
                if (IsNextStepClear(nextX, nextY))
                {
                    _landscape[y][x] = AppSettings.Field._Path;
                    _landscape[nextY][nextX] = AppSettings.Field._You;
                    _yourPosition = new Point(x + xas, y + yas);
                }
            }
            catch (IndexOutOfRangeException) { throw new OutOfTheGameException(); }
        }
        #endregion

        #region Attack
        private void AttackLogic()
        {

        }
        #endregion

        #region Bomb
        private void BombLogic()
        {
            int x = (int)(_yourPosition.X);
            int y = (int)(_yourPosition.Y);
            if (_commandoInteger > 0)
            {
                int stepsback = 0;
                string previousDirection = null;
                bool found = false;
                while (!found)
                {
                    previousDirection = GetPreviousCommand(_commandoInteger - stepsback);
                    if (EqualsCommand(previousDirection, AppSettings.Directons._All))
                        found = true;
                    stepsback++;
                }
                if (previousDirection.Equals(AppSettings.Directons._Right))
                { BombExplosion(x + 1, y + 0); }
                else if (previousDirection.Equals(AppSettings.Directons._Left))
                { BombExplosion(x - 1, y + 0); }
                else if (previousDirection.Equals(AppSettings.Directons._Up))
                { BombExplosion(x + 0, y - 1); }
                else if (previousDirection.Equals(AppSettings.Directons._Down))
                { BombExplosion(x + 0, y + 1); }
            }
        }
        private void BombExplosion(int x, int y)
        { if (IsExplodable(x, y)) { _landscape[y][x] = AppSettings.Field._Path; } }
        #endregion

        #region Help Logic Functions

        #region Equals Command
        private bool EqualsCommand(string task, params string[] commandos)
        {
            foreach (string command in commandos)
                if (task.Equals(command))
                    return true;
            return false;
        }
        #endregion

        #region PreviousCommand
        private string GetPreviousCommand(int currentIndex)
        {
            if (currentIndex > 0)
            { return _commandos[currentIndex - 1].Item1; }
            return null;
        }
        #endregion

        #region Path Is Clear
        private bool IsNextStepClear(int nextX, int nextY)
        {
            switch (_landscape[nextY][nextX])
            {
                case AppSettings.Field._Rock: return false;
                case AppSettings.Field._Wall: return false;
                case AppSettings.Field._Enemy1: return false;
                case AppSettings.Field._Path: return true;
            }
            return true;
        }
        #endregion

        #region Attackable
        private bool IsAttackable(int x, int y)
        {
            switch (_landscape[y][x])
            {
                case AppSettings.Field._Enemy1: return true;
            }
            return false;
        }
        #endregion

        #region Explodable
        private bool IsExplodable(int x, int y)
        {
            switch (_landscape[y][x])
            {
                case AppSettings.Field._Rock: return true;
                case AppSettings.Field._Wall: return false;
                case AppSettings.Field._Finnish: return false;
                case AppSettings.Field._You: return false;
                case AppSettings.Field._Enemy1: return true;
            }
            return false;
        }
        #endregion

        #endregion

        #endregion
    }
}