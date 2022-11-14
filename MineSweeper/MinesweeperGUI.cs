using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class MinesweeperGUI : Form
    {
        //user defined events
        public event EventHandler<PlaceBombsEventArgs> PlaceBombs;
        public event EventHandler<GameEndEventArgs> GameEnded;
        public event EventHandler RequestStats;

        Cell[,] cells = new Cell[10, 10];
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        bool firstClick = true;
        bool win = true;
        int time = 0;
        int clicks = 0;
        int numBombs = 10;

        /// <summary>
        /// Constructor
        /// </summary>
        public MinesweeperGUI()
        {
            InitializeComponent();
            CreateGrid();

            timer.Interval = 1000;
            timer.Tick += OnTick;
        }

        /// <summary>
        /// This method creates the gameboard of cells
        /// </summary>
        public void CreateGrid()
        {
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    //create cell and set it's location
                    cells[row, col] = new Cell();
                    cells[row, col].Location = new Point(col * cells[row, col].SizeOfCell , row * cells[row, col].SizeOfCell + 27);
                    cells[row, col].Row = row;
                    cells[row, col].Col = col;

                    //subscribe to CellClick event
                    cells[row, col].CellClick += CellClickHandler;

                    this.Controls.Add(cells[row, col]);
                }
            }
        }

        /// <summary>
        /// Handling method for tick event on the timer
        /// </summary>
        /// <param name="sedner"></param>
        /// <param name="e"></param>
        public void OnTick(object sedner, EventArgs e)
        {
            //increment time and update the statusLabel so it displays the correct time
            time++;
            statusLabel.Text = time.ToString();
        }

        /// <summary>
        /// Event handler for when a cell is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CellClickHandler(Object sender, EventArgs e)
        {
            //keep track of # of clicks for win condition
            clicks++;

            Cell current = ((Cell)sender);

            //Calls methods to set up the game
            if(firstClick)
            {
                timer.Start();
                PlaceBombsNow(current.Row, current.Col);
                UpdateBombNumbers();
                UpdateFontColor();
                firstClick = false;
            }

            //lose condition, calls methods to end game
            if (current.HasBomb == true)
            {
                timer.Stop();
                win = false;
                BombWasClicked(current);
            }

            //win condition, calls methods to end game and displays message to player
            if(clicks == (cells.GetLength(0) * cells.GetLength(1)) - numBombs)
            {
                timer.Stop();
                MessageBox.Show("You Won!");
                GameDone();
            }

            //game not over, call methods to perform click cascade if there were no adjacent bombs
            if (current.BombCount == 0)
            {
                CheckAbove(current);
                CheckBelow(current);
                CheckAdjacent(current);
            }
        }

        /// <summary>
        /// Method checks the cell above current cell for a bomb, if bomb free performs click on it
        /// </summary>
        /// <param name="current"></param>
        private void CheckAbove(Cell current)
        {
            if (current.Row > 0)
            {
                if (!cells[current.Row - 1, current.Col].HasBomb)
                {
                    cells[current.Row - 1, current.Col].PerformClick();
                }
            }
        }

        /// <summary>
        /// Method checks the cell below current cell for a bomb, if bomb free performs click on it
        /// </summary>
        /// <param name="current"></param>
        private void CheckBelow(Cell current)
        {
            if (current.Row < cells.GetLength(0) - 1)
            {
                if (!cells[current.Row + 1, current.Col].HasBomb)
                {
                    cells[current.Row + 1, current.Col].PerformClick();
                }
            }
        }

        /// <summary>
        /// Method checks the two cells adjacent to current cell for a bomb, if bomb free performs click on thrm
        /// </summary>
        /// <param name="current"></param>
        private void CheckAdjacent(Cell current)
        {
            if (current.Col > 0)
            {
                if (!cells[current.Row, current.Col - 1].HasBomb)
                {
                    cells[current.Row, current.Col - 1].PerformClick();
                }
            }
            if (current.Col < cells.GetLength(1) - 1)
            {
                if (!cells[current.Row, current.Col + 1].HasBomb)
                {
                    cells[current.Row, current.Col + 1].PerformClick();
                }
            }

        }

        /// <summary>
        /// Method loops through cell array to call methods on each cell
        /// </summary>
        public void UpdateBombNumbers()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Cell current = cells[i, j];

                    //Methods are called to update the bomb count on each cell adjacent to cell
                    //with the bomb
                    if (current.HasBomb == true)
                    {
                        UpdateAbove(current);
                        UpdateAdjacent(current);
                        UpdateBelow(current);
                    }

                }
            }
        }

        /// <summary>
        /// Method updates the bomb count on the cells above current (including diagonals)
        /// </summary>
        /// <param name="current"></param>
        private void UpdateAbove(Cell current)
        {
            //check if not the top row
            if (current.Row > 0)
            {
                //increment bomb count and enable the bomb label
                //repeat for diagonals
                cells[current.Row - 1, current.Col].BombCount += 1;
                cells[current.Row - 1, current.Col].BombDisplay.Text = cells[current.Row - 1, current.Col].BombCount.ToString();
                cells[current.Row - 1, current.Col].BombDisplay.Visible = true;
                cells[current.Row - 1, current.Col].BombDisplay.Enabled = true;

                if (current.Col > 0)
                {
                    cells[current.Row - 1, current.Col - 1].BombCount += 1;
                    cells[current.Row - 1, current.Col - 1].BombDisplay.Text = cells[current.Row - 1, current.Col - 1].BombCount.ToString();
                    cells[current.Row - 1, current.Col - 1].BombDisplay.Visible = true;
                    cells[current.Row - 1, current.Col - 1].BombDisplay.Enabled = true;
                }

                if (current.Col < cells.GetLength(1) - 1)
                {
                    cells[current.Row - 1, current.Col + 1].BombCount += 1;
                    cells[current.Row - 1, current.Col + 1].BombDisplay.Text = cells[current.Row - 1, current.Col + 1].BombCount.ToString();
                    cells[current.Row - 1, current.Col + 1].BombDisplay.Visible = true;
                    cells[current.Row - 1, current.Col + 1].BombDisplay.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Method updates the bomb count on the cells adjacent to the current cells
        /// </summary>
        /// <param name="current"></param>
        private void UpdateAdjacent(Cell current)
        {
            //if not in leftmost column
            if (current.Col > 0)
            {
                //increment bomb count and enable the bomb label
                //repeat for right adjacent cell in next block
                cells[current.Row, current.Col - 1].BombCount += 1;
                cells[current.Row, current.Col - 1].BombDisplay.Text = cells[current.Row, current.Col - 1].BombCount.ToString();
                cells[current.Row, current.Col - 1].BombDisplay.Visible = true;
                cells[current.Row, current.Col - 1].BombDisplay.Enabled = true;
            }
            if (current.Col < cells.GetLength(1) - 1)
            {
                cells[current.Row, current.Col + 1].BombCount += 1;
                cells[current.Row, current.Col + 1].BombDisplay.Text = cells[current.Row, current.Col + 1].BombCount.ToString();
                cells[current.Row, current.Col + 1].BombDisplay.Visible = true;
                cells[current.Row, current.Col + 1].BombDisplay.Enabled = true;
            }
        }

        /// <summary>
        /// Method updates the bomb count on the cells below current (including diagonals)
        /// </summary>
        /// <param name="current"></param>
        private void UpdateBelow(Cell current)
        {
            //if not in last row
            if (current.Row < cells.GetLength(0) - 1)
            {
                //increment bomb count and enable the bomb label
                //repeat for diagonals in next two blocks
                cells[current.Row + 1, current.Col].BombCount += 1;
                cells[current.Row + 1, current.Col].BombDisplay.Text = cells[current.Row + 1, current.Col].BombCount.ToString();
                cells[current.Row + 1, current.Col].BombDisplay.Visible = true;
                cells[current.Row + 1, current.Col].BombDisplay.Enabled = true;

                if (current.Col > 0)
                {
                    cells[current.Row + 1, current.Col - 1].BombCount += 1;
                    cells[current.Row + 1, current.Col - 1].BombDisplay.Text = cells[current.Row + 1, current.Col - 1].BombCount.ToString();
                    cells[current.Row + 1, current.Col - 1].BombDisplay.Visible = true;
                    cells[current.Row + 1, current.Col - 1].BombDisplay.Enabled = true;
                }
                if (current.Col < cells.GetLength(1) - 1)
                {
                    cells[current.Row + 1, current.Col + 1].BombCount += 1;
                    cells[current.Row + 1, current.Col + 1].BombDisplay.Text = cells[current.Row + 1, current.Col + 1].BombCount.ToString();
                    cells[current.Row + 1, current.Col + 1].BombDisplay.Visible = true;
                    cells[current.Row + 1, current.Col + 1].BombDisplay.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Method updates font color on cells based on adjacent bomb count
        /// </summary>
        public void UpdateFontColor()
        {
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    Cell current = cells[row, col];

                    if (!current.HasBomb)
                    {
                        if (current.BombCount == 1)
                        {
                            current.BombDisplay.ForeColor = Color.Blue;
                        }
                        if (current.BombCount == 2)
                        {
                            current.BombDisplay.ForeColor = Color.Green;
                        }
                        if (current.BombCount == 3)
                        {
                            current.BombDisplay.ForeColor = Color.Red;
                        }
                        if (current.BombCount == 4)
                        {
                            current.BombDisplay.ForeColor = Color.DarkBlue;
                        }
                        if (current.BombCount == 5)
                        {
                            current.BombDisplay.ForeColor = Color.DarkRed;
                        }
                        if (current.BombCount == 6)
                        {
                            current.BombDisplay.ForeColor = Color.Teal;
                        }
                        if (current.BombCount == 7)
                        {
                            current.BombDisplay.ForeColor = Color.Black;
                        }
                        if (current.BombCount == 8)
                        {
                            current.BombDisplay.ForeColor = Color.LightGray;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Raising method for PlaceBombs event
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void PlaceBombsNow(int row, int col)
        {
            //create instance of event args
            PlaceBombsEventArgs e = new PlaceBombsEventArgs(row, col);

            //call invoking method
            PlaceTheBombs(this, e);
        }

        /// <summary>
        /// Invoking method for PlaceBombs event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PlaceTheBombs(object sender, PlaceBombsEventArgs e)
        {
            PlaceBombs?.Invoke(sender, e);
        }

        /// <summary>
        /// Event handler for BombsPlaced event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BombsPlacedHandler(object sender, BombsPlacedEventArgs e)
        {
            //add the bombs to the appropriate cells from the locations returned in event args
            for (int i = 0; i < e.Responses.GetLength(1); i++)
            {
                cells[e.Responses[0, i], e.Responses[1, i]].HasBomb = true;
            }

        }

        /// <summary>
        /// Method handles lose condition
        /// </summary>
        /// <param name="current"></param>
        public void BombWasClicked(Cell current)
        {
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    //make the bombs visible to player
                    if (cells[row, col].HasBomb)
                    {
                        cells[row, col].CellButton.Visible = false;
                        cells[row, col].BackColor = Color.White;
                        cells[row, col].BombDisplay.Visible = true;
                        cells[row, col].BombDisplay.Text = "!";
                    }
                }
            }
            //tell player they lost
            MessageBox.Show("You Lost!");

            //call method to disable further gameplay
            GameDone();
        }

        /// <summary>
        /// Method disables all buttons to end the game
        /// </summary>
        public void GameDone()
        {
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    cells[row, col].CellButton.Enabled = false;
                }
            }

            //trigger game ended event
            GameHasEnded();
        }

        /// <summary>
        /// raising method for game ended event
        /// </summary>
        private void GameHasEnded()
        {
            //instantiate new event args to send data to logic class
            GameEndEventArgs e = new GameEndEventArgs();
            e.Time = time;
            e.Win = win;

            //call invoking method
            GameEndedStats(this, e);
        }

        /// <summary>
        /// Invoking method for GameEnded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GameEndedStats(object sender, GameEndEventArgs e)
        {
            GameEnded?.Invoke(sender, e);
        }

        /// <summary>
        /// handler method for when the exit option is clicked in menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Handler method for when the restart option is clicked in menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //reset variables
            firstClick = true;
            time = 0;
            clicks = 0;

            //reset gameboard to make game playable again
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    cells[row, col].CellButton.Enabled = true;
                    cells[row, col].CellButton.Visible = true;
                    cells[row, col].BombCount = 0;
                    cells[row, col].BombDisplay.Visible = false;
                    cells[row, col].HasBomb = false;
                    cells[row, col].BackColor = Color.Gray;
                }
            }
        }

        /// <summary>
        /// Raising method for RequestStats event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statsMenuItem_Click(object sender, EventArgs e)
        {
            RequestingStats(this, e);
        }

        /// <summary>
        /// Invoking method for RequestStats event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RequestingStats(object sender, EventArgs e)
        {
            RequestStats?.Invoke(sender, e);
        }

        /// <summary>
        /// Method displays lifetime stats to player when menu item is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DisplayStatsMenu(object sender, GameStatsEventArgs e)
        {
            MessageBox.Show("Average time to complete a game: " + e.AverageTime + " seconds\nWins: " + e.Wins + "\nLosses: " + e.Losses);

        }

        /// <summary>
        /// Handler method for when instructions is selected in the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //display the instructions to the player
            MessageBox.Show("Instructions: \nThe goal of Minesweeper is to clear the board without exploding any bombs. \n\nThe game is played by clicking on the tiles." +
                " If the tile clicked contains a bomb the game is over and you lost. If the tile does not contain a bomb, each adjacent tile without a bomb on it is " +
                "revealed.\n\nSome tiles have a number on them indicating the number of adjacent tiles (diagonals included) with bombs on them. ");
        }

        /// <summary>
        /// Handler method for when about is selected in the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This game was coded by Hayley Jaap for CS 3020.");
        }
    }

}
