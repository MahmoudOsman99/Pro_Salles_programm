using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pro_Salles.PL
{
    public partial class FRM_Product_List : FRM_Master
    {
        public FRM_Product_List()
        {
            InitializeComponent();
        }
        public override void Refresh_Data()
        {
            gridControl1.DataSource = Sessions.Product_View;                                   
            base.Refresh_Data();                                                               
        }                                                                                      
                                                                                               
        private void FRM_Product_List_Load(object sender, EventArgs e)                          //   dont forget to solve it
        {                                                                                       //   dont forget to solve it
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;                  //   dont forget to solve it
            btn_delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;                //   dont forget to solve it
            gridView1.OptionsBehavior.Editable = false;                                         //   dont forget to solve it
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;             //   dont forget to solve it
            gridView1.DoubleClick += GridView1_DoubleClick;                                     //   dont forget to solve it
                                                                                                //   dont forget to solve it
            Refresh_Data();                                                                     //   dont forget to solve it
                                                                                                //   dont forget to solve it
            gridControl1.ViewRegistered += GridControl1_ViewRegistered;                         //   dont forget to solve it
            gridView1.OptionsDetail.ShowDetailTabs = false;                                     //   dont forget to solve it
                                                                                                //   dont forget to solve it
            var ins = new Sessions.Product_View_Class();                                        //   dont forget to solve it
            gridView1.Columns[nameof(ins.Cat_Name)].Caption = "الفئه التابع لها";              //   dont forget to solve it
            gridView1.Columns[nameof(ins.discription)].Caption = "الوصف";                       //   dont forget to solve it
            gridView1.Columns[nameof(ins.Code)].Caption = "الكود";                              //   dont forget to solve it
            gridView1.Columns[nameof(ins.Name)].Caption = "الأسم";                               //   dont forget to solve it
            gridView1.Columns[nameof(ins.Is_Active)].Caption = "نشط";                           //   dont forget to solve it
            gridView1.Columns[nameof(ins.Type)].Caption = "النوع";                              //   dont forget to solve it
            gridView1.Columns[nameof(ins.ID)].Visible = false;                                  //   dont forget to solve it
        }                                                                                       //   dont forget to solve it
        public override void New()
        {
            var frm = new FRM_Product();
            FRM_MAIN.Open_Form(frm, true);
            Refresh_Data();
            base.New();
        }
        private void GridControl1_ViewRegistered(object sender, DevExpress.XtraGrid.ViewOperationEventArgs e)
        {
            if (e.View.LevelName == "Units")
            {
                GridView view = e.View as GridView;
                view.ViewCaption = "وحدات القياس";

                var ins = new Sessions.Product_View_Class.Product_UOM_View();
                view.OptionsView.ShowViewCaption = true;

                view.Columns[nameof(ins.Unit_ID)].Visible = false;

                view.Columns[nameof(ins.Unit_Name)].Caption = "اسم الوحده";
                view.Columns[nameof(ins.Factor)].Caption = "المعامل";
                view.Columns[nameof(ins.Buy_price)].Caption = "سعر الشراء";
                view.Columns[nameof(ins.Sell_price)].Caption = "سعر البيع";
                view.Columns[nameof(ins.Sell_Discount)].Caption = "خصم البيع";
                view.Columns[nameof(ins.Barcode)].Caption = "الباركود";
            }
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var ins = new Product();
                int id = 0;
                if (int.TryParse(gridView1.GetFocusedRowCellValue(nameof(ins.ID)).ToString(), out id) && id > 0)
                {
                    var frm = new FRM_Product(id);
                    FRM_MAIN.Open_Form(frm, true);
                    Refresh_Data();
                }
            }
        }        
        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {
                e.DisplayText = Master.Product_Types_List.Single(x => x.ID == Convert.ToInt32(e.Value)).Name;
            }
        }
    }
}