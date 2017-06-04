using System.Data;
using System.Drawing;

using System.Windows.Forms;
using Rsx;

using static DB.LINAA;

namespace DB.UI
{
    public interface ISampleColumn
    {
        string BindingPreferenceField { get; set; }
        DataRow BindingPreferenceRow { get; set; }
    }

    public partial class SampleCell : DataGridViewTextBoxCell
    {
        protected internal Color[] arrayOfBackColors = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.White };
        protected internal Color[] arrayOfForeColors = new Color[] { Color.Yellow, Color.Black, Color.Black };

        protected internal SampleColumn parent
        {
            get
            {
                return this.OwningColumn as SampleColumn;
            }
        }

        /// <summary>
        /// Sets the Style
        /// </summary>
        /// <param name="readonlyMode"></param>
        protected internal void setCellReadOnlyOrNormal(bool readonlyMode)
        {
            if (readonlyMode)
            {
                Style.BackColor = arrayOfBackColors[0];
                this.Style.ForeColor = arrayOfForeColors[0];

                this.Style.SelectionBackColor = arrayOfBackColors[0];
                this.Style.SelectionForeColor = arrayOfForeColors[0];
            }
            else
            {
                Style.BackColor = arrayOfBackColors[2];
                Style.ForeColor = arrayOfForeColors[2];
                Style.SelectionBackColor = arrayOfBackColors[1];
                Style.SelectionForeColor = arrayOfForeColors[1];
            }
        }

        /// <summary>
        /// Sets if should be read only or not
        /// </summary>
        protected internal void setReadOnlyOrNot()
        {
            if (!string.IsNullOrEmpty(parent?.BindingPreferenceField))
            {
                bool Isreadonly = false;
                Isreadonly = (bool)parent?.BindingPreferenceRow[parent?.BindingPreferenceField];
                setCellReadOnlyOrNormal(Isreadonly);
                if (Isreadonly != this.OwningColumn.ReadOnly)
                {
                    this.OwningColumn.ReadOnly = Isreadonly;
                }
            }
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            setReadOnlyOrNot();

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        public SampleCell() : base()
        {
            Style.Font = new Font("Segoe UI", 14.25f, FontStyle.Bold);
        }
    }

    public partial class SampleColumn : DataGridViewTextBoxColumn, ISampleColumn
    {
        protected internal string bindingField = string.Empty;

        protected internal DataRow bindingRow = null;

        public string BindingPreferenceField
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

        public DataRow BindingPreferenceRow
        {
            get
            {
                return bindingRow;
            }

            set
            {
                bindingRow = value;
                bool? Isreadonly = (bool?)bindingRow?[BindingPreferenceField];
                if (Isreadonly != null) this.ReadOnly = (bool)Isreadonly;
            }
        }

        public SampleColumn()
            : base()
        {
            CellTemplate = new SampleCell();
        }
    }

    public partial class UnitBoolColumn : DataGridViewTextBoxColumn
    {
        public UnitBoolColumn()
            : base()
        {
            CellTemplate = new UnitCell();
        }
    }

    public partial class UnitCell : DataGridViewTextBoxCell
    {
        protected internal Color defaultColor = Color.DarkRed;

        protected override void OnDoubleClick(DataGridViewCellEventArgs e)
        {
            UnitRow u = ((this.OwningRow?.DataBoundItem as DataRowView)?.Row) as UnitRow;
            if (EC.IsNuDelDetch(u)) return;

            u.ToDo = !u.ToDo;


         
          //  base.OnDoubleClick(e);

         //   setCellColor();
            base.DataGridView.NotifyCurrentCellDirty(true);
            base.DataGridView.ClearSelection();
          //  base.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
          
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            setCellColor();

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, this.Style, advancedBorderStyle, paintParts);
        }

        protected internal void setCellColor()
        {
            DataRowView rv = this.OwningRow?.DataBoundItem as DataRowView;
            if (rv != null)
            {
                UnitRow u = (rv.Row) as UnitRow;
                if (!EC.IsNuDelDetch(u))
                {
                    Color colr = Color.DarkGreen;
                    if (u.ToDo && u.IsBusy)
                    {
                        colr = System.Drawing.Color.DarkOrange;
                    }
                    else if (u.ToDo && !u.IsBusy)
                    {
                        colr = System.Drawing.Color.DarkRed;
                    }
                    Style.BackColor = colr;
                    Style.ForeColor = colr;
                }
            }
        }

        public UnitCell() : base()
        {
            Style.SelectionBackColor = Color.Transparent;
            Style.SelectionForeColor = Color.Transparent;

            Style.BackColor = defaultColor;
            Style.ForeColor = defaultColor;
        }
    }
}