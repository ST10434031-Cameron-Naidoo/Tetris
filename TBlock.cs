using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TBlock:Block
    {

        private readonly Position[][] Tiles = new Position[][]
        {
            new Position [] { new(0,1), new(1,0), new(1,1), new(1,2) },
            new Position[] { new (0,1), new (1,1), new (1,2), new (2,1) },
            new Position [] { new (1,0), new (1,1), new(1, 2), new (2,1) },
            new Position [] { new (0,1), new (1,0), new (1,1), new (2,1) }

        };

        public override int ID => 6;
        //this start position spawns blocks in the middle of the top row
        protected override Position startOffset => new Position(0, 3);
        //assigning Tiles array to tiles property
        protected override Position[][] tiles => Tiles;
    }
}
