using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using MultyAgentRobots.MainClasses.RobotControllerhelpFunction;

namespace MultyAgentRobots.MainClasses
{
    public class RobotController
    {
        //Информация о роботе находится в пременной robotInformation
        public RobotInformation robotInformation { get; set; }
        //Информация о других роботах и о общих данных будет находиться в connector
        public Connector connector{ get; set; }

        private Position initializePosition;
        Position currentPosition;
        string name;


        public RobotController()
        {

        }

        public RobotController(Position _pos)
        {
            initializePosition = _pos;
            currentPosition = initializePosition;
        }

        public Position Run(int[] data)
        {
           if(data[2] == MapManager.POSITION_FREE)
            {
                move(2);
            }
           else if(data[1] == MapManager.POSITION_FREE)
            {
                move(1);
            }
            else if (data[3] == MapManager.POSITION_FREE)
            {
                move(3);
            }
            else if (data[0] == MapManager.POSITION_FREE)
            {
                move(0);
            }
            return currentPosition; 
        }

        private void move(int curse)
        {
            if(curse == 0)
            {
                robotInformation.Y--;
            }
            else if(curse == 1)
            {
                robotInformation.X++;
            }
            else if (curse == 2)
            {
                robotInformation.Y++;
            }
            else if (curse == 3)
            {
                robotInformation.X--;
            }
        }

        public Position CurrentPosition { get {return currentPosition; } private set{; } }

        public static int[] GetSensorInfo(int[] mapSensorData, List<RobotInformation> info, 
            RobotInformation curRobot )
        {
            var res = mapSensorData;
            
                foreach(RobotInformation r in info)
                {
                    if (r.Name != curRobot.Name) {
                        if (mapSensorData[0] != MapManager.POSITION_OCUPED)
                        {
                            if (((curRobot.Y - 1) == r.Y) && ((curRobot.X) == r.X))
                            {
                                res[0] = MapManager.POSITION_OCUPED;
                            }
                        }

                        if (mapSensorData[1] != MapManager.POSITION_OCUPED)
                        {
                            if (((curRobot.Y) == r.Y) && ((curRobot.X + 1) == r.X))
                            {
                                res[1] = MapManager.POSITION_OCUPED;
                            }
                        }

                        if (mapSensorData[2] != MapManager.POSITION_OCUPED)
                        {
                            if (((curRobot.Y + 1) == r.Y) && ((curRobot.X) == r.X))
                            {
                                res[2] = MapManager.POSITION_OCUPED;
                            }
                        }

                        if (mapSensorData[3] != MapManager.POSITION_OCUPED)
                        {
                            if (((curRobot.Y) == r.Y) && ((curRobot.X - 1) == r.X))
                            {
                                res[3] = MapManager.POSITION_OCUPED;
                            }
                        }
                    }
                }
            return res;
        }
    }

    public class Position
    {
        public int  X { get; set; }
        public int  Y { get; set; }
    }

    public class RobotInformation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color RobotColor { get; set; }
        public string Name { get; set; }
    }
    //Является общим хранилищем данных для всех роботов
    public class Connector
    {
        public List<RobotInformation> infoAboutRobots { get; set; }
    }
}
