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
            pictureBox2.Image = manager.InitizlizeMap();

        }
        int strech = 1;
        private void Button1_Click(object sender, EventArgs e)
        {
            strech++;
            /*if (strech == 3)
                strech = 4;*/
            if (strech > 4)
                strech = 4;
            pictureBox2.Image = manager.SetStretch(strech);
            drawstrech();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strech--;
           /* if (strech == 3)
                strech = 2;*/
            if (strech < 1)
                strech = 1;
            pictureBox2.Image = manager.SetStretch(strech);
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            pictureBox2.Image = manager.DrawCell(e.X, e.Y, oc, strech);
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                oc = Occyped.NoOccyped;
            }
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                oc = Occyped.Occyped;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();

            if(s.ShowDialog() == DialogResult.OK )
            {
                manager.SaveImage(s.FileName);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
               strech =  manager.LoadImage(f.FileName);
                drawstrech();
                pictureBox2.Image = manager.InitizlizeMap();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormMap_Load(object sender, EventArgs e)
        {

        }
    }
}
