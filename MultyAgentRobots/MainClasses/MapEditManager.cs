using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace MultyAgentRobots.MainClasses
{
    public class MapEditManager
    {
        #region Constant
        public static int mapHeight = 500;
        public static int mapWidth = 500;

        public static int Cell = 10;

        int strCell = 1;
        #endregion

        Bitmap image;

        public MapEditManager()
        {
            image = new Bitmap(mapWidth, mapHeight);
            var g = Graphics.FromImage(image);
            var brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, 0, 0, mapWidth, mapHeight);
        }

        public Bitmap InitizlizeMap()
        {

            return DrawGrid(strCell);
        }

        public Bitmap SetStretch(int str)
        {
            var g = Graphics.FromImage(image);
            var brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, 0, 0, mapWidth, mapHeight);
            strCell = str;
            return DrawGrid(str);
        }


        public Bitmap DrawCell(int x, int y, Occyped occyped, int stracheCell)
        {
            Graphics g;

            strCell = stracheCell;

            g = Graphics.FromImage(image);

            int cx = x / (stracheCell* Cell);
            int cy = y / (stracheCell * Cell);

            int x1 = cx * (stracheCell * Cell);
            int x2 = (cx + 1) * (stracheCell * Cell);

            int y1 = cy * (stracheCell * Cell);
            int y2 = (cy + 1) * (stracheCell * Cell);

            SolidBrush blueBrush =  new SolidBrush(Color.Blue);
            if(occyped == Occyped.NoOccyped)
            {
                blueBrush = new SolidBrush(Color.Black);
            }
            else
            {
                blueBrush = new SolidBrush(Color.White);
            }

            g.FillRectangle(blueBrush, x1, y1, (stracheCell * Cell), (stracheCell * Cell));

            return DrawGrid(stracheCell);
        }

        private Bitmap DrawGrid(int s)
        {
            Bitmap _image = (Bitmap)image.Clone();

            Graphics g;

            g = Graphics.FromImage(_image);

            Pen myPen = new Pen(Color.Gray);

            myPen.Width = 1;

            int h = mapHeight / (s * Cell);

            for (int i = 1; i <= (h ); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = 0;
                y2 = mapHeight - 1;

                x1 = (i * Cell*s) - 1;
                x2 = x1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            for (int i = 1; i <= (h); i++)
            {
                int x1, y1;
                int x2, y2;


                y1 = (i * Cell*s) - 1;
                y2 = y1;

                x1 = 0;
                x2 = mapWidth - 1;

                g.DrawLine(myPen, x1, y1, x2, y2);
            }

            return _image;
        }

        static public Color GetColorOfPoint(Bitmap b, int x, int y, int str)
        {
           return b.GetPixel(x * str * Cell, y * str * Cell);
        }

        static public Bitmap DrawRectanglePointToMap(Bitmap b, int x, int y, int str,  Color color)
        {
            Graphics g = Graphics.FromImage(b);

            g.FillRectangle(new SolidBrush(color), x * str * Cell, y * str * Cell, str * Cell, str * Cell);

            return b;
        }

        public void SaveImage(String wayName)
        {
            //image.Save(wayName);

            SaveData sv = new SaveData();

            sv.Image = image;
            sv.Strechcell = strCell;

            MemoryStream memorystream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(wayName, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, sv);

                //Console.WriteLine("Объект сериализован");
            }

            //byte[] yourBytesToDb = memorystream.ToArray();
        }

        public int LoadImage(String loadName)
        {
            //image = new Bitmap(loadName);
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(loadName, FileMode.Open, FileAccess.Read))
            {
                SaveData newPerson = (SaveData)formatter.Deserialize(fs);

                image = newPerson.Image;
                strCell = newPerson.Strechcell;
            }
            return strCell;
        }

        public static SaveData LoadSaveData(String loadName)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SaveData newPerson = new MainClasses.SaveData();

            using (FileStream fs = new FileStream(loadName, FileMode.Open, FileAccess.Read))
            {
                newPerson = (SaveData)formatter.Deserialize(fs);

       
            }

            return newPerson;
        }
    }
    [Serializable]
    public class SaveData
    {
        public Bitmap Image { get; set; }
        public int Strechcell{ get; set; }

    }


    public enum Occyped
    {
        Occyped, NoOccyped
    }
}
