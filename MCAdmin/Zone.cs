using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCAdmin
{
    public class Zone
    {
        public Vector v1;
        public Vector v2;
        public int level;
        public int priority;

        public Zone() : this(0,0) { }

        public Zone(int Level, int Priority)
        {
            v1 = new Vector();
            v2 = new Vector();
            level = Level;
            priority = Priority;
        }

        public Zone(Vector V1, Vector V2, int Level, int Priority)
        {
            v1 = new Vector();
            v2 = new Vector();

            if (V1.x > V2.x)
            {
                v2.x = V1.x;
                v1.x = V2.x;
            }
            else
            {
                v1.x = V1.x;
                v2.x = V2.x;
            }

            if (V1.y > V2.y)
            {
                v2.y = V1.y;
                v1.y = V2.y;
            }
            else
            {
                v1.y = V1.y;
                v2.y = V2.y;
            }

            if (V1.z > V2.z)
            {
                v2.z = V1.z;
                v1.z = V2.z;
            }
            else
            {
                v1.z = V1.z;
                v2.z = V2.z;
            }
            level = Level;
            priority = Priority;
        }

        public virtual bool IsInZone(Vector v)
        {
            return (v.x >= v1.x && v.x <= v2.x && v.y >= v1.y && v.y <= v2.y && v.z >= v1.z && v.z <= v2.z);
        }
    }
}
