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
        protected float angle;
        protected bool toBeDeleted;
        string attackName;
        float spawnTime;
        //properties
        public float Angle
        {
            get { return angle; }
            set { angle = value;}
        }
        public float SpawnTimer
        {
            get { return spawnTime; }
            set { spawnTime = value; }
        }
        public bool ToBeDeleted
        {
            get { return toBeDeleted; }
            set { toBeDeleted = value; }
        }
        public string AttackName
        {
            get { return attackName; }
            set { attackName = value; }
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

        public bool CheckCollision(GameObject object1, GameObject object2)//checks collision
        {
            if (object1.Position.Intersects(object2.Position))
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
