using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MultyAgentRobots.MainClasses
{
  public class MapManager
    {
        Bitmap image;
        Brush color;
        public MapManager(Bitmap _image, Brush _color)
        {
            image = _image;
            color = _color;
        }
        public Bitmap sense(Position position)
        {
            return drawgrid(drawrobot(position));
        }
        public Bitmap InizializeMap()
        {
            return drawgrid((Bitmap)image.Clone());
        }
        private Bitmap drawrobot(Position position)
        {
            Bitmap ima = (Bitmap)image.Clone();
            Graphics g = Graphics.FromImage(ima);
            g.FillEllipse(color, position.X*MapEditManager.Cell, position.Y*MapEditManager.Cell, MapEditManager.Cell, MapEditManager.Cell);
            return ima;
        }
        
        public int[,] scan(Position position)
        {
            int[,] sc = new int[7, 7];

            return sc;
        }
        private Bitmap drawgrid(Bitmap _image)
        {
            

            Graphics g;

            g = Graphics.FromImage(_image);

            Pen myPen = new Pen(Color.Gray);

            myPen.Width = 1;

            int h = MapEditManager.mapHeight / (MapEditManager.Cell);

            for (int i = 1; i <= (h); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = 0;
                y2 = MapEditManager.mapHeight - 1;

                x1 = (i * MapEditManager.Cell) - 1;
                x2 = x1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            for (int i = 1; i <= (h); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = (i * MapEditManager.Cell) - 1;
                y2 = y1;

                x1 = 0;
                x2 = MapEditManager.mapWidth - 1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            return _image;
        }
    }

}
