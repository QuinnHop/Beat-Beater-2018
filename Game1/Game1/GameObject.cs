using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class GameObject
    {
        //fields
        protected Rectangle position;
        protected Texture2D texture;

        //properties
        public int PositionX
        {
            get
            {
                return position.X;
            }

            set
            {
                position.X = value;
            }
        }

        public int PositionY
        {
            get
            {
                return position.Y;
            }

            set
            {
                position.Y = value;
            }
        }

        //constructor

        public GameObject(int x, int y, int height, int width)
        {
            position = new Rectangle(x, y, width, height);
        }
        //methods




    }
}
