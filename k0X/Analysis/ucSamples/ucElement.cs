using System;
using System.Drawing;
using System.Windows.Forms;
using DB;

namespace k0X
{
  public partial class ucElement : UserControl
  {
    public bool m_Checked;
    private Font font;
    public ucPeriodicTable ucPeriodic = null;

    private LINAA.ElementsRow elementRow;

    public LINAA.ElementsRow ElementRow
    {
      get { return elementRow; }
      set { elementRow = value; }
    }

    public ucElement(LINAA.ElementsRow _elementRow, ucPeriodicTable perTab)
    {
      InitializeComponent();

      elementRow = _elementRow;
      elementRow.ucControl = this;
      elementRow.Element = elementRow.Element.Trim();
      elementRow.ElementNameEn = elementRow.ElementNameEn.Trim();

      ucPeriodic = perTab;

      this.BS.DataSource = elementRow;
      this.m_Sym.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BS, "Element", true));

      font = new Font(m_Sym.Font, FontStyle.Bold);
      //this.m_Z.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BS, "Z", true));
      m_Sym.BackColor = Color.Transparent;
      //  m_Z.BackColor = Color.Transparent;
      m_Sym.ForeColor = Color.Black;
      //   m_Z.ForeColor = Color.Black;
      m_Sym.Font = font;
      Dock = DockStyle.Fill;
      Enabled = false;
    }

    public void PaintElement()
    {
      if (this.Enabled)
      {
        Icon elementIcon = (Icon)Properties.PT.ResourceManager.GetObject(this.elementRow.ElementNameEn);
        if (elementIcon != null) this.BackgroundImage = elementIcon.ToBitmap();

        m_Sym.Visible = false;
        m_Sym.Enabled = true;
        //    m_Z.Enabled = true;
        //       m_Sym.BackColor = Color.Indigo;
        //  m_Z.BackColor = Color.Thistle;
        //      m_Sym.ForeColor = Color.WhiteSmoke;
        //  m_Z.ForeColor = Color.WhiteSmoke;

        //     m_Sym.Font = font;
        //    m_Z.Font = new Font(m_Z.Font, FontStyle.Regular);
        m_Checked = false;
      }
      else
      {
        m_Sym.Enabled = false;
        m_Sym.Visible = true;
        //    m_Z.Enabled = false;

        if (this.BackgroundImage != null) this.BackgroundImage.Dispose();
        this.BackgroundImage = null;

        //    m_Z.Font = new Font(m_Z.Font, FontStyle.Regular);
      }
    }

    public override void Refresh()
    {
      if (!m_Checked)
      {
        // m_Sym.Font = new Font(m_Sym.Font, FontStyle.Underline | FontStyle.Bold);
        //    m_Sym.BackColor = Color.Green; //Image = Image.FromFile("c:\\green.jpg");
        //  m_Sym.ForeColor = System.Drawing.Color.White;

        m_Checked = true;
      }
      else
      {
        //  m_Sym.Font = new Font(m_Sym.Font, FontStyle.Bold);
        //  m_Sym.ForeColor = System.Drawing.Color.Black;
        //  m_Sym.BackColor = Color.LightGray;

        m_Checked = false;
      }
    }

    private void m_Sym_MouseHover(object sender, EventArgs e)
    {
      //  DB.LINAA.PeaksRow[] row = elementRow.GetPeaksRows();
      // ucPeaks2 Peaks = this.daddy as ucPeaks2;

      //  LINAA.PeaksDataTable clon = new LINAA.PeaksDataTable();

      //  row.CopyToDataTable<LINAA.PeaksRow>(clon, LoadOption.OverwriteChanges);

      //   ucPeriodic.ucPeaks.RefreshWeightedAvgAndSD(null);
    }
  }
}