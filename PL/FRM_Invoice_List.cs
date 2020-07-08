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

namespace Pro_Salles.PL
{
    public partial class FRM_Invoice_List : FRM_Master_List
    {
        Master.Invoice_Type Type;
        public FRM_Invoice_List(Master.Invoice_Type type)
        {
            InitializeComponent();
            this.Type = type;
        }

        private void FRM_Invoice_List_Load(object sender, EventArgs e)
        {
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    this.Text = "  فواتير المشتريات";
                    break;
                case Master.Invoice_Type.Salles:
                    this.Text = "  فواتير المبيعات";
                    break;
                case Master.Invoice_Type.Purchase_Return:
                    break;
                case Master.Invoice_Type.Salles_Return:
                    break;
                default:
                    throw new NotImplementedException();
            }

        }

        public override void Refresh_Data()
        {
            using (var db = new Pro_SallesDataContext())
            {
                var query = from inv in db.Invoice_Headers.Where(x => x.invoice_type == (byte)Type)
                            from prt in db.CustomersAndVendors.Where(x => x.ID == inv.part_id).DefaultIfEmpty()
                            from st in db.Stores.Where(x => x.ID == inv.branch).DefaultIfEmpty()
                            from dr in db.Drowers.Where(x => x.ID == inv.drower).DefaultIfEmpty()
                            select new
                            {
                                inv.ID,
                                inv.code,
                                inv.date,
                                Branch = dr.name,
                                inv.posted_to_store,
                                inv.total,
                                inv.tax,
                                inv.tax_value,
                                inv.expences,
                                inv.discount_value,
                                inv.discount_ratio,
                                inv.net,
                                inv.paid,
                                inv.remaing,
                                PartName = prt.name,
                                PayStatus = "",
                                ItemsCount = db.Invoice_Details.Where(x => x.invoice_id == inv.ID).Count(),
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
                gridControl1.DataSource = query;
            }


            base.Refresh_Data();
        }

        public override void Open_Form(int id)
        {
            FRM_MAIN.Open_Form(new FRM_Invoice(Type, id));
        }
    }
}