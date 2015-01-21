using Network.Levels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Levels
{
    public class ReaderLevel : ILevel
    {
        private int[] _Methods;
        private int[][] _Landscape;
        private List<int[]> _LandscapeRows = new List<int[]>();
        private string _Name;
        internal override int[] SetAvailableMethods()
        { return _Methods; }
        internal override int[][] SetLandscape()
        { return _Landscape; }
        internal override string SetNameOfClass()
        { return _Name; }
        public int[] Methods
        {
            set
            {
                this._Methods = value;
            }
        }
        public int[] NextLandscapeRow
        {
            set
            {
                this._LandscapeRows.Add(value);
            }
        }
        public string Name
        {
            set
            {
                this._Name = value;
            }
        }
        public void ConvertLandscapeRowToLandscape()
        {
            _Landscape = new int[_LandscapeRows.Count][];
            for (int i = 0; i < _LandscapeRows.Count; i++)
            { _Landscape[i] = _LandscapeRows[i]; }
        }
    }
}
