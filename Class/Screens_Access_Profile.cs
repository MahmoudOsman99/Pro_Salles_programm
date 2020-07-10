using DevExpress.Utils.Filtering.Internal;
using DevExpress.XtraBars.Ribbon.ViewInfo;
using Pro_Salles.PL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Pro_Salles.Class
{
    public class Screens_Access_Profile
    {
        public static int Max_ID = 1;
        public Screens_Access_Profile(string Name, Screens_Access_Profile parent = null)
        {
            Screen_Name = Name;
            Screen_ID = Max_ID++;
            if (parent != null)
                Parent_Screen_ID = parent.Screen_ID;
            else Parent_Screen_ID = 0;

            Actions = new List<Master.Actions>()
            {
                Master.Actions.Add,
                Master.Actions.Delete,
                Master.Actions.Edit,
                Master.Actions.Open,
                Master.Actions.Print,
                Master.Actions.Show
            };
        }
        public List<Master.Actions> Actions { get; set; }
        public int Screen_ID { get; set; }
        public int Parent_Screen_ID { get; set; }
        public string Screen_Name { get; set; }
        public string Screen_Caption { get; set; }
        public bool Can_Show { get; set; }
        public bool Can_Open { get; set; }
        public bool Can_Add { get; set; }
        public bool Can_Edit { get; set; }
        public bool Can_Delete { get; set; }
        public bool Can_Print { get; set; }

    }
    public static class Screens
    {
        public static Screens_Access_Profile mainSettings = new Screens_Access_Profile("elm_MainSettings")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "البيانات"
        };

        public static Screens_Access_Profile Company_Info = new Screens_Access_Profile(nameof(FRM_Company_Info), mainSettings)
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show, Master.Actions.Edit, Master.Actions.Open },
            Screen_Caption = "بيانات الشركه" 
        };


        public static Screens_Access_Profile Customers = new Screens_Access_Profile("elm_Customers")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "العملاء" 
        };
        public static Screens_Access_Profile Add_Customer = new Screens_Access_Profile("FRM_Customer",Customers)
        {
            Screen_Caption = "اضافه عميل" 
        };
        public static Screens_Access_Profile View_Customers = new Screens_Access_Profile("FRM_Customers_List", Customers)
        {        
            Screen_Caption = "عرض العملاء"
        };


        
        public static Screens_Access_Profile Vendors = new Screens_Access_Profile("elm_Vendors")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "الموردين"
        };

        public static Screens_Access_Profile Add_Vendor = new Screens_Access_Profile("FRM_Vendor", Vendors)
        {         
            Screen_Caption = "اضافه مورد"
        };

        public static Screens_Access_Profile View_Vendors = new Screens_Access_Profile("FRM_Vendors_List", Vendors)
        {         
            Screen_Caption = "عرض الموردين"
        };



        public static Screens_Access_Profile Stores = new Screens_Access_Profile("elm_Stores")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "المخازن | الفروع"
        };

        public static Screens_Access_Profile Add_Store = new Screens_Access_Profile(nameof(FRM_Stores), Stores)
        {
            //Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "اضافه مخزن | فرع"
        };

        public static Screens_Access_Profile View_Stores = new Screens_Access_Profile(nameof(FRM_Stores_List), Stores)
        {
            Screen_Caption = "عرض المخازن | الفروع"
        };



        public static Screens_Access_Profile Items = new Screens_Access_Profile("elm_Items")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "الأصناف"
        };
        public static Screens_Access_Profile Add_Item = new Screens_Access_Profile(nameof(FRM_Product),Items)
        {
            Screen_Caption = "اضافه صنف"
        };
        public static Screens_Access_Profile View_Items = new Screens_Access_Profile(nameof(FRM_Product_List), Items)
        {
            Screen_Caption = "عرض الأصناف"
        };
        public static Screens_Access_Profile Items_Categories = new Screens_Access_Profile(nameof(FRM_Product_Category), Items)
        {
            Screen_Caption = "فئات الأصناف"
        };

        /// /////////////////////////////////////////////////////////
        public static Screens_Access_Profile Drowers = new Screens_Access_Profile("elm_Drowers")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "الخزينه"
        };
        public static Screens_Access_Profile Add_Drower = new Screens_Access_Profile(nameof(FRM_Drower), Drowers)
        {
            Screen_Caption = "اضافه خزنه"
        };
        public static Screens_Access_Profile View_Drowers = new Screens_Access_Profile(nameof(FRM_Drowers_List), Drowers)
        {
            Screen_Caption = "عرض الخزنات"
        };


        /// /////////////////////////////////////////////////////////
        public static Screens_Access_Profile Purchase = new Screens_Access_Profile("elm_Purchase")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "المشتريات"
        };
        public static Screens_Access_Profile Add_Purchase_Invoice = new Screens_Access_Profile("FRM_Pruchase_Invoice", Purchase)
        {
            Screen_Caption = "اضافه فاتوره مشتريات"
        };
        public static Screens_Access_Profile View_Purchase_Invoices = new Screens_Access_Profile("FRM_Purchase_Invoices_List", Purchase)
        {
            Screen_Caption = "عرض فواتير المشتريات"
        };



        public static Screens_Access_Profile Salles = new Screens_Access_Profile("elm_Salles")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "المبيعات"
        };
        public static Screens_Access_Profile Add_Salles_Invoice = new Screens_Access_Profile("FRM_Salle_Invoice", Salles)
        {         
            Screen_Caption = "اضافه فاتوره مبيعات"
        };
        public static Screens_Access_Profile View_Sales_Invoices = new Screens_Access_Profile("FRM_Sales_Invoices_List", Salles)
        {
            Screen_Caption = "عرض فواتير المبيعات"
        };


        public static Screens_Access_Profile Settings = new Screens_Access_Profile("elm_Settings")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            Screen_Caption = "الاعدادات"
        };
        public static Screens_Access_Profile Add_User_Settings_Profile = new Screens_Access_Profile(nameof(FRM_User_Settings_Profile), Settings)
        {
            Screen_Caption = "اضافه نموذج اعدادات"
        };
        public static Screens_Access_Profile View_User_Settings_Profiles = new Screens_Access_Profile(nameof(FRM_User_Settings_Profile_List), Settings)
        {
            Screen_Caption = "عرض نماذج الاعدادات"
        };

        public static Screens_Access_Profile Add_Access_Profile = new Screens_Access_Profile(nameof(FRM_Access_Profile), Settings)
        {
            Screen_Caption = "اضافه نموذج صلاحيه وصول"
        };
        public static Screens_Access_Profile View_Access_Profiles = new Screens_Access_Profile(nameof(FRM_Access_Profile_List), Settings)
        {
            Screen_Caption = "عرض نماذج صلاحيات الوصول"
        };
        public static Screens_Access_Profile Add_User = new Screens_Access_Profile(nameof(FRM_User), Settings)
        {
            Screen_Caption = "اضافه مستخدم"
        };
        public static Screens_Access_Profile View_Users = new Screens_Access_Profile(nameof(FRM_Users_List), Settings)
        {
            Screen_Caption = "عرض المستخدمين"
        };

        //public static Screens_Access_Profile View_Sales_Invoices = new Screens_Access_Profile("FRM_Sales_Invoices_List", Salles)
        //{
        //    Screen_Caption = "عرض فواتير المبيعات"
        //};






        public static List<Screens_Access_Profile> Get_Screens 
        { get
            {
                Type t = typeof(Screens);
                //It returns every field that is only public (and || or) static
                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);

                var list = new List<Screens_Access_Profile>();
                foreach (var item in fields)
                {
                    var obj = item.GetValue(null);
                    if (obj != null && obj.GetType() == typeof(Screens_Access_Profile))
                        list.Add((Screens_Access_Profile)obj);
                }
                return list;
            }
        }
    }
}