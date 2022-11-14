using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Class used as event args for RequestStats event
    /// </summary>
    public class GameStatsEventArgs : EventArgs
    {
        int averageTime;
        int wins;
        int losses;

        public int AverageTime { get => averageTime; set => averageTime = value; }
        public int Wins { get => wins; set => wins = value; }
        public int Losses { get => losses; set => losses = value; }
    }
}
