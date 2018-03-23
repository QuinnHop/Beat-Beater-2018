using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Button : GameObject
    {
        //Fields
        
        //Constructors
        public Button(int x, int y, int width, int height, float speed) 
            : base (x, y, height, width, speed)
        {
            speed = 0;
        }

        //Methods

    }
}
