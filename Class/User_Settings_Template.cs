using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Commands;
using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pro_Salles.Class
{
    ////////////////////53/////////////////
    public class User_Settings_Template
    {
        int Profile_ID { get; set; }
        public User_Settings_Template(int profile_id)
        {
            Profile_ID = profile_id;
            General = new General_Settings(profile_id);
            Purchase = new Purchase_Settings(profile_id);
            Salles = new Salles_Settings(profile_id);
            Invoice = new Invoice_Settings(profile_id);
        }

        public General_Settings General { get; set; }
        public Invoice_Settings Invoice { get; set; }
        public Salles_Settings Salles { get; set; }
        public Purchase_Settings Purchase { get; set; }

        /// <summary>
        /// Get the property caption after it check it knew the property name
        /// </summary>
        /// <param name="propname"></param>
        /// <returns></returns>
        public static string Get_Prop_Caption(string propname)
        {
            User_Settings_Template ins;
            switch (propname)
            {
                case nameof(ins.General): return "اعدادات عامه";
                case nameof(ins.Invoice): return "اعدادات الفواتير";
                case nameof(ins.Purchase): return "اعدادات الشراء";
                case nameof(ins.Salles): return "اعدادات البيع";


                case nameof(ins.General.Default_Store): return "المخزن الافتراضي";
                case nameof(ins.General.Default_Customer): return "العميل الافتراضي";
                case nameof(ins.General.Default_Vendor): return "المورد الافتراضي";
                case nameof(ins.General.Default_Drower): return "الخزينه الافتراضيه";
                case nameof(ins.General.Default_Row_Store): return "المخزن الافتراضي للخامات";
                case nameof(ins.General.Can_Change_Customer): return "السماح بتغيير العميل";
                case nameof(ins.General.Can_Change_Drower): return "السماح بتغيير الخزينه";
                case nameof(ins.General.Can_Change_Vendor): return "السماح بتغيير المورد";
                case nameof(ins.General.Can_Change_Store): return "السماح بتغيير المخزن";
                case nameof(ins.General.Can_View_Document_History): return "السماح بمشاهده سجل التغييرات";

                case nameof(ins.Salles.Max_Discount_In_Invoice): return "اقصي خصم مسموح للفاتوره";
                case nameof(ins.Salles.Max_Discount_Per_Item):return "اقصي خصم مسموح للصنف";
                case nameof(ins.Salles.Can_Change_Paid_In_Salles): return "السماع بتغيير المبلغ المدفوع";
                case nameof(ins.Salles.Can_Not_Post_To_Store_In_Salles): return "انشاء فواتير بدون صرف";
                case nameof(ins.Salles.Can_Change_QTY_In_Salles): return "السماح بتغيير الكميه";
                case nameof(ins.Salles.Can_Change_Item_Price_In_Salles): return "السماح بتغيير سعر الصنف";
                case nameof(ins.Salles.Can_Sell_To_Vendors): return "السماح بالبيع للموردين";
                case nameof(ins.Salles.Hide_Cost_In_Salles): return "اخفاء التكلفه من الفاتوره";
                case nameof(ins.Salles.Can_Change_Salles_Invoice_Date): return "السماح بتغيير التاريخ ";
                case nameof(ins.Salles.Default_Pay_Method_In_Salles): return "طريقه الدفع الافتراضيه";
                case nameof(ins.Salles.When_Selling_To_Customer_Exceded_Max_Credit): return "عند البيع لعميل تجاوز حد الائتمان";
                case nameof(ins.Salles.When_Selling_Item_With_Price_Lower_Than_Cost_Price): return "عند البيع بسعر اقل من سعر التكلفه";

                case nameof(ins.Purchase.Can_Change_Item_Price_In_Purchase): return "السماح بتغيير سعر الشراء";
                case nameof(ins.Purchase.Can_Buy_From_Customers): return "السماح بالشراء من العملاء";
                case nameof(ins.Purchase.Can_Change_Purchase_Invoice_Date): return "السماح بتغيير التاريخ";

                case nameof(ins.Invoice.When_Selling_Item_With_QTY_More_Than_Avalible_QTY): return "عند صرف كميه من صنف اكثر من المتاح";
                case nameof(ins.Invoice.When_Selling_Item_Reached_Reorder_Level): return "عند صرف صنف وصل رصيده الي حد الطلب";
                case nameof(ins.Invoice.Can_Change_Tax): return "السماح بتغيير الضريبه";
                case nameof(ins.Invoice.Can_Delete_Items_In_Invoices): return "السماح بحذف صنف من الفاتوره";


                default: return "$" + propname + "$";
            }
        }

        public static BaseEdit Get_Property_Control(string propname, object propertyValue)
        {
            User_Settings_Template ins;
            BaseEdit edit = null;
            switch (propname)
            {
                case nameof(ins.General.Can_Change_Vendor):
                case nameof(ins.General.Can_Change_Customer):
                case nameof(ins.General.Can_Change_Store):
                case nameof(ins.General.Can_Change_Drower):
                case nameof(ins.General.Can_View_Document_History):

                case nameof(ins.Invoice.Can_Change_Tax):
                case nameof(ins.Invoice.Can_Delete_Items_In_Invoices):

                case nameof(ins.Purchase.Can_Buy_From_Customers):
                case nameof(ins.Purchase.Can_Change_Item_Price_In_Purchase):
                case nameof(ins.Purchase.Can_Change_Purchase_Invoice_Date):

                case nameof(ins.Salles.Can_Change_Paid_In_Salles):
                case nameof(ins.Salles.Can_Not_Post_To_Store_In_Salles):
                case nameof(ins.Salles.Can_Change_QTY_In_Salles):
                case nameof(ins.Salles.Can_Change_Item_Price_In_Salles):
                case nameof(ins.Salles.Can_Sell_To_Vendors):
                case nameof(ins.Salles.Hide_Cost_In_Salles):
                case nameof(ins.Salles.Can_Change_Salles_Invoice_Date):
                    edit = new ToggleSwitch();
                    ((ToggleSwitch)edit).Properties.OnText = "نعم";
                    ((ToggleSwitch)edit).Properties.OffText = "لا";
                    break;

                case nameof(ins.General.Default_Store):
                case nameof(ins.General.Default_Row_Store):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Sessions.Stores,"name","ID");
                    break;

                case nameof(ins.General.Default_Drower):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Sessions.Drowers, "name", "ID");
                    break;

                case nameof(ins.General.Default_Customer):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Sessions.Customers, "name", "ID");
                    break;

                case nameof(ins.General.Default_Vendor):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Sessions.Vendors, "name", "ID");
                    break;

                case nameof(ins.Salles.Default_Pay_Method_In_Salles):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Master.Pay_Methods_List);
                    break;

                case nameof(ins.Salles.Max_Discount_In_Invoice):
                case nameof(ins.Salles.Max_Discount_Per_Item):
                    edit = new SpinEdit();
                    //////////////////////////////////////////////////////////////////////////////////
                    break;

                case nameof(ins.Invoice.When_Selling_Item_Reached_Reorder_Level):
                case nameof(ins.Invoice.When_Selling_Item_With_QTY_More_Than_Avalible_QTY):
                case nameof(ins.Salles.When_Selling_To_Customer_Exceded_Max_Credit):
                case nameof(ins.Salles.When_Selling_Item_With_Price_Lower_Than_Cost_Price):
                    edit = new LookUpEdit();
                    ((LookUpEdit)edit).Initialize_Data(Master.Warning_Levels_List);
                    break;

                default:
                    break;
            }
            if (edit != null)
            {
                edit.Name = propname;
                edit.Properties.NullText = "";
                edit.EditValue = propertyValue;
            }
            if (edit != null && edit.GetType() == typeof(LookUpEdit))
            {
                var l = ((LookUpEdit)edit).Properties;
                l.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                l.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            }
            if (edit != null && edit.GetType() == typeof(SpinEdit))
            {
                var s = ((SpinEdit)edit).Properties;
                s.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                s.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                s.Increment = 0.01m;
                s.MaxValue = 1;
                s.Mask.UseMaskAsDisplayFormat = true;
                s.EditMask = "p";
            }
            return edit;
        }

    }
    public class General_Settings
    {
        int Profile_ID { get; set; }
        public General_Settings(int profile_id)
        {
            Profile_ID = profile_id;
        }

        public int Default_Row_Store { get { return Master.From_Byte_Array<int>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public int Default_Store { get { return Master.From_Byte_Array<int>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public int Default_Drower { get { return Master.From_Byte_Array<int>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public int Default_Customer { get { return Master.From_Byte_Array<int>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public int Default_Vendor { get { return Master.From_Byte_Array<int>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Store { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_View_Document_History { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Drower { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Vendor { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Customer { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }

    }

    public class Salles_Settings
    {
        int Profile_ID { get; set; }
        public Salles_Settings(int profile_id)
        {
            Profile_ID = profile_id;
        }
        public decimal Max_Discount_In_Invoice { get { return Master.From_Byte_Array<decimal>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public decimal Max_Discount_Per_Item { get { return Master.From_Byte_Array<decimal>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public Master.Pay_Methods Default_Pay_Method_In_Salles { get { return Master.From_Byte_Array<Master.Pay_Methods>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public Master.Warning_Levels When_Selling_To_Customer_Exceded_Max_Credit { get { return Master.From_Byte_Array<Master.Warning_Levels>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }       
        public Master.Warning_Levels When_Selling_Item_With_Price_Lower_Than_Cost_Price { get { return Master.From_Byte_Array<Master.Warning_Levels>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Paid_In_Salles { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Not_Post_To_Store_In_Salles { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Item_Price_In_Salles { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Hide_Cost_In_Salles { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Sell_To_Vendors { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Salles_Invoice_Date { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_QTY_In_Salles { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }

    }

    public class Purchase_Settings
    {
        int Profile_ID { get; set; }
        public Purchase_Settings(int profile_id)
        {
            Profile_ID = profile_id;
        }
        public bool Can_Change_Item_Price_In_Purchase { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Buy_From_Customers { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Purchase_Invoice_Date { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
    }
    public class Invoice_Settings
    {
        int Profile_ID { get; set; }
        public Invoice_Settings(int profile_id)
        {
            Profile_ID = profile_id;
        }
        public Master.Warning_Levels When_Selling_Item_Reached_Reorder_Level { get { return Master.From_Byte_Array<Master.Warning_Levels>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public Master.Warning_Levels When_Selling_Item_With_QTY_More_Than_Avalible_QTY { get { return Master.From_Byte_Array<Master.Warning_Levels>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Change_Tax { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }
        public bool Can_Delete_Items_In_Invoices { get { return Master.From_Byte_Array<bool>(Master.Get_Property_Value(Master.Get_Caller_Name(), Profile_ID)); } }

    }
}