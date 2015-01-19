using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Levels
{
    public class Level1 : ILevel
    {
        internal override int[][] SetLandscape()
        {
            int[] row1 = new int[] { 0, 0, 0, 0 };
            int[] row2 = new int[] { 1, 0, 1, 0 };
            int[] row3 = new int[] { 0, 0, 1, 0 };
            int[] row4 = new int[] { 1, 1, 1, 0 };
            return new int[][] { row1, row2, row3, row4 };
        }
        internal override bool[] SetAvailableMethods()
        {
            return method_help(true, false, false, false);
        }
        internal override string SetNameOfClass()
        { return "Level 1"; }
    }
}
