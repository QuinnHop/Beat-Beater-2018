using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Collectable : GameObject
    {
        //fields

        //properties

        //constructor
        public Collectable(int x, int y, int width, int height, float speed) : base(x, y, height, width, speed)
        {
            speed = 0;
        }

        //methods
    }
}
