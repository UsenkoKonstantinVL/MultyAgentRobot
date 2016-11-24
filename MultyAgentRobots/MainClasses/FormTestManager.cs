using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using MultyAgentRobots.MainClasses.ControlSystem;

namespace MultyAgentRobots.MainClasses
{
    public class FormTestManager
    {

        int X, Y;
        MapFormer mapformer;
        bool isLoaded;
        public FormTestManager()
        {
            X = -1;
            Y = -1;
            mapformer = new MapFormer();
            isLoaded = false;
        }

        public void SetData(SaveData d)
        {
            mapformer.SetData(d.Image, d.Strechcell);
            isLoaded = true;
        }

        Color color = Color.Yellow;

        public Bitmap ClickOnBitmap(int x, int y)
        {
            if(isLoaded)
            {
                X = x;
                Y = y;

                return mapformer.SetPoint(mapformer.SetColorIfNotBlack(x, y, color), X, Y);
            }
            else
            {
                return null;
            }
        }

        public Bitmap GetClearMap()
        {
            return mapformer.SetPoint(mapformer.GetOriginalClearMap(), X, Y);
        }

        public Bitmap MouseMove(int x, int y)
        {
            if (isLoaded)
            {
                return mapformer.SetPoint(mapformer.SetColorIfNotBlack(x, y, color), X, Y);
            }
            else
            {
                return null;
            }
        }

        public string GetPoint(int x, int y)
        {
            if(isLoaded)
            {
                var res = mapformer.GetPointOfMouse(x, y);
                return "X: " + res[0].ToString() + Environment.NewLine + "Y: " + res[1].ToString();
            }
            else
            {
                return "Загрузите карту...";
            }
        }

        public void StartWorking(string numberOfRobots)
        {
            if(X == -1)
            {
                throw new Exception("Загрузите данные и установите начальную точку...");
            }
            int i = 0;
            string _name = "Robot_";

            List<Color> colors = GetColors();
            List<RobotController> robots = new List<RobotController>();
            List<RobotInformation> rInfo = new List<RobotInformation>();


            Connector connector = new Connector();
            GraphManager manager = new GraphManager();

            manager.connector = connector;
                try {
                int count  = Int32.Parse(numberOfRobots);
                for(int j = 0; j < count; j++)
                {
                    RobotController r = new RobotController();
                    r.manager = manager;
                    RobotInformation info = new RobotInformation();
                    rInfo.Add(info);
                    info.Name = _name + i.ToString();
                    i++;
                    var res = mapformer.GetPointOfMouse(X, Y);
                    info.X = res[0];
                    info.Y = res[1];
                    info.RobotColor = GetRandomColor(colors);
                    //Цвет робота
                    r.robotInformation = info;
                    r.connector = connector;
                    r.Initialize();
                    robots.Add(r);
                }
                connector.infoAboutRobots = rInfo;
                connector.robots = robots;
                //После всего передаем данные
                //Создаем окно, передаем данные, открываем окно...

                FormWorkArea form = new FormWorkArea(robots, mapformer.GetSaveData());
                form.ShowDialog();

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message); 
            }

        }

        private List<Color> GetColors()
        {
            List<Color> colors = new List<Color>();


            colors.Add(Color.HotPink);
            colors.Add(Color.Yellow);
            colors.Add(Color.Green);
            colors.Add(Color.Blue);
            colors.Add(Color.Violet);
            colors.Add(Color.ForestGreen);
            colors.Add(Color.SkyBlue);
            colors.Add(Color.SeaGreen);
            colors.Add(Color.RoyalBlue);
            colors.Add(Color.Violet);


            return colors;
        }    

        private Color GetRandomColor(List<Color> colors)
        {
            Random randomGen = new Random();
            int i = randomGen.Next(colors.Count);
            Color c = colors[i];
            colors.RemoveAt(i);
            return c;

        }
    }
    public class StartData
    {
        public Bitmap map;
        public int X;
        public int Y;
    }

    public class MessageToStart
    {
        public List<RobotController> robots;
        public SaveData data;
    }
}
