using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyAgentRobots.MainClasses
{
    public class RobotController
    {
        private Position initializePosition;
        Position currentPosition;
        public RobotController(Position _pos)
        {
            initializePosition = _pos;
            currentPosition = initializePosition;
        }

        public Position Run(int[, ] data)
        {
            return currentPosition; 
        }

        public Position CurrentPosition { get {return currentPosition; } private set{; } }
    }

    public class Position
    {
        public int  X { get; set; }
        public int  Y { get; set; }
    }
}
