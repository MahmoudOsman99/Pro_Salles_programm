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
    public partial class FRM_Customers_Vendors_List : FRM_Master
    {
        bool Is_Customer;
        public FRM_Customers_Vendors_List(bool is_customer)
        {
            InitializeComponent();
            Is_Customer = is_customer;
            this.Text = Is_Customer ? "   قائمه العملاء" : "   قائمه الموردين";
            this.Load += FRM_Customers_Vendors_List_Load;
        }

        private void FRM_Customers_Vendors_List_Load(object sender, EventArgs e)
        {
            Refresh_Data();
            CustomersAndVendor ins;
            gridView1.Columns[nameof(ins.ID)].Visible = false;
            gridView1.Columns[nameof(ins.account_id)].Visible = false;
            gridView1.Columns[nameof(ins.Is_Customer)].Visible = false;
            gridView1.Columns[nameof(ins.name)].Caption = "الأسم";
            gridView1.Columns[nameof(ins.phone)].Caption = "الهاتف";
            gridView1.Columns[nameof(ins.mobile)].Caption = "الموبايل";
            gridView1.Columns[nameof(ins.address)].Caption = "العنوان";
            gridView1.Columns[nameof(ins.max_Credit)].Caption = "حد الإئتمان";
            gridView1.OptionsBehavior.Editable = false;
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.DoubleClick += GridView1_DoubleClick;
            if (Is_Customer) Sessions.Customers.ListChanged += Vendors_ListChanged;
            else Sessions.Vendors.ListChanged += Vendors_ListChanged;
            gridView1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            gridView1.Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
        }

        private void Vendors_ListChanged(object sender, ListChangedEventArgs e)
        {
            ////////////////////////This is for vendors or customers list changed//////////////////////////////
            Refresh_Data();
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var frm = new FRM_Customer_Vendor(Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID")));
                FRM_MAIN.Open_Form(frm, true);
            }
        }

        public override void Refresh_Data()
        {
            gridControl1.DataSource = (Is_Customer) ? Sessions.Customers : Sessions.Vendors;
            base.Refresh_Data();
        }
        public override void New()
        {
            base.New();
        }
    }
}
