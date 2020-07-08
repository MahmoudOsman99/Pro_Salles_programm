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
    public partial class FRM_Master : XtraForm
    {
        public bool isNew = false;
        public FRM_Master()
        {
            InitializeComponent();
        }
        public static string Error_Text
        {
            get
            {
                return "هذا الحقل مطلوب";
            }
        }
        public virtual void Save()
        {
            XtraMessageBox.Show("تم الحفظ بنجاح", "تم");
            Refresh_Data();
            isNew = false;
            btn_delete.Enabled = true;
        }
        public virtual void Delete()
        {
            XtraMessageBox.Show("تم الحذف بنجاح", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public virtual void New()
        {
            Get_Data();
            isNew = true;
            btn_delete.Enabled = false;
        }
        public virtual void Get_Data()
        {

        }
        public virtual void Set_Data()
        {

        }
        public virtual void Refresh_Data()
        {

        }
        public virtual void Print()
        {

        }
        public virtual bool Is_Data_Valide()
        {
            return true;
        }
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, isNew ? Master.Actions.Add : Master.Actions.Edit) == true)
                if (Is_Data_Valide())
                {
                    Save();
                    btn_delete.Enabled = true;
                }
        }

        private void btn_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            New();

        }

        public static bool CheckActionAuthorization(string formName, Master.Actions actions, User user = null)
        {
            if (user == null) user = Sessions.CurrentUser;
            if (user.User_Type == (byte)Master.User_Type.Admin)
                return true;
            else
            {
                var screen = Sessions.Screens_Access.SingleOrDefault(x => x.Screen_Name == formName);
                bool flag = true;
                if (screen != null)
                {
                    switch (actions)
                    {
                        case Master.Actions.Show:
                            flag = screen.Can_Show;
                            break;
                        case Master.Actions.Open:
                            flag = screen.Can_Open;
                            break;
                        case Master.Actions.Add:
                            flag = screen.Can_Add;
                            break;
                        case Master.Actions.Edit:
                            flag = screen.Can_Edit;
                            break;
                        case Master.Actions.Delete:
                            flag = screen.Can_Delete;
                            break;
                        case Master.Actions.Print:
                            flag = screen.Can_Print;
                            break;
                        default:
                            break;
                    }
                    if (flag == false)
                    {
                        XtraMessageBox.Show("عفوا... ليس لديك صلاحيه للدخول",
                        caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    }
                }
                return flag;
            }
        }
        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, Master.Actions.Delete) == true)
                Delete();
        }

        private void FRM_Master_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btn_save.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_new.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                btn_print.PerformClick();
            }
            if (e.KeyCode == Keys.F4)
            {
                btn_delete.PerformClick();
            }
        }

        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CheckActionAuthorization(this.Name, Master.Actions.Print) == true)
                Print();
        }

        public static bool AskForDeletion()
        {
            return (XtraMessageBox.Show("هل تريد الحذف ؟", caption: "تأكيد الحذف"
                , buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes);
        }
    }
}