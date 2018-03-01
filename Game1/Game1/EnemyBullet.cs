using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class EnemyBullet : GameObject
    {
        //fields
        private float angle;
        private bool toBeDeleted;
        //properties
        public float Angle
        {
            get { return angle; }
            set { angle = value;}
        }

        public bool ToBeDeleted
        {
            get { return toBeDeleted; }
            set { toBeDeleted = value; }
        }
        //constructor
        public EnemyBullet(int x, int y, int width, int height, float speed) : base(x, y, height, width, speed)
        {
            this.speed = speed;
        }
        //methods
        public bool CheckDelete(int x, int y)
        {
            if(Math.Abs(x)>= 1000 || Math.Abs(y)>= 1000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public float FindAngle(GameObject bullet, GameObject player)
        {
            float angleToReturn;
            angleToReturn = (float)Math.Atan2(player.PositionY - bullet.PositionY, player.PositionX - bullet.PositionX);
            angle = angleToReturn;
            return angleToReturn;
        }
    }
}
