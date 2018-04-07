using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Button
    {
        //Fields
        public Rectangle rectangle { get; set; }
        
        
        //Constructors
        public Button(Rectangle rectangle) 
        {
            this.rectangle = rectangle;
        }

        //Methods
        public bool checkHover(MouseState mouse) 
        {
            if(rectangle.Contains(new Rectangle(mouse.X, mouse.Y, 25, 25)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool checkPressed( MouseState mouse)
        {
            if ((mouse.LeftButton == ButtonState.Pressed) && rectangle.Contains(mouse.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
