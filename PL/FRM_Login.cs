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
using Liphsoft.Crypto.Argon2;
using Pro_Salles.Class;
using DevExpress.XtraSplashScreen;
using System.Threading;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509.Qualified;
using System.Reflection;
using System.Diagnostics;

namespace Pro_Salles.PL
{
    public partial class FRM_Login : XtraForm
    {
        public FRM_Login()
        {
            InitializeComponent();
        }

        public void btn_login_Click(object sender, EventArgs e)
        {
            if (txt_user_name.IsStringValid() == false)
                return;
            if (txt_password.IsStringValid() == false)
                return;

            using (var db = new Pro_SallesDataContext())
            {
                var username = txt_user_name.Text;
                var password = txt_password.Text;


                var user = db.Users.SingleOrDefault(x => x.User_Name == username);
                if (user == null)
                    goto LogInFaild;

                else
                {
                    if (user.Is_Active == false)
                    {
                        XtraMessageBox.Show("تم تعطيل هذا الحساب", caption: ""
                        , buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
                        return;
                    }
                    var passWordHash = user.Password;
                    var Hashe = new PasswordHasher();
                    if (Hashe.Verify(passWordHash, password))
                    {
                        //Successfully login
                        this.Hide();
                        SplashScreenManager.ShowForm(parentForm: FRM_MAIN.Instance, typeof(FRM_Start_Splash));
                        Sessions.Set_User(user);

                        //Loading data here till the five secodes finished
                        //Thread.Sleep(5000);


                        Type T = typeof(Sessions);
                        var properties = T.GetProperties(BindingFlags.Public | BindingFlags.Static);

                        foreach (var item in properties)
                        {
                            var obj = item.GetValue(null);
                        }


                        FRM_MAIN.Instance.Show();

                        this.Close();
                        SplashScreenManager.CloseForm();
                        return;
                        //////////////////////
                    }
                    else
                        goto LogInFaild;

                }
            }
        LogInFaild:
            XtraMessageBox.Show("اسم المستخدم او كلمه السر غير صحيحه", caption: ""
            , buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            return;
        }
    }
}