using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class PowerUps : PickUps
    {
        //fields
        private string type;

        //properties
        public string Type { get { return type; } }

        //constructor
        public PowerUps(int x, int y, int width, int height, float speed, string type) : base(x, y, height, width, speed)
        {
            speed = 0;
            this.type = type;
        }
        //methods

        public void Shield()
        {
            
        }

        public void Heal()
        {
            
        }

        public void SpeedUp()
        {

        }

        public void AltFireSpread()
        {

        }

        public void AltFireBig()
        {

        }
    }
}
