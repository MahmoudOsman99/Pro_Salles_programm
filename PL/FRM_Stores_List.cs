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
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Pro_Salles.PL
{
    public partial class FRM_Stores_List : XtraForm
    {
        public FRM_Stores_List()
        {
            InitializeComponent();
        }
        Store store;
        private void Stores_ListChanged(object sender, ListChangedEventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            gridControl1.DataSource = Sessions.Stores.Select(x => new { x.ID, x.name });
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
             ;
                var frm = new FRM_Stores(Convert.ToInt32(gridView1.GetFocusedRowCellValue(nameof(store.ID))));
                FRM_MAIN.Open_Form(frm, true);
            }
        }

        private void btn_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new FRM_Stores();

            FRM_MAIN.Open_Form(frm, true);
        }

        private void FRM_Stores_List_Load(object sender, EventArgs e)
        {
            Refresh();
            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns[nameof(store.ID)].Visible = false;
            gridView1.Columns[nameof(store.name)].Caption = "أسم المخزن";
            gridView1.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            gridView1.Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
            gridView1.DoubleClick += GridView1_DoubleClick;
            Sessions.Stores.ListChanged += Stores_ListChanged;
        }
    }
}