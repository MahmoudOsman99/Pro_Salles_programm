using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting.Native;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pro_Salles.Class.Master;

namespace Pro_Salles.PL
{
    public partial class FRM_Product : FRM_Master
    {
        Product prod;
        Pro_SallesDataContext sdb = new Pro_SallesDataContext();
        RepositoryItemLookUpEdit repolook = new RepositoryItemLookUpEdit();
        public FRM_Product()
        {
            InitializeComponent();
            Refresh_Data();
            New();
        }

        public FRM_Product(int id)
        {
            InitializeComponent();
            Refresh_Data();
            Load_Product(id);
        }
        void Load_Product(int id)
        {
            using (var db = new Pro_SallesDataContext())
            {
                prod = db.Products.Single(x => x.ID == id);
            }
            this.Text = string.Format("   بيانات صنف: {0}", prod.name);
            Get_Data();
        }
        public override void New()
        {
            prod = new Product() { code = Get_Last_Product_Code(), is_active = true };
            var db = new Pro_SallesDataContext();
            var cat = db.Product_Categories.
                Where(x => db.Product_Categories.Where(c => c.parent_ID == x.ID).Count() == 0).FirstOrDefault();
            if (cat != null)
                prod.Category_ID = cat.ID;
            base.New();
            this.Text = "اضافه صنف جديد";
            var data = gridView1.DataSource as BindingList<Product_Unit>;
            if (db.Units_names.Count() == 0)
            {
                db.Units_names.InsertOnSubmit(new Units_name() { name = "قطعه" });
                db.SubmitChanges();
                Refresh_Data();
            }
            data.Add(new Product_Unit() { factor = 1, unit_id = db.Units_names.First().ID, barcode = Get_New_Barcode() });
        }
        public override void Save()
        {
            gridView1.UpdateCurrentRow();
            if (Validate_Data() == false)
                return;
            var db = new Pro_SallesDataContext();
            if (prod.ID == 0)
                db.Products.InsertOnSubmit(prod);
            else
                db.Products.Attach(prod);
            Set_Data();
            db.SubmitChanges();
            var data = gridView1.DataSource as BindingList<Product_Unit>;
            foreach (var item in data)
            {
                item.product_id = prod.ID;
            }
            sdb.SubmitChanges();
            this.Text = string.Format("   بيانات صنف: {0}", prod.name);
            base.Save();
        }
        public override void Get_Data()
        {
            txt_code.Text = prod.code;
            txt_name.Text = prod.name;
            look_cat.EditValue = prod.Category_ID;
            look_type.EditValue = prod.type;
            look_CostCalculatingMethod.EditValue = prod.CostCalculatingMethod;
            cb_active.Checked = prod.is_active;
            memoEdit1.Text = prod.discription;

            if (prod.image != null)
            {
                prod_pic.Image = Get_Image(prod.image.ToArray());
            }
            else prod_pic.Image = null;

            gridControl1.DataSource = sdb.Product_Units.Where(x => x.product_id == prod.ID);
            base.Get_Data();
        }
        public override void Set_Data()
        {
            prod.code = txt_code.Text;
            prod.name = txt_name.Text;
            prod.Category_ID = Convert.ToInt32(look_cat.EditValue);
            prod.type = Convert.ToByte(look_type.EditValue);
            prod.CostCalculatingMethod = Convert.ToByte(look_CostCalculatingMethod.EditValue);
            prod.is_active = cb_active.Checked;
            prod.discription = memoEdit1.Text;
            prod.image = Get_Byte_From_Image(prod_pic.Image);
            base.Set_Data();
        }
        public override void Refresh_Data()
        {
            using (var db = new Pro_SallesDataContext())
            {
                look_cat.Properties.DataSource =
                    db.Product_Categories.
                    Where(x => db.Product_Categories.
                    Where(w => w.parent_ID == x.ID).Count() == 0).ToList();

                repolook.DataSource = db.Units_names.ToList();
            }
            look_CostCalculatingMethod.LookUp_DataSource(Master_Inventory.CostCalculatingMethod_List);
            base.Refresh_Data();
        }
        byte[] Get_Byte_From_Image(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    if (img == null)
                        return ms.ToArray();
                    img.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
                catch
                {
                    return ms.ToArray();
                }
            }
        }                                                                              
        Image Get_Image(byte[] b)                                                       /////////
        {                                                                               /////////
            try                                                                         /////////
            {                                                                           /////////
                Image img;                                                              /////////
                MemoryStream ms = new MemoryStream(b, false);                           /////////
                return img = Image.FromStream(ms);                                      /////////
            }                                                                           /////////
            catch                                                                       /////////
            {                                                                           /////////
                return null;                                                            /////////
            }                                                                           /////////
        }                                                                               /////////
        bool Validate_Data()
        {
            if (txt_code.Text.Trim() == string.Empty)
            {
                txt_code.ErrorText = Error_Text;
                return false;
            }
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = Error_Text;
                return false;
            }
            if (look_cat.EditValue is int == false || Convert.ToInt32(look_cat.EditValue) <= 0)
            {
                look_cat.ErrorText = Error_Text;
                return false;
            }
            int v = 0;
            if (int.TryParse(look_type.EditValue.ToString(), out v) && v < 0)
            {
                look_type.ErrorText = Error_Text;
                return false;
            }

            var db = new Pro_SallesDataContext();

            if (db.Products.Where(x => x.name.Trim() == txt_name.Text.Trim() && x.ID != prod.ID).Count() > 0)
            {
                txt_name.ErrorText = "هذا الأسم مسجل مسبقا";
                return false;
            }
            if (db.Products.Where(x => x.code.Trim() == txt_code.Text.Trim() && x.ID != prod.ID).Count() > 0)
            {
                txt_code.ErrorText = "هذا الكود مسجل مسبقا";
                return false;
            }
            return true;
        }

        Product_Unit ins = new Product_Unit();
        private void FRM_Product_Load(object sender, EventArgs e)
        {
            Product_Category pr;
            look_cat.Properties.DisplayMember = nameof(pr.name);
            look_cat.Properties.ValueMember = nameof(pr.ID);
            look_cat.ProcessNewValue += Look_cat_ProcessNewValue;

            look_type.Properties.DataSource = Product_Types_List;
            look_type.LookUp_DataSource(Product_Types_List, "Name", "ID");

            look_cat.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.Columns[nameof(ins.ID)].Visible = false;
            gridView1.Columns[nameof(ins.product_id)].Visible = false;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Top; //مكان السطر الجديد فوق اللى بتضيف فيها وحده القياس الجديده

            //تقدر تقول لاضافه اله حاسبه للكولن اللي موجود عندك مع منع كتابه احرف
            RepositoryItemCalcEdit calcEdit = new RepositoryItemCalcEdit();

            gridControl1.RepositoryItems.Add(calcEdit);
            gridControl1.RepositoryItems.Add(repolook);

            gridView1.Columns[nameof(ins.sell_price)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.buy_price)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.sell_discount)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.factor)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.unit_id)].ColumnEdit = repolook;

            gridView1.Columns[nameof(ins.unit_id)].Caption = "اسم الوحده";
            gridView1.Columns[nameof(ins.factor)].Caption = "معامل التحويل";
            gridView1.Columns[nameof(ins.buy_price)].Caption = "سعر الشراء";
            gridView1.Columns[nameof(ins.sell_price)].Caption = "سعر البيع";
            gridView1.Columns[nameof(ins.sell_discount)].Caption = "خصم البيع";
            gridView1.Columns[nameof(ins.barcode)].Caption = "الباركود";

            repolook.DisplayMember = "name";
            repolook.ValueMember = "ID";

            repolook.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            repolook.NullText = "";
            repolook.ProcessNewValue += Repolook_ProcessNewValue;

            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.InvalidRowException += GridView1_InvalidRowException;
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;
            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;
            Refresh_Data();
        }

        private void GridView1_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == nameof(ins.unit_id))
            {
                var d = ((Collection<Product_Unit>)gridView1.DataSource).Select(x => x.unit_id).ToList();
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();
                repo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repo.NullText = "";
                repo.ProcessNewValue += Repolook_ProcessNewValue;
                using (var db = new Pro_SallesDataContext())
                {
                    var current_id = (Int32?)e.CellValue;
                    d.Remove(current_id ?? 0);
                    repo.DataSource = db.Units_names.Where(x => d.Contains(x.ID) == false).ToList();
                    repo.DisplayMember = "name";
                    repo.ValueMember = "ID";
                    repo.PopulateColumns();
                    repo.Columns["name"].Caption = "الأسم";
                    repo.Columns["ID"].Visible = false;
                    e.RepositoryItem = repo;
                }
            }
        }

        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //هنا في تحكم ان السل بتاعت الجريد فيو يتوقف فيها التعديل مادام رقم السطر == 0 اما لو مش بيساوي صفر ف التعديل هيشتغل
            gridView1.Columns[nameof(ins.factor)].OptionsColumn.AllowEdit = !(e.FocusedRowHandle == 0);
        }

        private void GridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var row = e.Row as Product_Unit;
            var view = sender as GridView;
            if (row == null)
                return;
            if (row.factor <= 1 && e.RowHandle != 0)
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.factor)], "يجب ان تكون القيمه اكبر من 1");
            }
            if (row.unit_id <= 0)
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.unit_id)], Error_Text);
            }
            if (Check_Exists_Barcode(row.barcode, prod.ID))
            {
                e.Valid = false;
                view.SetColumnError(view.Columns[nameof(row.barcode)], "هذا الكود موجود بالفعل");
            }
        }

        private void Repolook_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string st && st.Trim() != string.Empty)
            {
                var os = new Units_name() { name = st };
                using (var db = new Pro_SallesDataContext())
                {
                    db.Units_names.InsertOnSubmit(os);
                    db.SubmitChanges();
                }
                ((List<Units_name>)repolook.DataSource).Add(os);
                /*هنا الريبو الاساسي انت بتعرض فيه الوحدات و لكن هنا مش هترف توصله ف انت اخدته من السيندر لانه صاحب الحدث الاول ف اخدته و عملتله تحويل و اخدت منه الداتا و ضيفت عليها الوحده الجديده و بالتالى الريبو اللى بنعمل فيها تحقق هتاخد من الريبو الكبيره او اللى بتاخد منها او اللى عملت الحدث*/
                ((List<Units_name>)(((LookUpEdit)sender).Properties.DataSource)).Add(os);
                e.Handled = true;
            }
        }

        private void Look_cat_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string st && st.Trim() != string.Empty)
            {
                var o = new Product_Category(){ name = st, parent_ID = 0, number = "0"};
                using (var db = new Pro_SallesDataContext())
                {
                    db.Product_Categories.InsertOnSubmit(o);
                    db.SubmitChanges();
                }
                ((List<Product_Category>)look_cat.Properties.DataSource).Add(o);
                e.Handled = true;
            }
        }
        string Get_New_Barcode()
        {
            string max_code;
            using (var db = new Pro_SallesDataContext())
            {
                max_code = db.Product_Units.Select(x => x.barcode).Max();
            }
            return Get_Next_Code(max_code);
        }
        string Get_Last_Product_Code()
        {
            string max_code;
            using (var db = new Pro_SallesDataContext())
            {
                max_code = db.Products.Select(x => x.code).Max();
            }
            return Get_Next_Code(max_code);
        }
        bool Check_Exists_Barcode(string bar, int id)
        {
            using (var db = new Pro_SallesDataContext())
            {
                return db.Product_Units.Where(x => x.barcode == bar && x.product_id != id).Count() > 0;
            }
        }
    }
}