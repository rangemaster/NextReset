using Network;
using Settings.Singleton;
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

namespace Main.Screen.Admin
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : PageFunction<String>
    {
        private int _amountOfChanges = 0;
        public AdminPage()
        {
            InitializeComponent();
            this._Feedback_tx.Text = "";
            this._Input_tx.Text = AppSettings.Command.CompleetAll;
            this.ShowsNavigationUI = false;
        }

        private void _Command_bn_Click(object sender, RoutedEventArgs e)
        {
            string command = _Input_tx.Text.ToLower();
            if (command.Contains(AppSettings.Command.UncompleetAll))
            { UnCompleetAll(); }
            else if (command.Contains(AppSettings.Command.CompleetAll))
            { CompleetAll(); }
            else { Feedback(AppSettings.Command.Unknown); _amountOfChanges--; }
            _amountOfChanges++;
        }
        private void _Done_bn_Click(object sender, RoutedEventArgs e)
        {
            if (_amountOfChanges > 0)
                OnReturn(new ReturnEventArgs<string>(AppSettings.Return.Succes));
            else
                OnReturn(new ReturnEventArgs<string>(AppSettings.Return.NoSucces));
        }
        private void Feedback(string feedback)
        { this._Feedback_tx.Text += "[" + TimeNow() + "]" + feedback + "\n"; }
        private string TimeNow()
        { return DateTime.Now.ToString("yyyy-MM-dd - hh:mm:ss"); }

        #region Command Functions
        private void CompleetAll()
        { SetAllLevels(true); }
        private void UnCompleetAll()
        { SetAllLevels(false); }
        #endregion

        #region Functions

        private void SetAllLevels(bool result)
        {
            var list = LevelData.Get.LevelCompleted;
            List<string> keys = new List<string>();
            foreach (string key in list.Keys)
            { keys.Add(key.Trim()); }
            foreach (string key in keys)
            {
                LevelData.Get.SetLevelCompleted(key, result);
                Feedback("Level [" + key + "] set to: " + (result ? "completed" : "uncompleted"));
            }
            LevelData.Get.Save_State();
            Feedback("All levels are Saved");
        }
        #endregion
    }
}
