using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                //when we update the current block, the Reset() is called to set the correct start position and rotation
                currentBlock.Reset();

                //move new block down if theres nothing in the way
                for( int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!blockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        //properties for gamegrid, blockqueue and gameover
        public GameGrid GameGrid { get; }

        public BlockQueue BlockQueue { get; }

        public bool GameOver { get; private set; }

        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            //initialise the gamegrid with 22 rows and 10 cols
            GameGrid = new GameGrid(22, 10);


            //initialise blockQueue and use it to get a randomBlock for the currentBlock property
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.getAndUpdateBlock();
            CanHold = true;
        }

        //checks if the current block is in a legal position or not
        private bool blockFits()
        {
            //loops over the tile positions of the current block and checks if they are outside the grid or overlapping another block, which flags false
            foreach (Position p in CurrentBlock.tilePositions())
            {
                if (!GameGrid.isEmpty(p.row, p.column))
                {
                    return false;
                }
            }
            return true;
        }

        public void holdBlock()
        {
            //if we cannot hold then return
            if (!CanHold)
            {
                return;
            }

            //no block is held, hold current and get new current
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.getAndUpdateBlock();
            }
            //if block is held swop with current 
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            //so we cannot spam hold
            CanHold = false;
        }

        //rotate block clockwise only if possible in grid
        public void rotateBlockCW()
        {
            //rotate the block cw
            CurrentBlock.rotateCW();

            //check if it fits, if it doesn't rotate it back ccw
            if (!blockFits())
            {
                CurrentBlock.rotateCCW();
            }
        }

        //rotate block counterclockwise only if possible in grid
        public void rotateBlockCCW()
        {
            //  rotate the block ccw
            CurrentBlock.rotateCCW();

            //check if it fits, if it doesn't rotate it back cw
            if (!blockFits())
            {
                CurrentBlock.rotateCW();
            }

        }


        //move block left if possible in grid
        public void moveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!blockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        //move block right if possible in grid
        public void moveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!blockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool isGameOver()
        {
            //if either of the invisible rows at the top are not empty then the game is lost
            return !(GameGrid.isRowEmpty(0) && GameGrid.isRowEmpty(1));
        }

        //called when the current block cannot be moved down
        private void placeBlock()
        {
            //loops over the tile positions of the current block and sets those positions in the gamegrid equal to that blocks ID
            foreach(Position p in CurrentBlock.tilePositions())
            {
                GameGrid.setValue(p.row,p.column, CurrentBlock.ID);
            }

            //clear any potentially fully rows
            Score+=GameGrid.clearFullRows();

            //check if the game is over
            if (isGameOver())
            {
                //if game is over set game over property to true
                GameOver = true;
            }
            else
            {
                //if the game isnt over, update current block
                CurrentBlock = BlockQueue.getAndUpdateBlock();
                CanHold = true;
            }
        }

        //move block down
        public void moveBlockDown()
        {
            //move block one down
            CurrentBlock.Move(1, 0);

            //if block does not fit, move it back up and place the block there
            if (!blockFits())
            {
                CurrentBlock.Move(-1, 0);
                placeBlock();
            }
        }

        //takes a position and returns number of rows below it
        private int tileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.isEmpty(p.row + drop + 1, p.column))
            {
                drop++;
            }

            return drop;
        }

        //invoke tileDropDistance for each tile in block and take minimum
        public int blockDropDistance()
        {
            int drop = GameGrid.rows;

            foreach(Position p in CurrentBlock.tilePositions())
            {
                drop = System.Math.Min(drop, tileDropDistance(p));
            }

            return drop;
        }

        public void dropBlock()
        {
            CurrentBlock.Move(blockDropDistance(), 0);
            placeBlock();

        }
    }
}
