using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Position
    {

        public int row;
        public int column;


        //traditional getters and setters
        public int getRow()
        {
            return row;
        }

        public int getColumns()
        {
            return getColumns();
        }

        public void setRow(int r)
        {
            row = r;            
        }

        public void setColumns(int c)
        {
            column = c;
        }

        public Position(int r, int c)
        {
            row = r;
            column = c;
        }


    }
}
