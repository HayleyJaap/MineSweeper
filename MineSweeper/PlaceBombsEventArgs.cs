using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Class used as event args for PlaceBombs event
    /// </summary>
    public class PlaceBombsEventArgs : EventArgs
    {
        int[] clickLocation;
        
        /// <summary>
        /// Constructor that initializes clickLocation
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public PlaceBombsEventArgs(int row, int col)
        {
            clickLocation = new int[2];
            clickLocation[0] = row;
            clickLocation[1] = col;
        }

        public int[] ClickLocation { get => clickLocation; set => clickLocation = value; }
    }
}
