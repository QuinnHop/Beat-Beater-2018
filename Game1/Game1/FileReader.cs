using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

namespace Game1
{
    class FileReader
    {
        //field
        StreamReader stream;

        float timerStamp;
        string attackName;
        int numOfAttacks;
        Vector2 position;
        bool levelComplete;


        //property
        public float TimeStamp
        {
            get { return timerStamp; }
            set { timerStamp = value; }
        }
        public string AttackName
        {
            get { return attackName; }
            set { attackName = value; }
        }
        public int NumberOfAttacks
        {
            get { return numOfAttacks; }
            set { numOfAttacks = value; }
        }
        public float xPosition
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float yPosition
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public bool LevelComplete
        {
            get { return levelComplete; }
        }


        //constructor
        public FileReader(string fileName)
        {
            stream = new StreamReader(fileName);
        }
        //methods
        public void ReadLine()
        {
            string tempLine;
            string[] tempArray;
            
            tempLine = stream.ReadLine();
            if (tempLine == null) { levelComplete = true; return;}
            tempArray = tempLine.Split(',');
            timerStamp = float.Parse(tempArray[0]);
            attackName = tempArray[1];
            numOfAttacks = int.Parse(tempArray[2]);
            xPosition = int.Parse(tempArray[3]);
            yPosition = int.Parse(tempArray[4]);
            
        }

    }
}
