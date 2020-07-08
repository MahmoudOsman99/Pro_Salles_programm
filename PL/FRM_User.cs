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
using Pro_Salles.Class;
using Liphsoft.Crypto.Argon2;

namespace Pro_Salles.PL
{
    public partial class FRM_User : FRM_Master
    {
        User user;
        public FRM_User()
        {
            InitializeComponent();
            Refresh_Data();
            New();
        }
        public FRM_User(int id)
        {
            InitializeComponent();
            Refresh_Data();
            using (var db = new Pro_SallesDataContext())
            {
                user = db.Users.SingleOrDefault(x => x.ID == id);
                Get_Data();
            }
        }
        public override void New()
        {
            user = new User();
            user.Is_Active = true;
            base.New();
        }
        public override void Refresh_Data()
        {
            using (var db = new Pro_SallesDataContext())
            {
                look_screen_profile_ID.LookUp_DataSource(db.User_Access_Profile_Names.Select(x => new { x.ID, x.Name }).ToList());
                look_settings_profile_ID.LookUp_DataSource(db.UserSettingsProfiles.Select(x => new { x.ID, x.Name }).ToList());
                look_user_type.LookUp_DataSource(Master.User_Type_List);                
            }
            base.Refresh_Data();
        }
        public override void Get_Data()
        {

            txt_name.Text = user.Name;
            txt_user_name.Text = user.User_Name;
            toggle_is_active.IsOn = user.Is_Active;
            txt_password.Text = user.Password;
            look_screen_profile_ID.EditValue = user.Screen_Profile_ID;
            look_settings_profile_ID.EditValue = user.Settings_Profile_ID;
            look_user_type.EditValue = user.User_Type;

            base.Get_Data();
        }
        public override void Set_Data()
        {
            if (user.Password != txt_password.Text)
            {
                var hasher = new PasswordHasher();
                string myHash = hasher.Hash(txt_password.Text);
                txt_password.Text = myHash;
            }

            user.Name = txt_name.Text;
            user.User_Name = txt_user_name.Text.Trim();
            user.Is_Active = toggle_is_active.IsOn;
            user.Password = txt_password.Text;
            user.Screen_Profile_ID = Convert.ToInt32(look_screen_profile_ID.EditValue);
            user.Settings_Profile_ID = Convert.ToInt32(look_settings_profile_ID.EditValue);
            user.User_Type = Convert.ToByte(look_user_type.EditValue);
            base.Set_Data();
        }
        public override bool Is_Data_Valide()
        {
            int flag = 0;

            using (var db = new Pro_SallesDataContext())
            {
                if (db.Users.Where(x => x.User_Name.Trim() == txt_user_name.Text.Trim() && (x.ID != user.ID)).Count() > 0)
                {
                    flag++;
                    txt_user_name.ErrorText = "هذا الأسم مسجل بالفعل";
                };

                if (db.Users.Where(x => x.Name.Trim() == txt_name.Text.Trim() && (x.ID != user.ID)).Count() > 0)
                {
                    flag++;
                    txt_name.ErrorText = "هذا الأسم مسجل بالفعل";
                };
            }

            flag += txt_name.IsStringValid() ? 0 : 1;
            flag += txt_user_name.IsStringValid() ? 0 : 1;
            flag += txt_password.IsStringValid() ? 0 : 1;
            flag += look_screen_profile_ID.IsEditValueValidAndNotZero() ? 0 : 1;
            flag += look_settings_profile_ID.IsEditValueValidAndNotZero() ? 0 : 1;
            flag += look_user_type.IsEditValueValidAndNotZero() ? 0 : 1;

            return (flag == 0);
        }
        public override void Save()
        {
            if (Is_Data_Valide() == false)
                return;
            using (var db = new Pro_SallesDataContext())
            {
                if (user.ID == 0)
                    db.Users.InsertOnSubmit(user);
                else
                    db.Users.Attach(user);
                Set_Data();
                db.SubmitChanges();
            }

            base.Save();
        }

        private void FRM_User_Load(object sender, EventArgs e)
        {
            Refresh_Data();
        }
    }
}