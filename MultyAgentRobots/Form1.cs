using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultyAgentRobots
{
    public partial class Form1 : Form
    {
        FormMap formmap;
        Formtest tester;
        public Form1()
        {
            InitializeComponent();
        
            formmap = new MultyAgentRobots.FormMap();
            tester = new MultyAgentRobots.Formtest();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            formmap.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tester.ShowDialog();

        }
    }
}
