using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int [,] grid;
        public int rows;
        public int columns;

        // traditional getter and setter methods
        public int getRows()
        {
            return rows;
        }

        public int getColumns()
        {
            return columns;
        }

        public int getValue(int r, int c)
        {
            return grid[r, c];
        }

        public void setValue(int r, int c, int value)
        {
            if (isInside(r, c))
            {
                grid[r, c] = value;
            }
        }

        //constructor for mini or maxi versions of the game
        public GameGrid(int r, int c)
        {
            rows = r;
            columns = c;
            grid = new int[rows, columns];
        }

        // convenience methods

        //checks if specific row and column combination is inside the grid or not
        public bool isInside(int r, int c)
        {
            // if r/c is greater or equal to 0 AND less than rows/columns respectively, then that block is inside the grid 
            if ((r >= 0 && r < rows) && (c >= 0 && c < columns))
            {
                return true;
            }
            else
                return false;
        }

        //checks if a given block is empty or not 
        public bool isEmpty(int r, int c)
        {
            //first, the block must be insdie the grid
            //in the int array, 0 indicates the block is empty, any other value (1-8) would indicate the block contains part of a tetris tile
            if ((isInside(r,c)==true)&&(grid[r, c] == 0))
            {
                return true;
            }
            else
                return false;
        }


        //checks if an entire row is full
        public bool isRowFull(int r)
        {
            //cycle through each row element, by incrementing column
            for(int c = 0; c < columns; c++)
            {
                //if one block is empty, the row is empty
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }

            //if the none of the blocks have a value of 0, then the row is full
            return true;
        }


        //checks if an entire row is empty
        public bool isRowEmpty(int r)
        {
            //cycle through each row element, by incrementing column
            for (int c = 0; c < columns; c++)
            {
                //if one block is not empty, the row is not empty
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }

            //if all the blocks are empty, the row is empty
            return true;
        }


        //clears a row
        private void clearRow(int r)
        {
            for(int c = 0; c < columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        //Moves row down by a certain number of rows
        private void moveRowDown(int r,int numRows)
        {
            // rows are counted from top to bottom, to move down add the desired numRows to the current row (r) number, and assign grid[r,c] val to desired location and empty current location
            for(int c = 0; c < columns; c++)
            {
                grid[r+numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        // clears full rows
        public int clearFullRows()
        {
            int cleared = 0;

            //Start from the bottom, check if each row is full, if is  clear it and increment clear
            for(int r = rows - 1; r >= 0; r--)
            {

                if (isRowFull(r) == true)
                {
                    clearRow(r);
                    cleared++;

                }
                else if (cleared > 0)
                {
                    //when cleared rows>0, the next non-clear row above it must be moved cleared rows down
                    moveRowDown(r, cleared);
                }
            }

            return cleared;
        }

    }
}
