using DevExpress.Charts.Native;
using DevExpress.Utils;
using DevExpress.Utils.Behaviors.Common;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.Design;
using Pro_Salles.DAL;
using Pro_Salles.PL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Pro_Salles.Class
{
    public static class Master
    {
        public class Value_And_ID
        {
            public int ID { get; set; }

            public string Name { get; set; }
        }
        public static List<Value_And_ID> Product_Types_List = new List<Value_And_ID> {
            new Value_And_ID { ID = (int)Product_Type.Inventory, Name = "مخزني" },
            new Value_And_ID { ID = (int)Product_Type.Service, Name = "خدمي"}
        };
        public enum Product_Type
        {
            Inventory,
            Service
        }

        public static List<Value_And_ID> Invoices_Part_Type_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)Part_Type.Vendor, Name = "مورد" },
            new Value_And_ID { ID = (int)Part_Type.Customer, Name = "عميل" }
        };
        public static List<Value_And_ID> Part_Type_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)Part_Type.Vendor, Name = "مورد" },
            new Value_And_ID { ID = (int)Part_Type.Customer, Name = "عميل" },
            new Value_And_ID { ID = (int)Part_Type.Account, Name = "حساب" }
        };
        public enum Part_Type
        {
            Vendor,
            Customer,
            Account
        }


        public static List<Value_And_ID> Invoice_Type_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)Invoice_Type.Purchase, Name = "مشتروات" },
            new Value_And_ID { ID = (int)Invoice_Type.Purchase_Return, Name = "مبيعات" },
            new Value_And_ID { ID = (int)Invoice_Type.Salles, Name = "مردود مشتروات" },
            new Value_And_ID { ID = (int)Invoice_Type.Salles_Return, Name = "مردود مبيعات" }
        };
        public enum Invoice_Type
        {
            Purchase = Source_Types.Purchase,
            Salles = Source_Types.Salles,
            Purchase_Return = Source_Types.Purchase_Return,
            Salles_Return = Source_Types.Salles_Return
        }

        public enum Source_Types
        {
            Purchase, // When you add a new invoice, use this enum not in the above
            Salles,
            Purchase_Return,
            Salles_Return
        }

        public enum Cost_Distribution_Options
        {
            By_Price,
            By_QTY
        }


        public enum Print_Mode
        {
            Direct,
            ShowPreview,
            ShowDialog
        }




        public static List<Value_And_ID> Pay_Methods_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)Pay_Methods.Cash, Name = "نقدي" },
            new Value_And_ID { ID = (int)Pay_Methods.Credit, Name = "اجل" },
        };
        public enum Pay_Methods
        {
            Cash = 1,
            Credit
        }



        public static List<Value_And_ID> User_Type_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)User_Type.Admin, Name = "مدير نظام" },
            new Value_And_ID { ID = (int)User_Type.User, Name = "دخول مخصص" },
        };
        public enum User_Type
        {
            Admin = 1,
            User
        }



        public static List<Value_And_ID> Warning_Levels_List = new List<Value_And_ID>()
        {
            new Value_And_ID { ID = (int)Warning_Levels.Do_Not_Interrupt, Name = "عدم التداخل" },
            new Value_And_ID { ID = (int)Warning_Levels.Show_Warning, Name = "تحذير" },
            new Value_And_ID { ID = (int)Warning_Levels.Prevent, Name = "منع" },
        };
        public enum Warning_Levels
        {
            Do_Not_Interrupt = 1,
            Show_Warning,
            Prevent
        }



        public enum Actions
        {
            Show = 1,
            Open,
            Add,
            Edit,
            Delete,
            Print
        }



        public static int Find_Row_By_RowObject(this GridView view, object row)
        {
            if (row != null)
            {
                for (int i = 0; i < view.DataRowCount; i++)
                {
                    if (row.Equals(view.GetRow(i)))
                        return i;
                }
            }
            return GridControl.InvalidRowHandle;
        }

        /// <summary>
        /// Check if the datetime smaller thaan 1950 or not
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsDateTimeValid(this DateEdit dt)
        {
            if (dt.DateTime.Year < 1950)
            {
                dt.ErrorText = FRM_Master.Error_Text;
                return false;
            }
            return true;
        }
        /// <summary>
        /// Make a check if the text doesn't have spaces or empty
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsStringValid(this TextEdit txt)
        {
            if (txt.Text.Trim() == string.Empty)
            {
                txt.ErrorText = FRM_Master.Error_Text;
                return false;
            }
            return true;
        }


        public static void Initialize_Data(this RepositoryItemLookUpEditBase repo, object datasource, GridColumn cl, GridControl grid)
        {
            Initialize_Data(repo, datasource, cl, grid, "name", "ID");
        }
        public static void Initialize_Data(this RepositoryItemLookUpEditBase repo, object datasource, GridColumn cl, GridControl grid, string displaymember, string valuemember)
        {
            if (repo == null)
                repo = new RepositoryItemLookUpEdit();

            repo.DataSource = datasource;
            repo.DisplayMember = displaymember;
            repo.ValueMember = valuemember;
            repo.NullText = "";
            repo.BestFitMode = BestFitMode.BestFitResizePopup;
            if (cl != null)
                cl.ColumnEdit = repo;
            if (grid != null)
                grid.RepositoryItems.Add(repo);
            repo.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            repo.Appearance.TextOptions.VAlignment = VertAlignment.Center;
        }

        /// <summary>
        /// The style of the lookupedit, it takes the data source only
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="sender"></param>
        public static void Initialize_Data(this LookUpEdit look, object datasource)
        {
            Initialize_Data(look, datasource, "Name", "ID");
        }
        /// <summary>
        /// The style of the lookupedit and it takes data source and display member and the value member
        /// </summary>
        /// <param name="look"></param>
        /// <param name="datasource"></param>
        /// <param name="displaymember"></param>
        /// <param name="valuemember"></param>
        public static void Initialize_Data(this LookUpEdit look, object datasource, string displaymember, string valuemember)
        {
            look.Properties.DataSource = datasource;
            look.Properties.DisplayMember = displaymember;
            look.Properties.ValueMember = valuemember;
            look.Properties.Columns.Clear();
            look.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo
            {
                FieldName = displaymember,

            });

            //look.Properties.PopulateColumns();
            //look.Properties.Columns[valuemember].Visible = false;
            //look.Properties.Columns[displaymember].Caption = "الأسم";
        }




        public static void InitializeData(this GridLookUpEdit look, object datasource)
        {
            InitializeData(look, datasource, "name", "ID");
        }
        public static void InitializeData(this GridLookUpEdit look, object datasource, string displaymember, string valuemember)
        {
            look.Properties.DataSource = datasource;
            look.Properties.DisplayMember = displaymember;
            look.Properties.ValueMember = valuemember;

            look.Properties.ValidateOnEnterKey = true;
            look.Properties.AllowNullInput = DefaultBoolean.False;//to don't let the user put null values
            look.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;//To make the size fit to the fields
            look.Properties.ImmediatePopup = true;

            var part_idView = look.Properties.View;
            part_idView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            part_idView.OptionsSelection.UseIndicatorForSelection = true;
            part_idView.OptionsView.ShowAutoFilterRow = true;
            part_idView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            part_idView.PopulateColumns(look.Properties.DataSource);
        }





        /// <summary>
        /// This method makes check if the lookupedit doesn't equal int or byte, Thin if the condition equals true, there is a message will be showen 
        /// </summary>
        /// <param name="look"></param>
        /// <returns></returns>
        public static bool IsEditValueValid(this LookUpEditBase look, bool setError = true)
        {
            if (look.Is_Edit_Value_Int() == false)
            {
                if (setError)
                    look.ErrorText = FRM_Master.Error_Text;
                return false;
            }
            return true;
        }


        public static bool IsValueNotLessThanZero(this SpinEdit edit, bool setError = true)
        {
            if (setError && edit.Value < 0)
                edit.ErrorText = "لا يمكن للقيمه ان تكون اصغر من الصفر";
            return edit.Value >= 0;
        }

        public static bool IsValueBiggerThanZero(this SpinEdit edit, bool setError = true)
        {
            if (setError && edit.Value <= 0)
                edit.ErrorText = "يجب ان تكون القيمه اكبر من الصفر";
            return edit.Value > 0;
        }



        /// <summary>
        /// This method makes check if the lookupedit doesn't equal null || or || if the lookupedit value equals zero
        /// </summary>
        /// <param name="look"></param>
        /// <returns></returns>
        public static bool IsEditValueValidAndNotZero(this LookUpEditBase look, bool setError = true)
        {
            if (look.Is_Edit_Value_Int() == false || Convert.ToInt32(look.EditValue) == 0)
            {
                if (setError)
                    look.ErrorText = FRM_Master.Error_Text;
                return false;
            }
            return true;
        }
        /// <summary>
        /// This method returns if the data that inside the lookupedit is integer or byte or not
        /// </summary>
        /// <param name="look"></param>
        /// <returns></returns>
        public static bool Is_Edit_Value_Int(this LookUpEditBase look)
        {
            var val = look.EditValue;
            return (val is int || val is byte);
        }

        public static string Get_Next_Code(string num)
        {
            if (num == string.Empty || num == null)
                return "1";

            string st = "";
            foreach (char c in num)
                st = char.IsDigit(c) ? st + c.ToString() : "";

            if (st == string.Empty)
                return num + "1";

            string st2 = st.Insert(0, "1");
            st2 = (Convert.ToInt64(st2) + 1).ToString();

            string st3 = st2[0] == '1' ? st2.Remove(0, 1) : st2.Remove(0, 1).Insert(0, "1");
            int index = num.LastIndexOf(st);
            num = num.Remove(index);
            num = num.Insert(index, st3);
            return num;
        }
        public static T From_Byte_Array<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                return (T)bf.Deserialize(ms);
            }
        }
        public static byte[] To_Byte_Array<T>(T obj)
        {
            if (obj == null) return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }


        }
        public static byte[] Get_Property_Value(string PropertyName, int profileid)
        {
            using (var db = new Pro_SallesDataContext())
            {
                var prop = db.UserSettingsProfileProperties.SingleOrDefault(x => x.Profile_ID == profileid &&
                x.Property_Name == PropertyName);
                if (prop == null)
                    return null;
                return prop.Property_Value.ToArray();
            }
        }
        public static string Get_Caller_Name([CallerMemberName] string callername = "")
        {
            return callername;
        }
    }
}