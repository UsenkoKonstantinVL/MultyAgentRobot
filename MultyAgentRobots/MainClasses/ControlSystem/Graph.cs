using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyAgentRobots.MainClasses.ControlSystem
{
    public enum GraphCondition
    {
        isNew, inInProcess, isCompleted 
    }
    public class Graph
    {
        public GraphCondition Condition;

        public List<Point2D> LinkPoints;
        public List<Graph> Graphs;

        public int Direction;
        public  String RobotName;

        public Graph()
        {
            LinkPoints = new List<Point2D>();
            Graphs = new List<Graph>();
            Condition = GraphCondition.isNew;
        }

        public void SetGraph(Graph g)
        {
            this.Condition = g.Condition;
            this.Direction = g.Direction;
            this.Graphs = g.Graphs;
            this.LinkPoints = g.LinkPoints;
            this.RobotName = g.RobotName;
        }
    }

    public class Point2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public List<Point2D> LinkPoints;
 

        public Point2D()
        {
            LinkPoints = new List<Point2D>();
        }

        public String GetName()
        {
            return X.ToString() + ":" + Y.ToString();
        }

        public static Point2D GetPoint2dFromString(string name)
        {
            Point2D point = new Point2D();
            var names = name.Split(':');

            try {
                point.X = Convert.ToInt32(names[0]);
                point.Y = Convert.ToInt32(names[1]);
            }
            catch
            {

            }

            return point;
        }
    }
        
    public class GraphManager
    {
        List<Graph> Graphs;
        List<Graph> NewGraph;
        List<Graph> WorkingGraph;
        List<Point2D> Points;

        public Boolean IsInitialized { get; set; }
        private Point2D startPosition;
        public Connector connector;

        public GraphManager()
        {
            Graphs = new List<Graph>();
            NewGraph = new List<Graph>();
            WorkingGraph = new List<Graph>();
            Points = new List<Point2D>();
            IsInitialized = false;
        }

        public List<Graph> GetGraphs()
        {
           /* for (int i = 0; i < Graphs.Count; i++)
            {
                if (Graphs[i].Condition == GraphCondition.inInProcess)
                {
                    bool match = false;
                    for (int j = 0; j < WorkingGraph.Count; j++)
                    {
                        if(WorkingGraph[j].Direction == Graphs[i].Direction &&
                            WorkingGraph[j].RobotName == Graphs[i].RobotName &&
                             WorkingGraph[j].LinkPoints[0].X == Graphs[i].LinkPoints[0].X &&
                              WorkingGraph[j].LinkPoints[0].Y == Graphs[i].LinkPoints[0].Y)
                        {
                            match = true;
                        }
                    }

                    if(!match)
                    {
                        Graphs.RemoveAt(i);
                        i--;
                    }
                }

                if (Graphs[i].Condition == GraphCondition.isNew)
                {

                    bool match = false;
                    for (int j = 0; j < NewGraph.Count; j++)
                    {
                        if (NewGraph[j].Direction == Graphs[i].Direction &&
                            NewGraph[j].RobotName == Graphs[i].RobotName &&
                             NewGraph[j].LinkPoints[0].X == Graphs[i].LinkPoints[0].X &&
                              NewGraph[j].LinkPoints[0].Y == Graphs[i].LinkPoints[0].Y)
                        {
                            match = true;
                        }
                    }

                    if (!match)
                    {
                        Graphs.RemoveAt(i);
                        i--;
                    }

                    

                }
            }*/

            for(int i = 0; i < Graphs.Count; i++)
            {
                if(Graphs[i] != null)
                {
                    for(int j = 0; j < Graphs[i].Graphs.Count; j++)
                    {
                        if(Graphs[i].Graphs[j] == null)
                        {
                            Graphs[i].Graphs.RemoveAt(j);
                            j--;
                        }
                    }
                }
                else
                {
                    Graphs.RemoveAt(i);
                }
            }


            return Graphs;
        }

        public void InitializeWorking(Graph firstGraph)
        {
            if (!IsInitialized)
            {
                firstGraph.Condition = GraphCondition.inInProcess;
                Graphs.Add(firstGraph);
                WorkingGraph.Add(firstGraph);
                Points.Add(firstGraph.LinkPoints[0]);
                startPosition = firstGraph.LinkPoints[0];
                IsInitialized = true;
            }
        }
        public void InitializeWorking(Graph firstGraph, Graph mainGraph, List<Graph> listGraph)
        {
            if (!IsInitialized)
            {
                /*Graph graph = new Graph();
                graph.LinkPoints.Add(point);
                graph.Direction = direction;
                IsInitialized = true;

                Graphs.Add(graph);
                NewGraph.Add(graph);*/
                firstGraph.Condition = GraphCondition.inInProcess;

                Graphs.Add(mainGraph);
                Graphs.Add(firstGraph);
                Graphs.AddRange(listGraph);

                WorkingGraph.Add(firstGraph);
                NewGraph.AddRange(listGraph);

                Points.Add(firstGraph.LinkPoints[0]);
                startPosition = firstGraph.LinkPoints[0];
                IsInitialized = true;
            }
        }

        public Point2D GetStartPosition()
        {
            return startPosition;
        }

        public Graph GetFreeGraph(string nameOfRobot)
        {
            
            if (NewGraph.Count != 0)
            {
                Random random = new Random();
                var graph = NewGraph[random.Next(NewGraph.Count)];

                graph.RobotName = nameOfRobot;

                WorkingGraph.Add(graph);

                NewGraph.Remove(graph);

                return graph;
            }
            else
            {
                return null;
            }
        }

        public Graph GetFreeGraph(string nameOfRobot, Point2D p, Dijkstras.Dijkstras d)
        {
            //Выбор ближайшей цели
           
            if (NewGraph.Count != 0)
            {
                int i = -1;
                int min = 10000;
                for (int j = 0; j < NewGraph.Count; j++)
                {
                    var res = d.shortest_path(p.GetName(), NewGraph[j].LinkPoints[0].GetName());
                    var res2 = GetNearFreePoint(p, NewGraph[j].LinkPoints[0].GetName(), d);
                    if (res.Count <= res2)
                    {
                        i = j;
                        min = res.Count;
                    }
                }

                if (i != -1)
                {
                    Random random = new Random();
                    var graph = NewGraph[/*random.Next(NewGraph.Count)*/i];

                    graph.RobotName = nameOfRobot;

                    WorkingGraph.Add(graph);

                    NewGraph.Remove(graph);

                    return graph;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private int GetNearFreePoint(Point2D p, String name, Dijkstras.Dijkstras d)
        {
            List<Point2D> robotspoint = new List<Point2D>();
            
            for(int i = 0; i < connector.infoAboutRobots.Count; i++)
            {
                if (connector.infoAboutRobots[i].Condition != Condition.AtHome && connector.infoAboutRobots[i].Condition != Condition.Research) {
                    if (!(p.X == connector.infoAboutRobots[i].X && p.Y == connector.infoAboutRobots[i].Y))
                    {
                        Point2D point = new Point2D();
                        point.X = connector.infoAboutRobots[i].X;
                        point.Y = connector.infoAboutRobots[i].Y;

                        robotspoint.Add(point);

                    }
                }
            }
            int min = 10000;

            //if(min != 0)
            {
                for(int i = 0; i < robotspoint.Count; i++)
                {
                    var res = d.shortest_path(robotspoint[i].GetName(), name);
                    if (min > res.Count)
                    {
                        min = res.Count;
                    }

                }
            }

            return min;
        }

        public void GraphComplete(Graph completeGraph)
        {
            int i = -1;
            for(int j = 0; j < WorkingGraph.Count; j++)
            {
                if(WorkingGraph[j].RobotName == completeGraph.RobotName)
                {
                    i = j;
                }
            }
            if(i != -1)
            {

                WorkingGraph.Remove(completeGraph);
                completeGraph.Condition = GraphCondition.isCompleted;
            }

        }

        public void CancelGo(Graph researchGraph)
        {
            WorkingGraph.Remove(researchGraph);
           // Graphs.Remove(researchGraph);
        }

        public void AbortGraph(Graph curGraph)
        {
            WorkingGraph.Remove(curGraph);
            NewGraph.Add(curGraph);
        }

        public void AddToGraph(Graph graph)
        {
            foreach(Graph g in Graphs)
            {
                if(g.LinkPoints.Count == 2)
                {
                    if(RobotController.MatchTwoPoints(g.LinkPoints[1], graph.LinkPoints[1])
                        && !RobotController.MatchTwoPoints(g.LinkPoints[0], graph.LinkPoints[0]))
                    {
                        graph.Graphs.Add(g);
                        g.Graphs.Add(graph);
                        return;
                    }
                }
            }
        }

        public void SetNewGraphs(List<Graph> graphs, Graph currentGraph)
        {
            // foreach(Point2D point in Points)
            foreach (Graph point in Graphs)
            {
               /* if(point.LinkPoints.Count > 1 && 
                    (point.LinkPoints[0].X == currentGraph.LinkPoints[1].X &&
                    point.LinkPoints[0].Y == currentGraph.LinkPoints[1].Y) ||
                    (point.LinkPoints[1].X == currentGraph.LinkPoints[0].X &&
                    point.LinkPoints[1].Y == currentGraph.LinkPoints[0].Y))*/
                {
                    

                    foreach(Graph g in Graphs)
                    {
                        if (g.Graphs != null && g.Graphs.Count > 1 && g.LinkPoints.Count > 1 &&
                            (g.LinkPoints[1].X == graphs[0].LinkPoints[0].X &&
                            g.LinkPoints[1].Y == graphs[0].LinkPoints[0].Y ||
                            g.LinkPoints[0].X == currentGraph.LinkPoints[1].X &&
                            g.LinkPoints[0].Y == currentGraph.LinkPoints[1].Y))
                        {
                            
                            int reverseDir = 0;
                            switch(currentGraph.Direction)
                            {
                                case 0:
                                    {
                                        reverseDir = 2;
                                        break;
                                    }

                                case 1:
                                    {
                                        reverseDir = 3;
                                        break;
                                    }

                                case 2:
                                    {
                                        reverseDir = 0;
                                        break;
                                    }

                                case 3:
                                    {
                                        reverseDir = 1;
                                        break;
                                    }
                            }

                            for(int i = 0; i < g.Graphs.Count; i++)
                            {
                                if(g.Graphs[i].Direction == reverseDir)
                                {
                                    //NewGraph.Remove(g.Graphs[i]);
                                    NewGraph.Remove(g.Graphs[i]);
                                    WorkingGraph.Remove(g.Graphs[i]);
                                    g.Graphs[i].Condition = GraphCondition.isCompleted;
                                    Graphs.Remove(g.Graphs[i]);
                                    g.Graphs[i] = currentGraph;
                                    currentGraph.Graphs.Clear();
                                    //currentGraph.Graphs.Add(g.Graphs[i]);
                                    return;
                                    for (int j = 0; j < NewGraph.Count; j++)
                                    {
                                        
                                        /*if (NewGraph[j].Direction == reverseDir &&
                                            NewGraph[j].LinkPoints[0].X == g.Graphs[i].LinkPoints[0].X &&
                                            NewGraph[j].LinkPoints[0].Y == g.Graphs[i].LinkPoints[0].Y)
                                        {
                                            NewGraph.RemoveAt(j);
                                            g.Graphs[i] = currentGraph;

                                            return;
                                        }*/
                                    }
                                    g.Graphs[i].Condition = currentGraph.Condition;
                                    g.Graphs[i].Direction = currentGraph.Direction;
                                    g.Graphs[i].Graphs = currentGraph.Graphs;

                                    g.Graphs[i].LinkPoints = currentGraph.LinkPoints;
                                    g.Graphs[i].RobotName = currentGraph.RobotName;

                                   /* g.Graphs[i].Condition = currentGraph.Condition;

                                    g.Condition = GraphCondition.isCompleted;*/

                                    return;
                                }


                            }

                            

                        }
                    }

                    //return;
                }
            }

            foreach (Graph g in graphs)
            {
                Points.Add(g.LinkPoints[0]);
            }

            NewGraph.AddRange(graphs);
            Graphs.AddRange(graphs);
        }

        public Graph GetNearestGraph(Point2D    p)
        {
            foreach(Graph g in Graphs)
            {
                if( g.LinkPoints.Count > 1 &&
                    RobotController.MatchTwoPoints(p, g.LinkPoints[0]))
                {
                    return g;
                }
            }

            return null;
        }

        public bool IsResearchComplete()
        {
            if(NewGraph.Count == 0 && WorkingGraph.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetNewCrossRoad()
        {

        }
    }
}
