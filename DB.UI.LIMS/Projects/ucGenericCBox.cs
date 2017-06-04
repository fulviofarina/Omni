using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DB.UI
{

    public interface IGenericCBox
    {
        string BindingField { get; set; }
        Action CallBack { get; set; }
        bool Enabled { get; set; }
        bool EnterPressed { get; set; }
        Action HideChildControl { set; }
        string[] InputProjects { get; set; }
        int KeyValue { get; set; }
        string Label { get; set; }
        Color LabelBackColor { get; set; }
        Color LabelForeColor { get; set; }
        bool Offline { get; set; }
        Color TextBackColor { get; set; }
        string TextContent { get; set; }
        Color TextForeColor { get; set; }

        event EventHandler DeveloperMethod;
        event EventHandler PopulateListMethod;
        event EventHandler RefreshMethod;

        void SetBindingSource(ref BindingSource bsSample);
    }
    public partial class ucGenericCBox : UserControl, IGenericCBox
    {
        protected internal Binding binding;
        protected internal string bindingField = string.Empty;
        protected internal Action callBack;
        protected internal bool enterPressed;
        protected internal Action hideChildControl;

        protected internal int keyValue = 0;
        protected internal bool offline = false;

        public string BindingField
        {
            get
            {
                return bindingField;
            }

            set
            {
                bindingField = value;
            }
        }

        /// <summary>
        /// NOT EMPLOYED BUT COULD BE
        /// </summary>
        public Action CallBack
        {
            get
            {
                return callBack;
            }

            set
            {
                callBack = value;
            }
        }

        public new bool Enabled
        {
            get
            {
                return this.projectbox.Enabled;
            }
            set
            {
                this.projectbox.Enabled = value;
            }
        }

        public bool EnterPressed
        {
            get
            {
                return enterPressed;
            }

            set
            {
                enterPressed = value;
            }
        }

        public Action HideChildControl
        {
            set
            {
                hideChildControl = value;
            }
        }

        public string[] InputProjects
        {
            get
            {
                return projectbox.Items.OfType<string>().ToArray();
            }

            set
            {
                projectbox.Items.Clear();
                projectbox.Items.AddRange(value);
                // Rsx.Dumb.UIControl.FillABox(projectbox, value, true, false);
            }
        }

        public int KeyValue
        {
            get
            {
                return keyValue;
            }

            set
            {
                keyValue = value;
            }
        }

        public string Label
        {
            get
            {
                return projectlabel.Text;
            }

            set
            {
                projectlabel.Text = value;
            }
        }

        public bool Offline
        {
            get { return offline; }
            set { offline = value; }
        }

        public string TextContent
        {
            get
            {
                return projectbox.Text.Trim().ToUpper();
            }
            set
            {
                projectbox.Text = value.Trim().ToUpper();

                if (PopulateListMethod != null)
                {
                    PopulateListMethod?.Invoke(null, EventArgs.Empty);

                    this.keyUpPressed(this.projectbox, new KeyEventArgs(Keys.Enter));
                }
            }
        }
        public Color TextBackColor
        {
            get
            {
                return this.projectbox.BackColor;
            }

            set
            {
                this.projectbox.BackColor = value;
            }
        }

        public Color TextForeColor
        {
            get
            {
                return this.projectbox.ForeColor;
            }

            set
            {
                this.projectbox.ForeColor = value;
            }
        }
        public Color LabelBackColor
        {
            get
            {
                return this.projectlabel.BackColor;
            }

            set
            {
                this.projectlabel.BackColor = value;
            }
        }

        public Color LabelForeColor
        {
            get
            {
                return this.projectlabel.ForeColor;
            }

            set
            {
                this.projectlabel.ForeColor = value;
            }
        }

        public event EventHandler DeveloperMethod;

        public event EventHandler PopulateListMethod;

        /// <summary>
        /// </summary>
        public event EventHandler RefreshMethod;

        public void SetBindingSource(ref BindingSource bsSample)
        {
            binding = Rsx.Dumb.BS.ABinding(ref bsSample, bindingField);
            projectbox.DataBindings.Add(binding);
            this.projectbox.DropDownClosed += dropDownClosed;
            this.projectlabel.Click += openDropDown;
            this.projectbox.DropDown += setNoUpdates;
        }
        protected internal void dropDownClosed(object sender, EventArgs e)
        {
            // ComboBox box = sender as ComboBox; if (!box.IsOnDropDown) return;
            if (projectbox.SelectedItem != null)
            {
                string name = projectbox.SelectedItem.ToString();
                //string col = Interface.IDB.SubSamples.SubSampleNameColumn.ColumnName;
                BindingSource bs = binding?.DataSource as BindingSource;
                if (bs != null)
                {
                    int pos = bs.Find(bindingField, name);
                    bs.Position = pos;
                }
                setReceiveUpdates();
            }
        }

        /// <summary>
        /// This could be outside since it does not depend on anything
        /// </summary>
        /// <param name="ProjectOrOrder"></param>
        protected internal void keyUpPressed(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            this.enterPressed = (key == Keys.Enter);

            this.keyValue = e.KeyValue;

            bool noEnter = key != Keys.Enter;

            noEnter = noEnter && (keyValue < 47 || keyValue > 105);

          
            if (noEnter)
            {

                return;
            }

         //   setNoUpdates(sender, e);

            if (TextContent.Equals("DEV"))
            {
                DeveloperMethod?.Invoke(sender, EventArgs.Empty);
                return;
            }
            RefreshMethod?.Invoke(sender, e);
            //end edit
            (binding?.DataSource as BindingSource)?.EndEdit();

            if (enterPressed)
            {
               // setReceiveUpdates();
                //the items list
                PopulateListMethod?.Invoke(sender, e);
            }

            callBack?.Invoke();
        }

        // private Color color;
        protected internal void openDropDown(object sender, EventArgs e)
        {
            // if (projectbox.DroppedDown)
            {
                hideChildControl?.Invoke();
            }
            if (!projectbox.DroppedDown)
            {
                // hideChildControl?.Invoke();

                projectbox.DroppedDown = true;
            }
            else
            {
                projectbox.DroppedDown = false;
            }
        }

        protected internal void setNoUpdates(object sender, EventArgs e)
        {
            // ComboBox box = sender as ComboBox;

            projectbox
                .DataBindings
                .DefaultDataSourceUpdateMode = DataSourceUpdateMode.Never;
        }
        protected internal void setReceiveUpdates()
        {
            projectbox
               .DataBindings
               .DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
        }

        public ucGenericCBox()
        {
            InitializeComponent();
          
            this.projectbox.KeyUp += keyUpPressed;

          
            // this.projectlabel.MouseEnter += Projectlabel_MouseEnter;

           //  this.projectbox.GotFocus += Projectbox_GotFocus;
          //  this.projectbox.LostFocus += Projectbox_LostFocus;
        }

      //   private void Projectbox_LostFocus(object sender, EventArgs e)
        // {
        //   setReceiveUpdates();
       // }

       //  private void Projectbox_GotFocus(object sender, EventArgs e) { setNoUpdates(sender, e); }
    }
}