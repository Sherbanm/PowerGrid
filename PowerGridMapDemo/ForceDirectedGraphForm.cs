using PowerGrid.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace PowerGridMapDemo
{
    public partial class ForceDirectedGraphForm : Form
    {
        const int width = 64;
        const int height = 32;
        Stopwatch stopwatch = new Stopwatch();

        PositionalGraph m_fdgGraph;
        ForceDirected2D m_fdgPhysics;
        Renderer m_fdgRenderer;

        System.Timers.Timer timer = new System.Timers.Timer(30);


        public ForceDirectedGraphForm()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.Width = (width + 1) * 20;
            this.Height = (height + 1) * 20 + 100;
            this.MaximumSize = new Size(this.Width, this.Height);
            this.MaximizeBox = false;

            tbStiffness.Text = "81.76";
            tbRepulsion.Text = "20000.0";
            tbDamping.Text = "0.5";
            
            m_fdgGraph = new PositionalGraph();
            m_fdgPhysics = new ForceDirected2D(m_fdgGraph, 81.76f,20000.0f, 0.5f);
            m_fdgRenderer = new Renderer(m_fdgPhysics, pDrawPanel.Size.Height, pDrawPanel.Size.Width);
           

            pDrawPanel.Paint += new PaintEventHandler(DrawPanel_Paint);

            timer.Interval = 30;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start();

        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pDrawPanel.Invalidate();
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
            stopwatch.Stop();
            
            m_fdgRenderer.Draw(0.05f, e); // TODO: Check Time

            stopwatch.Reset();
            stopwatch.Start();
        }

        private void btnChangeProperties_Click(object sender, EventArgs e)
        {
            float stiffNess = Convert.ToSingle(tbStiffness.Text);
            m_fdgPhysics.Stiffness = stiffNess;
            float repulsion = Convert.ToSingle(tbRepulsion.Text);
            m_fdgPhysics.Repulsion = repulsion;
            float damping = Convert.ToSingle(tbDamping.Text);
            m_fdgPhysics.Damping = damping;
        }

        private bool addNode(string nodeName, string region)
        {
            nodeName = nodeName.Trim();
            if (m_fdgGraph.GetNode(tbNodeName.Text) != null)
            {
                return false;
            }
            GridBox gridBox = new GridBox(0, 0, BoxType.Normal, nodeName);
            City newNode = m_fdgGraph.CreateNode(nodeName, region, gridBox);
            m_fdgRenderer.Boxes[newNode] = gridBox;

            cbbFromNode.Items.Add(nodeName);
            cbbToNode.Items.Add(nodeName);
            lbNode.Items.Add(nodeName);
            return true;
        }
        private void btnAddNode_Click(object sender, EventArgs e)
        {
            tbNodeName.Text=tbNodeName.Text.Trim();
            if (tbNodeName.Text == "")
            {
                MessageBox.Show("Please type in the node name to insert!");
                return;
            }
            if (!addNode(tbNodeName.Text, string.Empty))
            {
                MessageBox.Show("Node already exists in the graph!");
                return;
            }
        }
        private bool addEdge(string nodeName1, string nodeName2, int length)
        {
            nodeName1 = nodeName1.Trim();
            nodeName2 = nodeName2.Trim();
            if (nodeName1 == nodeName2)
            {
                return false;
            }
            City node1 = m_fdgGraph.GetNode(nodeName1);
            City node2 = m_fdgGraph.GetNode(nodeName2);
            
            string label = nodeName1 + "-" + nodeName2;
            
            var gridLine = new GridLine(0, 0, 0, 0);
            Connection newEdge = m_fdgGraph.CreateEdge(node1, node2, length, label, gridLine);
            m_fdgRenderer.Lines[newEdge] = gridLine;

            lbEdge.Items.Add(label);
            return true;
        }
        private void btnAddEdge_Click(object sender, EventArgs e)
        {
            string nodeName1 = cbbFromNode.Text;
            string nodeName2 = cbbToNode.Text;
            if (!addEdge(nodeName1,  nodeName2, 0))
            {
                MessageBox.Show("Edge cannot be connected to same node!");
                return;
            }
        }

        private void btnRemoveNode_Click(object sender, EventArgs e)
        {
            if (lbNode.SelectedIndex != -1)
            {
                int selectedIdx = lbNode.SelectedIndex;
                string nodeName=(string)lbNode.SelectedItem;
                City removeNode=m_fdgGraph.GetNode(nodeName);

                m_fdgRenderer.Boxes.Remove(removeNode);
                List<Connection> edgeList = m_fdgGraph.GetEdges(removeNode);
                foreach(Connection edge in edgeList)
                {
                    m_fdgRenderer.Lines.Remove(edge);
                    int edgeIndex=lbEdge.FindString(edge.CityA.Name + "_" + edge.CityB.Name);
                    lbEdge.Items.RemoveAt(edgeIndex);
                }
                m_fdgGraph.RemoveNode(removeNode);


                cbbFromNode.Items.RemoveAt(lbNode.SelectedIndex);
                cbbToNode.Items.RemoveAt(lbNode.SelectedIndex);

                lbNode.Items.RemoveAt(lbNode.SelectedIndex);
                if (selectedIdx != 0)
                    lbNode.SelectedIndex = selectedIdx-1;
                lbNode.Focus();
            }
            else
            {
                MessageBox.Show("Please select a node to remove!");
            }
        }

        private void btnRemoveEdge_Click(object sender, EventArgs e)
        {
            if (lbEdge.SelectedIndex != -1)
            {
                int selectedIdx = lbEdge.SelectedIndex;
                string edgeName = (string)lbEdge.SelectedItem;
                Connection removeEdge = m_fdgGraph.GetEdge(edgeName);
                m_fdgRenderer.Lines.Remove(removeEdge);
                m_fdgGraph.RemoveEdge(removeEdge);
                lbEdge.Items.RemoveAt(lbEdge.SelectedIndex);
                if (selectedIdx != 0)
                {
                    lbEdge.SelectedIndex = selectedIdx - 1;
                }
                lbEdge.Focus();
            }
            else
            {
                MessageBox.Show("Please select an edge to remove!");
            }
        }

        City clickedNode;
        GridBox clickedGrid;
        private void pDrawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (KeyValuePair<City, GridBox> keyPair in m_fdgRenderer.Boxes)
            {
                if(keyPair.Value.boxRec.IntersectsWith(new Rectangle(e.Location,new Size(1,1))))
                {
                    clickedNode = keyPair.Key;
                    clickedGrid = keyPair.Value;
                    clickedGrid.boxType = BoxType.Pinned;

                    if (e.Button == MouseButtons.Left)
                        clickedNode.Build = true;
                    else if (e.Button == MouseButtons.Right)
                        clickedNode.Build = false;
                }
            }
        }

        private void pDrawPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (clickedNode != null)
            {

                FDGVector2 vec = m_fdgRenderer.ScreenToGraph(new Tuple<int, int>(e.Location.X, e.Location.Y));
                clickedGrid.boxType = BoxType.Pinned;
                var fd = m_fdgRenderer.forceDirected as ForceDirected2D;
                var clicked = fd.graph.NodesWithGridBox.First(x => x.Key.Equals(clickedNode));
                m_fdgPhysics.GetPoint(clicked).position = vec;
            }
            else
            {
                foreach (KeyValuePair<City, GridBox> keyPair in m_fdgRenderer.Boxes)
                {
                    if(keyPair.Value.boxRec.IntersectsWith(new Rectangle(e.Location,new Size(1,1))))
                    {
                        keyPair.Value.boxType = BoxType.Pinned;
                        foreach (var gridLine in m_fdgGraph.ConnectionsWithGridLines)
                        {
                            gridLine.Value.Highlight = false;
                        }
          
                        var path = this.m_fdgGraph.CalculateCostToNetwork(keyPair.Key);
                        keyPair.Value.Cost = path.Length;
                        foreach (var edge in path.Edges)
                        {
                            m_fdgGraph.ConnectionsWithGridLines[edge].Highlight = true;
                        }
                    }
                    else
                    {
                        keyPair.Value.boxType = BoxType.Normal;

                    }
                }

            }
        }

        private void pDrawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (clickedNode != null)
            {
                clickedGrid.boxType = BoxType.Normal;
                clickedNode = null;
                clickedGrid = null;
            }
        }

        private void tbNodeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddNode_Click(sender, e);
                tbNodeName.Focus();
            }
        }

        private void tbStiffness_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChangeProperties_Click(sender, e);
                tbStiffness.Focus();
            }
        }

        private void tbRepulsion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChangeProperties_Click(sender, e);
                tbRepulsion.Focus();
            }
        }

        private void tbDamping_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChangeProperties_Click(sender, e);
                tbDamping.Focus();
            }
        }

        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = fileDialog.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    Int32 size = text.Length;

                    StringReader mapXML = new StringReader(text);
                    XmlTextReader xmlReader = new XmlTextReader(mapXML);
                    while (xmlReader.Read())
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element: // The node is an Element.
                                if (xmlReader.Name == "Node")
                                {
                                    loadNode(xmlReader);
                                }
                                else if (xmlReader.Name == "Edge")
                                {
                                    loadEdge(xmlReader);
                                }
                                break;

                            case XmlNodeType.Text: //Display the text in each element.
                                break;
                            case XmlNodeType.EndElement: //Display end of element.
                                break;
                            case XmlNodeType.Whitespace: //Display end of element.
                                break;
                        }
                    }
                }
                catch (IOException)
                {
                }
            }


        }

        private void loadNode(XmlTextReader iXmlReader)
        {
            string name = string.Empty;
            string region = string.Empty;

            while (iXmlReader.MoveToNextAttribute()) // Read attributes.
            {
                if (iXmlReader.Name == "nodeName")
                    name = iXmlReader.Value;
                if (iXmlReader.Name == "region")
                    region = iXmlReader.Value;
            }
            addNode(name, region);
        }
        private void loadEdge(XmlTextReader iXmlReader)
        {
            string nodeName1 = null;
            string nodeName2 = null;
            int length = 0;
            while (iXmlReader.MoveToNextAttribute()) // Read attributes.
            {
                if (iXmlReader.Name == "nodeName1")
                    nodeName1=iXmlReader.Value;
                if (iXmlReader.Name == "nodeName2")
                    nodeName2 = iXmlReader.Value;
                if (iXmlReader.Name == "length")
                    length = int.Parse(iXmlReader.Value);
            }
            addEdge(nodeName1, nodeName2, length);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_fdgPhysics.Clear();
            m_fdgGraph.Clear();
            m_fdgRenderer.Boxes.Clear();
            m_fdgRenderer.Lines.Clear();
            lbEdge.Items.Clear();
            lbNode.Items.Clear();
            cbbFromNode.Items.Clear();
            cbbToNode.Items.Clear();
        }
    }
}
