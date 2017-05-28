using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Rsx.SQL;

namespace VTools
{
    public partial class ucSQLConnection : UserControl, IucSQLConnection
    {
    

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

                try
                {
                    if (string.IsNullOrEmpty(HyperLabRTB.Text)) return;

                    IList<dynamic> boxes = null;

                    boxes = new List<dynamic>();

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



                    //  List<string> ls = SQL.GetLocalSqlServerInstancesByCallingSqlWmi32();
                    // ls.AddRange( SQL.GetLocalSqlServerInstancesByCallingSqlWmi64());
                    //   Rsx.Dumb.UIControl.FillABox(hsrv, ls, true, false);

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

            this.Dock = DockStyle.Fill;
        }
    }
}