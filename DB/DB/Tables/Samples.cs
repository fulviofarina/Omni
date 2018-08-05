using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
      

        /// <summary>
        /// Cleaned
        /// </summary>
        public partial class SubSamplesDataTable : IColumn
        {
            public EventHandler<EventData> CalcParametersHandler;

            public EventHandler InvokeCalculations;

            public EventHandler<EventData> AddMatrixHandler;

            private DataColumn[] geometriesNonNullable = null;

            private DataColumn[] irradiationNonNullable = null;

            private DataColumn[] nonNullableUnit = null;

            private DataColumn[] simplenonNullable = null;
            private DataColumn[] standardsNonNullable = null;

            private DataColumn[] timesNonNullable = null;

            //WHEN NNULL IT BYPASES
            public IEnumerable<DataColumn> ForbiddenNullCols
            {
                get
                {
                    return null;
                }
            }

            public IEnumerable<DataColumn> GeometriesNonNullables
            {
                get
                {
                    if (geometriesNonNullable == null)
                    {
                        geometriesNonNullable = new DataColumn[] { DirectSolcoiColumn, this.GeometryNameColumn, this.CapsulesIDColumn };
                    }
                    return geometriesNonNullable;
                }
            }

            public IEnumerable<DataColumn> IrradiationNonNullable
            {
                get
                {
                    if (irradiationNonNullable == null)
                    {
                        irradiationNonNullable = new DataColumn[] { fColumn, ENAAColumn, AlphaColumn, this.ChCapsuleIDColumn, this.IrradiationRequestsIDColumn };
                    }
                    return irradiationNonNullable;
                }
            }

            public IEnumerable<DataColumn> NonNullableUnit
            {
                get
                {
                    if (nonNullableUnit == null)
                    {
                        nonNullableUnit = new DataColumn[] { CalcDensityColumn, this.Gross2Column, this.Gross1Column, this.FillHeightColumn, this.RadiusColumn, this.columnMatrixID, this.NetColumn };
                    }

                    return nonNullableUnit;
                }
            }

            public IEnumerable<DataColumn> NonNullBasicUnits
            {
                get
                {
                    if (nonNullBasicUnits == null)
                    {
                        nonNullBasicUnits = new DataColumn[] { this.NetColumn, this.FillHeightColumn, this.RadiusColumn, this.columnMatrixID };
                    }

                    return nonNullBasicUnits;
                }
            }

            protected internal DataColumn[] nonNullBasicUnits = null;

            public IEnumerable<DataColumn> SimpleNonNullable
            {
                get
                {
                    if (simplenonNullable == null)
                    {
                        simplenonNullable = new DataColumn[]{ columnSubSampleName,
                     columnSubSampleCreationDate,SubSampleDescriptionColumn,columnVol,
                     columnFC, columnCapsuleName, columnMatrixName };
                    }

                    return simplenonNullable;
                }
            }

            /*
            public DataColumn[] OtherNonNullableUnit
            {
                get
                {
                    if (otherNonNullableUnit == null)
                    {
                        otherNonNullableUnit = new DataColumn[] { CalcDensityColumn, NetColumn, this.Gross2Column, this.Gross1Column, this.FillHeightColumn, this.RadiusColumn };
                    }

                    return otherNonNullableUnit;
                }
            }
            private DataColumn[] otherNonNullableUnit = null;
            */

            public IEnumerable<DataColumn> StandardsNonNullables
            {
                get
                {
                    if (standardsNonNullable == null)
                    {
                        standardsNonNullable = new DataColumn[] { SubSampleTypeColumn, this.MonitorsIDColumn, this.StandardsIDColumn, this.ReferenceMaterialIDColumn, BlankIDColumn };
                    }
                    return standardsNonNullable;
                }
            }

            public IEnumerable<DataColumn> TimesNonNullable
            {
                get
                {
                    if (timesNonNullable == null)
                    {
                        timesNonNullable = new DataColumn[] { InReactorColumn, OutReactorColumn, IrradiationTotalTimeColumn };
                    }
                    return timesNonNullable;
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
                    r.Checking(e);
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }

        partial class IPeakAveragesDataTable
        {
            public IPeakAveragesRow NewIPeakAveragesRow(Int32 k0Id, ref SubSamplesRow s)
            {
                IPeakAveragesRow ip = this.NewIPeakAveragesRow();
                this.AddIPeakAveragesRow(ip);
                ip.k0ID = k0Id;
                // ip.Radioisotope = iso; ip.Element = sym; ip.Energy = energy;
                if (!EC.IsNuDelDetch(s))
                {
                    ip.Sample = s.SubSampleName;
                    // if ( !s.IsIrradiationCodeNull()) ip.Project = s.IrradiationCode;
                }
                return ip;
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

        partial class StandardsDataTable : IColumn
        {
            private IEnumerable<DataColumn> nonNullables = null;

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

        partial class UnitDataTable : IColumn
        {
            public EventHandler AddSSFsHandler;

            public EventHandler CleanSSFsHandler;
            public EventHandler InvokeCalculations;

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
                    r.Checking(e);
                }
                catch (SystemException ex)
                {
                    (this.DataSet as LINAA).AddException(ex);
                    EC.SetRowError(e.Row, e.Column, ex);
                }
            }
        }
    }
}