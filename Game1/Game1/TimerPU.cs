using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class TimerPU
    {
        private int incShield = 0;
        private int incSpeed = 0;
        private int incSpread = 0;
        private int incBigShot = 0;

        Game1 g = new Game1();

        public void Timer()
        {
            if (g.Shield == true)
            {
                incShield++;
                if (incShield == 100)
                {
                    g.Shield = false;
                }
            }
            if (g.Speed == true)
            {
                incSpeed++;
                if (incSpeed == 50)
                {
                    g.Speed = false;
                }
            }
            if (g.Spread == true)
            {
                incSpread++;
                if (incSpread == 150)
                {
                    g.Spread = false;
                }
            }
            if (g.BigShot == true)
            {
                incBigShot++;
                if (incBigShot == 150)
                {
                    g.BigShot = false;
                }
            }
        }
    }
}
