using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Pro_Salles.PL
{
    public partial class FRM_Master_List : FRM_Master
    {
        public FRM_Master_List()
        {
            InitializeComponent();
        }

        private void FRM_Master_List_Load(object sender, EventArgs e)
        {
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            gridView1.Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
            gridView1.DoubleClick += GridView1_DoubleClick;
            Refresh_Data();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            {
                if (info.InRow || info.InRowCell)
                {
                    Open_Form(Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID")));
                }
            }
        }

        public virtual void Open_Form(int id)
        {

        }

        public override void Save()
        {

        }
        public override void Delete()
        {
            
        }
    }
}