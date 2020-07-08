﻿using DevExpress.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Helpers;
using DevExpress.XtraPrinting.Preview.Native;
using DevExpress.XtraReports.UI;
using Microsoft.Win32;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using Pro_Salles.Reporting;
using Pro_Salles.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Pro_Salles.Class.Master_Finance;

namespace Pro_Salles.PL
{
    public partial class FRM_Invoice : FRM_Master
    {
        Pro_SallesDataContext generaldb;
        Invoice_Detail v;
        Invoice_Header Invoice;
        Store store;
        Drower drower;
        Master.Invoice_Type Type;
        RepositoryItemGridLookUpEdit repo_items;
        RepositoryItemLookUpEdit repoUOM;
        RepositoryItemLookUpEdit repoStores;
        RepositoryItemLookUpEdit repoAll;

        public FRM_Invoice(Master.Invoice_Type _type)
        {
            InitializeComponent();
            /////46
            Intialize_Events();


            Type = _type;
            //Refresh_Data();
            New();
        }

        public FRM_Invoice(Master.Invoice_Type _type, int id)
        {
            InitializeComponent();
            Intialize_Events();


            Type = _type;
            //Refresh_Data();
            using (var db = new Pro_SallesDataContext())
            {
                Invoice = db.Invoice_Headers.Single(x => x.ID == id);
                Get_Data();
                isNew = false;
            }
        }

        void Intialize_Events()
        {
            /////46
            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.InvalidRowException += GridView1_InvalidRowException;

            look_part_type.EditValueChanged += Look_part_type_EditValueChanged;

            look_grid_part_id.EditValueChanged += Look_grid_part_id_EditValueChanged;
        }


        string Get_Next_Invoice_Code()
        {
            string max_code;
            using (var db = new Pro_SallesDataContext())
            {
                max_code = db.Invoice_Headers.Where(x => x.invoice_type == (int)Type).Select(x => x.code).Max();
            }
            return Master.Get_Next_Code(max_code);
        }
        public override void New()
        {
            Invoice = new Invoice_Header()
            {
                code = Get_Next_Invoice_Code(),
                drower = Sessions.Defaults.Drower,
                posted_to_store = true,
                post_date = DateTime.Now,
                date = DateTime.Now,
                deliviry_date = DateTime.Now
            };
            switch (Type)
            {
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Purchase:
                    Invoice.part_type = (int)Master.Part_Type.Vendor;
                    Invoice.part_id = Sessions.Defaults.Vendor;
                    Invoice.branch = Sessions.Defaults.Raw_Store;
                    break;
                case Master.Invoice_Type.Salles_Return:
                case Master.Invoice_Type.Salles:
                    Invoice.part_type = (int)Master.Part_Type.Customer;
                    Invoice.part_id = Sessions.Defaults.Customer;
                    Invoice.branch = Sessions.Defaults.Store;
                    break;
                default:
                    break;
            }
            base.New();
            Move_Focus_To_Grid();
        }

        void Move_Focus_To_Grid(bool FocusToItem = false)
        {
            this.gridView1.Focus();
            gridView1.FocusedColumn = (FocusToItem) ? gridView1.Columns[nameof(v.item_id)] : gridView1.Columns["Code"];
            gridView1.AddNewRow();
            gridView1.UpdateCurrentRow();
        }
        public override bool Is_Data_Valide()
        {
            int Number_Of_Erorrs = 0;
            if (gridView1.RowCount == 0)
            {
                Number_Of_Erorrs++;
                XtraMessageBox.Show("برجاء ادخال صنف واحد على الأقل", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Number_Of_Erorrs += txt_code.IsStringValid() ? 0 : 1;
            //لو هي انتجر يبقي ترو و فيها قيما يبقي مش هعد الأخطاء اما لو فوولس يبقي هنضيف ايرور يعني مفيش  داتا او بيانات فيها
            Number_Of_Erorrs += look_part_type.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_branch.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_drower.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_grid_part_id.IsEditValueValidAndNotZero() ? 0 : 1;

            Number_Of_Erorrs += date_date.IsDateTimeValid() ? 0 : 1;

            if (checkbox_posted_to_store.Checked)
            {
                Number_Of_Erorrs += date_post_date.IsDateTimeValid() ? 0 : 1;
                layoutControlGroup2.Expanded = true;
            }


            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    break;
                case Master.Invoice_Type.Salles:
                    if (Invoice.discount_ratio != Convert.ToDouble(spin_discount_ratio.EditValue) && Sessions.UserSettings.Salles.Max_Discount_In_Invoice < Convert.ToDecimal(spin_discount_ratio.EditValue))
                    {
                        Number_Of_Erorrs++;
                        spin_discount_ratio.ErrorText = "هذا الخصم غير مسموح به";
                    }

                    if (Invoice.ID == 0)
                    {
                        var id = Convert.ToInt32(look_grid_part_id.EditValue);
                        CustomersAndVendor account;
                        if (id != 0)
                        {
                            if (look_part_type.EditValue.Equals((byte)Master.Part_Type.Vendor))
                            {
                                account = Sessions.Vendors.SingleOrDefault(x => x.ID == id);
                            }
                            else
                            {
                                account = Sessions.Customers.SingleOrDefault(x => x.ID == id);
                            }
                            if (account.max_Credit <= accountBalance.BalanceAmount && accountBalance.BalanceType == AccountBalance.BalanceTypes.Debit)
                            {
                                switch (Sessions.UserSettings.Salles.When_Selling_To_Customer_Exceded_Max_Credit)
                                {
                                    case Master.Warning_Levels.Do_Not_Interrupt:
                                        break;
                                    case Master.Warning_Levels.Show_Warning:
                                        if (XtraMessageBox.Show("لقد تخطي هذا العميل حد الأئتمان..هل تريد المتابعه؟", "تأكيد عمليه البيع",
                                            buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.No)
                                        {
                                            Number_Of_Erorrs++;
                                        }
                                        break;
                                    case Master.Warning_Levels.Prevent:
                                        Number_Of_Erorrs++;
                                        XtraMessageBox.Show("لا يمكن البيع لهذا العميل حيث تخطي حد الأئتمان", "تحذير", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Salles_Return:
                default:
                    throw new NotImplementedException();

            }
            //هنا هترجع قيمه ترو فقط لو كان عدد الأخطاء بيساوي صفر اما لو اكتر هيرجع فوولس
            return (Number_Of_Erorrs == 0);
        }
        private void FRM_Invoice_Load(object sender, EventArgs e)
        {
            btn_print.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            Refresh_Data();

            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    this.Text = "   فاتوره مشتريات";
                    this.Name = Screens.Add_Purchase_Invoice.Screen_Name;
                    checkbox_posted_to_store.Enabled = false;
                    checkbox_posted_to_store.Checked = true;
                    break;
                case Master.Invoice_Type.Salles:
                    this.Text = "   فاتوره مبيعات ";
                    this.Name = Screens.Add_Salles_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Purchase_Return:
                    this.Text = "   فاتوره مرتجع مشتروات";
                    break;
                case Master.Invoice_Type.Salles_Return:
                    this.Text = "   فاتوره مرتجع مبيعات";
                    break;
                default:
                    throw new NotImplementedException();
            }

            //look_branch.Properties.Columns[nameof(store.Cost_Of_Sold_Goods_Account_ID)].Visible = false;
            //look_branch.Properties.Columns[nameof(store.Discount_Allowed_Account_ID)].Visible = false;
            //look_branch.Properties.Columns[nameof(store.Discount_Received_Account_ID)].Visible = false;
            //look_branch.Properties.Columns[nameof(store.Salles_Return_Account_ID)].Visible = false;
            //look_branch.Properties.Columns[nameof(store.Salles_Account_ID)].Visible = false;
            //look_branch.Properties.Columns[nameof(store.Inventory_Account_ID)].Visible = false;

            look_part_type.LookUp_DataSource(Master.Part_Type_List);
            look_part_type.Properties.PopulateColumns();
            look_part_type.Properties.Columns["ID"].Visible = false;
            ///////////////////28   
            look_grid_part_id.ButtonClick += Look_part_id_ButtonClick;

            look_grid_part_id.Properties.ValidateOnEnterKey = true;
            look_grid_part_id.Properties.AllowNullInput = DefaultBoolean.False;//to don't let the user put null values
            look_grid_part_id.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;//To make the size fit to the fields
            look_grid_part_id.Properties.ImmediatePopup = true;
            look_grid_part_id.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
            look_grid_part_id.Properties.ButtonClick += Repo_items_ButtonClick;


            var part_idView = look_grid_part_id.Properties.View;
            part_idView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            part_idView.OptionsSelection.UseIndicatorForSelection = true;
            part_idView.OptionsView.ShowAutoFilterRow = true;
            part_idView.PopulateColumns(look_grid_part_id.Properties.DataSource);

            ////////////////////There is a commint down////////////////
            look_branch.EditValueChanging += Look_branch_EditValueChanging;

            #region RepositoryItem_Properties
            repoUOM.LookUp_DataSource(Sessions.Unit_Names, gridView1.Columns[nameof(v.item_unit_id)], gridControl1);
            repoStores.LookUp_DataSource(Sessions.Stores, gridView1.Columns[nameof(v.store_id)], gridControl1);

            repo_items = new RepositoryItemGridLookUpEdit();
            repo_items.LookUp_DataSource(Sessions.Product_View.Where(x => x.Is_Active == true), gridView1.Columns[nameof(v.item_id)], gridControl1, "Name", "ID");
            repo_items.ValidateOnEnterKey = true;
            repo_items.AllowNullInput = DefaultBoolean.False;//to don't let the user put null values
            repo_items.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;//To make the size fit to the fields
            repo_items.ImmediatePopup = true;
            repo_items.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
            repo_items.ButtonClick += Repo_items_ButtonClick;

            var repoview = repo_items.View;
            repoview.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            repoview.OptionsSelection.UseIndicatorForSelection = true;
            repoview.OptionsView.ShowAutoFilterRow = true;

            repoview.PopulateColumns(repo_items.DataSource);
            Sessions.Product_View_Class pr;
            if (repoview.RowCount > 0)
            {
                repoview.Columns[nameof(pr.ID)].Visible = false;
                repoview.Columns[nameof(pr.Type)].Visible = false;
                repoview.Columns[nameof(pr.Is_Active)].Visible = false;
                repoview.Columns[nameof(pr.Name)].Caption = "الأسم";
                repoview.Columns[nameof(pr.discription)].Caption = "الوصف";
                repoview.Columns[nameof(pr.Cat_Name)].Caption = "الصنف";
                repoview.Columns[nameof(pr.Code)].Caption = "الكود";
            }

            repoAll.LookUp_DataSource(Sessions.Product_View, gridView1.Columns[nameof(v.item_id)], gridControl1, "Name", "ID");

            RepositoryItemSpinEdit spinedit = new RepositoryItemSpinEdit();
            gridView1.Columns[nameof(v.price)].ColumnEdit = spinedit;
            gridView1.Columns[nameof(v.item_qty)].ColumnEdit = spinedit;
            gridView1.Columns[nameof(v.discount_value)].ColumnEdit = spinedit;

            RepositoryItemSpinEdit spinratioedit = new RepositoryItemSpinEdit();
            spinratioedit.Increment = 0.01m;
            spinratioedit.Mask.EditMask = "p";
            spinratioedit.Mask.UseMaskAsDisplayFormat = true;
            spinratioedit.MaxValue = 1;
            gridView1.Columns[nameof(v.discount)].ColumnEdit = spinratioedit;
            #endregion

            #region GridView_Properties

            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            gridView1.Columns[nameof(v.ID)].Visible = false;
            gridView1.Columns[nameof(v.invoice_id)].Visible = false;

            gridControl1.RepositoryItems.Add(spinedit);
            gridControl1.RepositoryItems.Add(spinratioedit);

            gridView1.Columns[nameof(v.total_price)].OptionsColumn.AllowFocus = false;

            gridView1.CellValueChanged += GridView1_CellValueChanged;

            gridView1.Columns.Add(new GridColumn()
            {
                Name = "CLM_Code",
                FieldName = "Code",
                Caption = "الباركود",
                UnboundType = DevExpress.Data.UnboundColumnType.String,
                MaxWidth = 90
            });

            gridView1.Columns.Add(new GridColumn()
            {
                Name = "CLM_Index",
                FieldName = "Index",
                Caption = "م",
                UnboundType = DevExpress.Data.UnboundColumnType.Integer,
                MaxWidth = 40
            });

            gridView1.Columns.Add(new GridColumn()
            {
                Name = "CLM_Balance",
                FieldName = "Balance",
                Caption = "الرصيد",
                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                MaxWidth = 90,
            });

            gridView1.Columns[nameof(v.item_id)].Caption = "المنتج";
            gridView1.Columns[nameof(v.item_qty)].Caption = "الكميه";
            gridView1.Columns[nameof(v.item_unit_id)].Caption = "وحده القياس";
            gridView1.Columns[nameof(v.price)].Caption = "السعر";
            gridView1.Columns[nameof(v.cost_value)].Caption = "سعر التكلفه";
            gridView1.Columns[nameof(v.discount)].Caption = "ن الخصم";
            gridView1.Columns[nameof(v.discount_value)].Caption = "ق الخصم";
            gridView1.Columns[nameof(v.store_id)].Caption = "المخزن";
            gridView1.Columns[nameof(v.total_cost_value)].Caption = "اجمالى التكلفه";
            gridView1.Columns[nameof(v.total_price)].Caption = "المبلغ الاجمالي";
            gridView1.Columns["Index"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["Index"].OptionsColumn.AllowEdit = false;

            gridView1.Columns["Balance"].OptionsColumn.AllowFocus = false;
            gridView1.Columns["Balance"].OptionsColumn.AllowEdit = false;

            gridView1.Columns[nameof(v.cost_value)].OptionsColumn.AllowFocus = false;
            gridView1.Columns[nameof(v.total_cost_value)].OptionsColumn.AllowFocus = false;


            gridView1.Columns[nameof(v.item_id)].MinWidth = 125;
            gridView1.Columns[nameof(v.item_qty)].MaxWidth = 100;

            gridView1.Columns["Index"].VisibleIndex = 0;
            gridView1.Columns["Code"].VisibleIndex = 1;
            gridView1.Columns[nameof(v.item_id)].VisibleIndex = 2;
            gridView1.Columns[nameof(v.item_unit_id)].VisibleIndex = 3;
            gridView1.Columns["Balance"].VisibleIndex = 4;
            gridView1.Columns[nameof(v.item_qty)].VisibleIndex = 5;
            gridView1.Columns[nameof(v.price)].VisibleIndex = 6;
            gridView1.Columns[nameof(v.discount)].VisibleIndex = 7;
            gridView1.Columns[nameof(v.discount_value)].VisibleIndex = 8;
            gridView1.Columns[nameof(v.total_price)].VisibleIndex = 9;
            gridView1.Columns[nameof(v.cost_value)].VisibleIndex = 10;
            gridView1.Columns[nameof(v.total_cost_value)].VisibleIndex = 11;
            gridView1.Columns[nameof(v.store_id)].VisibleIndex = 12;

            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            //gridView1.Appearance.EvenRow.BackColor = Color.WhiteSmoke;

            gridView1.OptionsView.EnableAppearanceOddRow = true;
            gridView1.Appearance.OddRow.BackColor = Color.FromArgb(150, 255, 249, 196);

            RepositoryItemButtonEdit ButtonEdit = new RepositoryItemButtonEdit();
            gridControl1.RepositoryItems.Add(ButtonEdit);
            ButtonEdit.Buttons.Clear();
            ButtonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
            ButtonEdit.ButtonClick += ButtonEdit_ButtonClick;
            GridColumn CLM_Delete = new GridColumn()
            {
                Name = "CLM_Delete",
                Caption = "",
                FieldName = "Delete",
                VisibleIndex = 13,
                ColumnEdit = ButtonEdit,
                Width = 7,
                MaxWidth = 7,
            };
            //gridView1.Columns["Delete"].Caption = "";

            ButtonEdit.TextEditStyle = TextEditStyles.HideTextEditor;
            gridView1.Columns.Add(CLM_Delete);
            #endregion

            #region Events_Spin_Edit
            spin_discount_value.Enter += Spin_discount_value_Enter;
            spin_discount_value.Leave += Spin_discount_value_Leave;
            spin_discount_value.EditValueChanged += Spin_discount_value_EditValueChanged;
            spin_discount_ratio.EditValueChanged += Spin_discount_value_EditValueChanged;
            //spin_total.EditValue = 800;
            spin_tax_value.Enter += Spin_tax_value_Enter;
            spin_tax_value.Leave += Spin_tax_value_Leave;
            spin_tax_value.EditValueChanged += Spin_tax_value_EditValueChanged;
            spin_tax_ratio.EditValueChanged += Spin_tax_value_EditValueChanged;

            spin_tax_value.EditValueChanged += Spin_EditValueChanged;
            spin_discount_value.EditValueChanged += Spin_EditValueChanged;

            //spin_discount_ratio.EditValueChanging += Spin_discount_ratio_EditValueChanging;

            spin_expences.EditValueChanged += Spin_EditValueChanged;
            spin_total.EditValueChanged += Spin_EditValueChanged;

            spin_paid.EditValueChanged += Spin_paid_EditValueChanged;
            spin_net.EditValueChanged += Spin_paid_EditValueChanged;

            //Changing//
            spin_net.EditValueChanging += Spin_net_EditValueChanging;

            spin_net.DoubleClick += Spin_net_DoubleClick;


            gridView1.CustomRowCellEditForEditing += GridView1_CustomRowCellEditForEditing;
            gridView1.RowCountChanged += GridView1_RowCountChanged;
            gridView1.RowUpdated += GridView1_RowUpdated;
            gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
            gridControl1.ProcessGridKey += GridControl1_ProcessGridKey;
            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.InvalidRowException += GridView1_InvalidRowException;
            gridView1.CellValueChanging += GridView1_CellValueChanging;
            this.Activated += FRM_Invoice_Activated;
            this.KeyPreview = true;
            this.KeyDown += FRM_Invoice_KeyDown;
            #endregion


            ReadUserSettings();
        }

        AccountBalance accountBalance;
        private void Look_grid_part_id_EditValueChanged(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(look_grid_part_id.EditValue);
            if (id != 0)
            {
                CustomersAndVendor account;
                if (look_part_type.EditValue.Equals((byte)Master.Part_Type.Vendor))
                {
                    account = Sessions.Vendors.SingleOrDefault(x => x.ID == id);
                }
                else
                {
                    account = Sessions.Customers.SingleOrDefault(x => x.ID == id);
                }
                if (account != null)
                {
                    txt_part_address.Text = account.address;
                    spin_part_maxCredit.EditValue = Convert.ToDouble(account.max_Credit);
                    txt_part_phone.Text = account.phone;
                    accountBalance = GetAccountBalance(account.account_id);
                    txt_part_balance.Text = accountBalance.Balance;
                }
            }
            else
            {
                txt_part_address.Text = "";
                spin_part_maxCredit.EditValue = 0;
                txt_part_phone.Text = "";
                txt_part_balance.Text = "";
            }
        }

        private void GridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            // untill change the unit when the exists item added changed
            if (e.Column.FieldName == nameof(v.item_id))
            {
                var row = gridView1.GetRow(e.RowHandle) as Invoice_Detail;
                if (row != null)
                {
                    if (row.item_id != 0 && e.Value.Equals(row.item_id) == false)
                        row.item_unit_id = 0;
                }

            }
        }

        private void Repo_items_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Plus)
                FRM_MAIN.Open_Form_By_Name(nameof(FRM_Product));
        }

        private void FRM_Invoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                Move_Focus_To_Grid(e.Modifiers == Keys.Shift);
            else if (e.KeyCode == Keys.F6)
            {
                //To do go to product by index
            }

            else if (e.KeyCode == Keys.F7)
                look_grid_part_id.Focus();

            else if (e.KeyCode == Keys.F8)
                txt_code.Focus();

            else if (e.KeyCode == Keys.F9)
                spin_discount_value.Focus();

            else if (e.KeyCode == Keys.F10)
                spin_tax_value.Focus();

            else if (e.KeyCode == Keys.F11)
                spin_expences.Focus();

            else if (e.KeyCode == Keys.F12)
                spin_paid.Focus();
        }

        #region Gridview_Events
        private void GridView1_RowUpdated(object sender, RowObjectEventArgs e)
        {
            //To calculate the sum in the gridview by function sum;
            var items = gridView1.DataSource as Collection<Invoice_Detail>;
            if (items == null)
                spin_total.EditValue = 0;
            else
                spin_total.EditValue = items.Sum(x => x.total_price);
        }

        int CurrentRowsCount = 0;
        private void GridView1_RowCountChanged(object sender, EventArgs e)
        {
            if (CurrentRowsCount < gridView1.RowCount)
            {
                var Rows = (gridView1.DataSource as Collection<Invoice_Detail>);
                var lastRow = Rows.Last();
                var row = Rows.FirstOrDefault(x => x.item_id == lastRow.item_id && x.item_unit_id == lastRow.item_unit_id && x != lastRow);
                if (row != null)
                {
                    row.item_qty += lastRow.item_qty;
                    GridView1_CellValueChanged(sender,
                        new CellValueChangedEventArgs(gridView1.Find_Row_By_RowObject(row),
                        gridView1.Columns[nameof(row.item_qty)], row.item_qty));
                    Rows.Remove(lastRow);
                }
            }
            CurrentRowsCount = gridView1.RowCount;
            GridView1_RowUpdated(sender, null);
        }
        private void GridView1_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            if (e.Row == null || (e.Row as Invoice_Detail).item_id == 0)
            {
                gridView1.DeleteRow(e.RowHandle);
                e.ExceptionMode = ExceptionMode.Ignore;
                return;
            }
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void GridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            var row = e.Row as Invoice_Detail;
            if (row == null || row.item_id == 0)
            {
                e.Valid = false;
                return;
            }
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    break;
                case Master.Invoice_Type.Salles:
                    if (row.cost_value > 0)
                    {
                        if (row.cost_value > row.price)
                        {
                            switch (Sessions.UserSettings.Salles.When_Selling_Item_With_Price_Lower_Than_Cost_Price)
                            {
                                case Master.Warning_Levels.Do_Not_Interrupt:
                                    break;
                                case Master.Warning_Levels.Prevent:
                                    e.Valid = false;
                                    gridView1.SetColumnError(gridView1.Columns[nameof(v.price)], "سعر البيع اقل من سعر التكلفه");

                                    break;
                                case Master.Warning_Levels.Show_Warning:
                                    if (XtraMessageBox.Show("سعر البيع اقل من سعر التكلفه..هل تريد المتابعه؟", "تأكيد عمليه البيع",
                                        buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        e.Valid = false;
                                        gridView1.SetColumnError(gridView1.Columns[nameof(v.price)], "سعر البيع اقل من سعر التكلفه");
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (Convert.ToDecimal(row.discount_value) > Sessions.UserSettings.Salles.Max_Discount_Per_Item)
                    {
                        e.Valid = false;
                        gridView1.SetColumnError(gridView1.Columns[nameof(v.discount_value)], "نسبه الخصم غير مسموح بها");
                    }
                    break;
                case Master.Invoice_Type.Purchase_Return:
                    break;
                case Master.Invoice_Type.Salles_Return:
                    break;
                default:
                    break;
            }
        }
        private void GridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            var row = gridView1.GetRow(e.RowHandle) as Invoice_Detail;
            if (row == null) return;

            Sessions.Product_View_Class itemV = null;
            Sessions.Product_View_Class.Product_UOM_View unitV = null;
            if (e.Column.FieldName == "Code")
            {
                //////////42/////42//42//42//42//////////
                string Item_Code = e.Value.ToString();
                if (Sessions.GlobalSettings.Read_From_Scale_Barcode &&
                    Item_Code.Length == Sessions.GlobalSettings.Barcode_Length &&
                    Item_Code.StartsWith(Sessions.GlobalSettings.Scale_Barcode_Prefix))
                {
                    var Item_Code_String = e.Value.ToString().
                        Substring(Sessions.GlobalSettings.Scale_Barcode_Prefix.Length,
                        Sessions.GlobalSettings.Product_Code_Length);

                    Item_Code = Convert.ToInt32(Item_Code_String).ToString();
                    string Read_Value = e.Value.ToString().Substring(
                        Sessions.GlobalSettings.Scale_Barcode_Prefix.Length +
                        Sessions.GlobalSettings.Product_Code_Length);
                    if (Sessions.GlobalSettings.Ignore_Check_Digit)
                        Read_Value = Read_Value.Remove(Read_Value.Length - 1, 1);
                    double Value = Convert.ToDouble(Read_Value);
                    Value = Value / (Math.Pow(10, Sessions.GlobalSettings.Devide_Value_By));
                    if (Sessions.GlobalSettings.Read_Mode == Sessions.GlobalSettings.Read_Value_Mode.Weight)
                        row.item_qty = Value;
                    else if (Sessions.GlobalSettings.Read_Mode == Sessions.GlobalSettings.Read_Value_Mode.Price)
                    {
                        itemV = Sessions.Product_View.FirstOrDefault(x => x.Units.Select(u => u.Barcode).Contains(Item_Code));

                        if (itemV != null)
                        {
                            unitV = itemV.Units.First(x => x.Barcode == Item_Code);

                            switch (Type)
                            {
                                case Master.Invoice_Type.Purchase_Return:
                                case Master.Invoice_Type.Purchase:
                                    row.item_qty = Value / unitV.Buy_price;
                                    break;
                                case Master.Invoice_Type.Salles_Return:
                                case Master.Invoice_Type.Salles:
                                    row.item_qty = Value / unitV.Sell_price;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                //////////////////////////////////////37//////42/42///42//42//////////////////////////////
                if (itemV == null)
                    itemV = Sessions.Product_View.FirstOrDefault(x => x.Units.Select(u => u.Barcode).Contains(Item_Code));
                if (itemV != null)
                {
                    row.item_id = itemV.ID;
                    if (unitV == null)
                        unitV = itemV.Units.First(x => x.Barcode == Item_Code);

                    row.item_unit_id = unitV.Unit_ID;
                    GridView1_CellValueChanged(sender, new CellValueChangedEventArgs
                        (e.RowHandle, gridView1.Columns[nameof(v.item_id)], row.item_id));
                    GridView1_CellValueChanged(sender, new CellValueChangedEventArgs
                        (e.RowHandle, gridView1.Columns[nameof(v.item_unit_id)], row.item_unit_id));
                    EnteredCode = string.Empty;
                    return;
                }
                EnteredCode = string.Empty;
            }

            //////////////////////
            if (row.item_id == 0) return;

            itemV = Sessions.Product_View.Single(x => x.ID == row.item_id);
            if (row.item_unit_id == 0)
            {
                row.item_unit_id = itemV.Units.FirstOrDefault().Unit_ID;
                GridView1_CellValueChanged(sender,
                new CellValueChangedEventArgs
                (e.RowHandle, gridView1.Columns[nameof(v.item_unit_id)], row.item_unit_id));
            }

            unitV = itemV.Units.Single(x => x.Unit_ID == row.item_unit_id);//////////////firstordefault || single //////////////////

            /////////////////////
            switch (e.Column.FieldName)
            {
                //TODO read from barcode
                case nameof(v.item_id):
                    if (row.item_id == 0)
                    {
                        gridView1.DeleteRow(e.RowHandle);
                        return;
                    }

                    if (row.store_id == 0 && look_branch.IsEditValueValidAndNotZero())
                        row.store_id = Convert.ToInt32(look_branch.EditValue);

                    break;
                case nameof(v.item_unit_id):

                    switch (Type)
                    {
                        case Master.Invoice_Type.Purchase:
                            row.price = unitV.Buy_price;
                            break;
                        case Master.Invoice_Type.Salles:
                            row.price = unitV.Sell_price;
                            break;
                        case Master.Invoice_Type.Salles_Return:
                        case Master.Invoice_Type.Purchase_Return:
                        default:
                            throw new NotImplementedException();
                    }

                    if (row.item_qty == 0) row.item_qty = 1;

                    GridView1_CellValueChanged(sender,
                        new CellValueChangedEventArgs
                        (e.RowHandle, gridView1.Columns[nameof(v.price)], row.price));
                    //TODO calculate current item balance
                    break;

                case nameof(v.price):
                case nameof(v.discount):
                case nameof(v.item_qty):
                    //TODO should get the price of the cost when change the qty of the item || the item itself
                    row.discount_value = row.discount * (row.item_qty * row.price);
                    GridView1_CellValueChanged(sender,
                    new CellValueChangedEventArgs
                    (e.RowHandle, gridView1.Columns[nameof(v.discount_value)], row.discount_value));

                    break;
                case nameof(v.discount_value):
                    row.total_price = (row.item_qty * row.price) - row.discount_value;
                    if (gridView1.FocusedColumn.FieldName == nameof(v.discount_value))
                        row.discount = row.discount_value / (row.item_qty * row.price);
                    row.total_price = (row.item_qty * row.price) - row.discount_value;

                    switch (Type)
                    {
                        case Master.Invoice_Type.Purchase:
                            row.cost_value = row.total_price / row.item_qty;
                            row.total_cost_value = row.total_price;
                            break;
                        case Master.Invoice_Type.Salles:

                            var store = (row.store_id == 0) ? Convert.ToInt32(look_branch.EditValue) : row.store_id;
                            var costPerMainUnit = Master_Inventory.GetCostCalculatingMethod(row.item_id, store, row.item_qty);
                            row.cost_value = costPerMainUnit * unitV.Factor;
                            row.total_cost_value = row.cost_value * row.item_qty;
                            break;
                        case Master.Invoice_Type.Purchase_Return:
                        case Master.Invoice_Type.Salles_Return:
                        default:
                            throw new NotImplementedException();
                    }

                    break;
            }
        }

        private void GridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //34
            if (e.Column.FieldName == nameof(v.item_unit_id))
            {
                RepositoryItemLookUpEdit repo = new RepositoryItemLookUpEdit();
                repo.NullText = "";
                e.RepositoryItem = repo;
                var row = gridView1.GetRow(e.RowHandle) as Invoice_Detail;
                if (row == null)
                    return;
                var item = Sessions.Product_View.SingleOrDefault(x => x.ID == row.item_id);
                if (item == null)
                    return;
                repo.DataSource = item.Units;
                repo.DisplayMember = "Unit_Name";
                repo.ValueMember = "Unit_ID";
            }
            else if (e.Column.FieldName == nameof(v.item_id))
            {   //////39///////
                e.RepositoryItem = repo_items;
            }
        }
        private void GridControl1_ProcessGridKey(object sender, KeyEventArgs e)
        {
            ////////////////////////38/////////////////////////
            GridControl control = sender as GridControl;
            if (control == null) return;
            GridView view = control.FocusedView as GridView;
            if (view == null) return;

            if (view.FocusedColumn == null) return;

            if (e.KeyCode == Keys.Return)
            {
                string focusedColumn = view.FocusedColumn.FieldName;
                if (view.FocusedColumn.FieldName == "Code" || view.FocusedColumn.FieldName == nameof(v.item_id))
                {
                    GridControl1_ProcessGridKey(sender, new KeyEventArgs(Keys.Tab));
                }
                if (view.FocusedRowHandle < 0)
                {
                    view.AddNewRow();
                    view.FocusedColumn = view.Columns[focusedColumn];
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                view.FocusedColumn = view.VisibleColumns[view.FocusedColumn.VisibleIndex - 1];
                e.Handled = true;
            }
        }

        string EnteredCode;
        private void GridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Code")
            {
                if (e.IsSetData)
                    EnteredCode = e.Value.ToString();

                else if (e.IsGetData)
                    e.Value = EnteredCode;

            }

            else if (e.Column.FieldName == "Index")
                e.Value = gridView1.GetVisibleRowHandle(e.ListSourceRowIndex) + 1;

            else if (e.Column.FieldName == "Balance")
            {
                var row = e.Row as Invoice_Detail;
                if (row == null || row.item_id == 0 || row.store_id == 0 || row.item_unit_id == 0)
                {
                    e.Value = null;
                    return;
                }
                var proBalance = Sessions.Product_Balances.FirstOrDefault(x => x.Product_ID == row.item_id && x.Store_ID == row.store_id);
                if (proBalance == null)
                {
                    e.Value = 0;
                    return;
                }

                var balance = proBalance.Balance;
                var product = Sessions.Product_View.Single(x => x.ID == row.item_id);
                var factor = product.Units.Single(x => x.Unit_ID == row.item_unit_id).Factor;/////////////////////// double?  ???
                //if (factor != null)
                e.Value = balance / factor;
                //e.Value = Master_Inventory.GetProductBalanceInStore(row.item_id, row.store_id);
                //Debug.Print()
            }
        }

        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
            if (view.FocusedRowHandle >= 0)
            {
                view.DeleteSelectedRows();
            }
        }
        #endregion

        #region SpinEditCalcRigen
        private void Spin_paid_EditValueChanged(object sender, EventArgs e)
        {
            var net = Convert.ToDouble(spin_net.EditValue);
            var paid = Convert.ToDouble(spin_paid.EditValue);
            spin_remaining.EditValue = net - paid;
        }

        private void Spin_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spin_total.EditValue);
            var discount = Convert.ToDouble(spin_discount_value.EditValue);
            var tax = Convert.ToDouble(spin_tax_value.EditValue);
            var expences = Convert.ToDouble(spin_expences.EditValue);
            spin_net.EditValue = (total + tax - discount + expences);
        }
        bool Is_tax_value_focused;
        private void Spin_tax_value_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spin_total.EditValue);
            var tax_ratio = Convert.ToDouble(spin_tax_ratio.EditValue);
            var tax_value = Convert.ToDouble(spin_tax_value.EditValue);
            if (Is_tax_value_focused)
                spin_tax_ratio.EditValue = (tax_value / total);
            else
                spin_tax_value.EditValue = total * tax_ratio;
        }

        private void Spin_tax_value_Leave(object sender, EventArgs e)
        {
            Is_tax_value_focused = false;
        }

        private void Spin_tax_value_Enter(object sender, EventArgs e)
        {
            Is_tax_value_focused = true;
        }

        private void Spin_discount_value_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spin_total.EditValue);
            var dis_value = Convert.ToDouble(spin_discount_value.EditValue);
            var dis_ratio = Convert.ToDouble(spin_discount_ratio.EditValue);
            if (Is_discount_Value_Focused)
                spin_discount_ratio.EditValue = (dis_value / total);
            else
                spin_discount_value.EditValue = total * dis_ratio;
        }

        bool Is_discount_Value_Focused;
        private void Spin_discount_value_Leave(object sender, EventArgs e)
        {
            Is_discount_Value_Focused = false;
        }

        private void Spin_discount_value_Enter(object sender, EventArgs e)
        {
            Is_discount_Value_Focused = true;
        }

        private void Look_part_id_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                //////////////////////Don't forget to think of this step
                using (var frm = new FRM_Customer_Vendor(Convert.ToInt32(look_part_type.EditValue) == (int)Master.Part_Type.Customer))
                {
                    FRM_MAIN.Open_Form(frm, true);
                    Refresh_Data();
                }
            }
        }
        #endregion

        private void FRM_Invoice_Activated(object sender, EventArgs e)
        {
            Move_Focus_To_Grid();
        }

        private void Look_branch_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            ////If the old value in the << branch look up edit >> equals the value inside the invoice, there is an update will happen//////
            var item = gridView1.DataSource as Collection<Invoice_Detail>;
            if (e.OldValue is int && e.NewValue is int)
            {
                foreach (var row in item)
                {
                    if (row.store_id == Convert.ToInt32(e.OldValue))
                        row.store_id = Convert.ToInt32(e.NewValue);
                }
            }
        }

        private void Spin_net_DoubleClick(object sender, EventArgs e)
        {
            spin_paid.EditValue = spin_net.EditValue;
        }

        private void Spin_net_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            ////////////73
            if (Type == Master.Invoice_Type.Salles && Sessions.UserSettings.Salles.Default_Pay_Method_In_Salles == Master.Pay_Methods.Cash)
            {
                if (Convert.ToDouble(e.OldValue) == Convert.ToDouble(spin_paid.EditValue))
                    spin_paid.EditValue = e.NewValue;
            }
            else if (Type == Master.Invoice_Type.Salles && Sessions.UserSettings.Salles.Default_Pay_Method_In_Salles == Master.Pay_Methods.Credit)
            {
                spin_paid.EditValue = 0;
            }
        }

        private void Look_part_type_EditValueChanged(object sender, EventArgs e)
        {
            if (look_part_type.Is_Edit_Value_Int())
            {
                //////////////////////Don't forget to understand this part
                var parttype = Convert.ToInt32(look_part_type.EditValue);
                if (parttype == (int)Master.Part_Type.Customer)
                    look_grid_part_id.GridLookUp_Style(Sessions.Customers);
                else if (parttype == (int)Master.Part_Type.Vendor)
                    look_grid_part_id.GridLookUp_Style(Sessions.Vendors);
            }
        }

        public override void Refresh_Data()
        {
            look_branch.LookUp_DataSource(Sessions.Stores, nameof(store.name), nameof(store.ID));
            look_drower.LookUp_DataSource(Sessions.Drowers, nameof(drower.name), nameof(drower.ID));
            //look_drower.Properties.Columns[nameof(drower.account_id)].Visible = false;////////////////////////////////////////////////
            base.Refresh_Data();
        }
        public override void Get_Data()
        {
            look_branch.EditValue = Invoice.branch;
            look_drower.EditValue = Invoice.drower;
            look_part_type.EditValue = Invoice.part_type;
            look_grid_part_id.EditValue = Invoice.part_id;

            txt_code.Text = Invoice.code;

            date_date.DateTime = Invoice.date;
            date_delivery.EditValue = Invoice.deliviry_date;
            date_post_date.EditValue = Invoice.post_date;

            memo_note.Text = Invoice.notes;
            memo_shipping_address.Text = Invoice.shipping_address;

            checkbox_posted_to_store.Checked = Invoice.posted_to_store;

            spin_total.EditValue = Invoice.total;
            spin_tax_value.EditValue = Invoice.tax_value;
            spin_tax_ratio.EditValue = Invoice.tax;
            spin_discount_ratio.EditValue = Invoice.discount_ratio;
            spin_discount_value.EditValue = Invoice.discount_value;
            spin_net.EditValue = Invoice.net;
            spin_remaining.EditValue = Invoice.remaing;
            spin_paid.EditValue = Invoice.paid;
            spin_expences.EditValue = Invoice.expences;

            generaldb = new Pro_SallesDataContext();
            gridControl1.DataSource = generaldb.Invoice_Details.Where(x => x.invoice_id == Invoice.ID);

            base.Get_Data();
        }
        public override void Set_Data()
        {
            Invoice.branch = Convert.ToInt32(look_branch.EditValue);
            Invoice.drower = Convert.ToInt32(look_drower.EditValue);
            Invoice.part_type = Convert.ToByte(look_part_type.EditValue);
            Invoice.part_id = Convert.ToInt32(look_grid_part_id.EditValue);

            Invoice.code = txt_code.Text;

            Invoice.date = date_date.DateTime;
            Invoice.deliviry_date = date_delivery.EditValue as DateTime?;
            Invoice.post_date = date_post_date.EditValue as DateTime?;

            Invoice.notes = memo_note.Text;
            Invoice.shipping_address = memo_shipping_address.Text;

            Invoice.posted_to_store = checkbox_posted_to_store.Checked;

            Invoice.total = Convert.ToDouble(spin_total.EditValue);
            Invoice.tax_value = Convert.ToDouble(spin_tax_value.EditValue);
            Invoice.tax = Convert.ToDouble(spin_tax_ratio.EditValue);
            Invoice.discount_ratio = Convert.ToDouble(spin_discount_ratio.EditValue);
            Invoice.discount_value = Convert.ToDouble(spin_discount_value.EditValue);
            Invoice.net = Convert.ToDouble(spin_net.EditValue);
            Invoice.remaing = Convert.ToDouble(spin_remaining.EditValue);
            Invoice.paid = Convert.ToDouble(spin_paid.EditValue);
            Invoice.expences = Convert.ToDouble(spin_expences.EditValue);

            Invoice.invoice_type = (byte)Type;



            base.Set_Data();
        }
        public override void Save()
        {
            gridView1.UpdateCurrentRow();
            var db = new Pro_SallesDataContext();
            if (Invoice.ID == 0)
            {
                db.Invoice_Headers.InsertOnSubmit(Invoice);
            }
            else
            {
                db.Invoice_Headers.Attach(Invoice);
            }

            Set_Data();

            var items = gridView1.DataSource as Collection<Invoice_Detail>;

            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    if (Invoice.expences > 0)
                    {
                        var total_Price = items.Sum(x => x.total_price);
                        var total_QTY = items.Sum(x => x.item_qty);

                        var By_Price_Unit = Invoice.expences / total_Price; // The price for every one pound

                        var By_QTY_Unit = Invoice.expences / total_QTY; // The price for every choosen unit
                        XtraDialogArgs args = new XtraDialogArgs();
                        Cost_Distribution_Option cost = new Cost_Distribution_Option();

                        args.Caption = "";
                        args.Content = cost;
                        ((XtraBaseArgs)args).Buttons = new DialogResult[] // Dev express advice you to use (xtrabaseargs) by casting
                        {
                DialogResult.OK
                        };
                        args.Showing += Args_Showing;
                        XtraDialog.Show(args);

                        foreach (var item in items)
                        {
                            if (cost.Selected_Option == Master.Cost_Distribution_Options.By_QTY)
                                item.cost_value = (item.total_price / item.item_qty) + (By_Price_Unit * item.price); // توزيع بالسعر
                            else
                                item.cost_value = (item.total_price / item.item_qty) + (By_QTY_Unit);//توزيع بالكميه

                            item.total_cost_value = item.item_qty * item.cost_value;
                        }
                    }
                    else
                    {
                        foreach (var row in items)
                        {
                            row.cost_value = row.total_price / row.item_qty;
                            row.total_cost_value = row.total_price;
                        }
                    }
                    break;
                case Master.Invoice_Type.Salles:
                    break;
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Salles_Return:
                default:
                    throw new NotImplementedException();
            }



            //var msg = string.Format("فاتوره مبيعات رقم {0} لعميل {1}", Invoice.ID, look_grid_part_id.Text);
            ////////////////////////////////////44///////////////////////////////////////////////// To do tommorow 
            #region Journals
            db.Journals.DeleteAllOnSubmit(db.Journals.Where(x => x.Source_Type == (byte)Type && x.Source_ID == Invoice.ID));
            db.SubmitChanges();
            //var PartAccountID = db.CustomersAndVendors.Single(x => x.ID == Invoice.part_id).account_id;
            var store_journal = db.Stores.Single(x => x.ID == Invoice.branch);
            var drower = db.Drowers.Single(x => x.ID == Invoice.drower);
            string msg;


            int StoreAccount;
            int TaxAccount;
            int PartAccountID;
            int DiscountAccount;
            bool IsPartCredit;
            bool InsertCostOsSoldGoodsJournal;
            bool Is_In;

            PartAccountID = db.CustomersAndVendors.Single(x => x.ID == Invoice.part_id).account_id;
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    StoreAccount = store_journal.Inventory_Account_ID;
                    TaxAccount = Sessions.Defaults.Purchase_Tax;
                    DiscountAccount = Sessions.Defaults.Discount_Received_Account;
                    IsPartCredit = true;
                    Is_In = true;
                    InsertCostOsSoldGoodsJournal = false;
                    msg = string.Format("فاتوره مشتروات رقم {0} : {1}", Invoice.ID, look_grid_part_id.Text);
                    break;
                case Master.Invoice_Type.Salles:
                    StoreAccount = store_journal.Salles_Account_ID;
                    TaxAccount = Sessions.Defaults.Salles_Tax;
                    DiscountAccount = Sessions.Defaults.Discount_Allowed_Account;
                    IsPartCredit = false;
                    Is_In = false;
                    InsertCostOsSoldGoodsJournal = true;
                    msg = string.Format("فاتوره مبيعات رقم {0} : {1}", Invoice.ID, look_grid_part_id.Text);
                    break;
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Salles_Return:
                default:
                    throw new NotImplementedException();
            }



            db.Journals.InsertOnSubmit(new Journal() // Vendor | Part
            {
                Account_ID = PartAccountID,
                Code = 54545,
                Credit = (IsPartCredit) ? Invoice.total + Invoice.tax_value + Invoice.expences : 0,
                Debit = (IsPartCredit == false) ? Invoice.total + Invoice.tax_value + Invoice.expences : 0,
                Insert_Date = Invoice.date,
                Notes = msg,
                Source_ID = Invoice.ID,
                Source_Type = (byte)Type
            });
            db.Journals.InsertOnSubmit(new Journal() // Store Inventory
            {
                Account_ID = store_journal.Inventory_Account_ID,
                Code = 54545,
                Credit = (IsPartCredit == false) ? Invoice.total + Invoice.expences : 0,
                Debit = (IsPartCredit) ? Invoice.total + Invoice.expences : 0,
                Insert_Date = Invoice.date,
                Notes = msg,
                Source_ID = Invoice.ID,
                Source_Type = (byte)Type
            });
            //If the invoice has tax more than zero to record it inside the database on Account_Table
            if (Invoice.tax > 0)
                db.Journals.InsertOnSubmit(new Journal() // Store Tax
                {
                    Account_ID = Sessions.Defaults.Purchase_Tax,
                    Code = 54545,
                    Credit = (IsPartCredit == false) ? Invoice.tax_value : 0,
                    Debit = (IsPartCredit) ? Invoice.tax_value : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg + " - ضريبه مضافه",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });

            //if (invoice.expences > 0)
            //    db.Journals.InsertOnSubmit(new Journal() // 44
            //    {
            //        Account_ID = Sessions.Defaults.Purchase_Expences,
            //        Code = 54545,
            //        Credit = 0,
            //        Debit = invoice.expences,
            //        Insert_Date = invoice.date,
            //        Notes = msg + " - مصروفات شراء",
            //        Source_ID = invoice.ID,
            //        Source_Type = (byte)Type
            //    });


            if (Invoice.discount_value > 0)
            {
                db.Journals.InsertOnSubmit(new Journal() // 44
                {
                    //Account_ID = Sessions.Defaults.Discount_Received_Account,
                    Account_ID = DiscountAccount,
                    Code = 54545,
                    Credit = (IsPartCredit) ? Invoice.discount_value : 0,
                    Debit = (!IsPartCredit) ? Invoice.discount_value : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg + " - خصم شراء",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
                db.Journals.InsertOnSubmit(new Journal() // 
                {
                    Account_ID = PartAccountID,
                    Code = 54545,
                    Credit = (!IsPartCredit) ? Invoice.discount_value : 0,
                    Debit = (IsPartCredit) ? Invoice.discount_value : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg + " - خصم شراء",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
            }

            if (InsertCostOsSoldGoodsJournal)
            {
                var TotalCost = items.Sum(x => x.total_cost_value);

                db.Journals.InsertOnSubmit(new Journal() // CostOfSolds
                {
                    Account_ID = store_journal.Inventory_Account_ID,
                    Code = 54545,
                    Credit = (!IsPartCredit) ? TotalCost : 0,
                    Debit = (IsPartCredit) ? TotalCost : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg = " - تكلفه البضاعه المباعه",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
                db.Journals.InsertOnSubmit(new Journal() // CostOfSolds
                {
                    Account_ID = store_journal.Cost_Of_Sold_Goods_Account_ID,
                    Code = 54545,
                    Credit = (IsPartCredit) ? TotalCost : 0,
                    Debit = (!IsPartCredit) ? TotalCost : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg = " - تكلفه البضاعه المباعه",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
            }

            if (Invoice.paid > 0)
            {
                db.Journals.InsertOnSubmit(new Journal() // 
                {
                    Account_ID = drower.account_id,
                    Code = 54545,
                    Credit = (IsPartCredit) ? Invoice.paid : 0,
                    Debit = (!IsPartCredit) ? Invoice.paid : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg + " - سداد",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
                db.Journals.InsertOnSubmit(new Journal() // 
                {
                    Account_ID = PartAccountID,
                    Code = 54545,
                    Credit = (!IsPartCredit) ? Invoice.paid : 0,
                    Debit = (IsPartCredit) ? Invoice.paid : 0,
                    Insert_Date = Invoice.date,
                    Notes = msg + " - سداد",
                    Source_ID = Invoice.ID,
                    Source_Type = (byte)Type
                });
            }
            #endregion

            foreach (var row in items)
                row.invoice_id = Invoice.ID;

            generaldb.SubmitChanges();
            db.Store_Logs.DeleteAllOnSubmit(db.Store_Logs.Where(x => x.source_type == (byte)Type && x.source_id == Invoice.ID));
            db.SubmitChanges();
            if (Invoice.posted_to_store)
            {
                foreach (var row in items)
                {
                    var unitV = Sessions.Product_View.Single(x => x.ID == row.item_id).Units.Single(x => x.Unit_ID == row.item_unit_id);
                    db.Store_Logs.InsertOnSubmit(new Store_Log
                    {
                        product_id = row.item_id,
                        insert_time = Invoice.post_date.Value,
                        source_id = row.ID,
                        source_type = (byte)Type,
                        notes = msg,
                        is_in_transaction = Is_In,
                        store_id = row.store_id,
                        qty = row.item_qty * unitV.Factor,
                        cost_value = row.cost_value / unitV.Factor
                    });
                }
            }
            db.SubmitChanges();
            base.Save();
        }
        private void Args_Showing(object sender, XtraMessageShowingArgs e)
        {
            //e.Form.Text = "Custom Text";
            e.Form.ControlBox = false;
            e.Form.Height = 80;
            e.Buttons[DialogResult.OK].Text = "متابعه و حفظ";
        }
        public override void Print()
        {
            /////////49////////
            using (var db = new Pro_SallesDataContext())
            {
                var Order_Invoice = (from inv in db.Invoice_Headers
                                     join str in db.Stores on inv.branch equals str.ID
                                     from part in db.CustomersAndVendors.Where(x => x.ID == inv.part_id).DefaultIfEmpty()
                                     from dr in db.Drowers.Where(x => x.ID == inv.drower).DefaultIfEmpty()
                                     where inv.ID == Invoice.ID
                                     select new
                                     {
                                         inv.ID,
                                         inv.code,
                                         Store = str.name,
                                         Drower = dr.name,
                                         PartName = part.name,
                                         Phone = part.phone,
                                         inv.date,
                                         inv.discount_value,
                                         inv.expences,
                                         Invoicetype =
                                         (inv.invoice_type == (byte)Master.Invoice_Type.Purchase) ? "فاتوره مشتريات" :
                                         (inv.invoice_type == (byte)Master.Invoice_Type.Purchase_Return) ? "فاتوره مردود مشتريات" :
                                         (inv.invoice_type == (byte)Master.Invoice_Type.Salles) ? "فاتوره مبيعات" :
                                         (inv.invoice_type == (byte)Master.Invoice_Type.Salles_Return) ? "فاتوره مردود مبيعات" : "Undefinded",
                                         inv.net,
                                         inv.notes,
                                         inv.paid,
                                         PartType =
                                         (inv.part_type == (byte)Master.Part_Type.Customer) ? "عميل" :
                                         (inv.part_type == (byte)Master.Part_Type.Vendor) ? "مورد" : "Undefinded",
                                         inv.remaing,
                                         inv.tax_value,
                                         inv.total,
                                         Products_Count = db.Invoice_Details.Where(x => x.invoice_id == inv.ID).Count(),
                                         Products =
                                         (
                                         from de in db.Invoice_Details.Where(x => x.invoice_id == inv.ID)
                                         from p in db.Products.Where(x => x.ID == de.item_id)
                                         from un in db.Units_names.Where(x => x.ID == de.item_unit_id).DefaultIfEmpty()
                                         select new
                                         {
                                             ProductName = p.name,
                                             UnitName = un.name,
                                             de.item_qty,
                                             de.price,
                                             de.total_price,
                                         }).ToList()
                                     }).ToList();
                RPT_Invoice.Print(Order_Invoice);
            }
            base.Print();
        }
        public override void Delete()
        {
            if (AskForDeletion())
            {
                using (var db = new Pro_SallesDataContext())
                {
                    db.Store_Logs.DeleteAllOnSubmit
                        (db.Store_Logs.Where(x => x.source_type == (byte)Type &&
                        db.Invoice_Details.Where(d => d.invoice_id == Invoice.ID).Select(d => d.ID).Contains(x.source_id)));
                    db.SubmitChanges();

                    db.Invoice_Details.DeleteAllOnSubmit(db.Invoice_Details.Where(x => x.invoice_id == Invoice.ID));
                    db.SubmitChanges();

                    db.Invoice_Headers.Attach(Invoice);
                    db.Invoice_Headers.DeleteOnSubmit(Invoice);
                    db.SubmitChanges();
                    btn_new.PerformClick();
                }
                base.Delete();
            }
        }
        void ReadUserSettings()
        {
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    break;
                case Master.Invoice_Type.Salles:
                    spin_paid.Enabled = Sessions.UserSettings.Salles.Can_Change_Paid_In_Salles;
                    checkbox_posted_to_store.Enabled = Sessions.UserSettings.Salles.Can_Not_Post_To_Store_In_Salles;
                    gridView1.Columns[nameof(v.price)].OptionsColumn.AllowEdit = Sessions.UserSettings.Salles.Can_Change_Item_Price_In_Salles;
                    gridView1.Columns[nameof(v.item_qty)].OptionsColumn.AllowEdit = Sessions.UserSettings.Salles.Can_Change_QTY_In_Salles;

                    gridView1.Columns[nameof(v.cost_value)].Visible =
                    gridView1.Columns[nameof(v.cost_value)].OptionsColumn.ShowInCustomizationForm =
                    gridView1.Columns[nameof(v.total_cost_value)].OptionsColumn.ShowInCustomizationForm =
                    gridView1.Columns[nameof(v.total_cost_value)].Visible = !Sessions.UserSettings.Salles.Hide_Cost_In_Salles;


                    look_part_type.Enabled = Sessions.UserSettings.Salles.Can_Sell_To_Vendors;
                    date_date.Enabled = Sessions.UserSettings.Salles.Can_Change_Salles_Invoice_Date;
                    break;
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Salles_Return:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}