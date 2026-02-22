using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class OBlock: Block
    {
        // OBlock is unique in that its position doesnt need to change because of the location of its pivot point
        private readonly Position[][] Tiles = new Position[][]
        {
            new Position [] { new (0,0), new (0,1), new (1,0), new (1,1) }

        };

        public override int ID => 4;
        //this start position spawns blocks in the middle of the top row
        protected override Position startOffset => new Position(0,4);
        //assigning Tiles array to tiles property
        protected override Position[][] tiles => Tiles;
    }
}
