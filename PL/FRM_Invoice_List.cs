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
using Pro_Salles.Class;
using Pro_Salles.DAL;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

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
        }

        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            var View = e.View as GridView;
            if (View != null & View.LevelName == "Products")
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
                    look_grid_part_id.GridLookUp_Style(Sessions.Customers);
                    lyc_part.Text = "العميل";
                }
                else if (part_type == (int)Master.Part_Type.Vendor)
                {
                    look_grid_part_id.GridLookUp_Style(Sessions.Vendors);
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
                    this.Name = Screens.Add_Purchase_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Salles:
                    this.Text = "  فواتير المبيعات";
                    this.Name = Screens.Add_Salles_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Purchase_Return:
                    break;
                case Master.Invoice_Type.Salles_Return:
                    break;
                default:
                    throw new NotImplementedException();
            }

            look_part_type.LookUp_DataSource(Master.Part_Type_List, "Name", "ID");
            look_part_type.Properties.PopulateColumns();
            //look_part_type.Properties.Columns["ID"].Visible = false;

            look_drawer.LookUp_DataSource(Sessions.Drowers, "name", "ID");
            look_branch.LookUp_DataSource(Sessions.Stores, "name", "ID");
        }

        public override void Refresh_Data()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (date_from.DateTime.Year > 1950) fromDate = date_from.DateTime;
            if (date_to.DateTime.Year > 1950) toDate = date_to.DateTime;

            using (var db = new Pro_SallesDataContext())
            {
                var query = from inv in db.Invoice_Headers.Where(x => x.invoice_type == (byte)Type)
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
                                PayStatus = "",
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

                repoBranch.LookUp_DataSource(Sessions.Stores, gridView1.Columns[nameof(ins.branch)], gridControl1);
                repoBranch.LookUp_DataSource(Sessions.Drowers, gridView1.Columns[nameof(ins.drower)], gridControl1);
                repoPartType.LookUp_DataSource(Master.Part_Type_List, gridView1.Columns[nameof(ins.part_type)], gridControl1, "Name", "ID");
                repoPart.LookUp_DataSource(parts, gridView1.Columns[nameof(ins.part_id)], gridControl1);
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
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            Refresh_Data();
            //flyoutPanel1.HidePopup();
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
    }
}