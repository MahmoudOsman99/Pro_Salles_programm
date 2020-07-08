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
using Pro_Salles.DAL;

namespace Pro_Salles.PL
{
    public partial class FRM_User_Settings_Profile_List : FRM_Master
    {
        public FRM_User_Settings_Profile_List()
        {
            InitializeComponent();
            Refresh_Data();
            gridView1.DoubleClick += GridView1_DoubleClick;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["Name"].Caption = "الأسم";
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }
        public override void New()
        {
            var frm = new FRM_User_Settings_Profile();
            FRM_MAIN.Open_Form(frm, true);
            base.New();
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var frm = new FRM_User_Settings_Profile(Convert.ToInt32(view.GetFocusedRowCellValue("ID")));
                FRM_MAIN.Open_Form(frm, true);
                Refresh_Data();
            }
        }
        public override void Refresh_Data()
        {
            using (var db = new Pro_SallesDataContext())
            {
                gridControl1.DataSource = db.UserSettingsProfiles.ToList();
            }
            base.Refresh_Data();
        }

        private void FRM_User_Settings_Profile_List_Load(object sender, EventArgs e)
        {

        }
    }
}