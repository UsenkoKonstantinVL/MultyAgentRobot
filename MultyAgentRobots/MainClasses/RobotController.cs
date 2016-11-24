using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using MultyAgentRobots.MainClasses.RobotControllerhelpFunction;
using MultyAgentRobots.MainClasses.ControlSystem;
using MultyAgentRobots.MainClasses.ControlSystem.Dijkstras;

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
        public Condition condition = Condition.Waiting;
        public GraphManager manager;
        public double TimeCount { get; set; }

        public RobotController()
        {
            
        }

        public RobotController(Position _pos, GraphManager _manager)
        {
            initializePosition = _pos;
            currentPosition = initializePosition;
            manager = _manager;
            TimeCount = 0;
        }

        public void Initialize()
        {
            robotInformation.Condition = condition;
        }

        int[] sensorData;
        Dijkstras dijkstras;
        public Position Run(int[] data, Dijkstras d)
        {
            sensorData = data;
            dijkstras = d;
            StrategicLevelRun();
            /*
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
            }*/


            robotInformation.Condition = condition;
            return currentPosition; 
        }



        #region Strategic level

        public void StrategicLevelRun()
        {
            switch (condition)
            {
                case Condition.Waiting:
                case Condition.AtHome:
                case Condition.GoToHome:
                    {
                        FunctionWaiting();
                        break;
                    }
                case Condition.GoToPoint:
                    {
                        if (currentGraph.Condition != GraphCondition.isCompleted)
                        {
                            FunctionGoToPoint();
                        }
                        else
                        {
                            manager.GraphComplete(currentGraph);
                            condition = Condition.Waiting;
                            massOfPoints = null;
                            currentGraph = null;
                        }
                        TimeCount++;
                        break;
                    }
                case Condition.Research:
                    {
                        FunctionResearch();
                        TimeCount++;
                        break;
                    }
            }
        }


        Graph currentGraph;

        private void FunctionWaiting()
        {
            if (!manager.IsInitialized)
            {
                Graph graph = new Graph();
                graph.RobotName = robotInformation.Name;
                Point2D point = new Point2D();
                point.X = robotInformation.X;
                point.Y = robotInformation.Y;
                graph.LinkPoints.Add(point);

                int dir = -1;

                List<int> masDir = new List<int>();

                if (sensorData[2] == MapManager.POSITION_FREE)
                {
                    dir = (2);
                    masDir.Add(dir);
                }
                if (sensorData[1] == MapManager.POSITION_FREE)
                {
                    dir = (1);
                    masDir.Add(dir);
                }
                if (sensorData[3] == MapManager.POSITION_FREE)
                {
                    dir = (3);
                    masDir.Add(dir);
                }
                if (sensorData[0] == MapManager.POSITION_FREE)
                {
                    dir = (0);
                    masDir.Add(dir);
                }

                List<Graph> graphs = new List<Graph>();
                Graph mainGraph = new Graph();
                if(masDir.Count > 1)
                {
                    mainGraph.Direction = 0;
                    mainGraph.LinkPoints.Add(robotInformation.GetPoint());
                    mainGraph.LinkPoints.Add(robotInformation.GetPoint());
                    mainGraph.Condition = GraphCondition.isCompleted;
                    mainGraph.Graphs.Add(graph);
                }
                foreach(int ind in masDir)
                {
                    Graph gr = new Graph();
                    gr.Direction = ind;
                    gr.LinkPoints.Add(robotInformation.GetPoint());
                    mainGraph.Graphs.Add(gr);
                    graphs.Add(gr);
                }

                graph.Direction = dir;

                currentGraph = graph;

                if (graphs.Count == 1)
                {
                    manager.InitializeWorking(graph);
                }
                else
                {
                    manager.InitializeWorking(graph, mainGraph, graphs);
                }

                condition = Condition.Research;
                move(dir);
            }
            else if(manager.IsResearchComplete())
            {
                //Возврат в начальную точку
                condition = Condition.GoToHome;
                if(massOfPoints == null)
                {
                    massOfPoints = GetPath(dijkstras.shortest_path(robotInformation.X + ":" + robotInformation.Y,
                              manager.GetStartPosition().GetName()));
                }
                GoHome();
            }
            else
            {
                masIndex = 0;
                //currentGraph = manager.GetFreeGraph(robotInformation.Name);
                Point2D p = new Point2D();
                p.X = robotInformation.X;
                p.Y = robotInformation.Y;
                currentGraph = manager.GetFreeGraph(robotInformation.Name, p, dijkstras);
                if(currentGraph != null)
                {
                    massOfPoints = null;
                    condition = Condition.GoToPoint;
                    massOfPoints = GetPath(dijkstras.shortest_path(robotInformation.X + ":" + robotInformation.Y, 
                        currentGraph.LinkPoints[0].GetName()));
                    currentGraph.Condition = GraphCondition.inInProcess;
                }
            }
        }

        private List<Point2D> GetPath(List<String> list)
        {
            List<Point2D> listOfPoint = new List<Point2D>();

            foreach(String l in list)
            {
                Point2D point = Point2D.GetPoint2dFromString(l);
                listOfPoint.Add(point);
            }

            List<Point2D> newList = new List<Point2D>();

            for(int i = listOfPoint.Count; i > 0; i--)
            {
                newList.Add(listOfPoint[i - 1]);
            }

            return newList;
        }

        int j = 0;
        private void GoHome()
        {
            //Получить массив точек, через которые пойдут роботы
            if (!(robotInformation.X == manager.GetStartPosition().X
                && robotInformation.Y == manager.GetStartPosition().Y))
            {
                if ((robotInformation.X == massOfPoints[j].X
                    && robotInformation.Y == massOfPoints[j].Y))
                {
                    j++;
                }
                //Движение к следующей точке
                if (robotInformation.X + 1 == massOfPoints[j].X)
                {
                    move(1);
                }
                else if (robotInformation.X - 1 == massOfPoints[j].X)
                {
                    move(3);
                }
                else if (robotInformation.Y - 1 == massOfPoints[j].Y)
                {
                    move(0);
                }
                else if (robotInformation.Y + 1 == massOfPoints[j].Y)
                {
                    move(2);
                }
            }
            else
            {
                condition = Condition.AtHome;
            }
        }

        List<Point2D> massOfPoints;
        int masIndex = 0;//i
        private void FunctionGoToPoint()
        {
           if(massOfPoints != null)
            {
                //Получить массив точек, через которые пойдут роботы
                if(!(robotInformation.X == currentGraph.LinkPoints[0].X
                    && robotInformation.Y == currentGraph.LinkPoints[0].Y))
                {
                    if((robotInformation.X == massOfPoints[masIndex].X
                    && robotInformation.Y == massOfPoints[masIndex].Y))
                    {
                        if (IsAnyRobotFoth(massOfPoints[masIndex]) == null)
                        {
                            masIndex++;
                        }
                        else
                        {
                            massOfPoints = null;
                            condition = Condition.Waiting;
                        }
                    }

                    if(IsAnyRobotFoth(massOfPoints[masIndex], Condition.Waiting) != null)
                    {
                        massOfPoints = null;
                        condition = Condition.Waiting;
                        manager.CancelGo(currentGraph);
                        return;
                    }
                    //Движение к следующей точке
                    if(robotInformation.X + 1 == massOfPoints[masIndex].X)
                    {
                        move(1);
                    }
                    else if(robotInformation.X - 1 == massOfPoints[masIndex].X)
                    {
                        move(3);
                    }
                    else if (robotInformation.Y - 1 == massOfPoints[masIndex].Y)
                    {
                        move(0);
                    }
                    else if (robotInformation.Y + 1 == massOfPoints[masIndex].Y)
                    {
                        move(2);
                    }
                }
                else
                {
                    move(currentGraph.Direction);
                    condition = Condition.Research;
                    massOfPoints = null;
                    masIndex = 0;
                }
            }
            else
            {

            }
        }

        private void FunctionResearch()
        {
            if (!IsCrossRoadOrDeadEnd(currentGraph.Direction))
            {
                move(currentGraph.Direction);
            }
            else
            {
                var r = IsRobotFoth();
                if (r != null)
                {
                    currentGraph.LinkPoints.Add(r.currentGraph.LinkPoints[0]);
                    r.condition = Condition.Waiting;
                    manager.GraphComplete(currentGraph);
                    manager.GraphComplete(r.currentGraph);
                    condition = Condition.Waiting;
                }
                Point2D p = new Point2D();
                p.X = robotInformation.X;
                p.Y = robotInformation.Y;
                currentGraph.LinkPoints[0].LinkPoints.Add(p);
                currentGraph.LinkPoints.Add(p);
                currentGraph.Condition = GraphCondition.isCompleted;
                manager.GraphComplete(currentGraph);
                if(!IsDeadEnd(currentGraph.Direction))
                {
                    manager.SetNewGraphs(GetNewGraps(currentGraph.Direction), currentGraph);
                }
                currentGraph = null;
                condition = Condition.Waiting;
            }
        }


        RobotController IsAnyRobotFoth(Point2D obstRobPos)
        {
            RobotController res = null;

            //int[,] incr = new int[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            /*Point2D obstRobPos = new Point2D();
            obstRobPos.X = robotInformation.X + incr[currentGraph.Direction, 0];
            obstRobPos.Y = robotInformation.Y + incr[currentGraph.Direction, 1];*/

            foreach (var robot in connector.robots)
            {
                if (MatchTwoPoints(obstRobPos, robot.robotInformation.GetPoint()))
                {
                    if (robot.condition == Condition.Research)
                        res = robot;
                }
            }

            return res;
        }

        RobotController IsAnyRobotFoth(Point2D obstRobPos, Condition cond)
        {
            RobotController res = null;

            //int[,] incr = new int[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            /*Point2D obstRobPos = new Point2D();
            obstRobPos.X = robotInformation.X + incr[currentGraph.Direction, 0];
            obstRobPos.Y = robotInformation.Y + incr[currentGraph.Direction, 1];*/

            foreach (var robot in connector.robots)
            {
                if (MatchTwoPoints(obstRobPos, robot.robotInformation.GetPoint()))
                {
                    if (robot.condition == cond)
                        res = robot;
                }
            }

            return res;
        }

        RobotController IsRobotFoth()
        {
            RobotController res = null;

            int[,] incr = new int[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            Point2D obstRobPos = new Point2D();
            obstRobPos.X = robotInformation.X + incr[currentGraph.Direction, 0];
            obstRobPos.Y = robotInformation.Y + incr[currentGraph.Direction, 1];

            foreach(var robot in connector.robots)
            {
                if(MatchTwoPoints(obstRobPos, robot.robotInformation.GetPoint()))
                {
                    if(robot.condition == Condition.Research)
                        res = robot;
                }
            }

            return res;
        }
        private bool IsCrossRoadOrDeadEnd(int direction)
        {
            int[] c = new int[4];
            switch(direction)
            {
                case 0:
                {
                        c = new int[4] { 0, 1, 2, 3 };
                    break;
                }

                case 1:
                    {
                        c = new int[4] { 1, 2, 3, 0};
                        break;
                    }

                case 2:
                    {
                        c = new int[4] { 2, 3, 0, 1};
                        break;
                    }

                case 3:
                    {
                        c = new int[4] {3, 0, 1, 2};
                        break;
                    }
            }
            if(sensorData[c[0]] == MapManager.POSITION_OCUPED ||
                sensorData[c[1]] == MapManager.POSITION_FREE ||
                sensorData[c[3]] == MapManager.POSITION_FREE)
                return true;
            else
            {
                return false;
            }
        }

        private bool IsDeadEnd(int direction)
        {
            int[] c = new int[4];
            switch (direction)
            {
                case 0:
                    {
                        c = new int[4] { 0, 1, 2, 3 };
                        break;
                    }

                case 1:
                    {
                        c = new int[4] { 1, 2, 3, 0 };
                        break;
                    }

                case 2:
                    {
                        c = new int[4] { 2, 3, 0, 1 };
                        break;
                    }

                case 3:
                    {
                        c = new int[4] { 3, 0, 1, 2 };
                        break;
                    }
            }
            if(sensorData[c[0]] == MapManager.POSITION_OCUPED &&
                sensorData[c[1]] == MapManager.POSITION_OCUPED &&
                sensorData[c[3]] == MapManager.POSITION_OCUPED)
                return true;
            else
            {
                return false;
            }
        }

        private List<Graph> GetNewGraps(int dir)
        {
            List<Graph> newGraphs = new List<Graph>();
            Point2D currentPoint = new Point2D();
            currentPoint.X = robotInformation.X;
            currentPoint.Y = robotInformation.Y;

            if (dir == 0)
            {
                if(sensorData[0] == MapManager.POSITION_FREE)
                {

                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y - 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 0;

                    newGraphs.Add(graph);

                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[1] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X + 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 1;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                /*if (sensorData[2] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y + 1;
                    graph.LinkPoints.Add(point);
                    graph.Direction = 2;

                    newGraphs.Add(graph);
                }*/

                if (sensorData[3] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X - 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 3;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }
            }

            else if (dir == 1)
            {
                if (sensorData[0] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y - 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 0;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[1] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X + 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 1;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[2] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y + 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 2;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                /*if (sensorData[3] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    Point2D point = new Point2D();
                    point.X = robotInformation.X - 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);
                    graph.Direction = 3;

                    newGraphs.Add(graph);
                }*/
            }

            else if (dir == 2)
            {
                /*if (sensorData[0] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y - 1;
                    graph.LinkPoints.Add(point);
                    graph.Direction = 0;

                    newGraphs.Add(graph);
                }*/

                if (sensorData[1] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X + 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 1;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[2] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y + 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 2;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[3] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X - 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 3;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }
            }

            else if (dir == 3)
            {
                if (sensorData[0] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y - 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 0;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                /*if (sensorData[1] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    Point2D point = new Point2D();
                    point.X = robotInformation.X + 1;
                    point.Y = robotInformation.Y;
                    graph.LinkPoints.Add(point);
                    graph.Direction = 1;

                    newGraphs.Add(graph);
                }*/

                if (sensorData[2] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X;
                    point.Y = robotInformation.Y + 1;
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 2;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }

                if (sensorData[3] == MapManager.POSITION_FREE)
                {
                    Graph graph = new Graph();
                    /*Point2D point = new Point2D();
                    point.X = robotInformation.X - 1;
                    point.Y = robotInformation.Y;*
                    graph.LinkPoints.Add(point);*/
                    graph.Direction = 3;

                    newGraphs.Add(graph);
                    currentGraph.Graphs.Add(graph);
                }
            }

            foreach(Graph g in newGraphs)
            {
                g.LinkPoints.Add(currentPoint);
            }
            return newGraphs;
        }

        #endregion

        #region Tactic level

        #endregion

        #region Executive level
        private void move(int curse)
        {
            if(curse == 0)
            {
                if(sensorData[0] == MapManager.POSITION_FREE)
                    robotInformation.Y--;
            }
            else if(curse == 1)
            {
                if (sensorData[1] == MapManager.POSITION_FREE)
                    robotInformation.X++;
            }
            else if (curse == 2)
            {
                if (sensorData[2] == MapManager.POSITION_FREE)
                    robotInformation.Y++;
            }
            else if (curse == 3)
            {
                if (sensorData[3] == MapManager.POSITION_FREE)
                    robotInformation.X--;
            }
        }
        #endregion

        public Position CurrentPosition { get {return currentPosition; } private set{; } }

        public static bool MatchTwoPoints(Point2D p1, Point2D p2)
        {
            if(p1.X == p2.X && p1.Y == p2.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int[] GetSensorInfo(int[] mapSensorData, List<RobotInformation> info, 
            RobotInformation curRobot )
        {
            var res = mapSensorData;
            
                foreach(RobotInformation r in info)
                {
                    if (r.Name != curRobot.Name)
                    {
                        if (r.Condition != Condition.AtHome)
                        {
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
                }
            return res;
        }
    }

    public class Position
    {
        public int  X { get; set; }
        public int  Y { get; set; }

        public Position()
        {

        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public String GetName()
        {
            String name = X.ToString() + ":" + Y.ToString();
            return name;
        }
    }

    public class RobotInformation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color RobotColor { get; set; }
        public string Name { get; set; }
        public Condition Condition { get; set; }

        public Point2D GetPoint()
        {
            Point2D point = new Point2D();

            point.X = X;
            point.Y = Y;

            return point;
        }
    }

    //Является общим хранилищем данных для всех роботов
    public class Connector
    {
        public List<RobotController> robots { get; set; }
        public List<RobotInformation> infoAboutRobots { get; set; }
    }
}
