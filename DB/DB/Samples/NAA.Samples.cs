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
        public partial class SubSamplesDataTable
        {
            private DataColumn[] nonNullable;

            private DataColumn[] nonNullableUnit;

            public DataColumn[] NonNullable
            {
                get
                {
                    if (nonNullable == null)
                    {
                        nonNullable = new DataColumn[]{columnSubSampleName,
                     columnSubSampleCreationDate,columnSubSampleDescription,columnVol,
                     columnFC, columnCapsuleName, columnMatrixName };
                    }

                    return nonNullable;
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

            public void DataColumnChanged(object sender, DataColumnChangeEventArgs e)
            {

                LINAA.SubSamplesRow subs = e.Row as LINAA.SubSamplesRow; //cast
                if (NonNullable.Contains(e.Column))
                {
                    EC.CheckNull(e.Column, e.Row);
                    return;
                }

                SSFPrefRow pref = (this.DataSet as LINAA).SSFPref.FirstOrDefault();

                try
                {
                    subs.Check(e.Column, pref.CalcMass, pref.AARadius, pref.AAFillHeight, pref.CalcDensity);
                }
                catch (SystemException ex)
                {
                    EC.SetRowError(e.Row, e.Column, ex);
                    (this.DataSet as LINAA).AddException(ex);
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

                    if (change) r.UnitRow?.ValueChanged();
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
            handlers.Add(Standards.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Standards));

            handlers.Add(Monitors.DataColumnChanged);
            dTWithHandlers.Add(Tables.IndexOf(Monitors));

            handlers.Add(Unit.DataColumnChanged);
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