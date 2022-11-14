using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{   
    /// <summary>
    /// Class used as event args for the GameEnded event to pass information into logic
    /// </summary>
    public class GameEndEventArgs : EventArgs
    {
        int time;
        bool win;

        public int Time { get => time; set => time = value; }
        public bool Win { get => win; set => win = value; }
    }
}
