using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin
{
    public class Vector
    {
        public int x;
        public int y;
        public int z;
        public Vector()
        {
            x = 0; y = 0; z = 0;
        }
        public Vector(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public Vector(Vector v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
    }
}
