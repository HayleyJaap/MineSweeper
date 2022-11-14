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
    public partial class Cell : UserControl
    {
        Random rand = new Random();
        Button cellButton = new Button();
        Label bombDisplay = new Label();
        int bombCount = 0;
        bool hasBomb = false;
        int sizeOfCell = 32;
        int row = -1;
        int col = -1;

        //user defined event for the cell being clicked
        public EventHandler CellClick;

        /// <summary>
        /// Constructor used to set the desired starting values for each cell and 
        /// its components
        /// </summary>
        public Cell()
        {
            InitializeComponent();
            this.Size = new Size(sizeOfCell, sizeOfCell);
            this.BackColor = Color.Gray;
            cellButton.Size = new Size(sizeOfCell, sizeOfCell);
            cellButton.FlatStyle = FlatStyle.Flat;
            cellButton.Location = new Point(0, 0);
            cellButton.BackColor = Color.LightGray;
            cellButton.Click += ButtonClickHandler;
            this.Controls.Add(cellButton);

            bombDisplay.Enabled = false;
            bombDisplay.Text = "0";
            bombDisplay.Visible = false;
            this.Controls.Add(bombDisplay);
        }

        //properties
        public int SizeOfCell { get => sizeOfCell; }
        public int Col { get => col; set => col = value; }
        public int Row { get => row; set => row = value; }
        public Button CellButton { get => cellButton; set => cellButton = value; }
        public int BombCount { get => bombCount; set => bombCount = value; }
        public bool HasBomb { get => hasBomb; set => hasBomb = value; }
        public Label BombDisplay { get => bombDisplay; set => bombDisplay = value; }

        /// <summary>
        /// Raising method for cell click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonClickHandler(object sender, EventArgs e)
        {
            ((Button)sender).Visible = false;
            OnCellClick(this, e);
        }

        /// <summary>
        /// Invoking method for cell click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCellClick(object sender, EventArgs e)
        {
            CellClick?.Invoke(sender, e);
        }

        /// <summary>
        /// This method performs a click event on a cell
        /// </summary>
        public void PerformClick()
        {
            cellButton.PerformClick();
        }
    }
}
