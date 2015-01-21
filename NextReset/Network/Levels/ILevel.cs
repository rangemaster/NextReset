using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Network.Levels
{
    public abstract class ILevel
    {
        private int[][] _landscape;
        private int[] _availablemethods;
        private string _name;
        public ILevel()
        { }
        public void Init()
        {
            Debug.WriteLine("ILevel Init");
            _landscape = SetLandscape();
            _availablemethods = SetAvailableMethods();
            _name = SetNameOfClass();
        }
        /// <summary>
        /// 0 = path <br></br>
        /// 1 = Wall <br></br>
        /// 98 = You <br></br>
        /// 99 = Finnish <br></br>
        /// </summary>
        /// <returns></returns>
        internal abstract int[][] SetLandscape();
        internal abstract int[] SetAvailableMethods();
        internal abstract String SetNameOfClass();
        public int[][] Landscape { get { return _landscape; } }
        public int[] Methods { get { return _availablemethods; } }
        public string Name { get { return _name; } }
        public override string ToString()
        { return "Class: " + _name + ", (x" + _landscape[0].Length + ", y" + _landscape.Length + ")"; }
        internal int[] landscape_row_help(params int[] x_as)
        { return x_as; }
        internal int[][] landscape_help(params int[][] y_as)
        { return y_as; }
        public class AvailableMethodBuilder
        {
            static int right, left, up, down, attack, bomb;
            public int[] Build()
            { return new int[] { right, left, up, down, attack, bomb }; }
            public AvailableMethodBuilder Right(int value) { right = value; return this; }
            public AvailableMethodBuilder Left(int value) { left = value; return this; }
            public AvailableMethodBuilder Up(int value) { up = value; return this; }
            public AvailableMethodBuilder Down(int value) { down = value; return this; }
            public AvailableMethodBuilder Attack(int value) { attack = value; return this; }
            public AvailableMethodBuilder Bomb(int value) { bomb = value; return this; }
            public AvailableMethodBuilder Below5()
            {
                right = 5;
                left = 5;
                down = 5;
                up = 5;
                return this;
            }
        }
    }
}
