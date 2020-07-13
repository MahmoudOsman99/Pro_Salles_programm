using System.Drawing;
using DevExpress.XtraReports.UI;
using Pro_Salles.DAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;

namespace Pro_Salles.Reporting
{
    public partial class RPT_Grid_Print : XtraReport
    {
        public RPT_Grid_Print()
        {
            InitializeComponent();
            lbl_company_address.Text = Sessions.Company_Info.address;
            lbl_company_mobile.Text = Sessions.Company_Info.phone + " - " + Sessions.Company_Info.mobile;
            lbl_company_name.Text = Sessions.Company_Info.name;
            if (Sessions.Company_Info.Logo != null)
                company_pic.Image = Class.Utils.Get_Image_FromByte_Array(Sessions.Company_Info.Logo.ToArray());
        }
        public static void Print(GridControl control, string reportName, string filters)
        {
            GridView view = control.MainView as GridView;
            GridMultiSelectMode mode = view.OptionsSelection.MultiSelectMode;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

            view.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            view.Appearance.Row.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

            view.AppearancePrint.EvenRow.BackColor = Color.LightGray;
            view.AppearancePrint.OddRow.BackColor = Color.WhiteSmoke;
            view.OptionsPrint.EnableAppearanceEvenRow =
            view.OptionsPrint.EnableAppearanceOddRow = true;

            view.AppearancePrint.HeaderPanel.BackColor = Color.FromArgb(43, 87, 151);
            view.AppearancePrint.HeaderPanel.Font = new Font(view.AppearancePrint.HeaderPanel.Font, FontStyle.Bold);
            view.AppearancePrint.EvenRow.ForeColor = Color.WhiteSmoke;

            view.AppearancePrint.HeaderPanel.Options.UseBackColor = true;
            view.AppearancePrint.HeaderPanel.Options.UseFont = true;
            view.AppearancePrint.HeaderPanel.Options.UseForeColor = true;

            view.OptionsPrint.AllowMultilineHeaders = true;
            view.OptionsPrint.AutoWidth = true;
            view.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            //The lines between columns themselves
            view.AppearancePrint.Lines.BackColor = Color.LightGray;

            view.OptionsPrint.UsePrintStyles = true;
            

            //84
            RPT_Grid_Print rpt = new RPT_Grid_Print();
            rpt.cell_report_filter.Text = filters;
            rpt.cell_report_name.Text = reportName;
            PrintableComponentLink link = new PrintableComponentLink();
            link.Component = control;
            rpt.printableComponentContainer1.PrintableComponent = link;

            rpt.ShowRibbonPreviewDialog();

            view.OptionsSelection.MultiSelectMode = mode;

        }
    }
}
