using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;

namespace k0X
{
    public partial class ucSamples
    {
        private void TV_DragDrop(object sender, DragEventArgs e)
        {
            ucSamples s2ToAadd = (ucSamples)e.Data.GetData(typeof(ucSamples));

            if (this.Name.CompareTo(s2ToAadd.Name) != 0)
            {
                this.Orderbox.Visible = true;
                s2ToAadd.Orderbox.Visible = true;

                //// Open an Order based on 2 ucSamples controls
                if (this.ucOrder == null && s2ToAadd.ucOrder == null)
                {
                    this.Orderbox.Text = "Please set a New Name";
                    //make new
                    LINAA Linaa = (LINAA)Interface.Get();
                    this.ucOrder = new ucOrder(ref Linaa, this.Orderbox.Text);
                    ucSamples s1 = this;
                    this.ucOrder.AddProject(ref s1);
                }
                if (s2ToAadd.ucOrder == null && ucOrder != null)
                {
                    this.ucOrder.AddProject(ref s2ToAadd);
                }
                else if (ucOrder == null && s2ToAadd.ucOrder != null)
                {
                    ucSamples s1 = this;
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
            if (!this.TV.Enabled) return;		  //so it does not run on tv disabled...

            object nodeTag = e.Node.Tag;	//get node tag
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

            if (useranalysing && TV.Tag == null) Interface.IPopulate.ISamples.LoadSampleData(true);
            bool measNode = true;

            if (nodeTag == null) //selection on MAIN NODE
            {
                TV.Tag = true;
                foreach (TreeNode node in e.Node.Nodes) node.Checked = e.Node.Checked;
                TV.Tag = null;
            }
            else
            {
                if (Dumb.IsNuDelDetch(row))
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

            if (TV.Tag == null)
            {
                if (useranalysing) Interface.IPopulate.ISamples.LoadSampleData(false);
                TreeNode node = node = e.Node;
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
            Analysis.Checked = Interface.IPreferences.CurrentPref.UsrAnal;

            if (e.Node.Tag == null)
            {
                this.TV.CollapseAll();
                e.Node.Expand();
                this.TV.TopNode = e.Node.PrevNode;
                return;
            }
            //datarow is null?
            System.Data.DataRow row = e.Node.Tag as System.Data.DataRow;
            if (Dumb.IsNuDelDetch(row))
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
                    this.ViewMatSSF.Image = red;
                    ViewMatSSF.Enabled = false;
                    if (!l.NeedsSSF)
                    {
                        ViewMatSSF.Enabled = true;
                        if (!Dumb.HasErrors(l.GetMatSSFRows())) this.ViewMatSSF.Image = green;
                    }

                    this.MeasurementsHyperLab.Enabled = false;
                    this.MeasurementsHyperLab.Image = red;
                    if (!l.NeedsMeasurements)
                    {
                        this.MeasurementsHyperLab.Enabled = true;
                        if (!Dumb.HasErrors(l.GetMeasurementsRows()))
                        {
                            this.MeasurementsHyperLab.Image = green;
                        }
                    }

                    e.Node.ContextMenuStrip = this.CMSSample;
                    this.TV.SelectedNode = e.Node;

                    Analysis.Checked = Interface.IPreferences.CurrentPref.UsrAnal;

                    this.CMSSample.Show(Control.MousePosition.X, Control.MousePosition.Y);
                }

                //show analysis
                else
                {
                    if (TV.SelectedNode != null)
                    {
                        bool sameNode = TV.SelectedNode.Equals(e.Node);  //different node than before...
                        if (!sameNode) this.TV.SelectedNode.Collapse();   //collapse last selected node for expanding the next one...
                    }

                    this.TV.SelectedNode = e.Node;

                    if (Analysis.Checked)
                    {
                        //  if (node.IsExpanded) node.Collapse();
                        if (!l.Selected) e.Node.Checked = true;  //selects all measurements when the user forgot to select at least 1
                        else
                        {
                            Program.MainForm.Analysis_Click(null, EventArgs.Empty);
                            pTable = Program.UserControls.OfType<ucPeriodicTable>().Where(o => !o.IsDisposed).FirstOrDefault();
                            if (pTable != null) pTable.Query(ref l);
                        }
                    }

                    if (e.Node.IsExpanded && collapse && !Analysis.Checked) e.Node.Collapse();
                    else if (Analysis.Checked) e.Node.ExpandAll();
                }
            }
            else if (type.Equals(typeof(LINAA.MeasurementsRow)))
            {
                if (!rightClick) return;

                #region Rightclick

                LINAA.MeasurementsRow m = (LINAA.MeasurementsRow)e.Node.Tag;
                this.Peaks.Image = red;
                Peaks.Enabled = false;
                if (!m.NeedsPeaks)
                {
                    Peaks.Enabled = true;
                    if (!Dumb.HasErrors(m.GetPeaksRows()))
                    {
                        this.Peaks.Image = green;
                    }
                }
                e.Node.ContextMenuStrip = this.CMSMeasurement;
                this.TV.SelectedNode = e.Node;
                this.CMSMeasurement.Show(Control.MousePosition.X, Control.MousePosition.Y);

                #endregion Rightclick
            }
        }

        private void Analysis_CheckedChanged(object sender, EventArgs e)
        {
            Interface.IPreferences.CurrentPref.UsrAnal = Analysis.Checked;
        }

        private void TV_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (e.Node.Tag == null) return;

            DataRow row = e.Node.Tag as DataRow;

            if (Dumb.IsNuDelDetch(row))
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
    }
}