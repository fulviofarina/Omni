using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
using static DB.LINAA;

namespace DB.UI
{
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
            CellTemplate = new UnitBoolCell();
        }
    }

    public partial class UnitSSFCell : DataGridViewTextBoxCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            setCellColors();

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, this.Style, advancedBorderStyle, paintParts);
        }

        public UnitSSFCell() : base()
        {
            this.Style.Font = new Font("Segoe UI Semibold", 14.25f, FontStyle.Regular);
            this.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        protected internal void setCellColors()
        {
            UnitSSFColumn columna = this.OwningColumn as UnitSSFColumn;

            Color gris = Color.FromArgb(64, 64, 64);

            if (!columna.ReadOnly)
            {
                Color another = Color.Chartreuse;
                if (columna.SSFCellType == 2)
                {
                    another = Color.LemonChiffon;
                }
                else if (columna.SSFCellType == 3)
                {
                    another = Color.Orange;
                }

                Style.BackColor = gris;
                Style.ForeColor = another;
                Style.SelectionBackColor = Color.Black;
                Style.SelectionForeColor = Color.White;
            }
            else
            {
                Style.BackColor = gris;
                Style.ForeColor = gris;
                Style.SelectionBackColor = gris;
                Style.SelectionForeColor = gris;
            }
        }
    }

    public partial class BindableDGVColumn : DataGridViewTextBoxColumn
    {
        public BindableDGVColumn() : base()
        {
        }

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
            }
        }
    }

    public partial class UnitSSFColumn : DataGridViewTextBoxColumn, IUnitSSFColumn
    {
        public UnitSSFColumn() : base()
        {
            base.CellTemplate = new UnitSSFCell();
        }

        protected internal string bindableAsteriskField = string.Empty;

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
            }
        }

        public void SetEnable()
        {
            if (bindingRow != null)
            {
                if (!string.IsNullOrEmpty(BindingPreferenceField))
                {
                    bool enable = (bool)bindingRow.Field<bool>(BindingPreferenceField);
                    this.ReadOnly = !enable;
                }
            }
        }

        public void PaintHeader()
        {
            SetEnable();

            string add = ASTERISK;

            // UnitSSFColumn col = this;//as UnitSSFColumn;

            SSFPrefRow pref = bindingRow as SSFPrefRow;

            if (pref != null)
            {
                bool overrider = (bool)pref?.Field<bool>(BindableAsteriskField);

                if (overrider)
                {
                    if (!this.ToolTipText.Contains(SEPARATOR))
                    {
                        this.ToolTipText += TOOLTIP_TEXT;
                        // this.GtM.ToolTipText += TOOLTIP_TEXT;
                        // this.sSFDataGridViewTextBoxColumn.ToolTipText += TOOLTIP_TEXT;
                    }
                    // this.GFast.HeaderText = "GFast *";
                }
                else
                {
                    add = string.Empty;
                    if (ToolTipText.Contains(SEPARATOR))
                    {
                        this.ToolTipText = this.ToolTipText.Replace(TOOLTIP_TEXT, null);
                    }
                }
            }

            HeaderText = originalHeaderText + add;
        }

        public void SetRounding()
        {
            SSFPrefRow pref = bindingRow as SSFPrefRow;

            if (pref != null)
            {
                try
                {
                    string rounding = (string)pref?.Field<string>(bindingRoundingField);

                    if (rounding.Count() == 2)
                    {
                        char first = rounding.ToUpper()[0];
                        char[] formatchars = { 'G', 'F', 'N', 'E', 'C' };
                        if (formatchars.Contains(first))
                        {
                            char second = rounding[1];
                            if (char.IsNumber(second))
                            {
                                this.DefaultCellStyle.Format = rounding;
                            }
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
                catch (FormatException)
                {
                }
            }
        }

        public string BindableAsteriskField
        {
            get
            {
                return bindableAsteriskField;
            }

            set
            {
                bindableAsteriskField = value;
            }
        }

        protected internal int sSFCellType = 1;

        public int SSFCellType
        {
            get
            {
                return sSFCellType;
            }

            set
            {
                sSFCellType = value;
            }
        }

        // public Color[] matSSFBkg = new Color[] { Color.FromArgb(64, 64, 64), Color.White,
        // Color.Chartreuse }; public Color[] matSSFFore = new Color[] { Color.FromArgb(64, 64, 64),
        // Color.White, Color.Orange };

        // public Color[] chilianBkg = new Color[] { Color.FromArgb(64, 64, 64), Color.Black,
        // Color.FromArgb(64, 64, 64) }; public Color[] chilianFore = new Color[] {
        // Color.FromArgb(64, 64, 64), Color.White, Color.LemonChiffon };

        protected internal static string ASTERISK = " *";

        // protected static string gch = "Gt(Ch)";

        // protected static string gech = "Ge(Ch)";
        protected internal string originalHeaderText = "Ge(M)";

        // protected static string gfast = "GFast(M)"; protected static string gm = "Gt(M)";

        protected internal static string SEPARATOR = "but";
        protected internal static string TOOLTIP_TEXT = " but with user-defined parameters";

        public string OriginalHeaderText
        {
            get
            {
                return originalHeaderText;
            }
            set
            {
                originalHeaderText = value;
            }
        }

        protected internal string bindingRoundingField = string.Empty;

        public string BindingRoundingField
        {
            get
            {
                return bindingRoundingField;
            }
            set
            {
                bindingRoundingField = value;
            }
        }
    }

    public partial class UnitBoolCell : DataGridViewTextBoxCell
    {
        protected internal Color defaultColor = Color.DarkRed;

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            UnitRow u = ((this.OwningRow?.DataBoundItem as DataRowView)?.Row) as UnitRow;
            if (!EC.IsNuDelDetch(u))
            {
                u.ToDo = !u.ToDo;

                base.DataGridView.NotifyCurrentCellDirty(true);
                base.DataGridView.ClearSelection();
            }
            base.OnMouseClick(e);
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

        public UnitBoolCell() : base()
        {
            Style.SelectionBackColor = Color.Transparent;
            Style.SelectionForeColor = Color.Transparent;

            Style.BackColor = defaultColor;
            Style.ForeColor = defaultColor;
        }
    }
}