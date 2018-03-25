using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Rsx.Dumb;
using Rsx.DGV;
using static DB.LINAA;

namespace DB.UI
{
   
    /*
    public partial class SampleColumn : DataGridViewTextBoxColumn, ISampleColumn
    {
        protected internal string bindingField = string.Empty;

        protected internal DataRow bindingRow = null;

       // protected internal Action

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
    */

    public  class SampleColumn : BindableDGVColumn
    {
        public SampleColumn()
            : base()
        {
            CellTemplate = new BindableDGVCell();
             this.DefaultAction = DefaultAction.ReadOnly;
       //  this.arrayOfBackColors = new Color[] { Color.FromArgb(64, 64, 64), Color.White, Color.White };
     //   this.arrayOfForeColors = new Color[] { Color.Yellow, Color.Black, Color.Black };

    }
}
    public  class UnitBoolColumn : DataGridViewTextBoxColumn
    {
        public UnitBoolColumn()
            : base()
        {
            CellTemplate = new UnitBoolCell();
        }
    }

    public  class UnitSSFCell : DataGridViewTextBoxCell
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


  
    public  class UnitSSFColumn : BindableDGVColumn, IUnitSSFColumn
    {
        public UnitSSFColumn() : base()
        {
            base.DefaultAction = DefaultAction.Enable;
            base.CellTemplate = new UnitSSFCell();
        }

        protected internal string bindableAsteriskField = string.Empty;


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

    
    }

    public  class UnitBoolCell : DataGridViewTextBoxCell
    {
      //  protected internal Color defaultColor = Color.DarkRed;

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
            UnitRow u = ((this.OwningRow?.DataBoundItem as DataRowView)?.Row) as UnitRow;

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

        public UnitBoolCell() : base()
        {
            Style.SelectionBackColor = Color.Transparent;
            Style.SelectionForeColor = Color.Transparent;

            Color defaultColor = Color.DarkRed;
            Style.BackColor = defaultColor;
            Style.ForeColor = defaultColor;
        }
    }
}