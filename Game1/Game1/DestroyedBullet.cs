using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class DestroyedBullet
    {
        //Fields
        private Rectangle rectangle;
        private Texture2D texture;
        private float appearTimer;

        //Properties
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }
        public Texture2D Texture
        {
            get { return texture; }
        }
        public float AppearTimer
        {
            get { return appearTimer; }
            set { appearTimer = value; }
        }

        //Constructors
        public DestroyedBullet(Rectangle rect, Texture2D text)
        {
            rectangle = rect;
            texture = text;
        }

        //Methods
    }
}
