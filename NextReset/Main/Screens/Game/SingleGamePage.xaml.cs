using Network.Commando;
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
        private List<Tuple<string, DelegateCommand>> _posibleCommands;
        private List<Tuple<string, DelegateCommand>> _commandos;
        private DispatcherTimer commandoTimer = null;
        private int commandoInteger = 0;
        private int _amountOfAvailableCommands = 0;
        public SingleGamePage()
        {
            InitializeComponent();
            _posibleCommands = new List<Tuple<string, DelegateCommand>>();
            _commandos = new List<Tuple<string, DelegateCommand>>();
            #region InitTimer
            commandoTimer = new DispatcherTimer();
            commandoTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            commandoTimer.Tick += commandoTimer_Tick;
            #endregion

            AddAvailable("Right", AddRight);
            AddAvailable("Left", AddLeft);
            AddAvailable("Up", AddUp);
            AddAvailable("Down", AddDown);

            for (int i = 0; i < 40; i++)
            {
                AddCommand("Right", MoveRight);
                AddCommand("Left", MoveLeft);
                AddCommand("Up", MoveUp);
                AddCommand("Down", MoveDown);
            }

            UpdatePosibleList();
            UpdateCommandoList();
        }

        private void AddCommand(string text, Action<object, RoutedEventArgs> method)
        { _commandos.Add(new Tuple<string, DelegateCommand>(text, new DelegateCommand(method))); UpdateCommandoList(); }
        private void AddAvailable(string text, Action<object, RoutedEventArgs> method)
        { _posibleCommands.Add(new Tuple<string, DelegateCommand>(text, new DelegateCommand(method))); UpdateCommandoList(); }
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
        { Debug.WriteLine("Method: Move Right();"); }
        private void MoveLeft(object sender, RoutedEventArgs e)
        { Debug.WriteLine("Method: Move Left();"); }
        private void MoveUp(object sender, RoutedEventArgs e)
        { Debug.WriteLine("Method: Move Up();"); }
        private void MoveDown(object sender, RoutedEventArgs e)
        { Debug.WriteLine("Method: Move Down();"); }
        private void Unknown(object sender, RoutedEventArgs e)
        { }

        #endregion

        #region Update
        private void UpdatePosibleList()
        {
            StackPanel sp = null;
            foreach (Tuple<string, DelegateCommand> action in _posibleCommands)
            {
                if (_amountOfAvailableCommands % 5 == 0)
                {
                    if (sp != null)
                        _AvailableCommandos_panel.Children.Add(sp);
                    sp = new StackPanel();
                    sp.Width = 100;
                }
                Debug.WriteLine(action.Item1);
                Button button = new Button();
                button.Content = action.Item1;
                button.Click += new RoutedEventHandler(action.Item2.Execute);
                sp.Children.Add(button);


                this._amountOfAvailableCommands++;
            }
            _AvailableCommandos_panel.Children.Add(sp);
        }
        private void UpdateCommandoList()
        {
            int index = 1;
            _Commando_List.Children.Clear();
            foreach (Tuple<string, DelegateCommand> commando in _commandos)
            {
                TextBlock text = new TextBlock();
                text.Text = "[" + (index > 1000 ? "0" : (index > 100 ? "00" : (index >= 10 ? "000" : (index < 10 ? "0000" : "")))) + index + "] " + commando.Item1;
                _Commando_List.Children.Add(text);
                index++;
            }
        }
        private void UpdateView()
        {

        }
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
            Tuple<string, DelegateCommand> action = null;
            try
            { action = _commandos[commandoInteger]; }
            catch (ArgumentOutOfRangeException) { StopCommandList(); return; }
            if (action != null)
            {
                Debug.WriteLine("Action: " + action.Item1);
                action.Item2.Execute(this, new RoutedEventArgs());
            }


            (_Commando_List.Children[commandoInteger] as TextBlock).Foreground = new SolidColorBrush(Colors.Red);
            commandoInteger++;
        }

        #endregion
        #region Stop
        private void StopCommandList()
        {
            this.commandoTimer.Stop();
            commandoInteger = 0;
            UpdateCommandoList();
            Debug.WriteLine("Timer Stopped");
        }
        #endregion
        #region Clear
        private void ClearCommandList()
        {
            _commandos.Clear();
            UpdateCommandoList();
        }
        #endregion
        #endregion
        #region Game Logic

        #endregion
    }
}
