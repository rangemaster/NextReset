using Network;
using Network.Levels;
using Settings.Levels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class GameController
    {
        private string fileLocation = "GameData"; // TODO: Magic cookie
        private string filename = @"Gamedata.reset"; // TODO: Magic cookie
        private Dictionary<string, ILevel> _levels = null;
        private bool readingPath = false;
        public GameController()
        { }

        #region HasLoaded
        public bool HasLoaded()
        {
            if (_levels != null)
                return true;
            if (Directory.Exists(fileLocation))
            {
                if (File.Exists(fileLocation + "/" + filename))
                {
                    List<ILevel> levels = new List<ILevel>();
                    using (StreamReader reader = new StreamReader(fileLocation + "/" + filename))
                    {
                        ReaderLevel level = null;
                        while (reader.Peek() >= 0)
                        {
                            try
                            {
                                string line = reader.ReadLine();
                                if (!line.StartsWith(AppSettings.Seperate.Exclude))
                                {
                                    if (level == null)
                                        level = new ReaderLevel();
                                    if (!line.Equals(AppSettings.Seperate.End))
                                    { SeperateLoadedLine(line, level); }
                                    else
                                    {
                                        level.Init();
                                        levels.Add(level);
                                        level = null;
                                    }

                                }
                                else
                                {
                                    Debug.WriteLine("Reading line: " + line);
                                }
                            }
                            catch (FormatException) { }
                        }
                        _levels = new Dictionary<string, ILevel>();
                        foreach (ILevel lvl in levels)
                        {
                            try
                            {
                                string name = lvl.Name.Trim();
                                _levels.Add(name, lvl);
                            }
                            catch (NullReferenceException) { }
                        }
                        return true;
                    }
                }
                else
                { CreateExample(); }
            }
            else { CreateDirectory(); }
            return false;
        }
        #endregion

        #region Line Editing

        private void SeperateLoadedLine(String line, ReaderLevel Level)
        {
            string ln = AppSettings.Seperate.LvlName;
            string mn = AppSettings.Seperate.MethodNames;
            if (line.StartsWith(mn))
            {
                String methods = line.Substring(mn.Length, line.Length - mn.Length);
                int[] amountsOfMethod = MethodsToAmount(methods);
                Level.Methods = amountsOfMethod;
            }
            if (line.StartsWith(AppSettings.Seperate.PathEnd))
            {
                readingPath = false;
                Level.ConvertLandscapeRowToLandscape();
            }
            else if (readingPath)
            {
                string[] array = line.Split(AppSettings.Seperate.PathSplit);
                int[] landscaperow = new int[array.Length];
                for (int i = 0; i < landscaperow.Length; i++)
                {
                    landscaperow[i] = int.Parse(array[i].Trim());
                }

                Level.NextLandscapeRow = landscaperow;
            }
            else if (line.StartsWith(ln))
            {
                string name = line.Substring((ln).Length, line.Length - ln.Length);
                name.Trim();
                Level.Name = name;
            }
            else if (line.StartsWith(AppSettings.Seperate.PathStart))
            { readingPath = true; }
        }
        private int[] MethodsToAmount(String methods)
        {
            int[] amounts = new int[AppSettings._MaximumAmountOfAvailableMethods];
            String[] array = methods.Split(AppSettings.Seperate.MethodSplit);
            for (int i = 0; i < array.Length; i++)
            {
                string method = array[i].Trim();
                if (method.Equals(AppSettings.Directons._Right))
                { amounts[0] = int.Parse(array[i + 1]); }
                else if (method.Equals(AppSettings.Directons._Left))
                { amounts[1] = int.Parse(array[i + 1]); }
                else if (method.Equals(AppSettings.Directons._Up))
                { amounts[2] = int.Parse(array[i + 1]); }
                else if (method.Equals(AppSettings.Directons._Down))
                { amounts[3] = int.Parse(array[i + 1]); }
                else if (method.Equals(AppSettings.Normal._Attack))
                { amounts[4] = int.Parse(array[i + 1]); }
                else if (method.Equals(AppSettings.Normal._Bomb))
                { amounts[5] = int.Parse(array[i + 1]); }
                else { Debug.WriteLine("else, " + method); i--; }
                i++;
            }
            return amounts;
        }

        #endregion

        #region Get

        public Dictionary<string, ILevel> GetLevels()
        { return _levels; }

        #endregion

        #region Create

        private void CreateDirectory()
        {
            Directory.CreateDirectory(fileLocation);
            CreateExample();
        }
        private void CreateExample()
        {
            using (StreamWriter write = new StreamWriter(fileLocation + "/" + filename.Substring(0, filename.Length - 5) + ".txt", false))
            {
                write.WriteLine(AppSettings.Seperate.Exclude + " Replace me for '" + filename + "'");
                write.WriteLine(AppSettings.Seperate.Exclude);
                write.WriteLine(AppSettings.Seperate.Exclude + " " + AppSettings.Seperate.LvlName + " ??");
                write.WriteLine(AppSettings.Seperate.Exclude + " " + AppSettings.Seperate.PathStart);
                write.WriteLine(AppSettings.Seperate.Exclude + " (" + AppSettings.Field._Path + " = path, " + AppSettings.Field._Rock + " = Rock, " + AppSettings.Field._Wall + " = Wall)");
                write.WriteLine(AppSettings.Seperate.Exclude + " (" + AppSettings.Field._You + " = you, " + AppSettings.Field._Finnish + " = Finnish)");
                write.WriteLine(AppSettings.Seperate.Exclude + " 98, 0, 0, 1, 99");
                write.WriteLine(AppSettings.Seperate.Exclude + " 1, 0, 0, 1, 0");
                write.WriteLine(AppSettings.Seperate.Exclude + " " + AppSettings.Seperate.PathEnd);
                write.WriteLine(AppSettings.Seperate.Exclude + " " + AppSettings.Seperate.MethodNames + " Right, 5, Left, 5, Up, 2");
                write.WriteLine(AppSettings.Seperate.Exclude + " " + AppSettings.Seperate.End);
            }
            if (!File.Exists(fileLocation + "/" + filename))
            {
                using (StreamWriter writer = new StreamWriter(fileLocation + "/" + filename))
                {
                    writer.WriteLine("Level name: level 1");
                    writer.WriteLine("Path [");
                    writer.WriteLine("1, 1, 1, 1, 1, 1");
                    writer.WriteLine("1, 98, 0, 0, 99, 1");
                    writer.WriteLine("1, 1, 1, 1, 1, 1");
                    writer.WriteLine("]");
                    writer.WriteLine("Methods: Right, 3");
                    writer.WriteLine("END");
                }
            }
        }
        #endregion
    }
}