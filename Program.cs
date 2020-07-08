using DevExpress.LookAndFeel;
using Pro_Salles.Class;
using Pro_Salles.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pro_Salles
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            UserLookAndFeel.Default.SkinName = Settings.Default.SkinName.ToString();
            UserLookAndFeel.Default.SetSkinStyle(Settings.Default.SkinName.ToString(), Settings.Default.PaletteName.ToString());

            var frm = new PL.FRM_Login();
            frm.Show();
            if (Debugger.IsAttached)
            {
                frm.txt_user_name.Text = "salles";
                frm.txt_password.Text = "1";
                frm.btn_login_Click(null, null);
            }
            Application.Run();

            //new PL.FRM_Start_Splash().Show();
            
            //Application.Run(new PL.FRM_User_Settings_Profile_List());

            //Application.Run(new PL.FRM_User_Settings_Profile());

            //Application.Run(new PL.FRM_MAIN());
            //Application.Run(new PL.FRM_MAIN());
        }
    }
}