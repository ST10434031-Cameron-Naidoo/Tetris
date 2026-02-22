using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    //responsible for picking the next block in the game
    public class BlockQueue
    {
        //contain a block array with an instance of each of the 7 block classes, which will be recycled.
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        //random object
        private readonly Random random = new Random();

        //next block in queue - currently a single block for preview, but can be modified for an array
        public Block nextBlock { get; private set; }

        //random block
        private Block randomBlock()
        {
            // returns a random block
            return blocks[random.Next(blocks.Length)];
        }

        //initialising nextBlock to a randomBlock
        public BlockQueue()
        {
            nextBlock = randomBlock();
        }

        //gets nextBlock and Updates property
        public Block getAndUpdateBlock()
        {
            Block block = nextBlock;

            //keep picking until a new block is chosen, to avoid returning the same block twice in a row
            do
            {
                nextBlock = randomBlock();
            }
            while (block.ID == nextBlock.ID);

            return block;
        }

    }
}
