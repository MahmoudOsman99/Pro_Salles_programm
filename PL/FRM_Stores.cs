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

namespace Pro_Salles.PL
{
    public partial class FRM_Stores : FRM_Master
    {
        Store store;
        public FRM_Stores()
        {
            InitializeComponent();
            New();
        }
        public FRM_Stores(int id)
        {
            InitializeComponent();
            var db = new Pro_SallesDataContext();
            store = db.Stores.Where(x => x.ID == id).First();
            Get_Data();
        }
        public override void Save()
        {
            if (txt_store_name.Text.Trim() == string.Empty)
            {
                txt_store_name.ErrorText = "برجاء ادخال اسم المخزن";
                return;
            }
            Account Salles_Account = new Account();
            Account Salles_Return_Account = new Account();
            Account Inventory_Account = new Account();
            Account Cost_Of_Sold_Goods_Account = new Account();

            var db = new Pro_SallesDataContext();

            if (store.ID == 0)
            {
                db.Stores.InsertOnSubmit(store);
                db.Accounts.InsertOnSubmit(Salles_Account);
                db.Accounts.InsertOnSubmit(Salles_Return_Account);
                db.Accounts.InsertOnSubmit(Inventory_Account);
                db.Accounts.InsertOnSubmit(Cost_Of_Sold_Goods_Account);

                store.Discount_Allowed_Account_ID = Sessions.Defaults.Discount_Allowed_Account;
                store.Discount_Received_Account_ID = Sessions.Defaults.Discount_Received_Account;
            }
            else
            {
                db.Stores.Attach(store);
                Salles_Account = db.Accounts.Single(x => x.ID == store.Salles_Account_ID);
                Salles_Return_Account = db.Accounts.Single(x => x.ID == store.Salles_Return_Account_ID);
                Inventory_Account = db.Accounts.Single(x => x.ID == store.Inventory_Account_ID);
                Cost_Of_Sold_Goods_Account = db.Accounts.Single(x => x.ID == store.Cost_Of_Sold_Goods_Account_ID);
            }

            Set_Data();
            Salles_Account.name = store.name + " - مبيعات";
            Salles_Return_Account.name = store.name + " - مردود مبيعات";
            Inventory_Account.name = store.name + " - المخزون";
            Cost_Of_Sold_Goods_Account.name = store.name + " - تكلفه البضاعه المباعه";
            db.SubmitChanges();

            store.Salles_Account_ID = Salles_Account.ID;
            store.Salles_Return_Account_ID = Salles_Return_Account.ID;
            store.Inventory_Account_ID = Inventory_Account.ID;
            store.Cost_Of_Sold_Goods_Account_ID = Cost_Of_Sold_Goods_Account.ID;

            db.SubmitChanges();
            XtraMessageBox.Show("تم الحفظ بنجاح", "تم");
        }
        public override void New()
        {
            store = new DAL.Store();
            Get_Data();
        }
        public override void Delete()
        {
            var db = new Pro_SallesDataContext();
            if (XtraMessageBox.Show("هل تريد الحذف؟", "نحذير", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var Log = db.Store_Logs.Where(x => x.store_id == store.ID).Count();

                var Account_Log = db.Journals.Where(x => x.Account_ID == store.Cost_Of_Sold_Goods_Account_ID
                || x.Account_ID == store.Salles_Account_ID
                || x.Account_ID == store.Salles_Return_Account_ID
                || x.Account_ID == store.Inventory_Account_ID).Count();
                if (Log + Account_Log > 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن حذف المخزن حيث تم استخدامه في النظام", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                db.Stores.Attach(store);
                db.Stores.DeleteOnSubmit(store);

                db.Accounts.DeleteAllOnSubmit(db.Accounts.Where(x =>
                 x.ID == store.Cost_Of_Sold_Goods_Account_ID
                || x.ID == store.Salles_Account_ID
                || x.ID == store.Salles_Return_Account_ID
                || x.ID == store.Inventory_Account_ID));

                db.SubmitChanges();
                XtraMessageBox.Show("تم الحذف بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                New();
            }
        }
        public override void Get_Data()
        {
            txt_store_name.Text = store.name;
        }
        public override void Set_Data()
        {
            store.name = txt_store_name.Text;
        }
        private void FRM_Stores_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F3)
            //    Delete();
        }
    }
}