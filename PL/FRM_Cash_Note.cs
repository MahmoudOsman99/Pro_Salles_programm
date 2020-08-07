using System;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using static Pro_Salles.Class.Master_Finance;
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
    public partial class FRM_Cash_Note : FRM_Master
    {
        bool isCashIn;
        Store store;
        Drower drawer;
        Account account;
        CustomersAndVendor cust_vend;
        public FRM_Cash_Note(bool _isCashIn)
        {
            InitializeComponent();
            isCashIn = _isCashIn;
        }
        private void FRM_Cash_Note_Load(object sender, EventArgs e)
        {
            Refresh_Data();
            look_part_type.EditValueChanged += Look_part_type_EditValueChanged;
            look_grid_part_id.EditValueChanged += Look_grid_part_id_EditValueChanged;
        }

        public override void Refresh_Data()
        {
            look_branch.Initialize_Data(Sessions.Stores, nameof(store.name), nameof(store.ID));
            look_drower.Initialize_Data(Sessions.Drowers, nameof(drawer.name), nameof(drawer.ID));
            look_part_type.Initialize_Data(Master.Part_Type_List);
            base.Refresh_Data();
        }

        private void Look_part_type_EditValueChanged(object sender, EventArgs e)
        {
            if (look_part_type.Is_Edit_Value_Int())
            {
                int partType = Convert.ToInt32(look_part_type.EditValue);
                if (partType == (int)Master.Part_Type.Customer)
                {
                    look_grid_part_id.InitializeData(Sessions.Customers, nameof(cust_vend.name), nameof(cust_vend.ID));
                    look_grid_part_id.EditValue = Sessions.Defaults.Customer;
                }
                else if (partType == (int)Master.Part_Type.Vendor)
                {
                    look_grid_part_id.InitializeData(Sessions.Vendors, nameof(cust_vend.name), nameof(cust_vend.ID));
                    look_grid_part_id.EditValue = Sessions.Defaults.Vendor;
                }
                else if (partType == (int)Master.Part_Type.Account)
                {
                    look_grid_part_id.InitializeData(Sessions.Accounts, nameof(account.name), nameof(account.ID));
                    look_grid_part_id.EditValue = null;
                }
            }
        }

        AccountBalance AccountBalance;
        private void Look_grid_part_id_EditValueChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(look_grid_part_id.EditValue);
            if (id != 0)
            {
                CustomersAndVendor cust_vend_account = null;

                if (Convert.ToByte(look_part_type.EditValue) == (int)Master.Part_Type.Vendor)
                    cust_vend_account = Sessions.Vendors.Single(x => x.ID == id);

                else if (Convert.ToByte(look_part_type.EditValue) == (int)Master.Part_Type.Customer)
                    cust_vend_account = Sessions.Customers.Single(x => x.ID == id);

                else if (Convert.ToByte(look_part_type.EditValue) == (int)Master.Part_Type.Account)
                    cust_vend_account = new CustomersAndVendor() { account_id = id };

                if (cust_vend_account != null)
                {
                    txt_part_address.Text = cust_vend_account.address;
                    txt_part_phone.Text = cust_vend_account.phone;
                    spin_part_maxCredit.EditValue = cust_vend_account.max_Credit;

                    AccountBalance = GetAccountBalance(cust_vend_account.account_id);
                    txt_part_balance.EditValue = AccountBalance.Balance;
                }

                return;

            }
            else
            {
                goto IfEmpty;
            }

        IfEmpty:
            txt_part_address.Text = "";
            txt_part_balance.Text = "";
            txt_part_phone.Text = "";
            spin_part_maxCredit.Text = "";
        }
    }
}
