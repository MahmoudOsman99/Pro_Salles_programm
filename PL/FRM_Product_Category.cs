using DevExpress.Utils.Text;
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
    public partial class FRM_Product_Category : FRM_Master
    {
        DAL.Product_Category cat;
        public FRM_Product_Category()
        {
            InitializeComponent();
            New();
        }
        public override void Save()
        {
            if (Data_Validate() == false)
                return;

            var db = new Pro_SallesDataContext();
            if (cat.ID == 0)
                db.Product_Categories.InsertOnSubmit(cat);
            else
                db.Product_Categories.Attach(cat);
            Set_Data();

            db.SubmitChanges();

            base.Save();
        }
        public override void New()
        {
            cat = new Product_Category();
            base.New();
        }
        public override void Get_Data()
        {
            txt_name.Text = cat.name;
            lookUpEdit1.EditValue = cat.parent_ID;

            cat.number = "0";

            base.Get_Data();
        }
        public override void Set_Data()
        {
            cat.name = txt_name.Text;
            cat.parent_ID = (lookUpEdit1.EditValue as int?) ?? 0;
            base.Set_Data();
        }
        public override void Delete()
        {
            base.Delete();
        }
        bool Data_Validate()
        {
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = "يجب ادخال الأسم";
                return false;
            }
            var db = new DAL.Pro_SallesDataContext();
            if (db.Product_Categories.Where(x => x.name.Trim() == txt_name.Text.Trim() && x.ID != cat.ID).Count() > 0)
            {
                txt_name.ErrorText = "هذا الأسم مسجل مسبقا";
                return false;
            }
            return true;
        }

        private void FRM_Product_Category_Load(object sender, EventArgs e)
        {
            Refresh_Data();
            lookUpEdit1.Properties.DisplayMember = nameof(cat.name);
            lookUpEdit1.Properties.ValueMember = nameof(cat.ID);

            treeList1.KeyFieldName = nameof(cat.ID);
            treeList1.ParentFieldName = nameof(cat.parent_ID);
            treeList1.Columns[nameof(cat.number)].Visible = false;
            treeList1.Columns[nameof(cat.name)].Caption = "الأسم";
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;
        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if(int.TryParse(e.Node.GetValue("ID".ToString()).ToString(), out id))
            {
                var db = new Pro_SallesDataContext();
                cat = db.Product_Categories.Single(x => x.ID == id);
                Get_Data();
            }
        }

        public override void Refresh_Data()
        {
            var db = new Pro_SallesDataContext();
            var groups = db.Product_Categories;
            lookUpEdit1.Properties.DataSource = groups;
            treeList1.DataSource = groups;
            treeList1.OptionsBehavior.Editable = false;
            treeList1.ExpandAll();
            base.Refresh_Data();
        }
    }
}