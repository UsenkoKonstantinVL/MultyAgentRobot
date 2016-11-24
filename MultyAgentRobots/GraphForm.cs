using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace MultyAgentRobots
{
    public partial class GraphForm : Form
    {
        ZedGraphControl zedGraph;
        ZedGraphControl zedGraph2;

        public GraphForm()
        {
            InitializeComponent();
            zedGraph = new ZedGraphControl();
            //ZedGraph.
            Control c = zedGraph;
            c.Dock = DockStyle.Fill;
            panel3.Controls.Add(zedGraph);

            zedGraph2 = new ZedGraphControl();
            //ZedGraph.
            c = zedGraph2;
            c.Dock = DockStyle.Fill;
            panel4.Controls.Add(zedGraph2);
        }

        public void DrawGraph(double TotalTime, List<double> ListTime)
        {
            DrawGraph1(ListTime);
            DrawGraph2(TotalTime, ListTime);

            label1.Text = "Общее кол-во тиков: " + TotalTime.ToString();
        }

        private void DrawGraph1(List<double> ListTime)
        {
            GraphPane pane = zedGraph.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            pane.Title.Text = "Время работы роботов";
            pane.XAxis.Title.Text = "Работ";
            pane.YAxis.Title.Text = "Время(тик)";

            int itemscount = 5;

            double[] list = new double[ListTime.Count];
            double[] xList = new double[ListTime.Count];

            for (int i = 0; i < ListTime.Count; i++)
            {
                xList[i] = i + 1;
                list[i] = ListTime[i];
            }

            BarItem bar1 = pane.AddBar("RobotsTime", xList, list, Color.Blue);

            // !!! Расстояния между столбиками в кластере (группами столбиков)
            pane.BarSettings.MinBarGap = 0.0f;

            // !!! Увеличим расстояние между кластерами в 2.5 раза
            pane.BarSettings.MinClusterGap = 2.5f;


            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();
        }

        private void DrawGraph2(double TotalTime, List<double> ListTime)
        {
            GraphPane pane = zedGraph2.GraphPane;

            // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
            pane.CurveList.Clear();

            pane.Title.Text = "Время работы роботов в %";
            pane.XAxis.Title.Text = "Работ";
            pane.YAxis.Title.Text = "Время(% от общего кол-ва тиков)";

            int itemscount = 5;

            double[] list = new double[ListTime.Count];
            double[] xList = new double[ListTime.Count];

            for (int i = 0; i < ListTime.Count; i++)
            {
                xList[i] = i + 1;
                list[i] = ListTime[i] / TotalTime;
            }

            BarItem bar1 = pane.AddBar("RobotsTimeInPercent", xList, list, Color.Blue);

            // !!! Расстояния между столбиками в кластере (группами столбиков)
            pane.BarSettings.MinBarGap = 0.0f;

            // !!! Увеличим расстояние между кластерами в 2.5 раза
            pane.BarSettings.MinClusterGap = 2.5f;


            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            zedGraph2.AxisChange();

            // Обновляем график
            zedGraph2.Invalidate();
        }
    }
}
