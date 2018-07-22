using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.UI;

//using DB.Interfaces;
using Rsx.Dumb; using Rsx;

namespace k0X
{
    public class ucTreeView : TreeView
    {
        public void CheckNode(ref LINAA.SubSamplesRow s)
        {
            //so it does not run on populating...
            Populating = true;

            try
            {
                TreeNode old = MakeSampleNode(ref s);
                LINAA.MeasurementsRow[] measurements = s.GetMeasurementsRows();
                //old.Nodes.Clear();
                foreach (LINAA.MeasurementsRow m in measurements)
                {
                    try
                    {
                        LINAA.MeasurementsRow aux = m;
                        TreeNode MeasNode = MakeMeasurementNode(ref aux);
                        SetAMeasurementNode(ref MeasNode);
                        if (!old.Nodes.Contains(MeasNode))
                        {
                            MeasNode.Collapse();
                            old.Nodes.Add(MeasNode);    //add childrens already
                        }
                    }
                    catch (SystemException ex)
                    {
                        ucSample.Interface.IStore.AddException(ex);
                    }
                }
                SetASampleNode(ucSample.sampleDescription.Checked, ref old);
            }
            catch (SystemException ex)
            {
                ucSample.Interface.IStore.AddException(ex);
            }

            Populating = false;
        }

        private void CleanNodes()
        {
            if (ucSample.samples == null) return;
            foreach (LINAA.SubSamplesRow s in ucSample.samples)
            {
                TreeNode n = (TreeNode)s.Tag;
                if (n != null)
                {
                    try
                    {
                        if (n.TreeView != null)
                        {
                            IntPtr a = n.TreeView.Handle;
                        }
                        else s.Tag = null;
                    }
                    catch (ObjectDisposedException ex)
                    {
                        s.Tag = null;
                    }
                }
            }
        }

        public T GetNodeDataRow<T>()
        {
            object o = null;
            if (this.SelectedNode != null) o = this.SelectedNode.Tag;
            return (T)o;
        }

        public bool Populating
        {
            set
            {
                if (value) this.AfterCheck -= this.TV_AfterCheck;
                else this.AfterCheck += this.TV_AfterCheck;
            }
        }

        protected internal void BuildTV()
        {
            if (ucSample.IsEmpty()) return;

            ucSample.progress.Value = 0;

            Populating = true;

            foreach (TreeNode n in clon) n.Nodes.Clear();

            this.BeginUpdate();
            this.Nodes.Clear();
            this.Nodes.AddRange(clon); //adds template

            string filterby = (ucSample.Interface.Get() as LINAA).SubSamples.SubSampleTypeColumn.ColumnName;
            bool samDesc = ucSample.sampleDescription.Checked;
            //	monitors = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Mon, ref samples);
            //	subSamples = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Smp, ref  samples);
            //	standards = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Std, ref samples);
            //	refMaterials = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.RefMat, ref samples);
            //	blanks = GetSamplesNodes(samDesc, filterby, DB.Properties.Samples.Blk, ref samples);

            ucSample.Interface.IReport.Msg("Building nodes", "Building sample nodes for " + this.Name, true);
            Application.DoEvents();

            ucSample.progress.Maximum = ucSample.samples.Count();

            CleanNodes();

            //  IList<TreeNode> samplesNodes = new List<TreeNode>();
            foreach (LINAA.SubSamplesRow sample in ucSample.samples)
            {
                LINAA.SubSamplesRow aux = sample;

                if (EC.IsNuDelDetch(aux)) continue;
                TreeNode SampleNode = MakeSampleNode(ref aux);

                // SetASampleNode(DescriptionAsNodeTitle, ref SampleNode);

                TreeNodeCollection col = Nodes[sample.SubSampleType].Nodes;

                TreeNode toAdd = SampleNode;
                if (ucSample.IsClone)
                {
                    toAdd = (TreeNode)SampleNode.Clone();
                    toAdd.Tag = sample;
                }

                //samplesNodes.Add( SampleNode);
                if (!col.Contains(toAdd))
                {
                    if (toAdd.Parent != null) toAdd.Remove();
                    col.Add(toAdd);
                }

                toAdd.Collapse();

                ucSample.progress.PerformStep();
            }

            IEnumerable<TreeNode> basis = this.Nodes.OfType<TreeNode>();
            IEnumerable<TreeNode> toDel = basis.Where(c => c.GetNodeCount(false) == 0).ToList();
            foreach (TreeNode n in toDel) n.Remove();

            this.CollapseAll();

            TreeNode first = basis.FirstOrDefault();
            if (first != null) first.ExpandAll();

            this.EndUpdate();
            Populating = false;
        }

        private static TreeNode MakeMeasurementNode(ref LINAA.MeasurementsRow m)
        {
            TreeNode MeasNode = m.Tag as TreeNode;
            if (MeasNode == null)
            {
                MeasNode = new TreeNode();

                MeasNode.Name = m.MeasurementID.ToString();   //name of node
                MeasNode.Text = MeasNode.Name;
                //linking
                MeasNode.Tag = m; //link toRow measurement interface
                m.Tag = MeasNode;   //backlink toRow measurement interface
            }

            return MeasNode;
        }

        private static TreeNode MakeSampleNode(ref LINAA.SubSamplesRow sample)
        {
            TreeNode SampleNode = sample.Tag as TreeNode;

            if (SampleNode == null)
            {
                SampleNode = new TreeNode();
                SampleNode.Name = sample.SubSamplesID.ToString();
                SampleNode.Text = SampleNode.Name;
                SampleNode.Tag = sample; //link subsample Row  or subsample interface
                sample.Tag = SampleNode;
            }

            return SampleNode;
        }

        private static void SetAMeasurementNode(ref TreeNode MeasNode)
        {
            if (IsBadNode(ref MeasNode)) return;
            LINAA.MeasurementsRow m = MeasNode.Tag as LINAA.MeasurementsRow;

            if (m.IsSelectedNull()) m.Selected = true;

            MeasNode.Text = m.Detector + " " + m.Position + " " + m.MeasurementNr;
            MeasNode.ToolTipText = "Detector: " + m.Detector + "\nPosition: " + m.Position + "\nCounting Time: " + m.CountTime.ToString() + "s\nStarted: " + m.MeasurementStart.ToString();

            if (!m.NeedsPeaks) MeasNode.ImageKey = "green";
            else MeasNode.ImageKey = "red";
            MeasNode.Checked = m.Selected;
            if (!m.SubSamplesRow.Selected && m.Selected) m.SubSamplesRow.Selected = true;
        }

        private static bool IsBadNode(ref TreeNode checkme)
        {
            if (checkme.Tag == null) return true;

            System.Data.DataRow row = checkme.Tag as System.Data.DataRow;

            if (EC.IsNuDelDetch(row))
            {
                checkme.Remove();
                return true;
            }
            return false;
        }

        private static void SetASampleNode(bool DescriptionAsNodeTitle, ref TreeNode SampleNode)
        {
            if (IsBadNode(ref SampleNode)) return;

            LINAA.SubSamplesRow sample = SampleNode.Tag as LINAA.SubSamplesRow;

            if (sample.IsSubSampleNameNull()) SampleNode.Text = sample.SubSamplesID.ToString();
            else SampleNode.Text = sample.SubSampleName;

            //if cadmium put in gray

            if (!sample.IsENAANull() && sample.ENAA) SampleNode.ForeColor = System.Drawing.Color.Gray;
            else SampleNode.ForeColor = System.Drawing.Color.Blue;

            SampleNode.ImageKey = "red";

            if (!sample.HasErrors())
            {
                if (!sample.NeedsSSF && !sample.NeedsMeasurements && !sample.NeedsPeaks) SampleNode.ImageKey = "green";
            }
            SampleNode.Checked = sample.Selected;

            if (DescriptionAsNodeTitle)  //if sample text as description
            {
                if (!sample.IsSubSampleDescriptionNull()) SampleNode.Text = sample.SubSampleDescription;
                else SampleNode.Text = sample.SubSampleName;
                if (!sample.IsSubSampleNameNull()) SampleNode.ToolTipText = sample.SubSampleName;
            }
            else
            {
                if (!sample.IsSubSampleNameNull()) SampleNode.Text = sample.SubSampleName;
                if (!sample.IsSubSampleDescriptionNull()) SampleNode.ToolTipText = sample.SubSampleDescription;
            }
        }

        public ImageList imgList;
        private TreeNode[] clon;

        ///////////////////

        public ucTreeView()
        {
            this.SuspendLayout();

            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Monitors", -2, 2);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Standards", -2, 2);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Ref Materials", -2, 2);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("SubSamples", -2, 2);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Blanks", -2, 2);

            this.imgList = new ImageList();

            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.CheckBoxes = true;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.ImageIndex = 0;
            this.ImageList = this.imgList;
            this.Indent = 10;
            this.ItemHeight = 26;
            this.Location = new System.Drawing.Point(0, 0);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TV";
            treeNode1.ForeColor = System.Drawing.Color.Crimson;
            treeNode1.ImageIndex = -2;
            treeNode1.Name = "Monitor";
            treeNode1.SelectedImageIndex = 2;
            treeNode1.Text = "Monitors";
            treeNode2.ForeColor = System.Drawing.Color.Indigo;
            treeNode2.ImageIndex = -2;
            treeNode2.Name = "Standard";
            treeNode2.SelectedImageIndex = 2;
            treeNode2.Text = "Standards";
            treeNode3.ForeColor = System.Drawing.Color.DarkGoldenrod;
            treeNode3.ImageIndex = -2;
            treeNode3.Name = "RefMaterial";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Text = "Ref Materials";
            treeNode4.ForeColor = System.Drawing.Color.DarkGreen;
            treeNode4.ImageIndex = -2;
            treeNode4.Name = "SubSample";
            treeNode4.SelectedImageIndex = 2;
            treeNode4.Text = "SubSamples";
            treeNode5.ForeColor = System.Drawing.Color.Black;
            treeNode5.ImageIndex = -2;
            treeNode5.Name = "Blank";
            treeNode5.NodeFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.SelectedImageIndex = 2;
            treeNode5.Text = "Blanks";
            Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.SelectedImageIndex = 2;
            this.ShowLines = false;
            this.ShowNodeToolTips = true;
            this.ShowPlusMinus = false;
            this.Size = new System.Drawing.Size(186, 454);
            this.TabIndex = 4;
            this.clon = new TreeNode[this.Nodes.Count];

            this.Nodes.CopyTo(clon, 0);
            this.ShowPlusMinus = false;

            this.ResumeLayout();
        }

        public void Set()
        {
            this.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TV_ItemDrag);
            this.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.TV_NodeMouseHover);
            this.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TV_NodeMouseClick);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.TV_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.TV_DragEnter);

            ucSample.Analysis.CheckedChanged += new System.EventHandler(this.Analysis_CheckedChanged);
        }

        //   public Interface Interface;
        public ucSamples ucSample;

        private void TV_DragDrop(object sender, DragEventArgs e)
        {
            ucSamples s2ToAadd = (ucSamples)e.Data.GetData(typeof(ucSamples));

            if (this.Name.CompareTo(s2ToAadd.Name) != 0)
            {
                this.ucSample.Orderbox.Visible = true;
                s2ToAadd.Orderbox.Visible = true;

                //// Open an Order based on 2 ucSamples controls
                if (this.ucSample.ucOrder == null && s2ToAadd.ucOrder == null)
                {
                    this.ucSample.Orderbox.Text = "Please set a New Name";
                    //make new
                    LINAA Linaa = (LINAA)ucSample.Interface.Get();
                    this.ucSample.ucOrder = new ucOrder(ref Linaa, ucSample.Orderbox.Text);
                    ucSamples s1 = ucSample;
                    this.ucSample.ucOrder.AddProject(ref s1);
                }
                if (s2ToAadd.ucOrder == null && ucSample.ucOrder != null)
                {
                    ucSample.ucOrder.AddProject(ref s2ToAadd);
                }
                else if (ucSample.ucOrder == null && s2ToAadd.ucOrder != null)
                {
                    ucSamples s1 = ucSample;
                    s2ToAadd.ucOrder.AddProject(ref s1);
                }
            }
        }

        private void TV_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void TV_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!this.Enabled) return;     //so it does not run on tv disabled...

            object nodeTag = e.Node.Tag;  //get node tag
            bool useranalysing = false;  //by default
            System.Data.DataRow row = null;
            Type tipo = null;
            if (nodeTag != null)
            {
                row = e.Node.Tag as System.Data.DataRow;
                tipo = row.GetType();
                if (tipo.Equals(typeof(LINAA.SubSamplesRow))) useranalysing = true;
            }
            else useranalysing = true;

            if (useranalysing && Tag == null) ucSample.Interface.IPopulate.ISamples.BeginEndLoadData(true);
            bool measNode = true;

            if (nodeTag == null) //selection on MAIN NODE
            {
                Tag = true;
                foreach (TreeNode node in e.Node.Nodes) node.Checked = e.Node.Checked;
                Tag = null;
            }
            else
            {
                if (EC.IsNuDelDetch(row))
                {
                    MessageBox.Show("The Node's linked DataRow was found in a deleted or detached state!", "Node problems with " + e.Node.Text);
                    //this.TV.Nodes.RemoveByKey(e.Node.Name);
                }
                else
                {
                    if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
                    {
                        LINAA.SubSamplesRow s = row as LINAA.SubSamplesRow;
                        //avoid refreshing the SR while deselecting measurements...
                        //speeds up incredibly the system...
                        s.Selected = e.Node.Checked;
                        Populating = true;
                        IEnumerable<LINAA.MeasurementsRow> meas = s.GetMeasurementsRows();
                        foreach (LINAA.MeasurementsRow m in meas) m.Selected = e.Node.Checked;
                        foreach (TreeNode node2 in e.Node.Nodes) node2.Checked = e.Node.Checked;

                        measNode = false;

                        Populating = false;
                    }
                    else
                    //if (tipo.Equals(typeof(LINAA.MeasurementsRow)))
                    {
                        LINAA.MeasurementsRow iMeas = (LINAA.MeasurementsRow)row;
                        iMeas.Selected = e.Node.Checked;
                        LINAA.SubSamplesRow s = iMeas.SubSamplesRow;
                        bool allunchecked = (s.GetMeasurementsRows().Where(m => !m.IsSelectedNull() && m.Selected).Count() == 0);
                        Populating = true;
                        s.Selected = !allunchecked;
                        e.Node.Parent.Checked = !allunchecked;

                        Populating = false;
                    }
                }
            }

            if (Tag == null)
            {
                if (useranalysing) ucSample.Interface.IPopulate.ISamples.BeginEndLoadData(false);
                TreeNode node = e.Node;
                if (measNode && e.Node.Parent != null) node = e.Node.Parent;

                TreeNodeMouseClickEventArgs args = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 1, node.Bounds.X, node.Bounds.Y);
                this.TV_NodeMouseClick(null, args);
            }
        }

        private void TV_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left)) return;

            this.DoDragDrop(this, DragDropEffects.Copy);
        }

        private void TV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool collapse = sender != null;
            //when sender is null is because I artificially called the method

            System.Drawing.Image red = imgList.Images["red"];
            System.Drawing.Image green = imgList.Images["green"];
            ucSample.Analysis.Checked = ucSample.Interface.IPreferences.CurrentPref.UsrAnal;

            if (e.Node.Tag == null)
            {
                this.CollapseAll();
                e.Node.Expand();
                this.TopNode = e.Node.PrevNode;
                return;
            }
            //datarow is null?
            System.Data.DataRow row = e.Node.Tag as System.Data.DataRow;
            if (EC.IsNuDelDetch(row))
            {
                e.Node.Remove();
                return;
            }

            //mouse intersects with node?
            System.Drawing.Rectangle rectangle = e.Node.Bounds;
            bool intersects = rectangle.IntersectsWith(new System.Drawing.Rectangle(e.X, e.Y, 1, 1));
            if (!intersects) return;
            bool rightClick = e.Button.Equals(MouseButtons.Right);

            Type type = row.GetType();

            if (type.Equals(typeof(LINAA.SubSamplesRow)))
            {
                LINAA.SubSamplesRow l = (LINAA.SubSamplesRow)e.Node.Tag;

                //show sample contextual menu
                if (rightClick)
                {
                    ucSample.ViewMatSSF.Image = red;
                    ucSample.ViewMatSSF.Enabled = false;
                    if (!l.NeedsSSF)
                    {
                        ucSample.ViewMatSSF.Enabled = true;
                        if (!EC.HasErrors(l.UnitRow.GetMatSSFRows())) ucSample.ViewMatSSF.Image = green;
                    }

                    ucSample.MeasurementsHyperLab.Enabled = false;
                    ucSample.MeasurementsHyperLab.Image = red;
                    if (!l.NeedsMeasurements)
                    {
                        ucSample.MeasurementsHyperLab.Enabled = true;
                        if (!EC.HasErrors(l.GetMeasurementsRows()))
                        {
                            ucSample.MeasurementsHyperLab.Image = green;
                        }
                    }

                    e.Node.ContextMenuStrip = ucSample.CMSSample;
                    this.SelectedNode = e.Node;

                    ucSample.Analysis.Checked = ucSample.Interface.IPreferences.CurrentPref.UsrAnal;

                    ucSample.CMSSample.Show(Control.MousePosition.X, Control.MousePosition.Y);
                }

                //show analysis
                else
                {
                    if (SelectedNode != null)
                    {
                        bool sameNode = SelectedNode.Equals(e.Node);  //different node than before...
                        if (!sameNode) this.SelectedNode.Collapse();   //collapse last selected node for expanding the next one...
                    }

                    this.SelectedNode = e.Node;

                    if (ucSample.Analysis.Checked)
                    {
                        //  if (node.IsExpanded) node.Collapse();
                        if (!l.Selected) e.Node.Checked = true;  //selects all measurements when the user forgot to select at least 1
                        else
                        {
                            MainForm main = Application.OpenForms.OfType<MainForm>().First();
                            main.Panel_Click(this, EventArgs.Empty);
                            ucSample.pTable = LIMSUI.UserControls.OfType<ucPeriodicTable>().Where(o => !o.IsDisposed).FirstOrDefault();
                            if (ucSample.pTable != null) ucSample.pTable.Query(ref l);
                        }
                    }

                    if (e.Node.IsExpanded && collapse && !ucSample.Analysis.Checked) e.Node.Collapse();
                    else if (ucSample.Analysis.Checked) e.Node.ExpandAll();
                }
            }
            else if (type.Equals(typeof(LINAA.MeasurementsRow)))
            {
                if (!rightClick) return;

                #region Rightclick

                LINAA.MeasurementsRow m = (LINAA.MeasurementsRow)e.Node.Tag;
                ucSample.Peaks.Image = red;
                ucSample.Peaks.Enabled = false;
                if (!m.NeedsPeaks)
                {
                    ucSample.Peaks.Enabled = true;
                    if (!EC.HasErrors(m.GetPeaksRows()))
                    {
                        ucSample.Peaks.Image = green;
                    }
                }
                e.Node.ContextMenuStrip = ucSample.CMSMeasurement;
                this.SelectedNode = e.Node;
                ucSample.CMSMeasurement.Show(Control.MousePosition.X, Control.MousePosition.Y);

                #endregion Rightclick
            }
        }

        private void Analysis_CheckedChanged(object sender, EventArgs e)
        {
            ucSample.Interface.IPreferences.CurrentPref.UsrAnal = ucSample.Analysis.Checked;
        }

        private void TV_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (e.Node.Tag == null) return;

            DataRow row = e.Node.Tag as DataRow;

            if (EC.IsNuDelDetch(row))
            {
                e.Node.ForeColor = System.Drawing.Color.Red;
                e.Node.Tag = null;
                return;
            }

            if (row.HasErrors)
            {
                string text = "Errors in:\n\n";
                DataColumn[] array = row.GetColumnsInError();
                foreach (DataColumn column in array)
                {
                    text += column.ColumnName + ": " + row.GetColumnError(column) + "\n";
                }
                e.Node.ToolTipText = text;
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTreeView));
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            //
            // imgList
            //
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "green");
            this.imgList.Images.SetKeyName(1, "red");
            this.imgList.Images.SetKeyName(2, "neutral");
            this.ResumeLayout(false);
        }

        private System.ComponentModel.IContainer components;
    }
}