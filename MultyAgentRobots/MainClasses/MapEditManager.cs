using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace MultyAgentRobots.MainClasses
{
    public class MapEditManager
    {
        #region Constant
        public static int mapHeight = 500;
        public static int mapWidth = 500;

        public static int Cell = 5;
        #endregion
        Bitmap image;

        public MapEditManager()
        {
            image = new Bitmap(mapWidth, mapHeight);
        }


        public Bitmap DrawCell(int x, int y, Occyped occyped, int stracheCell)
        {

            return DrawGrid();
        }

        private Bitmap DrawGrid()
        {
            Bitmap _image = (Bitmap)image.Clone();

            Graphics g;

            g = Graphics.FromImage(_image);

            Pen myPen = new Pen(Color.Gray);

            myPen.Width = 1;

            int h = mapHeight / Cell;

            for (int i = 1; i < (h - 1); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = 0;
                y2 = mapHeight - 1;

                x1 = (i * Cell) - 1;
                x2 = x1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            for (int i = 1; i < (h - 1); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = (i * Cell) - 1;
                y2 = y1;

                x1 = 0;
                x2 = mapWidth - 1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            return _image;
        }
    }


    public enum Occyped
    {
        Occyped, NoOccyped
    }
}
