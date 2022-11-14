using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class MinesweeperLogic
    {
        int avgTime;
        int totalTime;
        int numGames;
        int wins;
        int losses;
        int numBombs = 10;

        //user defined events to return values to gui class
        public event EventHandler<BombsPlacedEventArgs> BombsPlaced;
        public event EventHandler<GameStatsEventArgs> ReturnStats;

        /// <summary>
        /// Constructor
        /// </summary>
        public MinesweeperLogic()
        {
            avgTime = 0;
            numGames = 0;
            wins = 0;
            losses = 0;
        }

        /// <summary>
        /// Raising method for BombsPlaced event
        /// </summary>
        /// <param name="bombLocations"></param>
        private void BombsHaveBeenPlaced(int[,] bombLocations)
        {
            //instantiate event args and pass bomb locations in
            BombsPlacedEventArgs e = new BombsPlacedEventArgs();
            e.Responses = bombLocations;

            //call raising method
            OnBombsPlaced(this, e);
        }

        /// <summary>
        /// Invoking method for BombsPlaced event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnBombsPlaced(object sender, BombsPlacedEventArgs e)
        {
            BombsPlaced?.Invoke(sender, e);
        }

        /// <summary>
        /// Event handler for PlaceBombs event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlaceBombsHandler(object sender, PlaceBombsEventArgs e)
        {
            Random rand = new Random();
            int[,] bombLocations = new int[2, 10];

            //loop to generate bomb locations 
            for (int i = 0; i < numBombs; i++)
            {
                bool valid = false;

                //generate random values for x and y coordinates
                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);

                //validate locatiosn
                while (!valid)
                {
                    //check if location generated is not the 1 cell clicked
                    if (x != e.ClickLocation[0] && y != e.ClickLocation[1])
                    {
                        //call method to check if this is a repeat location 
                        bool repeat = CheckRepeats(bombLocations, x, y);

                        if (!repeat)
                        {
                            //add location to array of bomb locations
                            bombLocations[0, i] = x;
                            bombLocations[1, i] = y;
                            //valid location 
                            valid = true;
                        }
                        else
                        {
                            //regenerate location because it wasn't valid
                            x = rand.Next(0, 10);
                            y = rand.Next(0, 10);
                        }
                    }
                    else
                    {
                        //regenerate location because it wasn't valid
                        x = rand.Next(0, 10);
                        y = rand.Next(0, 10);
                    }
                }
            }

            //Trigger bombsplaced event
            BombsHaveBeenPlaced(bombLocations);

        }

        /// <summary>
        /// Method checks if repeat bomb location
        /// </summary>
        /// <param name="bombLocations"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool CheckRepeats(int[,] bombLocations, int x, int y)
        {
            bool repeat = false;

            for (int i = 0; i < bombLocations.GetLength(0); i++)
            {
                if (x == bombLocations[0, i] && y == bombLocations[1, i])
                {
                    repeat = true;
                }
            }

            return repeat;
        }

        /// <summary>
        /// Event handler for GameEnded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GameDoneHandler(object sender, GameEndEventArgs e)
        {
            //track number of games
            numGames++;

            //increment wins or losses
            if (e.Win)
            {
                wins++;
            }
            else
            {
                losses++;
            }

            //calculate average time per game
            totalTime += e.Time;
            avgTime = totalTime / numGames;
        }

        /// <summary>
        /// Event handler for RequestStats event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ReturnStatsHandler(object sender, EventArgs e)
        {
            //call raising method for ReturnStats event
            ReturnTheStats();
        }

        /// <summary>
        /// Raising method for ReturnStats event
        /// </summary>
        private void ReturnTheStats()
        {
            //instantiate event args to return stats info
            GameStatsEventArgs e = new GameStatsEventArgs();
            e.AverageTime = avgTime;
            e.Wins = wins;
            e.Losses = losses;

            //call invoking method
            OnReturnStats(this, e);
        }

        /// <summary>
        /// Invoking method for ReturnStats event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnReturnStats(object sender, GameStatsEventArgs e)
        {
            ReturnStats?.Invoke(sender, e);
        }

    }
}

