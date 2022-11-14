using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    /// <summary>
    /// Manager class
    /// </summary>
    class MinesweeperGame
    {
        MinesweeperGUI gui;
        MinesweeperLogic logic;

        /// <summary>
        /// constructor
        /// </summary>
        public MinesweeperGame()
        {
            //instantiate objects
            gui = new MinesweeperGUI();
            logic = new MinesweeperLogic();

            //subscribe handlers to events
            logic.BombsPlaced += gui.BombsPlacedHandler;
            gui.PlaceBombs += logic.PlaceBombsHandler;
            gui.GameEnded += logic.GameDoneHandler;
            gui.RequestStats += logic.ReturnStatsHandler;
            logic.ReturnStats += gui.DisplayStatsMenu;

            //run the gui
            Application.Run(gui);
        }

    }
}
