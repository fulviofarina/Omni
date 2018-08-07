using System;
using System.Windows.Forms;

namespace VTools
{
    public interface IOptions
    {
        bool DisableBasic { set; }
        bool DisableImportant { set; }
        EventHandler ShowProgress { get; }
        int Type { set; get; }

        event EventHandler AboutBoxClick;

        event EventHandler ConnectionBox;

        event EventHandler DatabaseInterfaceClick;

        event EventHandler DropDownClicked;

        event EventHandler ExplorerClick;

        event EventHandler HelpClick;

        event EventHandler PreferencesClick;

        event EventHandler RestoreFoldersClick;

        event EventHandler SaveClick;

        void ClickSave();

        void ResetProgress(int max);

        void Set();
    }

    public partial class ucOptions : UserControl, IOptions
    {
        private int type = 0;

        public bool DisableBasic
        {
            set
            {
                if (databaseToolStripMenuItem != null) databaseToolStripMenuItem.Enabled = value;
                // this.connectionsTSMI.Visible = enable; this.Save.Enabled = Visible;
                // folderRestoreTSMI.Visible = enable;
            }
        }

        public bool DisableImportant
        {
            set
            {
                if (this.explorerToolStripMenuItem != null) this.explorerToolStripMenuItem.Enabled = value;
                if (this.limsTSMI != null) this.limsTSMI.Enabled = value;
            }
        }

        public EventHandler ShowProgress
        {
            get
            {
                EventHandler pros = delegate
                {
                    this.progressBar?.PerformStep();
                    Application.DoEvents();
                };

                return pros;
            }
        }

        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public event EventHandler AboutBoxClick
        {
            add
            {
                if (aboutTSMI != null) aboutTSMI.Click += value;
            }

            remove
            {
                if (aboutTSMI != null) aboutTSMI.Click -= value;
            }
        }

        public event EventHandler ConnectionBox
        {
            add
            {
                if (connectionsTSMI != null) connectionsTSMI.Click += value;
            }
            remove
            {
                if (connectionsTSMI != null) connectionsTSMI.Click -= value;
            }
        }

        public event EventHandler DatabaseInterfaceClick
        {
            add
            {
                if (limsTSMI != null) limsTSMI.Click += value;
            }
            remove
            {
                if (limsTSMI != null) limsTSMI.Click -= value;
            }
        }

        public event EventHandler DropDownClicked
        {
            add
            {
                if (OptionsBtn != null) this.OptionsBtn.DropDownOpened += value;
            }

            remove
            {
                if (OptionsBtn != null) this.OptionsBtn.DropDownOpened -= value;
            }
        }

        public event EventHandler ExplorerClick
        {
            add
            {
                if (explorerToolStripMenuItem != null) explorerToolStripMenuItem.Click += value;
            }
            remove
            {
                if (explorerToolStripMenuItem != null) explorerToolStripMenuItem.Click -= value;
            }
        }

        public event EventHandler HelpClick
        {
            add
            {
                if (helpToolStripMenuItem2 != null) helpToolStripMenuItem2.Click += value;
                if (helpToolStripMenuItem != null) helpToolStripMenuItem.Click += value;
            }
            remove
            {
                if (helpToolStripMenuItem2 != null) helpToolStripMenuItem2.Click -= value;
                if (helpToolStripMenuItem != null) helpToolStripMenuItem.Click -= value;
            }
        }

        public event EventHandler PreferencesClick
        {
            add
            {
                if (preferencesTSMI != null) this.preferencesTSMI.Click += value;
            }

            remove
            {
                if (preferencesTSMI != null) this.preferencesTSMI.Click -= value;
            }
        }

        public event EventHandler RestoreFoldersClick
        {
            add
            {
                if (folderRestoreTSMI != null) this.folderRestoreTSMI.Click += value;
            }
            remove
            {
                if (folderRestoreTSMI != null) this.folderRestoreTSMI.Click -= value;
            }
        }

        public event EventHandler SaveClick
        {
            add
            {
                if (Save != null) this.Save.Click += value;
            }

            remove
            {
                if (Save != null) this.Save.Click -= value;
            }
        }

        public void ClickSave()
        {
            this.Save?.PerformClick();
        }

        public void ResetProgress(int max)
        {
            if (this.progressBar == null) return;
            this.progressBar.Minimum = 0;
            this.progressBar.Step = 1;
            if (max == 0)
            {
                this.progressBar.Maximum = 0;
                this.progressBar.Value = 0;
            }
            else this.progressBar.Maximum += max;
        }

        public void Set()
        {
            this.Save.Click += delegate
            {
                this.ParentForm?.Validate();
            };
        }

        public ucOptions()
        {
            InitializeComponent();
        }

        public ucOptions(int tipo)
        {
            InitializeComponent();
            type = tipo;
        }
    }
}