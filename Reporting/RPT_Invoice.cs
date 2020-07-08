using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Pro_Salles.Class;
using Pro_Salles.DAL;

namespace Pro_Salles.Reporting
{
    public partial class RPT_Invoice : DevExpress.XtraReports.UI.XtraReport
    {
        Master.Invoice_Type Type;
        public RPT_Invoice()
        {
            InitializeComponent();
            lbl_com_name.Text = Sessions.Company_Info.name;
            lbl_com_phone.Text = Sessions.Company_Info.phone;
            lbl_com_address.Text = Sessions.Company_Info.address;
        }
        void BindData()
        {
            lbl_code.DataBindings.Add("Text", this.DataSource, "code");
            xrBarCode1.DataBindings.Add("Text", this.DataSource, "ID");
            lbl_date.DataBindings.Add("Text", this.DataSource, "date");
            lbl_branch.DataBindings.Add("Text", this.DataSource, "Store");
            lbl_paid.DataBindings.Add("Text", this.DataSource, "paid");
            lbl_remaing.DataBindings.Add("Text", this.DataSource, "remaing");
            lbl_expences.DataBindings.Add("Text", this.DataSource, "expences");
            lbl_total.DataBindings.Add("Text", this.DataSource, "total");
            lbl_tax.DataBindings.Add("Text", this.DataSource, "tax_value");
            lbl_discount.DataBindings.Add("Text", this.DataSource, "discount_value");
            lbl_net.DataBindings.Add("Text", this.DataSource, "net");
            //lbl_net_text.DataBindings.Add("Text", this.DataSource, "/////");
            lbl_invoice_type.DataBindings.Add("Text", this.DataSource, "Invoicetype");
            lbl_part_type.DataBindings.Add("Text", this.DataSource, "PartName");
            lbl_phone.DataBindings.Add("Text", this.DataSource, "Phone");
            lbl_qty.DataBindings.Add("Text", this.DataSource, "Products_Count");

            cell_product.ExpressionBindings.Add(new ExpressionBinding("BeforePrint","Text", "ProductName"));
            cell_unit.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "UnitName"));
            cell_qty.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "item_qty"));
            cell_total.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "total_price"));
            cell_price.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "price"));
        }
        public static void Print(object DS)
        {
            RPT_Invoice rpt = new RPT_Invoice();
            rpt.DataSource = DS;
            rpt.DetailReport.DataSource = rpt.DataSource;
            rpt.DetailReport.DataMember = "Products";
            rpt.BindData();

            switch (Sessions.CurrentSettings.InvoicePrintMode)
            {
                case Master.Print_Mode.Direct:
                    rpt.Print();
                    break;
                case Master.Print_Mode.ShowPreview:
                    rpt.ShowPreview();
                    break;
                case Master.Print_Mode.ShowDialog:
                    rpt.PrintDialog();
                    break;
                default:
                    break;
            }
        }

        private void lbl_net_text_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lbl_net_text.Text =  NumberToText.Utils.ConvertMoneyToArabicText(lbl_net.Text);
        }

        int index = 1;
        private void cell_index_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cell_index.Text = (index++).ToString();
        }
    }
}
