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
using DevExpress.XtraEditors.Repository;

namespace Pro_Salles.PL
{
    public partial class FRM_Access_Profile : FRM_Master
    {
        User_Access_Profile_Name profile;
        Screens_Access_Profile ins;
        public FRM_Access_Profile()
        {
            InitializeComponent();
            New();
            Get_Data();
        }    
        public FRM_Access_Profile(int id)
        {
            InitializeComponent();
            using (var db = new Pro_SallesDataContext())
            {
                profile = db.User_Access_Profile_Names.SingleOrDefault(x => x.ID == id);
            }
            txt_name.Text = profile.Name;
            Get_Data();
        }
        public override void New()
        {
            profile = new User_Access_Profile_Name();
            txt_name.Text = profile.Name;
        }

        private void TreeList1_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
        {
            if (e.Node.Id >= 0)
            {
                var row = treeList1.GetRow(e.Node.Id) as Screens_Access_Profile;
                if (row != null)
                {
                    if (e.Column.FieldName == nameof(ins.Can_Add) && row.Actions.Contains(Master.Actions.Add) == false)                    
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    
                    else if (e.Column.FieldName == nameof(ins.Can_Delete) && row.Actions.Contains(Master.Actions.Delete) == false)
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();

                    else if (e.Column.FieldName == nameof(ins.Can_Edit) && row.Actions.Contains(Master.Actions.Edit) == false)
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    
                    else if (e.Column.FieldName == nameof(ins.Can_Open) && row.Actions.Contains(Master.Actions.Open) == false)
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    
                    else if (e.Column.FieldName == nameof(ins.Can_Print) && row.Actions.Contains(Master.Actions.Print) == false)
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    
                    else if (e.Column.FieldName == nameof(ins.Can_Show) && row.Actions.Contains(Master.Actions.Show) == false)
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                }
            }
        }

        public override void Get_Data()
        {
            List<Screens_Access_Profile> data;
            using (var db = new Pro_SallesDataContext())
            {
                ////////58
                data = (from s in Screens.Get_Screens
                        from d in db.User_Access_Profile_Details.Where
                        (x => x.Profile_ID == profile.ID && x.Screen_ID == s.Screen_ID).DefaultIfEmpty()
                        select new Screens_Access_Profile(s.Screen_Name)
                        {
                            Can_Add = (d == null) ? true : d.Can_Add,
                            Can_Delete = (d == null) ? true : d.Can_Delete,
                            Can_Edit = (d == null) ? true : d.Can_Edit,
                            Can_Open = (d == null) ? true : d.Can_Open,
                            Can_Print = (d == null) ? true : d.Can_Print,
                            Can_Show = (d == null) ? true : d.Can_Show,
                            Actions = s.Actions,
                            Screen_Name = s.Screen_Name,
                            Screen_Caption = s.Screen_Caption,
                            Screen_ID = s.Screen_ID,
                            Parent_Screen_ID = s.Parent_Screen_ID
                        }).ToList();
            }

            treeList1.DataSource = data;
            base.Get_Data();
        }

        public override bool Is_Data_Valide()
        {
            int flag = 0;
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = Error_Text;
                flag++;
            }
            return (flag == 0);
        }
        public override void Save()
        {
            var db = new Pro_SallesDataContext();
            if (profile.ID == 0)
            {
                db.User_Access_Profile_Names.InsertOnSubmit(profile);
            }
            else
            {
                db.User_Access_Profile_Names.Attach(profile);
            }
            profile.Name = txt_name.Text;
            db.SubmitChanges();
            db.User_Access_Profile_Details.DeleteAllOnSubmit
                (db.User_Access_Profile_Details.Where(x => x.Profile_ID == profile.ID));
            db.SubmitChanges();

            var data = treeList1.DataSource as List<Screens_Access_Profile>;
            var dbdata = data.Select(s => new User_Access_Profile_Detail
            {
                Can_Add = s.Can_Add,
                Can_Delete = s.Can_Delete,
                Can_Edit = s.Can_Edit,
                Can_Open = s.Can_Open,
                Can_Print = s.Can_Print,
                Can_Show = s.Can_Show,
                Profile_ID = profile.ID,
                Screen_ID = s.Screen_ID
            }).ToList();

            db.User_Access_Profile_Details.InsertAllOnSubmit(dbdata);
            db.SubmitChanges();

            base.Save();
        }

        public override void Set_Data()
        {
            base.Set_Data();
        }
        RepositoryItemCheckEdit repocheck;
        private void FRM_Access_Profile_Load(object sender, EventArgs e)
        {
            txt_name.Text = profile.Name;
            treeList1.CustomNodeCellEdit += TreeList1_CustomNodeCellEdit;
            treeList1.KeyFieldName = nameof(ins.Screen_ID);
            treeList1.ParentFieldName = nameof(ins.Parent_Screen_ID);
            treeList1.Columns[nameof(ins.Screen_Name)].Visible = false;
            treeList1.Columns[nameof(ins.Screen_Name)].OptionsColumn.AllowEdit = false;
            treeList1.Columns[nameof(ins.Screen_Caption)].OptionsColumn.AllowEdit = false;
            
            treeList1.BestFitColumns();

            treeList1.Columns[nameof(ins.Can_Add)].Caption = "اضافه";
            treeList1.Columns[nameof(ins.Can_Delete)].Caption = "حذف";
            treeList1.Columns[nameof(ins.Can_Edit)].Caption = "تعديل";
            treeList1.Columns[nameof(ins.Can_Open)].Caption = "فتح";
            treeList1.Columns[nameof(ins.Can_Print)].Caption = "طباعه";
            treeList1.Columns[nameof(ins.Can_Show)].Caption = "اظهار";
            treeList1.Columns[nameof(ins.Screen_Caption)].Caption = "الشاشه";

            repocheck = new RepositoryItemCheckEdit();
            repocheck.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgToggle1;

            treeList1.Columns[nameof(ins.Can_Add)].ColumnEdit =
            treeList1.Columns[nameof(ins.Can_Delete)].ColumnEdit =
            treeList1.Columns[nameof(ins.Can_Edit)].ColumnEdit =
            treeList1.Columns[nameof(ins.Can_Open)].ColumnEdit =
            treeList1.Columns[nameof(ins.Can_Print)].ColumnEdit =
            treeList1.Columns[nameof(ins.Can_Show)].ColumnEdit = repocheck;

            treeList1.Columns[nameof(ins.Can_Add)].Width =
         treeList1.Columns[nameof(ins.Can_Delete)].Width =
           treeList1.Columns[nameof(ins.Can_Edit)].Width =
           treeList1.Columns[nameof(ins.Can_Open)].Width =
          treeList1.Columns[nameof(ins.Can_Print)].Width =
           treeList1.Columns[nameof(ins.Can_Show)].Width = 25;
                                                    
        }
    }    
}