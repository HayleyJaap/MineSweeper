using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    /// <summary>
    /// Class used as event args for BombsPlaced event
    /// </summary>
    public class BombsPlacedEventArgs : EventArgs
    {
        int[,] responses;

        public BombsPlacedEventArgs()
        {
            responses = new int[2, 10];
        }

        public int[,] Responses { get => responses; set => responses = value; }
    }
}
