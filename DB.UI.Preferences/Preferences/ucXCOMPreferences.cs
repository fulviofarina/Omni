using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using DB.Tools;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
    public interface IXCOMPreferences
    {
        event EventHandler RoundingChanged;
        Form Parent { get; }
        void Set(ref Interface inter);
    }

    public partial class ucXCOMPreferences : UserControl, IXCOMPreferences
    {
        protected internal Interface Interface;

        public new Form Parent 
        {
            get { return this.ParentForm; }
        }
        protected internal event EventHandler roundingChanged = null;

        public event EventHandler RoundingChanged
        {
            add
            {
                roundingChanged += value;
            }
            remove
            {
                roundingChanged -= value;
            }
        }

        public void Set(ref Interface inter)
        {
            Interface = inter;

            try
            {
                BindingSource bs = Interface.IBS.XCOMPref;
                //   XCOMPrefDataTable dt = Interface.IDB.XCOMPref;

                //text binding
                Hashtable bindings2 = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty);
                setValueBindings(ref bindings2);

                Hashtable bindings = Rsx.Dumb.BS.ArrayOfBindings(ref bs, string.Empty, "CheckState");
                setCheckStatebindings(ref bindings);

            //    stepsBox.TextChanged += energyStep_TextChanged;
                useListbox.CheckedChanged += ASCIIInput_Click;
                this.roundingTextBox.KeyUp += roundingTextBox_TextChanged;
            }
            catch (Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        protected internal void roundingTextBox_TextChanged(object sender, EventArgs e)
        {
            roundingChanged?.Invoke(sender, e);
        }
        /*
        protected internal void energyStep_TextChanged(object sender, EventArgs e)
        {
            if (!stepsBox.Text.Equals(String.Empty))
            {
                if (Convert.ToInt32(stepsBox.Text) > 75) stepsBox.Text = "75";
            }
        }
        */
        protected internal void ASCIIInput_Click(object sender, EventArgs e)
        {

            if (useListbox.Checked)
            {
                string filepath = Encoding.UTF8.GetString(Interface.IPreferences.CurrentXCOMPref.ListOfEnergies);
                RichTextBox box= Rsx.Dumb.IO.RichTextBox(filepath , "Energies List", 14.25f);
                Form fm = (box.Parent as Form);
                fm.FormClosed += delegate
                {
                    Interface.IPreferences.CurrentXCOMPref.ListOfEnergies = Encoding.UTF8.GetBytes(box.Text);
                    Interface.IPreferences.SavePreferences();

                };
                fm.Show();


            }
        }

   


        private void setValueBindings( ref Hashtable bindings2)
        {
            XCOMPrefDataTable dt = Interface.IDB.XCOMPref;
            string colname = dt.StartEnergyColumn.ColumnName;
            this.minEneBox.DataBindings.Add(bindings2[colname] as Binding);

            colname = dt.EndEnergyColumn.ColumnName;
            this.maxEneBox.DataBindings.Add(bindings2[colname] as Binding);
            //
            colname = dt.StepsColumn.ColumnName;
            this.stepsBox.DataBindings.Add(bindings2[colname] as Binding);

            colname = Interface.IDB.XCOMPref.RoundingColumn.ColumnName;

            this.roundingTextBox.DataBindings.Add(bindings2[colname] as Binding);
        }

        protected internal void setCheckStatebindings( ref Hashtable bindings)
        {
            XCOMPrefDataTable dt = Interface.IDB.XCOMPref;
            string column = dt.LoopColumn.ColumnName;
            this.loopCheckBox.DataBindings.Add(bindings[column] as Binding);

            column = dt.UseListColumn.ColumnName;
            this.useListbox.DataBindings.Add(bindings[column] as Binding);

            column = dt.LogGraphColumn.ColumnName;
            this.logscaleBox.DataBindings.Add(bindings[column] as Binding);

            column = dt.ASCIIOutputColumn.ColumnName;
            this.asciibox.DataBindings.Add(bindings[column] as Binding);

            column = dt.ForceColumn.ColumnName;
            this.forceBox.DataBindings.Add(bindings[column] as Binding);



            column = dt.ISColumn.ColumnName;
            this.isbox.DataBindings.Add(bindings[column] as Binding);
            this.isbox.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.CSColumn.ColumnName;
            this.csbox.DataBindings.Add(bindings[column] as Binding);
            this.csbox.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.PPEFColumn.ColumnName;
            this.ppefbox.DataBindings.Add(bindings[column] as Binding);
            this.ppefbox.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.PPNFColumn.ColumnName;
            this.ppnfbox.DataBindings.Add(bindings[column] as Binding);
            this.ppnfbox.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.TNCSColumn.ColumnName;
            this.totncs.DataBindings.Add(bindings[column] as Binding);
            this.totncs.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.TCSColumn.ColumnName;
            this.totcs.DataBindings.Add(bindings[column] as Binding);
            this.totcs.CheckedChanged += roundingTextBox_TextChanged;
            column = dt.PEColumn.ColumnName;
            this.pebox.DataBindings.Add(bindings[column] as Binding);
            this.pebox.CheckedChanged += roundingTextBox_TextChanged;

        }

     

        public ucXCOMPreferences()
        {
            InitializeComponent();
        }
    }
}