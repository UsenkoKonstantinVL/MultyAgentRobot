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
    public partial class FormMap : Form
    {
        MapEditManager manager;
        public FormMap()
        {
            InitializeComponent();
            drawstrech();
            radioButton1.Checked = true;
            oc = Occyped.NoOccyped;
            manager = new MapEditManager();

        }
        int strech = 1;
        private void Button1_Click(object sender, EventArgs e)
        {
            strech++;
            if (strech > 3)
                strech = 3;
            drawstrech();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strech--;
            if (strech < 1)
                strech = 1;
            drawstrech();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void drawstrech()
        {
            label1.Text = "размер: " + strech.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                oc = Occyped.NoOccyped;
            }
        }
        Occyped oc;
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                oc = Occyped.Occyped;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
