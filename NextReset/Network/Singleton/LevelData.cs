using Network;
using Network.Levels;
using Settings.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Settings.Singleton
{
    public class LevelData
    {
        private static LevelData _Instance = null;
        public Dictionary<string, ILevel> LevelsToPlay { get; private set; }
        public Dictionary<string, bool> LevelCompleted { get; private set; }
        public int TimerSpeed { get; set; }
        private LevelData()
        {
            TimerSpeed = AppSettings.Timer._Slow;
        }
        public static LevelData Get
        {
            get
            {
                if (_Instance == null)
                    _Instance = new LevelData();
                return _Instance;
            }
        }

        #region Set
        public void SetLevelsToPlay(Dictionary<string, ILevel> levels)
        { SetLevelsToPlay(levels, true); }
        public void SetLevelsToPlay(Dictionary<string, ILevel> levels, bool show_dialog)
        {
            Debug.WriteLine("Set Levels To Play");
            if (this.LevelsToPlay != null && show_dialog)
            { MessageBox.Show(AppSettings.Messages.Errors.SetLevelsToPlay); }
            this.LevelsToPlay = levels;
        }
        public void SetLevelsCompleted(Dictionary<string, bool> completed)
        { SetLevelsCompleted(completed, true); }
        public void SetLevelsCompleted(Dictionary<string, bool> completed, bool show_dialog)
        {
            Debug.WriteLine("Set Levels Completed");
            if (this.LevelCompleted != null && show_dialog)
            { MessageBox.Show(AppSettings.Messages.Errors.SetLevelsCompleted); }
            this.LevelCompleted = completed;
        }
        public void SetLevelToPlay(string key, ILevel level)
        { LevelsToPlay[key] = level; }
        public void SetLevelCompleted(string key, bool result)
        { LevelCompleted[key] = result; }
        #endregion

        #region Save Level

        public void SaveLevel(string state, string level_name)
        {
            if (state.Equals(AppSettings.Command.Compleet))
            {
                level_name.Trim();
                LevelCompleted[level_name] = true;
            }
        }

        #endregion

        #region Save State
        public void Save_State()
        {
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
            string filename = AppSettings.SaveOrLoad._State_Filename;
            if (!Directory.Exists(location)) // TODO: Abstractie / Move
            { Directory.CreateDirectory(location); }
            using (StreamWriter writer = new StreamWriter(location + "/" + filename))
            {
                foreach (string key in LevelCompleted.Keys)
                { Save(writer, key, LevelCompleted[key]); }
            }
        }
        #endregion

        #region Load State
        public void Load_State()
        {
            string location = AppSettings.SaveOrLoad._Level_Source_Location;
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
                                if (LevelCompleted.ContainsKey(level_name))
                                { LevelCompleted[level_name] = bool.Parse(array[1].Trim()); }
                                else { Debug.WriteLine("Does not contain key: [" + level_name + "]"); }
                            }
                            catch (FormatException) { Debug.WriteLine("Loading Format exception"); }
                            catch (IndexOutOfRangeException) { Debug.WriteLine("Loading index exception"); }
                        }
                    }
                }
            }
        }
        #endregion

        #region Help Functions
        private void Save(StreamWriter writer, string levelname, bool state)
        { writer.WriteLine(levelname + ", " + state); }
        private string[] Load(StreamReader reader)
        { return reader.ReadLine().Split(','); }
        #endregion

    }
}
