using Rsx.DGV;
using System.Drawing;

namespace DB.UI
{
    public class MUESColumn : BindableDGVColumn
    {
        public MUESColumn()
            : base()
        {
            this.DefaultAction = DefaultAction.Visibility;

            CellTemplate = new BindableDGVCell();
            CellTemplate.Style.Font = new Font(CellTemplate.Style.Font.Name, 12f, FontStyle.Regular);
        }
    }
}