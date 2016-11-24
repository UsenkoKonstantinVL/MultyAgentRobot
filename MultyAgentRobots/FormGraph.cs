﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl;
using Microsoft.Msagl.Drawing;
using Color = Microsoft.Msagl.Drawing.Color;
using Node = Microsoft.Msagl.Drawing.Node;
using Shape = Microsoft.Msagl.Drawing.Shape;
using MyGraph = MultyAgentRobots.MainClasses.ControlSystem;
using MultyAgentRobots.MainClasses;

namespace MultyAgentRobots
{
    public partial class FormGraph : Form
    {
        public FormGraph()
        {
            InitializeComponent();
        }

        private void FormGraph_Load(object sender, EventArgs e)
        {

            Microsoft.Msagl.
            GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            this.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(viewer);
            this.ResumeLayout();
        }

        public void PrintGraph(List<MyGraph.Graph> GraphTree)
        {
            if(GraphTree != null)
            {
                this.Controls.Clear();
                Microsoft.Msagl.
               GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                //create a graph object 
                Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

                foreach (MyGraph.Graph gr in GraphTree)
                {
                   

                    String GraphName = "";

                    foreach(MyGraph.Point2D point in gr.LinkPoints)
                    {
                        GraphName += point.X + ":" + point.Y + Environment.NewLine;

                        
                    }

                    GraphName += gr.Condition.ToString();

                    foreach (MyGraph.Graph SubGraph in gr.Graphs)
                    {
                        

                        String SubGraphName = "";

                        foreach (MyGraph.Point2D point in SubGraph.LinkPoints)
                        {
                            SubGraphName += point.X + ":" + point.Y + Environment.NewLine;


                        }

                        SubGraphName += SubGraph.Condition.ToString();

                        graph.AddEdge(GraphName, SubGraphName);
                    }
                    
                }

                viewer.Graph = graph;
                //associate the viewer with the form 
                this.SuspendLayout();
                viewer.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Controls.Add(viewer);
                this.ResumeLayout();
            }
 
        }
        
        
        
         
    }
}
