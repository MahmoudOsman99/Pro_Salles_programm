using System;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using Pro_Salles.Properties;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Pro_Salles.PL
{
    public partial class FRM_MAIN : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private static FRM_MAIN _instance;
        public static FRM_MAIN Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FRM_MAIN();
                return _instance;
            }
        }

        public FRM_MAIN()
        {
            InitializeComponent();
        }
        //void Check_Open_Form()
        //{

        //}
        public static void Open_Form_By_Name(string Form_Name)
        {
            Form frm = null;

            switch (Form_Name)
            {
                case "FRM_Customer":
                    frm = new FRM_Customer_Vendor(true);
                    break;
                case "FRM_Vendor":
                    frm = new FRM_Customer_Vendor(false);
                    break;
                case "FRM_Customers_List":
                    frm = new FRM_Customers_Vendors_List(true);
                    break;
                case "FRM_Vendors_List":
                    frm = new FRM_Customers_Vendors_List(false);
                    break;
                case "FRM_Purchase_Invoice":
                    frm = new FRM_Invoice(Master.Invoice_Type.Purchase);
                    break;
                case "FRM_Sales_Invoice":
                    frm = new FRM_Invoice(Master.Invoice_Type.Salles);
                    break;
                case "FRM_Purchase_Invoice_List": // Show the salles invoices || all the invoices 
                    frm = new FRM_Invoice_List(Master.Invoice_Type.Purchase);
                    break;
                case "FRM_Sales_Invoice_List":  // Show the Purchase invoices || all the invoices 
                    frm = new FRM_Invoice_List(Master.Invoice_Type.Salles);
                    break;
                case "FRM_Purchase_Return_Invoice":
                    frm = new FRM_Invoice(Master.Invoice_Type.Purchase_Return);
                    break;
                case "FRM_Sales_Return_Invoice":
                    frm = new FRM_Invoice(Master.Invoice_Type.Salles_Return);
                    break;
                case "FRM_Purchase_Return_Invoice_List":
                    frm = new FRM_Invoice_List(Master.Invoice_Type.Purchase_Return);
                    break;
                case "FRM_Sales_Return_Invoice_List":
                    frm = new FRM_Invoice_List(Master.Invoice_Type.Salles_Return);
                    break;
                    
                default:
                    var ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == Form_Name);
                    if (ins != null)
                    {
                        frm = Activator.CreateInstance(ins) as Form;
                        if (Application.OpenForms[frm.Name] != null)
                        {
                            frm = Application.OpenForms[frm.Name];
                        }
                        else
                        {
                            //frm.Show();
                            Open_Form(frm);
                            return;
                        }
                        frm.BringToFront();
                    }
                    break;
            }


            if (frm != null)
            {
                frm.Name = Form_Name;
                Open_Form(frm);
            }
        }
        public static void Open_Form(Form frm, bool OpenInDialog = false)
        {
            if (Sessions.CurrentUser.User_Type == (byte)Master.User_Type.Admin)
            {
                //(OpenInDialog) ? frm.ShowDialog() : frm.Show();
                if (OpenInDialog == true)
                    frm.ShowDialog();
                else
                    frm.Show();
                frm.BringToFront();
                return;                
            }

            var screen = Sessions.Screens_Access.SingleOrDefault(x => x.Screen_Name == frm.Name);
            if (screen != null)
            {
                if (screen.Can_Open == true)
                {
                    if (OpenInDialog == true)
                        frm.ShowDialog();
                    else
                        frm.Show();
                    frm.BringToFront();
                    return;
                }
                else
                {
                    XtraMessageBox.Show("عفوا... ليس لديك صلاحيه للدخول",
                        caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void accordionControl1_ElementClick(object sender, DevExpress.XtraBars.Navigation.ElementClickEventArgs e)
        {
            var tag = e.Element.Tag as string;
            if (tag != string.Empty)
            {
                Open_Form_By_Name(tag);
            }
        }

        private void FRM_MAIN_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.SkinName = UserLookAndFeel.Default.SkinName;
            Settings.Default.PaletteName = UserLookAndFeel.Default.ActiveSvgPaletteName;
            Settings.Default.Save();
            Application.Exit();
        }

        private void FRM_MAIN_Load(object sender, EventArgs e)
        {
            accordionControl1.Elements.Clear();
            var screens = Sessions.Screens_Access.Where(x => x.Can_Show == true || Sessions.CurrentUser.User_Type == (byte)Master.User_Type.Admin);

            screens.Where(w => w.Parent_Screen_ID == 0).ToList().ForEach(s =>
              {
                  AccordionControlElement elm = new AccordionControlElement()
                  {
                      Text = s.Screen_Caption,
                      Tag = s.Screen_Name,
                      Name = s.Screen_Name,
                      Style = ElementStyle.Group
                  };
                  accordionControl1.Elements.Add(elm);
                  Add_Accordion_Element(elm, s.Screen_ID);
              });
        }
        void Add_Accordion_Element(AccordionControlElement parent, int parentid)
        {
            var screens = Sessions.Screens_Access.Where(x => x.Can_Show == true || Sessions.CurrentUser.User_Type == (byte)Master.User_Type.Admin);
            screens.Where(x => x.Parent_Screen_ID == parentid).ToList().ForEach(s =>
            {
                AccordionControlElement elm = new AccordionControlElement()
                {
                    Text = s.Screen_Caption,
                    Tag = s.Screen_Name,
                    Name = s.Screen_Name,
                    Style = ElementStyle.Item
                };
                parent.Elements.Add(elm);
            });
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            new FRM_Access_Profile().ShowDialog();
        }
    }
}
