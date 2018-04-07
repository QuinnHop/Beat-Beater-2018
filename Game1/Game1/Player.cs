using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Game1
{
    class Player : GameObject
    {
        //fields
        private int health;

        //properties
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        //constructor
        public Player(int x, int y, int width, int height, float speed)
            : base(x, y, width, height, speed)
        {
            this.speed = speed;
            health = 10; //Health initialized in constructor. Default value is 1, change if you want to
        }

        //methods
        
    }
}
