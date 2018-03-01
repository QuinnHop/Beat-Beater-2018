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
            get { return position.X; }

            set { position.X = value; }
        }

        public int PositionY
        {
            get { return position.Y; }

            set { position.Y = value; }
        }
        
        public int Width
        {
            get { return position.Width; }
            set { position.Width = value; }
        }

        public int Height
        {
            get { return position.Height; }
            set { position.Height = value; }
        }

        public Rectangle Position {
            get { return position; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        //constructor

        public GameObject(int x, int y, int height, int width)
        {
            position = new Rectangle(x, y, width, height);
        }
        
        //methods
        


    }
}
