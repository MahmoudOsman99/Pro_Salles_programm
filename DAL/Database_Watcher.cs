using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TableDependency.SqlClient;
using TableDependency.SqlClient.EventArgs;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Enums;
using System.Windows.Forms;
using TableDependency.SqlClient.Base.Abstracts;
using System.Diagnostics;
using System.Globalization;
using TableDependency.SqlClient.Base.Delegates;
using Pro_Salles.PL;
using Pro_Salles.Class;

namespace Pro_Salles.DAL
{
    class Database_Watcher
    {
        public class Stores : Store { };
        public class Drowers : Drower { };
        public class CustomersAndVendors : CustomersAndVendor { };
        public class Users : User { };

        /////////////////////////////////////////////////////////////////////////////////////
        public static SqlTableDependency<Stores> Store;
        public static void Stores_Changed(object sender, RecordChangedEventArgs<Stores> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        Sessions.Stores.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Sessions.Stores.IndexOf(Sessions.Stores.Single(x => x.ID == e.Entity.ID));
                        Sessions.Stores.Remove(Sessions.Stores.Single(x => x.ID == e.Entity.ID));
                        Sessions.Stores.Insert(index, e.Entity);
                        break;
                    case ChangeType.Delete:
                        Sessions.Stores.Remove(Sessions.Stores.Single(x => x.ID == e.Entity.ID));
                        break;
                }
            }));
        }


        public static SqlTableDependency<Store_Log> StoreLog;
        public static void StoreLog_Changed(object sender, RecordChangedEventArgs<Store_Log> e)
        {
            FRM_MAIN.Instance.Invoke(new Action(() =>
            {
                var Balance = Master_Inventory.GetProductBalanceInStore(e.Entity.product_id, e.Entity.store_id);
                Sessions.Product_Balances.Remove(Sessions.Product_Balances.FirstOrDefault(x => x.Product_ID == e.Entity.product_id && x.Store_ID == e.Entity.store_id));
                
                Sessions.Product_Balances.Add(new Sessions.Product_Balance() 
                {
                    Balance = Balance,
                    Product_ID = e.Entity.product_id,
                    Store_ID = e.Entity.store_id
                });

            }));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        public static SqlTableDependency<Drowers> drowers;
        public static void Drowers_Changed(object sender, RecordChangedEventArgs<Drowers> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        Sessions.Drowers.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Sessions.Drowers.IndexOf(Sessions.Drowers.Single(x => x.ID == e.Entity.ID));
                        Sessions.Drowers.Remove(Sessions.Drowers.Single(x => x.ID == e.Entity.ID));
                        Sessions.Drowers.Insert(index, e.Entity);
                        break;
                    case ChangeType.Delete:
                        Sessions.Drowers.Remove(Sessions.Drowers.Single(x => x.ID == e.Entity.ID));
                        break;
                }
            }));
        }
        /////////////////////////////////////////////////////////////////////////////////////
        public static SqlTableDependency<Users> Users_List;
        public static void Users_Changed(object sender, RecordChangedEventArgs<Users> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        Sessions.Users.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Sessions.Users.IndexOf(Sessions.Users.Single(x => x.ID == e.Entity.ID));
                        Sessions.Users.Remove(Sessions.Users.Single(x => x.ID == e.Entity.ID));
                        Sessions.Users.Insert(index, e.Entity);
                        break;
                    case ChangeType.Delete:
                        Sessions.Users.Remove(Sessions.Users.Single(x => x.ID == e.Entity.ID));
                        break;
                }
            }));
        }
        /////////////////////////////////////////////////////////////////////////////////////
        public static SqlTableDependency<Product> Products;
        public static void Products_Changed(object sender, RecordChangedEventArgs<Product> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:                                                                                      /////
                        Sessions.Product.Add(e.Entity);                                                                          /////
                        Sessions.Product_View.Add(Sessions.Product_View_Class.Get_Product(e.Entity.ID));                         /////
                        break;                                                                                                   /////
                    case ChangeType.Update:                                                                                      /////
                        var index = Sessions.Product.IndexOf(Sessions.Product.Single(x => x.ID == e.Entity.ID));                 /////
                        var ViewIndex = Sessions.Product_View.IndexOf(Sessions.Product_View.Single(x => x.ID == e.Entity.ID));   /////
                                                                                                                                 /////
                        Sessions.Product.Remove(Sessions.Product.Single(x => x.ID == e.Entity.ID));                              /////
                        Sessions.Product_View.Remove(Sessions.Product_View.Single(x => x.ID == e.Entity.ID));                    /////
                                                                                                                                 /////
                        Sessions.Product.Insert(index, e.Entity);                                                                /////
                        Sessions.Product_View.Insert(ViewIndex, Sessions.Product_View_Class.Get_Product(e.Entity.ID));           /////
                                                                                                                                 /////
                        break;                                                                                                   /////
                    case ChangeType.Delete:                                                                                      /////
                        Sessions.Product.Remove(Sessions.Product.Single(x => x.ID == e.Entity.ID));                              /////
                        Sessions.Product_View.Remove(Sessions.Product_View.Single(x => x.ID == e.Entity.ID));                    /////
                        break;
                }
            }));
        }

        /////////////////////////////////////////////////////////////////////////////////////
        public static SqlTableDependency<CustomersAndVendors> Vendors;
        public static void Vendor_Changed(object sender, RecordChangedEventArgs<CustomersAndVendors> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        Sessions.Vendors.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Sessions.Vendors.IndexOf(Sessions.Vendors.Single(x => x.ID == e.Entity.ID));
                        Sessions.Vendors.Remove(Sessions.Vendors.Single(x => x.ID == e.Entity.ID));
                        Sessions.Vendors.Insert(index, e.Entity);
                        break;
                    case ChangeType.Delete:
                        Sessions.Vendors.Remove(Sessions.Vendors.Single(x => x.ID == e.Entity.ID));
                        break;
                }
            }));
        }

        public class Vendors_Only : ITableDependencyFilter
        {
            public string Translate()
            {
                return "[Is_Customer] = 0";
            }
            /*
             * You can put another filters buy adding your condition
            public string Translate()
            {
                return "[Is_Customer] = 0 and [column name] = xxxx";
            }*/
        }

        /////////////////////////////////////////////////////////////////////////////////////        
        public static SqlTableDependency<CustomersAndVendors> Customers;
        public static void Customer_Changed(object sender, RecordChangedEventArgs<CustomersAndVendors> e)
        {
            Application.OpenForms[0].Invoke(new Action(() =>
            {
                switch (e.ChangeType)
                {
                    case ChangeType.Insert:
                        Sessions.Customers.Add(e.Entity);
                        break;
                    case ChangeType.Update:
                        var index = Sessions.Customers.IndexOf(Sessions.Customers.Single(x => x.ID == e.Entity.ID));
                        Sessions.Customers.Remove(Sessions.Customers.Single(x => x.ID == e.Entity.ID));
                        Sessions.Customers.Insert(index, e.Entity);
                        break;
                    case ChangeType.Delete:
                        Sessions.Customers.Remove(Sessions.Customers.Single(x => x.ID == e.Entity.ID));
                        break;
                }
            }));
        }

        public class Customers_Only : ITableDependencyFilter
        {
            public string Translate()
            {
                return "[Is_Customer] = 1";
            }
        }
    }
}