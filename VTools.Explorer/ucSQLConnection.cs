using Rsx.SQL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VTools
{
    public partial class ucSQLConnection : UserControl, IucSQLConnection
    {

        protected static string CONNECTION_CHANGE = "Would you like to manually modify the current database connection parameters?\n\n"
                 + "This is not necessary unless a redirection to a (known) remote server is desired";

        protected static string CONNECTION_CHANGE_TITLE = "Change connection string?";

        /// <summary>
        /// Shows the UI to change the connection String localDB and returns a copy to an equivalent
        /// DB qith Dev name
        /// </summary>
        /// <param name="connectionControl"></param>
        /// <param name="localDB">          </param>
        /// <returns></returns>
        public  string ChangeConnectionString( ref string localDB, bool skipMsg = false, bool showDialog = true)
        {
            //make dynamic to access the 2 elements, whic foloows...
            //  dynamic connectionControl = connectionUsrControl;
            //check if database creation is needed
            //ask the USER NOW if he agrees wih the following Connection String

            ConnectionString = localDB;

            DialogResult result = DialogResult.No;
            if (!skipMsg)
            {
                result = MessageBox.Show(CONNECTION_CHANGE, CONNECTION_CHANGE_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }

            if (result == DialogResult.Yes || showDialog)
            {
                this.Dock = DockStyle.Fill;

                Form form = new Form();

                form.StartPosition = FormStartPosition.CenterScreen;
                form.Text = "Check the database connection parameters";
                form.TopMost = true;

                form.AutoSizeMode = AutoSizeMode.GrowOnly;
             //   form.AutoSize = true;

                //    form.Size = new System.Drawing.Size(this.Width*3/2, this.Height*3/2);
                form.Size = new System.Drawing.Size(this.Width , this.Height );

                form.Controls.Add(this);

        

                form.ShowDialog();
                localDB = ConnectionString;
            }

         

             return localDB;
        }

    
        public string Database
        {

            get
            {
                return this.hdb.Text;
            }
            set
            {
                hdb.Text = value;
            }
        }



        public string ConnectionString
        {
            get
            {
                return HyperLabRTB.Text;
            }
            set
            {
                HyperLabRTB.Text = value;


                if (string.IsNullOrEmpty(HyperLabRTB.Text)) return;
                setConnectionStrings();
            }
        }

        private void setConnectionStrings()
        {
            try
            {
                IList<object> boxes = null;

                boxes = new List<object>();

                boxes.Add(this.hsrv);
                boxes.Add(this.hdb);
                boxes.Add(this.securityInfoHL);
                boxes.Add(this.hlogin);
                boxes.Add(this.hpass);
                boxes.Add(this.timeoutboxHL);
                boxes.Add(this.enlistBox);
                boxes.Add(this.poolingBox);


                List<string> ls = null;
                ls = SQL.GetLocalSqlServerInstancesByCallingSqlWmi32();
                ls.AddRange(SQL.GetLocalSqlServerInstancesByCallingSqlWmi64());
                ls.Add("(localDB)\\MSSQLLocalDB");
                Rsx.Dumb.UIControl.FillABox(hsrv, ls, true, false);// this.hsrv.Items.AddRange(value);


                SQL.ConnectionString sq = new SQL.ConnectionString(HyperLabRTB.Text);
                //set what to do when user updates boxes
                sq.SetUI(ref boxes);

                setTextAndTags(ref sq);

                //make handler to update the BOX
                EventHandler handler = delegate
                {
                    HyperLabRTB.Text = sq.GetUpdatedConnectionString;
                };
                foreach (object o in boxes)
                {
                    if (o.GetType().Equals(typeof(TextBox)))
                    {
                        TextBox t = o as TextBox;
                        t.TextChanged += handler;
                    }
                    else
                    {
                        ComboBox c = o as ComboBox;
                        c.TextChanged += handler;
                    }
                }


            }
            catch (SystemException ex)
            {
            }
        }

        private void setTextAndTags(ref SQL.ConnectionString sq)
        {
            hsrv.Text = sq.Server;
            hdb.Text = sq.DB;
            securityInfoHL.Text = sq.SecurityInfo;
            hlogin.Text = sq.Login;
            hpass.Text = sq.Password;
            timeoutboxHL.Text = sq.Timeout;
            poolingBox.Text = sq.Pooling;
            enlistBox.Text = sq.Enlist;

            hsrv.Tag = sq.ServerTag;
            hdb.Tag = sq.DBTag;
            securityInfoHL.Tag = sq.SecurityInfoTag;
            hlogin.Tag = sq.LoginTag;
            hpass.Tag = sq.PasswordTag;
            timeoutboxHL.Tag = sq.TimeoutTag;
            poolingBox.Tag = sq.PoolingTag;
            enlistBox.Tag = sq.EnlistTag;
        }

        public string Title
        {
            get
            {
                return HyperLab.Text;
            }
            set
            {
                HyperLab.Text = value;
            }
        }

        public ucSQLConnection()
        {
            InitializeComponent();

            this.hpass.Items.Add("hlpwd2002");
            this.hpass.Items.Add("hlpwd2005");
            this.hpass.Items.Add("hlpwd2008");
            this.hpass.Items.Add("hlpwd2009");
            this.hpass.Items.Add("Hlpwd2013");
            this.hpass.Items.Add("Hlpwd2014");
            this.hpass.Items.Add("naa");
            this.hpass.Items.Add("labo");
            this.hpass.Items.Add("sa");


            this.Dock = DockStyle.Fill;
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
          bool ok=   SQL.IsServerConnected(ConnectionString);
            Cursor.Current = Cursors.Default;
            if (ok) MessageBox.Show("Connection OK");
            else MessageBox.Show("Connection FAILED");

          
        }
    }
}