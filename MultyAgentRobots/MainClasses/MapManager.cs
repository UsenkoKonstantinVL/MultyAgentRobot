using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MultyAgentRobots.MainClasses.ControlSystem.Dijkstras;

namespace MultyAgentRobots.MainClasses
{
  public class MapManager
    {
        Bitmap originalImage;
        Bitmap fogMap;
        Brush color;
        Brush traceColor;
        int str = 1;



        public MapManager(Bitmap _image, int _str)
        {
            str = _str;
            originalImage = _image;

            fogMap = new Bitmap(originalImage.Width, originalImage.Height);

            var g = Graphics.FromImage(fogMap);
            var brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, 0, 0, fogMap.Width, fogMap.Height);
        }
        /*public Bitmap sense(Position position, Brush _color)
        {
            return drawgrid(drawrobot(position, _color));
        }*/
        public Bitmap InizializeMap()
        {
            return drawgrid((Bitmap)originalImage.Clone());
        }

        public Bitmap DrawRobots(int x, int y, Color color)
        {
            Position p = new Position();
            p.X = x;
            p.Y = y;

            return drawrobots(p, new SolidBrush(color));
        }

        public Bitmap drawrobots(Position position, Brush _color)
        {
            Graphics gg = Graphics.FromImage(originalImage);
            gg.FillRectangle(traceColor, position.X * str * MapEditManager.Cell, position.Y * str * MapEditManager.Cell, str * MapEditManager.Cell, str * MapEditManager.Cell);
            color = _color;
            Bitmap ima = (Bitmap)originalImage.Clone();
            Graphics g = Graphics.FromImage(ima);
            g.FillEllipse(color, position.X*str*MapEditManager.Cell, position.Y * str * MapEditManager.Cell, str * MapEditManager.Cell, str * MapEditManager.Cell);
            return ima;
        }
        /// <summary>
        /// Отрисовка карты с изображением роботов
        /// </summary>
        /// <param name="info"></param>
        public Bitmap DrawMap(List<RobotInformation> info)
        {
            /*int[] desX = new int[9] { -1,  0,  1, 1, 1, 0, -1, -1, 0 };
            int[] desY = new int[9] { -1, -1, -1, 0, 1, 1,  1,  0, 0 };*/
            int[] desX = new int[5] { 0, 1, 0, -1, 0};
            int[] desY = new int[5] { -1, 0, 1, 0, 0};

            foreach (RobotInformation r in info)
            {
                for(int i = 0; i < desX.Length; i++)
                {
                    var pixFogmap = fogMap.GetPixel(r.X * MapEditManager.Cell * str + desX[i]  * MapEditManager.Cell * str + MapEditManager.Cell * str / 2,
                        r.Y * MapEditManager.Cell * str + desY[i] * MapEditManager.Cell * str + MapEditManager.Cell * str / 2);
                    var pixOrMap  = originalImage.GetPixel(r.X * MapEditManager.Cell * str + desX[i]  * MapEditManager.Cell * str + MapEditManager.Cell * str / 2,
                        r.Y * MapEditManager.Cell * str + desY[i] * MapEditManager.Cell * str + MapEditManager.Cell * str / 2);

                    if(!MapFormer.MatchColor(pixFogmap, pixOrMap))
                    {
                        fogMap = MapEditManager.DrawRectanglePointToMap(fogMap, r.X + desX[i], 
                            r.Y + desY[i], str, pixOrMap);
                    }
                }
            }

            ///Рисование роботов
            /// 
            Bitmap cloneB = (Bitmap)fogMap.Clone();

            foreach (RobotInformation r in info)
            {
                //for (int i = 0; i < 8; i++)
                {

                    Graphics g = Graphics.FromImage(cloneB);

                    g.FillEllipse(new SolidBrush(r.RobotColor), r.X * MapEditManager.Cell * str + 1,
                        r.Y * MapEditManager.Cell * str + 1, MapEditManager.Cell * str - 2, 
                        MapEditManager.Cell * str - 2);

                    g.DrawEllipse(new Pen(Color.Tomato, 2), r.X * MapEditManager.Cell * str + 1,
                        r.Y * MapEditManager.Cell * str + 1, MapEditManager.Cell * str - 2,
                        MapEditManager.Cell * str - 2);

                }
            }

            return drawgrid(cloneB);

        }



        static public int POSITION_FREE = 0;
        static public int POSITION_OCUPED = -1;
        static public int POSITION_OTHERCAR = -2;
        static public int POSITION_MY_POSITION = 2;

        static public int POINTLEFT = -1;
        static public int POINTRIGHT = 1;
        static public int POINTABOVE = -1;
        static public int POINTBELOW = 1;

        /// <summary>
        /// Возвращает значение карты с препятствиями
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int[] ScanNearMap(int x, int y)
        {
            Position p = new Position();
            p.X = x;
            p.Y = y;
            return scan(p);
        }
        public int[] scan(Position position)
        {
            int[] scan = new int[4];
            if(position.Y - 1 > 0)
            {
                var _position = new Position();
                _position.X = position.X;
                _position.Y = position.Y - 1;
                scan[0] = getObstacle(_position);
            }
            else
            {
                scan[0] = POSITION_OCUPED;
            }


            if ((position.X + 1) < (MapEditManager.mapWidth/MapEditManager.Cell))
            {
                var _position = new Position();
                _position.X = position.X + 1;
                _position.Y = position.Y;
                scan[1] = getObstacle(_position);
            }
            else
            {
                scan[1] = POSITION_OCUPED;
            }

            if (position.Y + 1 < (MapEditManager.mapWidth / MapEditManager.Cell))
            {
                var _position = new Position();
                _position.X = position.X;
                _position.Y = position.Y + 1;
                scan[2] = getObstacle(_position);
            }
            else
            {
                scan[2] = POSITION_OCUPED;
            }

            if (position.X - 1 > 0)
            {
                var _position = new Position();
                _position.X = position.X - 1;
                _position.Y = position.Y;
                scan[3] = getObstacle(_position);
            }
            else
            {
                scan[3] = POSITION_OCUPED;
            }


            return scan;
        }

        public Dijkstras GetDijkstras()
        {
            Dijkstras graph = new Dijkstras();

            int maxWidth = fogMap.Width / (str * MapEditManager.Cell);
            int maxHeight = fogMap.Height / (str * MapEditManager.Cell);


            for (int i = 0; i < maxWidth; i++)
            {
                for(int j = 0; j < maxHeight; j++)
                {
                    Position pos = new Position();
                    pos.X = i;
                    pos.Y = j;
                    if(getObstacle(pos) == POSITION_FREE)
                    {
                        String posName = pos.GetName();
                        Dictionary<String, int> dictionary = new Dictionary<String, int> ();
                        if (pos.Y > 1)
                        {
                            Position newPos = new Position(pos.X, pos.Y - 1);
                            if(getObstacle(newPos) == POSITION_FREE)
                            {
                                dictionary.Add(newPos.GetName(), 1);
                            }
                        }

                        if (pos.Y < (maxHeight - 1))
                        {
                            Position newPos = new Position(pos.X, pos.Y + 1);
                            if (getObstacle(newPos) == POSITION_FREE)
                            {
                                dictionary.Add(newPos.GetName(), 1);
                            }
                        }

                        if (pos.X > 1)
                        {
                            Position newPos = new Position(pos.X - 1, pos.Y);
                            if (getObstacle(newPos) == POSITION_FREE)
                            {
                                dictionary.Add(newPos.GetName(), 1);
                            }
                        }

                        if (pos.X < (maxWidth - 1))
                        {
                            Position newPos = new Position(pos.X + 1, pos.Y);
                            if (getObstacle(newPos) == POSITION_FREE)
                            {
                                dictionary.Add(newPos.GetName(), 1);
                            }
                        }
                        graph.add_vertex(posName, dictionary);
                    } 
                }
            }

            return graph;
        }

        private int getObstacle(Position pos)
        {
            int res = POSITION_FREE;

            var color = originalImage.GetPixel(pos.X * str * MapEditManager.Cell, pos.Y * str * MapEditManager.Cell);
            if(MapFormer.MatchColor(color, Color.White))
            {
                res = POSITION_FREE;
            }
            else if (MapFormer.MatchColor(color, Color.Black))
            {
                res = POSITION_OCUPED;
            }


            return res;
        }

        private Bitmap drawgrid(Bitmap _image)
        {
            

            Graphics g;

            g = Graphics.FromImage(_image);

            Pen myPen = new Pen(Color.Gray);

            myPen.Width = 1;

            int h = MapEditManager.mapHeight / (str * MapEditManager.Cell);

            for (int i = 1; i <= (h); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = 0;
                y2 = MapEditManager.mapHeight - 1;

                x1 = (i * str * MapEditManager.Cell) - 1;
                x2 = x1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            for (int i = 1; i <= (h); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = (i * str * MapEditManager.Cell) - 1;
                y2 = y1;

                x1 = 0;
                x2 = MapEditManager.mapWidth - 1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            return _image;
        }
    }

}
