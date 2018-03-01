using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Player : GameObject
    {
        //fields


        //properties

        //constructor
        public Player(int x, int y, int width, int height, float speed)
            : base(x, y, width, height, speed)
        {
            this.speed = speed;
        }

        //methods
    }
}
