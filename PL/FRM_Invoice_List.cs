using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using Pro_Salles.Reporting;

namespace Pro_Salles.PL
{
    public partial class FRM_Invoice_List : FRM_Master_List
    {
        Master.Invoice_Type Type;

        RepositoryItemLookUpEdit repoPartType;
        RepositoryItemLookUpEdit repoPart;
        RepositoryItemLookUpEdit repoBranch;
        RepositoryItemLookUpEdit repoDrawer;
        public FRM_Invoice_List(Master.Invoice_Type type)
        {
            InitializeComponent();

            date_from.DateTime =
            date_to.DateTime = DateTime.Now;

            look_part_type.EditValueChanged += Look_part_type_EditValueChanged;
            gridView1.AddButtonToGroupHeader(Properties.Resources.editfilter, OpenFilter);
            //  flyoutPanel1.Options.CloseOnOuterClick = true;
            this.Type = type;
            //to rename the column caption by get into the gridView1 subItems
            gridControl1.ViewRegistered += GridControl1_ViewRegistered;
            //to disappeare the tabheader for the subItem header
            gridView1.OptionsDetail.ShowDetailTabs = false;
            gridView1.PopupMenuShowing += GridView1_PopupMenuShowing;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
        }

        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;
            var remaing = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "remaing"));
            var paid = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "paid"));
            var net = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "net"));
            if (e.Column.FieldName == "PayStatus")
            {
                if (remaing == 0)
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
                else if (remaing < net)
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
                else
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            }
        }

        private void GridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            //81
            if (e.HitInfo.InRowCell || e.HitInfo.InRow)
            {
                var DXButtonPrint = new DevExpress.Utils.Menu.DXMenuItem() { Caption = "طباعه" };
                DXButtonPrint.ImageOptions.SvgImage = Properties.Resources.print;
                DXButtonPrint.Click += DXButtonPrint_Click;
                e.Menu.Items.Add(DXButtonPrint);

                var DXButtonDelete = new DevExpress.Utils.Menu.DXMenuItem() { Caption = "حذف" };
                DXButtonDelete.ImageOptions.SvgImage = Properties.Resources.actions_trash;
                DXButtonDelete.Click += DXButtonDelete_Click;
                e.Menu.Items.Add(DXButtonDelete);
            }
        }

        private void DXButtonDelete_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedInvoicesIDs();
            if (ids != null)
                FRM_Invoice.Delete(ids, this.Type, this.Name);
        }

        private void DXButtonPrint_Click(object sender, EventArgs e)
        {
            List<int> ids = GetSelectedInvoicesIDs();
            if (ids != null)
                FRM_Invoice.Print(ids, this.Type, this.Name);
        }
        List<int> GetSelectedInvoicesIDs()
        {
            var Handles = gridView1.GetSelectedRows();
            List<int> ids = new List<int>();
            foreach (var Handle in Handles)
            {
                ids.Add(Convert.ToInt32(gridView1.GetRowCellValue(Handle, "ID")));
            }
            if (ids.Count == 0)
            {
                XtraMessageBox.Show("برجاء اختيار فاتوره واحده علي الأقل",
                    caption: "Note", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                return null;
            }
            return ids;
        }
        public override void Delete()
        {
            DXButtonDelete_Click(null, null);
        }

        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            var View = e.View as GridView;

            if (View != null && View.LevelName == "Products")
            {
                View.OptionsView.ShowViewCaption = true;
                View.ViewCaption = "الأصناف";
                View.Columns["Product"].Caption = "الصنف";
                View.Columns["Unit"].Caption = "الوحده";
                View.Columns["price"].Caption = "السعر";
                View.Columns["item_qty"].Caption = "العدد";
                View.Columns["discount_value"].Caption = "الخصم";
                View.Columns["total_price"].Caption = "الاجمالي";
            }
        }

        private void Look_part_type_EditValueChanged(object sender, EventArgs e)
        {
            if (look_part_type.Is_Edit_Value_Int())
            {
                int part_type = Convert.ToInt32(look_part_type.EditValue);
                if (part_type == (int)Master.Part_Type.Customer)
                {
                    look_grid_part_id.InitializeData(Sessions.Customers);
                    lyc_part.Text = "العميل";
                }
                else if (part_type == (int)Master.Part_Type.Vendor)
                {
                    look_grid_part_id.InitializeData(Sessions.Vendors);
                    lyc_part.Text = "المورد";
                }

            }
        }

        void OpenFilter(object sender, EventArgs e)
        {
            flyoutPanel1.ShowPopup();
        }

        private void FRM_Invoice_List_Load(object sender, EventArgs e)
        {
            date_from.AddClearValueButton();
            date_to.AddClearValueButton();
            look_drawer.AddClearValueButton();
            look_branch.AddClearValueButton();
            look_part_type.AddClearValueButton();
            look_grid_part_id.AddClearValueButton();
            TranslateCaption();

            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    this.Text = "  فواتير المشتريات";
                    this.Name = Screens.View_Purchase_Invoices.Screen_Name;
                    break;
                case Master.Invoice_Type.Salles:
                    this.Text = "  فواتير المبيعات";
                    this.Name = Screens.View_Sales_Invoices.Screen_Name;
                    break;
                case Master.Invoice_Type.Purchase_Return:
                    this.Text = "  فواتير مردود المشتريات";
                    this.Name = Screens.View_Purchase_Return_Invoices.Screen_Name;
                    break;
                case Master.Invoice_Type.Salles_Return:
                    this.Text = "  فواتير مردود المبيعات";
                    this.Name = Screens.View_Sales_Return_Invoices.Screen_Name;
                    break;
                default:
                    throw new NotImplementedException();
            }

            look_part_type.Initialize_Data(Master.Invoices_Part_Type_List, "Name", "ID");
            look_part_type.Properties.PopulateColumns();
            //look_part_type.Properties.Columns["ID"].Visible = false;

            look_drawer.Initialize_Data(Sessions.Drowers, "name", "ID");
            look_branch.Initialize_Data(Sessions.Stores, "name", "ID");

            btn_refresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        }

        public override void Refresh_Data()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (date_from.DateTime.Year > 1950) fromDate = date_from.DateTime;
            if (date_to.DateTime.Year > 1950) toDate = date_to.DateTime;

            using (var db = new Pro_SallesDataContext())
            {                                                                                       //i did OrderByDescending
                var query = from inv in db.Invoice_Headers.Where(x => x.invoice_type == (byte)Type).OrderByDescending(x => x.date)
                            select new
                            {
                                inv.ID,
                                inv.code,
                                inv.date,
                                inv.part_type,
                                inv.part_id,
                                inv.branch,
                                ItemsCount = db.Invoice_Details.Where(x => x.invoice_id == inv.ID).Count(),
                                inv.posted_to_store,
                                inv.total,
                                inv.discount_value,
                                inv.discount_ratio,
                                inv.tax,
                                inv.tax_value,
                                inv.expences,
                                inv.net,
                                inv.paid,
                                inv.drower,
                                inv.remaing,
                                PayStatus = (inv.remaing == 0) ? "مسدده" : (inv.remaing == inv.net) ? "غير مسدده" : "مسدده جزئيا",
                                Products = (from itm in db.Invoice_Details.Where(x => x.invoice_id == inv.ID)
                                            from pr in db.Products.Where(x => x.ID == itm.item_id).DefaultIfEmpty()
                                            from unt in db.Units_names.Where(x => x.ID == itm.item_unit_id).DefaultIfEmpty()
                                            select new
                                            {
                                                Product = pr.name,
                                                Unit = unt.name,
                                                itm.price,
                                                itm.item_qty,
                                                itm.discount_value,
                                                itm.total_price,
                                            }
                                            ).ToList()
                            };
                if (fromDate != null)
                    query = query.Where(x => x.date.Date >= fromDate.Value.Date);

                if (toDate != null)
                    query = query.Where(x => x.date.Date <= toDate.Value.Date);

                if (look_grid_part_id.IsEditValueValid(false))
                    query = query.Where(x => x.part_id == Convert.ToInt32(look_grid_part_id.EditValue));

                if (look_part_type.IsEditValueValid(false))
                    query = query.Where(x => x.part_type == Convert.ToInt32(look_part_type.EditValue));

                if (look_drawer.IsEditValueValid(false))
                    query = query.Where(x => x.drower == Convert.ToInt32(look_drawer.EditValue));

                if (look_branch.IsEditValueValid(false))
                    query = query.Where(x => x.branch == Convert.ToInt32(look_branch.EditValue));

                gridControl1.DataSource = query;

                var ins = query.FirstOrDefault();
                var parts = new List<CustomersAndVendor>();
                parts.AddRange(Sessions.Customers);
                parts.AddRange(Sessions.Vendors);

                repoBranch.Initialize_Data(Sessions.Stores, gridView1.Columns[nameof(ins.branch)], gridControl1);
                repoDrawer.Initialize_Data(Sessions.Drowers, gridView1.Columns[nameof(ins.drower)], gridControl1);
                repoPartType.Initialize_Data(Master.Invoices_Part_Type_List, gridView1.Columns[nameof(ins.part_type)], gridControl1, "Name", "ID");
                repoPart.Initialize_Data(parts, gridView1.Columns[nameof(ins.part_id)], gridControl1);
            }
            base.Refresh_Data();
        }

        public override void Open_Form(int id)
        {
            var frm = new FRM_Invoice(Type, id);
            FRM_MAIN.Open_Form(frm);
        }

        void TranslateCaption()
        {
            gridView1.Columns["ID"].Caption = "كود";
            gridView1.Columns["code"].Caption = "رقم";
            gridView1.Columns["date"].Caption = "تاريخ";
            gridView1.Columns["part_type"].Caption = "طرف التعامل";
            gridView1.Columns["part_id"].Caption = "اسم طرف التعامل";
            gridView1.Columns["branch"].Caption = "الفرع";
            gridView1.Columns["ItemsCount"].Caption = "عدد الأصناف";
            gridView1.Columns["posted_to_store"].Caption = "مرحل من المخزن";
            gridView1.Columns["total"].Caption = "الاجمالي";
            gridView1.Columns["discount_value"].Caption = "ق.خصم";
            gridView1.Columns["discount_ratio"].Caption = "ن.خصم";
            gridView1.Columns["tax_value"].Caption = "ق.ضريبه";
            gridView1.Columns["tax"].Caption = "ن.ضريبه";
            gridView1.Columns["expences"].Caption = "مصروفات اخري";
            gridView1.Columns["net"].Caption = "الصافي";
            gridView1.Columns["paid"].Caption = "المدفوع";
            gridView1.Columns["drower"].Caption = "الخزنه";
            gridView1.Columns["remaing"].Caption = "المتبقي";
            gridView1.Columns["PayStatus"].Caption = "حاله السداد";

            gridView1.Columns["date"].DisplayFormat.FormatString = "dd-MM-yyyy hh:mm tt";
            gridView1.Columns["date"].DisplayFormat.FormatType = FormatType.Custom;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            Refresh_Data();
            //flyoutPanel1.HidePopup();
        }

        public string GetFilters()
        {
            var str = "";

            if (date_from.EditValue != null)
                str += lyc_from_date.Text + " : " + date_from.Text;

            if (date_to.EditValue != null)
                str += " | " + lyc_to_date.Text + " : " + date_to.Text;

            if (look_branch.EditValue != null)
                str += " | " + lyc_branch.Text + " : " + look_branch.Text;

            if (look_drawer.EditValue != null)
                str += " | " + lyc_drawer.Text + " : " + look_drawer.Text;

            if (look_part_type.EditValue != null)
                str += " | " + lyc_part_type.Text + " : " + look_part_type.Text;

            if (look_grid_part_id.EditValue != null)
                str += " | " + lyc_part.Text + " : " + look_grid_part_id.Text;


            return str;
        }
        private void btn_clear_filters_Click(object sender, EventArgs e)
        {
            date_from.EditValue =
              date_to.EditValue =
          look_drawer.EditValue =
          look_branch.EditValue =
       look_part_type.EditValue =
    look_grid_part_id.EditValue = null;
            //btn_apply.PerformClick();
        }
        public override void Print()
        {
            RPT_Grid_Print.Print(gridControl1, "كشف فواتير المبيعات", GetFilters());
            base.Print();
        }
        public override void New()
        {
            FRM_MAIN.Open_Form(new FRM_Invoice(Type));
        }
    }
}