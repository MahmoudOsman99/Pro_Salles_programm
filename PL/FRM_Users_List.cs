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
using Pro_Salles.DAL;
using DevExpress.Charts.Model;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Pro_Salles.PL
{
    public partial class FRM_Users_List : FRM_Master
    {
        User user;
        public FRM_Users_List()
        {
            InitializeComponent();
            this.Load += FRM_Users_List_Load;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                int i = 0;
                i = Convert.ToInt32(gridView1.GetFocusedRowCellValue(nameof(user.ID)));
                new FRM_User(i).ShowDialog();
            }
        }

        private void FRM_Users_List_Load(object sender, EventArgs e)
        {
            Refresh_Data();
            btn_delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.Columns[nameof(user.ID)].Visible = false;
            gridView1.Columns[nameof(user.Password)].Visible = false;
            gridView1.Columns[nameof(user.User_Name)].Visible = false;
            gridView1.Columns[nameof(user.User_Type)].Visible = false;
            gridView1.Columns[nameof(user.Screen_Profile_ID)].Visible = false;
            gridView1.Columns[nameof(user.Settings_Profile_ID)].Visible = false;
            gridView1.Columns[nameof(user.Name)].Caption = "الأسم";
            gridView1.Columns[nameof(user.Is_Active)].Caption = "نشط";
        }

        public override void Refresh_Data()
        {
            gridControl1.DataSource = Sessions.Users;
            base.Refresh_Data();
        }
    }
}