using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Rsx;

namespace DB
{
    public partial class LINAA
    {

        partial class StandardsDataTable : IColumn
        {
            IEnumerable<DataColumn> nonNullables = null;
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[]{ stdNameColumn,
                     MatrixNameColumn,stdProducerColumn,stdUncColumn,
                      MonitorCodeColumn };
                    }

                    return nonNullables;

                }
            }


        }

        partial class MonitorsDataTable : IColumn
        {
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

        }


        partial class UnitDataTable : IColumn
        {
            // private DataColumn[] changeables;
            private DataColumn[] nonNullCols;

            /// <summary>
            /// fix this to use windows user
            /// </summary>
            public bool defaultValue
            {
                //TODO: windows user instead
                get
                {
                    // LINAA set = this.DataSet as LINAA;
                    return !(this.DataSet as LINAA).SSFPref.FirstOrDefault().Overrides;
                }
            }

            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }
            public IEnumerable<DataColumn> NonNullables
            {

                get
                {
                    if (nonNullCols == null)
                    {
                        nonNullCols = new DataColumn[] {
                            this.columnChRadius, this.columnChLength,
                            this.columnkth,this.columnkepi,
                            this.columnChCfg,
                            this.columnBellFactor,
                            this.pEpiColumn,
                            this.pThColumn,
                            this.WGtColumn,
                            this.nFactorColumn
                        };
                    }
                    return nonNullCols;
                }
            }

            /*
            public IEnumerable<DataColumn> NonNullables
            {
                get
                {
                    if (nonNullables == null)
                    {
                        nonNullables = new DataColumn[] {
                            this.columnChRadius, this.columnChLength,
                            this.columnkepi,this.columnkth,
                            this.columnChCfg,
                            this.columnLastCalc,
                           this.columnLastChanged,
                           this.columnWGt,
                           this.nFactorColumn,
                                  this.columnBellFactor,
                                  this.pEpiColumn,
                                  this.pThColumn
                               // this.SSFTableColumn
                        };
                    }
                    return nonNullables;
                }
            }
            */

            /// <summary>
            /// I think it is perfect like this, don't mess it up
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e">     </param>


            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                if (!NonNullables.Contains(e.Column)) return;

                DataRow row = e.Row;
                UnitRow r = row as UnitRow;

                try
                {
                    bool change = (e.ProposedValue.ToString().CompareTo(e.Row[e.Column].ToString()) != 0);

                    if (change)
                    {
                        r.valueChanged();
                    }
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }

        /// <summary>
        /// Cleaned
        /// </summary>
        public partial class SubSamplesDataTable : IColumn
        {
        //    private DataColumn[] nonNullable=null;
            private DataColumn[] simplenonNullable=null;
            private DataColumn[] nonNullableUnit=null;

            public DataColumn[] SimpleNonNullable
            {
                get
                {
                    if (simplenonNullable == null)
                    {
                        simplenonNullable = new DataColumn[]{ columnSubSampleName,
                     columnSubSampleCreationDate,columnSubSampleDescription,columnVol,
                     columnFC, columnCapsuleName, columnMatrixName };
                    }

                    return simplenonNullable;
                }
            }
            //WHEN NNULL IT BYPASES
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

            public DataColumn[] NonNullableUnit
            {
                get
                {
                    if (nonNullableUnit == null)
                    {
                        nonNullableUnit = new DataColumn[] { this.Gross1Column, this.FillHeightColumn, this.RadiusColumn };
                    }

                    return nonNullableUnit;
                }
            }

          

            public void DataColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                // DataColumn c = e.Column;
                //if (!NonNullables.Contains(c)) return;

                if (!NonNullableUnit.Contains(e.Column)) return;

                DataRow row = e.Row;
                SubSamplesRow r = row as SubSamplesRow;

                try
                {
                    // bool nullo = EC.CheckNull(c, row);

                    bool change = (e.ProposedValue.ToString().CompareTo(e.Row[e.Column].ToString()) != 0);

                    if (change) r.UnitRow?.valueChanged();
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }

    

        protected internal void handlersSamples()
        {
            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Standards));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Monitors));

            handlers.Add(DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Unit));

            // tableIRequestsAverages.ChThColumn.Expression = " ISNULL(1000 *
            // Parent(SigmasSal_IRequestsAverages).sigmaSal / Parent(SigmasSal_IRequestsAverages).Mat
            // ,'0')"; tableIRequestsAverages.ChEpiColumn.Expression = " ISNULL(1000 *
            // Parent(Sigmas_IRequestsAverages).sigmaEp / Parent(Sigmas_IRequestsAverages).Mat,'0')
            // "; tableIRequestsAverages.SDensityColumn.Expression = " 6.0221415 * 10 *
            // Parent(SubSamples_IRequestsAverages).DryNet / (
            // Parent(SubSamples_IRequestsAverages).Radius * (
            // Parent(SubSamples_IRequestsAverages).Radius + Parent(SubSamples_IRequestsAverages).FillHeight))";
        }
    }
}