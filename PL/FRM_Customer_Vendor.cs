using DevExpress.XtraEditors;
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
    public partial class FRM_Customer_Vendor : FRM_Master
    {
        DAL.CustomersAndVendor Cust_Vend;
        bool Is_Customer;
        public FRM_Customer_Vendor(bool is_customer)
        {
            InitializeComponent();
            Is_Customer = is_customer;
            this.Text = (Is_Customer) ? "  عميل" : "  مورد";
            this.Name = Is_Customer ? "FRM_Customer" : "FRM_Vendor";
            New();
        }
        public FRM_Customer_Vendor(int id)
        {
            InitializeComponent();
            this.Text = (Is_Customer) ? "  عميل" : "  مورد";
            this.Name = Is_Customer ? "FRM_Customer" : "FRM_Vendor";
            Load_Person(id);
        }
        void Load_Person(int id)
        {
            using (var db = new Pro_SallesDataContext())
            {
                Cust_Vend = db.CustomersAndVendors.Single(x => x.ID == id);
                Is_Customer = Cust_Vend.Is_Customer;
                Get_Data();
            }
        }
        private void FRM_Customer_Vendor_Load(object sender, EventArgs e)
        {
        }
        public override void New()
        {
            Cust_Vend = new DAL.CustomersAndVendor();
            Cust_Vend.max_Credit = 5000;
            base.New();
        }
        public override void Delete()
        {
            if (XtraMessageBox.Show("هل تريد الحذف؟", "نحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var db = new Pro_SallesDataContext())
                {
                    db.CustomersAndVendors.Attach(Cust_Vend);
                    db.CustomersAndVendors.DeleteOnSubmit(Cust_Vend);
                    db.SubmitChanges();
                    XtraMessageBox.Show("تم الحذف بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                base.Delete();
            }
        }
        public override void Get_Data()
        {
            txt_name.Text = Cust_Vend.name;
            txt_phone.Text = Cust_Vend.phone;
            txt_mobile.Text = Cust_Vend.mobile;
            txt_address.Text = Cust_Vend.address;
            txt_account_id.Text = Cust_Vend.account_id.ToString();
            spin_maxCredit.EditValue = Cust_Vend.max_Credit;
            base.Get_Data();
        }
        public override void Set_Data()
        {
            Cust_Vend.name = txt_name.Text;
            Cust_Vend.phone = txt_phone.Text;
            Cust_Vend.mobile = txt_mobile.Text;
            Cust_Vend.address = txt_address.Text;
            Cust_Vend.Is_Customer = Is_Customer;
            Cust_Vend.max_Credit = Convert.ToDouble(spin_maxCredit.EditValue);
            base.Set_Data();
        }
        public override void Save()
        {
            if (Data_Valid() == false)
                return;

            var db = new Pro_SallesDataContext();
            Account acc;

            if (Cust_Vend.ID == 0)
            {
                db.CustomersAndVendors.InsertOnSubmit(Cust_Vend);
                acc = new Account();
                db.Accounts.InsertOnSubmit(acc);
            }
            else
            {
                db.CustomersAndVendors.Attach(Cust_Vend);
                acc = db.Accounts.Single(x => x.ID == Cust_Vend.account_id);
            }

            Set_Data();
            acc.name = Cust_Vend.name;//هنا انت عملت حفظ لاسم الحساب من غير الرقم بتاعه لانه بيزيد تلقائي
            db.SubmitChanges();//هنا عملت حفظ عشان الاسم بتسجل و الرقم بتاع الحساب يزيد و بالتالى رقم الحساب بتاع العميل هيكون من الحساب اللي رقمه زاد لوحده بعد الحفظ
            Cust_Vend.account_id = acc.ID;//هنا بعد ما الرقم زاد و عايز العميل ياخد رقم الحساب بتاعه يبقي ياخده عادي لانه زاد و اتحفظ
            db.SubmitChanges();//هنا انت حفظت البيانات تاني بعد ما العميل اخد رقم حسابه
            base.Save();
        }
        bool Data_Valid()
        {
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            var db = new Pro_SallesDataContext();
            if (db.CustomersAndVendors.Where(x => x.name.Trim() == txt_name.Text.Trim()
            && x.Is_Customer == Is_Customer
            && x.ID != Cust_Vend.ID).Count() > 0)
            {
                txt_name.ErrorText = "هذا الأسم مسجل مسبقا";
                return false;
            }
            return true;
        }
    }
}
