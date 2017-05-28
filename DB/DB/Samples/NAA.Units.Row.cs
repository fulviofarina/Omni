using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Rsx;

namespace DB
{

    
    public partial class LINAA
    {
        /// <summary>
        /// Cleaned
        /// </summary>
        public partial class UnitRow : IRow
        {
            public void Check()
            {
                foreach (DataColumn column in this.tableUnit.Columns)
                {
                    Check(column);
                }
                //   return this.GetColumnsInError().Count() != 0;
            }

            public void Check(DataColumn c)
            {

                if (!this.tableUnit.NonNullables.Contains(c) && c != this.tableUnit.SSFTableColumn) return;

                bool nullo = EC.CheckNull(c, this);// string.IsNullOrEmpty(e.Row.GetColumnError(e.Column));

                if (this.tableUnit.kepiColumn == c)
                {
                    if (nullo)
                    {
                        kepi = 1;
                    }
                }
                else if (this.tableUnit.kthColumn == c)
                {
                    if (nullo)
                    {
                        kth = 0.6;
                    }
                }
                else if (this.tableUnit.ChRadiusColumn == c)
                {
                    // if (Convert.ToDouble(e.ProposedValue) == 0) e.ProposedValue = 1 ;
                    if (nullo)
                    {
                        ChRadius = 24;
                    }
                }
                else if (this.tableUnit.ChLengthColumn == c)
                {
                    if (nullo)
                    {
                        ChLength = 300;
                    }
                }
                else if (c == this.tableUnit.SSFTableColumn)
                {
                    checkSSFTable(nullo);
                }
                else if (c == this.tableUnit.ChCfgColumn)
                {
                    if (nullo) ChCfg = "0";
                    else
                    {
                        // bool nullFluxType = (EC.CheckNull(this.tableChannels.FluxTypeColumn, this));
                        bool isWGT = EC.CheckNull(this.tableUnit.WGtColumn, this);
                        isWGT = isWGT || this.tableUnit.defaultValue;
                        if (isWGT)
                        {
                            setWGt();
                        }
                        bool isBell = EC.CheckNull(this.tableUnit.BellFactorColumn, this);
                        isBell = isBell || this.tableUnit.defaultValue;
                        if (isBell)
                        {
                            setBellFactor();
                        }
                    }
                }
                else if (c == this.tableUnit.WGtColumn)
                {
                    if (nullo) WGt = 1;
                }
                else if (c == this.tableUnit.BellFactorColumn)
                {
                    if (nullo) BellFactor = 1.16;
                }
                else if (c == this.tableUnit.nFactorColumn)
                {
                    if (nullo) nFactor = 0.5;
                }
                else if (c == this.tableUnit.pThColumn)
                {
                    if (nullo) pTh = 0.964;
                }
                else if (c == this.tableUnit.pEpiColumn)
                {
                    if (nullo) pEpi = 0.82;
                }
            }

            public new bool HasErrors()
            {
                DataColumn[] colsInE = this.GetColumnsInError();
                int count = colsInE.Intersect(this.tableUnit.NonNullables)
                    .Count();
                return count != 0;
               
            }

            public void Clean()
            {
                SetGtChNull();
                SetGtNull();
                SetGtMCNull();
                SetMCLNull();
                SetPXSNull();
                SetEXSNull();
                SetColumnError(this.tableUnit.NameColumn, null);
                SetSSFTableNull();
                AcceptChanges();
            }

            public MatSSFDataTable GetMatSSFTable(string path)
            {
                LINAA.MatSSFDataTable dt = new MatSSFDataTable();

                if (!IsSSFTableNull())
                {
                    byte[] arr = SSFTable;
                    Rsx.Dumb.Tables.ReadDTBytes(path, ref arr, ref dt);
                }

                return dt;
            }

            public void SetParent<T>(ref T row, object[] args = null)
            {
                if (EC.IsNuDelDetch(row as DataRow)) return;
                Type t = row.GetType();
                bool isChannel = t.Equals(typeof(ChannelsRow));
                bool isMatrix = t.Equals(typeof(MatrixRow));
                if (isChannel)
                {
                    ChannelsRow c = row as ChannelsRow;
                    setChannel(ref c);
                }
               else  if (!isMatrix)
                {
                    if (t.Equals(typeof(VialTypeRow)))
                    {
                        LINAA.VialTypeRow v = row as VialTypeRow;
                         setRabbit(ref v);
                       
                    }
                    //if a sample (first time ADD)
                    else
                    {
                        SubSamplesRow s = row as SubSamplesRow;
                        setSample(ref s);
                        //set default channel from irradiation request
                        if (!EC.IsNuDelDetch(s.IrradiationRequestsRow))
                        {
                            ChannelsRow c = s.IrradiationRequestsRow.ChannelsRow;
                            SetParent(ref c);
                        }
                    }
                }
                else if (isMatrix)
                {
                    MatrixRow m = row as MatrixRow;

                    SubSamplesRow.MatrixID = m.MatrixID;
                    
                }
            }

          
        }

        public partial class UnitRow
        {
            internal void setRabbit(ref VialTypeRow v)
            {

                if (v.IsRabbit)
                {
                                     
                   if (SubSamplesRow.IsChCapsuleIDNull() || SubSamplesRow.ChCapsuleID != v.VialTypeID)
                   {
                       SubSamplesRow.ChCapsuleID = v.VialTypeID;
                    }
                   else
                    {
                        //JUST UPDATE
                         this.ChRadius = v.InnerRadius;
                         this.ChLength = v.MaxFillHeight;
                    }
                }
                else
                {
                  if (SubSamplesRow.IsCapsulesIDNull()  || SubSamplesRow.CapsulesID != v.VialTypeID)
                    {
                        SubSamplesRow.CapsulesID = v.VialTypeID;
                    }
                    else
                    {
                        this.SubSamplesRow.Radius = v.InnerRadius;
                        this.SubSamplesRow.FillHeight = v.MaxFillHeight;
                    }
                }
            }
            internal void setSample(ref SubSamplesRow s)
            {
                ToDo = true;
                LastCalc = DateTime.Now;
                LastChanged = DateTime.Now.AddMinutes(1);
                IrrReqID = s.IrradiationRequestsID;
                SampleID = s.SubSamplesID;
            }

            /// <summary>
            /// RESET THE UNIT TODO SO IT CAN BE RECALCULATED
            /// </summary>
            internal void valueChanged()
            {
                this.LastChanged = DateTime.Now;
                ToDo = true;
            }

            internal void setBellFactor()
            {
                if (ChCfg.Contains("2"))
                {
                    // WGt = 0.67;
                    BellFactor = 1.16;
                }
                else if (ChCfg.Contains("1"))
                {
                    // WGt = 0.93;
                    BellFactor = 1.30;
                }
                else
                {
                    // WGt = 1;
                    BellFactor = 1.16;
                }
            }

            internal void setWGt()
            {
                if (ChCfg.Contains("2"))
                {
                    WGt = 0.67;
                    // BellFactor = 1.16;
                }
                else if (ChCfg.Contains("1"))
                {
                    WGt = 0.93;
                    // BellFactor = 1.30;
                }
                else
                {
                    WGt = 1;
                    // BellFactor = 1.16;
                }
            }

            internal void checkSSFTable(bool nullo)
            {
                LINAA linaa = (this.tableUnit.DataSet as LINAA);
                //if there are rows bytes but no rows in the table, LOAD
                if (GetMatSSFRows().Count() == 0 && !nullo)
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\";
                    MatSSFDataTable dt = GetMatSSFTable(path);
                    linaa.MatSSF.Merge(dt, false, MissingSchemaAction.AddWithKey);
                }
                //if set null, delete the child rows (if any)
                else if (nullo)
                {
                    IEnumerable<MatSSFRow> ssfs = GetMatSSFRows();
                    if (ssfs.Count() != 0)
                    {
                        linaa.Delete(ref ssfs);
                        linaa.MatSSF.AcceptChanges();
                    }
                }

                nullo = EC.CheckNull(this.tableUnit.SSFTableColumn, this);// string.IsNullOrEmpty(e.Row.GetColumnError(e.Column));

                //finally... set todo to true
                if (nullo || GetMatSSFRows().Count() == 0)
                {
                    ToDo = true;
                }
                // else if (!IsToDoNull() && ToDo) ToDo = false;
            }

            /// <summary>
            /// Sets the specific channel data
            /// The Channel row from the irradiation request is the default Row in case there's nothing
            /// However each unit has its own channel row, which can override all
            /// data sample.IrradiationRequest.ChannelRow
            /// </summary>
            /// <param name="c"></param>
            internal void setChannel(ref LINAA.ChannelsRow c)
            {
                //associate the channel
                this.ChannelsRow = c;
                this.SubSamplesRow.f = c.f; //override f
                this.SubSamplesRow.Alpha = c.Alpha; //override Alpha (yes)
                this.kth = c.kth;
                this.kepi = c.kepi;
                this.ChCfg = c.FluxType;
                this.BellFactor = c.BellFactor;
                this.SubSamplesRow.FC = c.FC;
                this.WGt = c.WGt;
                this.nFactor = c.nFactor;
                this.pTh = c.pTh;
                this.pEpi = c.pEpi;
               
            }

           
        }
    }
}