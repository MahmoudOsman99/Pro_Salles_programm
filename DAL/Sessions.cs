using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient;
using TableDependency.SqlClient.EventArgs;
using TableDependency.SqlClient.Enumerations;
using static Pro_Salles.Class.Master;
using Pro_Salles.Class;

namespace Pro_Salles.DAL
{
    class Sessions
    {

        public static class Defaults
        {
            public static int Drower { get => 5; }
            public static int Customer { get => 1; }
            public static int Vendor { get => 1; }
            public static int Store { get => 8; }
            public static int Raw_Store { get => 8; }
            public static int Discount_Allowed_Account { get => 1021; }
            public static int Discount_Received_Account { get => 1020; }
            public static int Salles_Tax { get => 1030; }
            public static int Purchase_Tax { get => 1031; }
            public static int Purchase_Expences { get => 1032; }


        }
        public static class CurrentSettings
        {
            public static Print_Mode InvoicePrintMode { get => Print_Mode.ShowPreview; }
        }

        private static User_Settings_Template _user_Settings;
        public static User_Settings_Template UserSettings
        {
            get
            {
                if (_user_Settings == null)
                    _user_Settings = new User_Settings_Template(CurrentUser.Settings_Profile_ID);
                return _user_Settings;
            }
        }





        public static class GlobalSettings
        {
            public static bool Read_From_Scale_Barcode { get => true; }
            public static string Scale_Barcode_Prefix { get => "20"; }
            public static byte Product_Code_Length { get => 5; }
            public static byte Barcode_Length { get => 13; }
            public static byte Value_Code_Length { get => 5; }
            public static Read_Value_Mode Read_Mode { get => Read_Value_Mode.Weight; }
            public static bool Ignore_Check_Digit { get => true; }
            public static byte Devide_Value_By { get => 3; }

            public enum Read_Value_Mode
            {
                Weight,
                Price
            }
        }
        private static Company_Info company_info;
        public static Company_Info Company_Info
        {
            get
            {
                if (company_info == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        company_info = db.Company_Infos.FirstOrDefault();
                    }
                }
                return company_info;
            }
        }

        private static BindingList<Store> _store;
        public static BindingList<Store> Stores
        {
            get
            {
                if (_store == null)
                {
                    using (var db = new DAL.Pro_SallesDataContext())
                    {
                        _store = new BindingList<Store>(db.Stores.ToList());
                    }
                    Database_Watcher.Store = new SqlTableDependency<Database_Watcher.Stores>(Properties.Settings.Default.Salles_DBConnectionString);
                    Database_Watcher.Store.OnChanged += Database_Watcher.Stores_Changed;
                    Database_Watcher.Store.Start();
                }
                return _store;
            }
        }




        private static BindingList<User> _user;
        public static BindingList<User> Users
        {
            get
            {
                if (_user == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _user = new BindingList<User>(db.Users.ToList());
                    }
                    Database_Watcher.Users_List = new SqlTableDependency<Database_Watcher.Users>(Properties.Settings.Default.Salles_DBConnectionString);
                    Database_Watcher.Users_List.OnChanged += Database_Watcher.Users_Changed;
                    Database_Watcher.Users_List.Start();
                }
                return _user;
            }
        }






        private static BindingList<UserSettingsProfileProperty> _profile_properties;
        public static BindingList<UserSettingsProfileProperty> Profile_Properties
        {
            get
            {
                if (_profile_properties == null)
                {
                    using (var db = new DAL.Pro_SallesDataContext())
                    {
                        _profile_properties = new BindingList<UserSettingsProfileProperty>(db.UserSettingsProfileProperties.ToList());
                    }
                }
                return _profile_properties;
            }
        }


        private static BindingList<Units_name> _unit_names;
        public static BindingList<Units_name> Unit_Names
        {
            get
            {
                if (_unit_names == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _unit_names = new BindingList<Units_name>(db.Units_names.ToList());
                    }
                }
                return _unit_names;
            }
        }





        private static BindingList<Drower> _drower;
        public static BindingList<Drower> Drowers
        {
            get
            {
                if (_drower == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _drower = new BindingList<Drower>(db.Drowers.ToList());
                    }
                    Database_Watcher.drowers = new SqlTableDependency<Database_Watcher.Drowers>(Properties.Settings.Default.Salles_DBConnectionString);
                    Database_Watcher.drowers.OnChanged += Database_Watcher.Drowers_Changed;
                    Database_Watcher.drowers.Start();
                }
                return _drower;
            }
        }

        //private static BindingList<Product_Category> _category;
        //public static BindingList<Product_Category> Category
        //{
        //    get
        //    {
        //        if (_category == null)
        //        {
        //            using (var db = new Pro_SallesDataContext())
        //            {
        //                _category = new BindingList<Product_Category>(db.Product_Categories.ToList());
        //            }
        //        }
        //        return _category;
        //    }
        //}





        private static BindingList<Product> _Product;
        public static BindingList<Product> Product
        {
            get
            {
                if (_Product == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _Product = new BindingList<Product>(db.Products.ToList());
                    }
                    Database_Watcher.Products = new SqlTableDependency<Product>(Properties.Settings.Default.Salles_DBConnectionString);
                    Database_Watcher.Products.OnChanged += Database_Watcher.Products_Changed;
                    Database_Watcher.Products.Start();
                }
                return _Product;
            }
        }

        private static BindingList<Product_View_Class> _product_View_Class;
        public static BindingList<Product_View_Class> Product_View
        {
            get
            {
                if (_product_View_Class == null)
                {                                                                                       // Revision
                    using (var db = new Pro_SallesDataContext())                                        // Revision
                    {                                                                                   // Revision
                        var data = from p in Sessions.Product                                           // Revision
                                   join c in db.Product_Categories on                                   // Revision
                                   p.Category_ID equals c.ID                                            // Revision
                                   select new Product_View_Class                                        // Revision
                                   {                                                                    // Revision
                                       ID = p.ID,                                                       // Revision
                                       Code = p.code,                                                   // Revision
                                       Name = p.name,                                                   // Revision
                                       Type = p.type,                                                   // Revision
                                       discription = p.discription,                                     // Revision
                                       Is_Active = p.is_active,                                         // Revision
                                       Cat_Name = c.name,                                               // Revision
                                                                                                        // Revision
                                       Units =                                                          // Revision
                                           (from pu in db.Product_Units                                  // Revision
                                            where p.ID == pu.product_id                                  // Revision
                                            join un in db.Units_names on                                // Revision
                                            pu.unit_id equals un.ID                                      // Revision
                                            select new Product_View_Class.Product_UOM_View              // Revision
                                            {                                                           // Revision
                                                Unit_ID = pu.unit_id,
                                                Unit_Name = un.name,                                    // Revision
                                                Factor = pu.factor,                                      // Revision
                                                Buy_price = pu.buy_price,                                // Revision
                                                Sell_price = pu.sell_price,
                                                Sell_Discount = pu.sell_discount,
                                                Barcode = pu.barcode                                     // Revision
                                            }).ToList()                                                 // Revision
                                   };                                                                   // Revision
                        _product_View_Class = new BindingList<Product_View_Class>(data.ToList());       // Revision
                    };                                                                                  // Revision
                }                                                                                       // Revision
                return _product_View_Class;
            }
        }





        //71
        private static BindingList<Product_Balance> _product_balance;
        public static BindingList<Product_Balance> Product_Balances
        {
            get
            {
                if (_product_balance == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        var data = from sl in db.Store_Logs
                                   group sl by new { sl.product_id, sl.store_id } into g
                                   select new Product_Balance
                                   {
                                       Balance =
                                       (g.Where(x => x.is_in_transaction == true).Sum(x => (double?)x.qty) ?? 0) -
                                       (g.Where(x => x.is_in_transaction == false).Sum(x => (double?)x.qty) ?? 0),
                                       Product_ID = g.Key.product_id,
                                       Store_ID = g.Key.store_id
                                   };
                        _product_balance = new BindingList<Product_Balance>(data.ToList());
                    };
                    Database_Watcher.StoreLog = new SqlTableDependency<Store_Log>(Properties.Settings.Default.Salles_DBConnectionString);
                    Database_Watcher.StoreLog.OnChanged += Database_Watcher.StoreLog_Changed;
                    Database_Watcher.StoreLog.Start();
                }
                return _product_balance;
            }
        }








        public class Product_View_Class
        {
            public static Product_View_Class Get_Product(int id)
            {
                Product_View_Class obj;
                using (var db = new Pro_SallesDataContext())
                {
                    var data = from p in Product
                               where p.ID == id
                               join c in db.Product_Categories on
                               p.Category_ID equals c.ID
                               select new Product_View_Class
                               {
                                   ID = p.ID,
                                   Code = p.code,
                                   Name = p.name,
                                   Type = p.type,
                                   discription = p.discription,
                                   Is_Active = p.is_active,
                                   Cat_Name = c.name,

                                   Units =
                                       (from u in db.Product_Units
                                        where p.ID == u.product_id
                                        join un in db.Units_names on
                                        u.unit_id equals un.ID
                                        select new Product_View_Class.Product_UOM_View
                                        {
                                            Unit_ID = u.unit_id,
                                            Unit_Name = un.name,
                                            Factor = u.factor,
                                            Buy_price = u.buy_price,
                                            Sell_price = u.sell_price,
                                            Sell_Discount = u.sell_discount,
                                            Barcode = u.barcode
                                        }).ToList()
                               };
                    obj = data.First();
                };
                return obj;
            }
            public int ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Cat_Name { get; set; }
            public string discription { get; set; }
            public bool Is_Active { get; set; }
            public byte Type { get; set; }
            public List<Product_UOM_View> Units { get; set; }

            public class Product_UOM_View
            {
                public int Unit_ID { get; set; }
                public string Unit_Name { get; set; }
                public double Factor { get; set; }
                public double Buy_price { get; set; }
                public double Sell_price { get; set; }
                public double Sell_Discount { get; set; }
                public string Barcode { get; set; }
            }
        }

        public class Product_Balance
        {
            public int Product_ID { get; set; }
            public int Store_ID { get; set; }
            public double Balance { get; set; }
        }

        private static BindingList<CustomersAndVendor> _vend;
        public static BindingList<CustomersAndVendor> Vendors
        {
            get
            {
                if (_vend == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _vend = new BindingList<CustomersAndVendor>(db.CustomersAndVendors.Where(x => x.Is_Customer == false).ToList());
                    }
                    Database_Watcher.Vendors = new SqlTableDependency<Database_Watcher.CustomersAndVendors>(Properties.Settings.Default.Salles_DBConnectionString,
                        filter: new Database_Watcher.Vendors_Only()); //This is the filter for the vendor or customer
                    Database_Watcher.Vendors.OnChanged += Database_Watcher.Vendor_Changed;
                    Database_Watcher.Vendors.Start();
                }
                return _vend;
            }
        }

        private static BindingList<CustomersAndVendor> _cust;
        public static BindingList<CustomersAndVendor> Customers
        {
            get
            {
                if (_cust == null)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        _cust = new BindingList<CustomersAndVendor>(db.CustomersAndVendors.Where(x => x.Is_Customer == true).ToList());
                    }
                    Database_Watcher.Customers = new SqlTableDependency<Database_Watcher.CustomersAndVendors>(Properties.Settings.Default.Salles_DBConnectionString,
                        filter: new Database_Watcher.Customers_Only());
                    Database_Watcher.Customers.OnChanged += Database_Watcher.Customer_Changed;
                    Database_Watcher.Customers.Start();
                }
                return _cust;
            }
        }

        private static User _currentuser;
        public static User CurrentUser { get => _currentuser; }
        public static void Set_User(User user)
        {
            _currentuser = user;
            using (var db = new Pro_SallesDataContext())
            {
                _screens_Access = (from s in Screens.Get_Screens
                                   from d in db.User_Access_Profile_Details.Where
                                   (x => x.Profile_ID == user.Screen_Profile_ID && x.Screen_ID == s.Screen_ID).DefaultIfEmpty()
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

        }
        private static List<Screens_Access_Profile> _screens_Access;
        public static List<Screens_Access_Profile> Screens_Access
        {
            get
            {
                if (_currentuser.User_Type == (byte)Master.User_Type.Admin)
                    return Screens.Get_Screens;
                else
                    return _screens_Access;
            }
        }
    }
}