using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;

namespace k0X
{
    public partial class ucToDoPanel : UserControl
    {
        public ucToDoData ucToDoData = null;
        private AuxiliarForm auxform = null;
        internal ucSamples project1;
        internal ucSamples project2;

        private LINAA Linaa = null;

        public ucToDoPanel(ref LINAA set)
        {
            this.InitializeComponent();
            this.Undock1.Visible = false; //button hidden
            this.Undock2.Visible = false; //button hidden

            Linaa = set;

            ucToDoData = new ucToDoData(ref set);
            ucToDoData.Daddy = this;

            this.ToDoMenu.Items.Insert(0, ucToDoData.ToDoLabelBox);

            Dumb.FillABox(panel1box, set.ProjectsList, true, false);
            Dumb.FillABox(panel2box, set.ProjectsList, true, false);

            AuxiliarForm form = new AuxiliarForm();
            form.Text = "ToDo Panel - Simple View";
            UserControl control = this;
            form.Populate(ref control);

            form.Show();

            auxform = new AuxiliarForm();
            auxform.MaximizeBox = true;
            control = ucToDoData;
            auxform.Populate(ref control);
            auxform.Text = "ToDo Panel - Data View";

            View.PerformClick();
        }

        public void View_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (this.ParentForm.Visible)
            {
                ucToDoData.TS.Items.Insert(1, ucToDoData.ToDoLabelBox);
                auxform.Visible = true;
                this.ParentForm.Visible = false;
            }
            else
            {
                auxform.Visible = false;
                this.ToDoMenu.Items.Insert(0, ucToDoData.ToDoLabelBox);
                this.ParentForm.Visible = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void AddToDo_Click(object sender, EventArgs e)
        {
            if ((this.panel1.Controls.Count == 0) && (this.panel2.Controls.Count == 0)) return;

            IEnumerable<LINAA.IRequestsAveragesRow> ir = null;
            IEnumerable<LINAA.IRequestsAveragesRow> ir2 = null;

            this.ucToDoData.CheckIfEmptyToDo();

            if (sender.Equals(AddToDo))
            {
                LINAA.SubSamplesRow one = null;
                LINAA.SubSamplesRow two = null;

                one = this.project1.GetNodeDataRow<LINAA.SubSamplesRow>();
                two = this.project2.GetNodeDataRow<LINAA.SubSamplesRow>();

                if (one == null && two == null)
                {
                    MessageBox.Show("No sample data is associated to the nodes you have selected");
                    return;
                }

                if (this.panel1.Controls.Count != 0 && this.panel2.Controls.Count == 0)
                {
                    ir = one.GetIRequestsAveragesRows();
                    ir2 = ir;
                }
                if (this.panel1.Controls.Count == 0 && this.panel2.Controls.Count != 0)
                {
                    ir2 = two.GetIRequestsAveragesRows();
                    ir = ir2;
                }
                if (this.panel1.Controls.Count != 0 && this.panel2.Controls.Count != 0)
                {
                    ir = one.GetIRequestsAveragesRows();
                    ir2 = two.GetIRequestsAveragesRows();
                }
            }
            else
            {
                bool cdratio = true;

                ir = this.Linaa.IRequestsAverages.Where(r => r.Project.Contains(this.project1.Name)).ToList();
                ir2 = this.Linaa.IRequestsAverages.Where(r => r.Project.Contains(this.project2.Name)).ToList();

                if (cdratio)
                {
                    ir = ir.Where(o => !o.SubSamplesRow.ENAA).ToList();
                    ir2 = ir2.Where(o => o.SubSamplesRow.ENAA).ToList();
                }
            }

            if ((ir == null || ir2 == null) || (ir.Count() == 0 || ir2.Count() == 0))
            {
                MessageBox.Show("No isotopes data is associated to one of the Sample nodes you have selected");
                return;
            }

            bool Alike = true;
            this.Linaa.ToDo.AddToDoRow(ucToDoData.ToDoLabelBox.Text, Alike, ref ir, ref ir2);
            ucToDoData.SaveToDo_Click(sender, e);
            ucToDoData.Fill_Click(sender, e);
        }

        #region Panel Container

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ucToDoData.ListBS.Count != 0)
            {
                LINAA.ToDoRow t = Dumb.Cast<LINAA.ToDoRow>(ucToDoData.ListBS.Current as System.Data.DataRowView);

                if (EC.IsNuDelDetch(t.IRAvgRow)) return;
                if (EC.IsNuDelDetch(t.IRAvgRow2)) return;
                if (EC.IsNuDelDetch(t.IRAvgRow.SubSamplesRow)) return;
                if (EC.IsNuDelDetch(t.IRAvgRow2.SubSamplesRow)) return;

                string uno = t.IRAvgRow.SubSamplesRow.IrradiationCode.Replace(DB.Properties.Misc.Cd, null);
                string dos = t.IRAvgRow2.SubSamplesRow.IrradiationCode.Replace(DB.Properties.Misc.Cd, null);
                uno = this.Linaa.ProjectsList.First(x => x.Contains(uno));
                dos = this.Linaa.ProjectsList.First(x => x.Contains(dos));
                panel1box.Text = uno;
                panel2box.Text = dos;

                panelbox_KeyUp(panel1box, new KeyEventArgs(Keys.Enter));
                panelbox_KeyUp(panel2box, new KeyEventArgs(Keys.Enter));
            }
        }

        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            Panel anypanel = sender as Panel;

            ucSamples anyproject = (ucSamples)e.Data.GetData(typeof(ucSamples));
            if (anypanel.Equals(this.panel1))
            {
                if (this.panel1.Controls.Count != 0) Undock_Click(this.Undock1, e);
                this.project1 = anyproject;
                this.panel1box.Text = anyproject.Name;
                this.Undock1.Visible = true;
            }
            else
            {
                if (this.panel2.Controls.Count != 0) Undock_Click(this.Undock2, e);
                this.project2 = anyproject;
                this.panel2box.Text = anyproject.Name;
                this.Undock2.Visible = true;
            }

            //  anyproject.AutoSize = false;
            // anyproject.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;

            anypanel.Controls.Add(anyproject);
            anyproject.Dock = DockStyle.Fill;
            anyproject.Visible = true;
            anyproject.Show();
        }

        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        public void panelbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;

            ToolStripComboBox panelbox = sender as ToolStripComboBox;
            string project = panelbox.Text.ToUpper();

            if (!this.Linaa.ProjectsList.Contains(project)) return;

            MainForm M = Application.OpenForms.OfType<MainForm>().First();

            M.Box.Text = panelbox.Text;
            M.Box_KeyUp(this, e);

            ucSamples last = null;
            last = Program.FindLastControl<ucSamples>(project);

            if (last == null) return;
            if (last.IsDisposed) return;

            if (panelbox.Equals(this.panel2box))
            {
                if (this.panel2.Controls.Count != 0)
                {
                    if (!this.panel2.Controls.Contains(last)) Undock_Click(this.Undock2, e);
                    else return;
                }

                this.project2 = last as ucSamples;
                this.project2.Dock = DockStyle.Fill;
                this.panel2.Controls.Add(this.project2);
                this.Undock2.Visible = true;
            }
            else
            {
                if (this.panel1.Controls.Count != 0)
                {
                    if (!this.panel1.Controls.Contains(last)) Undock_Click(this.Undock1, e);
                    else return;
                }
                this.project1 = last as ucSamples;
                this.project1.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(this.project1);
                this.Undock1.Visible = true;
            }
        }

        private void Undock_Click(object sender, EventArgs e)
        {
            if (sender.Equals(this.Undock1))
            {
                if (this.Undock1.Visible)
                {
                    this.project1.NewForm();
                    this.project1 = null;
                    this.Undock1.Visible = false;
                }
            }
            else
            {
                if (this.Undock2.Visible)
                {
                    this.project2.NewForm();
                    this.project2 = null;
                    this.Undock2.Visible = false;
                }
            }
        }

        #endregion Panel Container
    }
}