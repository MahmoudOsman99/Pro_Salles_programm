using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Pro_Salles.Class.Master;

namespace Pro_Salles.UserControls
{
    class Cost_Distribution_Option : XtraUserControl
    {
        public RadioGroup group = new RadioGroup();
        public Cost_Distribution_Options Selected_Option { get
            {
                if (group.EditValue != null)
                    return ((Cost_Distribution_Options)group.EditValue);
                else return Cost_Distribution_Options.By_QTY;
            } }
        public Cost_Distribution_Option()
        {
            LayoutControl lc = new LayoutControl();
            lc.Dock = System.Windows.Forms.DockStyle.Fill;
            group.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[]
            {
                new DevExpress.XtraEditors.Controls.RadioGroupItem(Cost_Distribution_Options.By_Price,"حسب السعر"),
                new DevExpress.XtraEditors.Controls.RadioGroupItem(Cost_Distribution_Options.By_QTY,"حسب الكميه")
            });
            group.SelectedIndex = 1;
            lc.AddItem("طريقه توزيع المصاريف على الأصناف", group).TextLocation = DevExpress.Utils.Locations.Top;
            this.Controls.Add(lc);
            this.Dock = System.Windows.Forms.DockStyle.Top;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //this.Height = 80;
        }
    }
}