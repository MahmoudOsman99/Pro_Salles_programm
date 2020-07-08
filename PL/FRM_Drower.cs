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
    public partial class FRM_Drower : FRM_Master
    {
        Drower drower;
        Account account;
        public FRM_Drower()
        {
            InitializeComponent();
            New();
        }
        public FRM_Drower(int id)
        {
            InitializeComponent();
            using (var db = new Pro_SallesDataContext())
            {
                drower = db.Drowers.Single(x => x.ID == id);
                Get_Data();
            }
        }
        public override void New()
        {
            drower = new Drower();
            base.New();
        }
        public override void Get_Data()
        {
            txt_drower_name.Text = drower.name;
            base.Get_Data();
        }
        public override void Set_Data()
        {
            drower.name = txt_drower_name.Text;
            base.Set_Data();
        }
        public override void Save()
        {
            if (txt_drower_name.Text.Trim() == string.Empty)
            {
                txt_drower_name.ErrorText = "برجاء ادخل أسم الخزنه";
                return;
            }
            var db = new Pro_SallesDataContext();
            if (drower.ID == 0)
            {
                account = new Account();
                db.Drowers.InsertOnSubmit(drower);
                db.Accounts.InsertOnSubmit(account);
            }
            else
            {
                db.Drowers.Attach(drower);
                account = db.Accounts.Single(x => x.ID == drower.account_id);
            }

            account.name = drower.name;
            Set_Data();
            db.SubmitChanges();
            drower.account_id = account.ID;
            db.SubmitChanges();

            base.Save();
        }
    }
}