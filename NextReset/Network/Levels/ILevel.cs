using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Levels
{
    public abstract class ILevel
    {
        private int[][] _landscape;
        private bool[] _availablemethods;
        private string _name;
        public ILevel()
        {
            _landscape = SetLandscape();
            _availablemethods = SetAvailableMethods();
            _name = SetNameOfClass();
        }
        internal abstract int[][] SetLandscape();
        internal abstract bool[] SetAvailableMethods();
        internal abstract String SetNameOfClass();
        public int[][] Landscape { get { return _landscape; } }
        public bool[] Methods { get { return _availablemethods; } }
        public string Name { get { return _name; } }
        public override string ToString()
        {
            return "Class: " + _name;
        }
        internal bool[] method_help(bool right, bool left, bool up, bool down)
        {
            return new bool[] { right, left, up, down };
        }
        internal bool[] method_help_below10()
        { return new bool[] { true, true, true, true }; }
        internal bool[] method_help_below20()
        { return new bool[] { true, true, true, true, true, true, true, true }; }
        internal int[] landscape_row_help(params int[] x_as)
        { return x_as; }
        internal int[][] landscape_help(params int[][] y_as)
        { return y_as; }
    }
}
