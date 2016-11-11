using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultyAgentRobots.MainClasses;

namespace MultyAgentRobots
{
    public partial class FormWorkArea : Form
    {
        FormWorkAreaManager manager;
        public FormWorkArea(List<RobotController> robots, SaveData data)
        {
            InitializeComponent();
            timer1.Enabled = false;
            manager = new FormWorkAreaManager(pictureBox1, label2, robots, data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string n1 = "Старт";
            string n2 = "Стоп";
            if (button1.Text == n1) { 
                timer1.Enabled = true;
                button1.Text = n2;
            }
            else
            {
                timer1.Enabled = false;
                button1.Text = n1;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            manager.Work();
        }

        private void FormWorkArea_Load(object sender, EventArgs e)
        {
            manager.InitialaizeRobotsAndMap();
        }
    }
}
