using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pro_Salles.PL
{
    public partial class FRM_Drowers_List : FRM_Master
    {
        public FRM_Drowers_List()
        {
            InitializeComponent();
            Refresh_Data();
        }

        private void FRM_Drowers_List_Load(object sender, EventArgs e)
        {
            btn_delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["name"].Caption = "الأسم";
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var frm = new FRM_Drower(Convert.ToInt32(view.GetFocusedRowCellValue("ID")));
                FRM_MAIN.Open_Form(frm, true);
                Refresh_Data();
            }
        }

        public override void New()
        {
            var frm = new FRM_Drower();
            FRM_MAIN.Open_Form(frm, true);
            Refresh_Data();
        }
        public override void Refresh_Data()
        {
            using (var db = new Pro_SallesDataContext())
            {
                gridControl1.DataSource = db.Drowers.ToList();
            }
            base.Refresh_Data();
        }
    }
}