using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Network.Levels
{
    public class Level1 : ILevel
    {
        internal override int[][] SetLandscape()
        {
            int[] row1 = new int[] { 98, 00, 00, 99 };
            int[] row2 = new int[] { 01, 01, 01, 01 };
            int[] row3 = new int[] { 01, 01, 01, 01 };
            int[] row4 = new int[] { 01, 01, 01, 01 };
            return new int[][] { row1, row2, row3, row4 };
        }
        internal override int[] SetAvailableMethods()
        { return new AvailableMethodBuilder().Right(5).Build(); }
        internal override string SetNameOfClass()
        { return "Level 1"; }
    }
}
