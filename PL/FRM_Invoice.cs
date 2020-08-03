using System;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using Pro_Salles.Class;
using Pro_Salles.DAL;
using Pro_Salles.Reporting;
using Pro_Salles.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Pro_Salles.Class.Master_Finance;
using System.Diagnostics;

namespace Pro_Salles.PL
{
    public partial class FRM_Invoice : FRM_Master
    {
        Pro_SallesDataContext generaldb;
        CustomersAndVendor cust;
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
            Set_Form_Type();
            //Refresh_Data();
            New();
        }

        public FRM_Invoice(Master.Invoice_Type _type, int id)
        {
            InitializeComponent();
            Intialize_Events();


            Type = _type;
            Set_Form_Type();
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
                max_code = db.Invoice_Headers.Where(x => x.invoice_type == (int)Type).
                    OrderByDescending(x => x.date).Select(x => x.code ?? "1").FirstOrDefault();
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
                case Master.Invoice_Type.Purchase:
                    Invoice.part_type = (int)Master.Part_Type.Vendor;
                    Invoice.part_id = Sessions.Defaults.Vendor;
                    Invoice.branch = Sessions.Defaults.Raw_Store;
                    break;
                case Master.Invoice_Type.Salles:
                    Invoice.part_type = (int)Master.Part_Type.Customer;
                    Invoice.part_id = Sessions.Defaults.Customer;
                    Invoice.branch = Sessions.Defaults.Store;
                    break;

                case Master.Invoice_Type.Salles_Return:
                    Invoice.branch = Sessions.Defaults.Store;
                    Invoice.part_id = Sessions.Defaults.Customer;
                    break;

                case Master.Invoice_Type.Purchase_Return:
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
        bool IsCodeExists()
        {
            using (var db = new Pro_SallesDataContext())
            {
                var data = db.Invoice_Headers.Where(x => x.ID != Invoice.ID && x.invoice_type == (byte)Type && x.code == txt_code.Text).Count();
                if (data > 0)
                    txt_code.ErrorText = "هذا الكود متكرر";
                return (data > 0);
            }
        }
        public override bool Is_Data_Valide()
        {
            int Number_Of_Erorrs = 0;
            if (gridView1.RowCount == 0)
            {
                Number_Of_Erorrs++;
                XtraMessageBox.Show("برجاء ادخال صنف واحد على الأقل", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.FocusedRowHandle = i;
                GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));/////////////////////
                if (gridView1.HasColumnErrors)
                {
                    Number_Of_Erorrs++;
                    XtraMessageBox.Show("برجاء التحقق من الأخطاء في الجدول", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridView1_ValidateRow(gridView1, new ValidateRowEventArgs(i, gridView1.GetRow(i)));/////////////////////
                    return false;
                }
            }

            Number_Of_Erorrs += txt_code.IsStringValid() ? 0 : 1;
            //لو هي انتجر يبقي ترو و فيها قيما يبقي مش هعد الأخطاء اما لو فوولس يبقي هنضيف ايرور يعني مفيش  داتا او بيانات فيها
            Number_Of_Erorrs += look_part_type.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_branch.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_drower.IsEditValueValid() ? 0 : 1;
            Number_Of_Erorrs += look_grid_part_id.IsEditValueValidAndNotZero() ? 0 : 1;

            //This is diffrent because if it more than zero, it will return true
            Number_Of_Erorrs += IsCodeExists() ? 1 : 0;

            Number_Of_Erorrs += spin_discount_value.IsValueNotLessThanZero() ? 0 : 1;
            Number_Of_Erorrs += spin_expences.IsValueNotLessThanZero() ? 0 : 1;
            Number_Of_Erorrs += spin_paid.IsValueNotLessThanZero() ? 0 : 1;
            Number_Of_Erorrs += spin_tax_value.IsValueNotLessThanZero() ? 0 : 1;

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
                    if (Invoice.discount_ratio != Convert.ToDouble(spin_discount_ratio.EditValue) &&
                        Sessions.UserSettings.Salles.Max_Discount_In_Invoice < Convert.ToDecimal(spin_discount_ratio.EditValue))
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
                                        var DR = XtraMessageBox.Show("لقد تخطي هذا العميل حد الأئتمان..هل تريد المتابعه؟", "تأكيد عمليه البيع",
                                            buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question);
                                        if (DR == DialogResult.No)
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
                    break;
                default:
                    throw new NotImplementedException();

            }
            //هنا هترجع قيمه ترو فقط لو كان عدد الأخطاء بيساوي صفر اما لو اكتر هيرجع فوولس
            return (Number_Of_Erorrs == 0);
        }
        //98
        public override void Get_History()
        {
            if (isNew)
                return;
            var MainScreenID = Screens.Get_Screens.SingleOrDefault(x => x.Screen_Name == this.Name).Screen_ID;
            var listScreen = Screens.Get_Screens.SingleOrDefault(x => x.Screen_Name == "FRM_Invoice_List");
            int listScreenID = MainScreenID;

            //هنا انت عاوز اي دي واحد بس.. ف انت بتاخد الاساسي ولكن لو الاي دي بتاع الليست مش نل يبقي هياخد القيمه بتاعت الليست
            if (listScreen != null)
                listScreenID = listScreen.Screen_ID;

            using (var db = new Pro_SallesDataContext())
            {
                var data = db.User_Logs.Where(x => x.Part_ID == Invoice.ID &&
                (x.Screen_ID == MainScreenID || x.Screen_ID == listScreenID));
                gridControl_action_history.DataSource = (from d in data.OrderByDescending(x => x.Action_Date)
                                                         join u in db.Users on d.User_ID equals u.ID
                                                         select new
                                                         {
                                                             u.Name,
                                                             d.Action_Date,
                                                             Action =
                                                             (d.Action_Type == (byte)Action_Type.Add) ? "اضافه" :
                                                             (d.Action_Type == (byte)Action_Type.Delete) ? "حذف" :
                                                             (d.Action_Type == (byte)Action_Type.Edit) ? "تعديل" :
                                                             (d.Action_Type == (byte)Action_Type.Print) ? "طباعه" : ""
                                                         }).ToList();
            }
            gridView_action_history.Columns["Action_Date"].Caption = "التاريخ";
            gridView_action_history.Columns["Name"].Caption = "المسنخدم";
            gridView_action_history.Columns["Action"].Caption = "الحدث";
            gridView_action_history.Columns["Action_Date"].DisplayFormat.FormatType = FormatType.Custom;
            gridView_action_history.Columns["Action_Date"].DisplayFormat.FormatString = "yyyy/MM/dd hh:mm tt";

            //gridView_action_history.Columns["Action_Date"].OptionsColumn.AllowFocus = false;
            //gridView_action_history.Columns["Name"].OptionsColumn.AllowFocus = false;
            //gridView_action_history.Columns["Action"].OptionsColumn.AllowFocus = false;
            //gridView_action_history.Columns["Action_Date"].OptionsColumn.AllowEdit = false;
            //gridView_action_history.Columns["Name"].OptionsColumn.AllowEdit = false;
            //gridView_action_history.Columns["Action"].OptionsColumn.AllowEdit = false;            

            gridView_action_history.RowStyle += GridView_action_history_RowStyle;
        }

        private void GridView_action_history_RowStyle(object sender, RowStyleEventArgs e)
        {
            var view = sender as GridView;
            var action = view.GetRowCellValue(e.RowHandle, "Action");

            if (action == null) return;

            //switch (action.ToString())
            //{
            //    case "اضافه":
            //        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            //        break;
            //    case "حذف":
            //        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            //        break;
            //    case "تعديل":
            //        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            //        break;
            //    case "طباعه":
            //        e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            //        break;
            //    default:
            //        break;
            //}

            if (action == null) return;
            if (action.ToString() == "اضافه")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            else if (action.ToString() == "حذف")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            else if (action.ToString() == "تعديل")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            else if (action.ToString() == "طباعه")
                e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
        }

        void Set_Form_Type()
        {
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
                    this.Name = Screens.Add_Sales_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Purchase_Return://99
                    this.Text = "   فاتوره مردود مشتروات";
                    this.Name = Screens.Add_Purchase_Return_Invoice.Screen_Name;
                    checkbox_posted_to_store.Enabled = false;
                    checkbox_posted_to_store.Checked = true;
                    lyc_sourceID.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lyc_SelectItemsFromSource.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                case Master.Invoice_Type.Salles_Return://99
                    this.Text = "   فاتوره مردود مبيعات";
                    this.Name = Screens.Add_Sales_Return_Invoice.Screen_Name;
                    checkbox_posted_to_store.Enabled = false;
                    checkbox_posted_to_store.Checked = true;
                    lyc_sourceID.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lyc_SelectItemsFromSource.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    break;
                default:
                    throw new NotImplementedException();
            }

        }
        /// <summary>
        /// 101 Here you repited the code or you put the code that need data from gridview but the gridview didn't take its value
        /// </summary>
        void SetUpGridViewByInvoiceType()
        {
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    gridView1.Columns[nameof(v.store_id)].OptionsColumn.AllowFocus = false;
                    break;
                case Master.Invoice_Type.Purchase_Return://99
                    gridView1.Columns[nameof(v.store_id)].OptionsColumn.AllowFocus = false;
                    gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;

                    AddReturnColumns();

                    break;
                case Master.Invoice_Type.Salles_Return://99                    
                    gridView1.Columns[nameof(v.store_id)].OptionsColumn.AllowFocus = false;
                    gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.None;

                    AddReturnColumns();

                    break;
                default:
                    break;
            }
        }
        void AddReturnColumns()
        {
            //101,102
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "CLM_Source_Qty",
                FieldName = "Source_Qty",
                Caption = "الكميه الأساسيه",
                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                VisibleIndex = gridView1.Columns[nameof(v.item_qty)].VisibleIndex - 1,
            });
            gridView1.Columns.Add(new GridColumn()
            {
                Name = "CLM_Other_Qty",
                FieldName = "Other_Qty",
                Caption = "كميات اخري",
                UnboundType = DevExpress.Data.UnboundColumnType.Decimal,
                VisibleIndex = gridView1.Columns[nameof(v.item_qty)].VisibleIndex - 1,
            });

            gridView1.Columns["Other_Qty"].OptionsColumn.AllowEdit =
            gridView1.Columns["Source_Qty"].OptionsColumn.AllowEdit = false;
        }
        private void FRM_Invoice_Load(object sender, EventArgs e)
        {
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;

            SetUpGridViewByInvoiceType();

            btn_print.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            Refresh_Data();


            look_part_type.LookUp_DataSource(Master.Part_Type_List);
            look_part_type.Properties.PopulateColumns();
            look_part_type.Properties.Columns["ID"].Visible = false;
            ///////////////////28   
            look_grid_part_id.ButtonClick += Look_part_id_ButtonClick;
            look_grid_source.EditValueChanged += Look_grid_source_EditValueChanged;

            look_grid_part_id.Properties.ValidateOnEnterKey = true;
            look_grid_part_id.Properties.AllowNullInput = DefaultBoolean.False;//to don't let the user put null values
            look_grid_part_id.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;//To make the size fit to the fields
            look_grid_part_id.Properties.ImmediatePopup = true;
            look_grid_part_id.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Plus));

            var part_idView = look_grid_part_id.Properties.View;
            part_idView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            part_idView.OptionsSelection.UseIndicatorForSelection = true;
            part_idView.OptionsView.ShowAutoFilterRow = true;
            part_idView.PopulateColumns(look_grid_part_id.Properties.DataSource);

            if (look_part_type != null || part_idView != null)
            {
                part_idView.Columns[nameof(cust.Is_Customer)].Visible =
                part_idView.Columns[nameof(cust.account_id)].Visible =
                part_idView.Columns[nameof(cust.ID)].Visible = false;
                part_idView.Columns[nameof(cust.name)].Caption = "الأسم";
                part_idView.Columns[nameof(cust.address)].Caption = "العنوان";
                part_idView.Columns[nameof(cust.phone)].Caption = "الموبايل";
                part_idView.Columns[nameof(cust.mobile)].Caption = "الهاتف";
                part_idView.Columns[nameof(cust.max_Credit)].Caption = "حد الأتمان";
            }
            var st = Sessions.Stores.FirstOrDefault();
            if (look_branch != null)
            {
                look_branch.Properties.PopulateColumns();
                look_branch.Properties.Columns[nameof(st.Cost_Of_Sold_Goods_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Discount_Allowed_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Discount_Received_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Salles_Return_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Salles_Return_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Salles_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.Inventory_Account_ID)].Visible = false;
                look_branch.Properties.Columns[nameof(st.ID)].Caption = "الكود";
                look_branch.Properties.Columns[nameof(st.name)].Caption = "أسم المخزن";
            }

            ////////////////////There is a commint down////////////////
            look_branch.EditValueChanging += Look_branch_EditValueChanging;

            #region RepositoryItem_Properties
            repoUOM.LookUp_DataSource(Sessions.Unit_Names, gridView1.Columns[nameof(v.item_unit_id)], gridControl1);

            repoStores = new RepositoryItemLookUpEdit();
            repoStores.LookUp_DataSource(Sessions.Stores, gridView1.Columns[nameof(v.store_id)], gridControl1, "name", "ID");
            repoStores.PopulateColumns();
            repoStores.Columns[nameof(st.Cost_Of_Sold_Goods_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Discount_Allowed_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Discount_Received_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Salles_Return_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Salles_Return_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Salles_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.Inventory_Account_ID)].Visible = false;
            repoStores.Columns[nameof(st.ID)].Caption = "الكود";
            repoStores.Columns[nameof(st.name)].Caption = "أسم المخزن";


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
            gridView1.Columns[nameof(v.store_id)].VisibleIndex = 4;
            gridView1.Columns[nameof(v.item_qty)].VisibleIndex = 5;
            gridView1.Columns[nameof(v.price)].VisibleIndex = 6;
            gridView1.Columns[nameof(v.discount)].VisibleIndex = 7;
            gridView1.Columns[nameof(v.discount_value)].VisibleIndex = 8;
            gridView1.Columns[nameof(v.total_price)].VisibleIndex = 9;
            gridView1.Columns[nameof(v.cost_value)].VisibleIndex = 10;
            gridView1.Columns[nameof(v.total_cost_value)].VisibleIndex = 11;
            gridView1.Columns["Balance"].VisibleIndex = 12;

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
                Caption = "حذف",
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
            if (Debugger.IsAttached == false)
            {
                gridView1.RestoreLayOut(this.Name);
                layoutControl1.RestoreLayOut(this.Name);
            }

            //90
            btn_customize_Layout.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btn_customize_Layout.ItemClick += (ss, ee) => { layoutControl1.ShowCustomizationForm(); };
            layoutControl1.CustomizationMode = DevExpress.XtraLayout.CustomizationModes.Quick;
            layoutControl1.OptionsCustomizationForm.DefaultPage = DevExpress.XtraLayout.CustomizationPage.LayoutTreeView;
            layoutControl1.OptionsCustomizationForm.ShowSaveButton =
            layoutControl1.OptionsCustomizationForm.ShowLoadButton = false;
            //layoutControl1.OptionsCustomizationForm.ShowPropertyGrid = true;
            foreach (BaseLayoutItem item in layoutControl1.Items)
                item.AllowHide = false;

            this.FormClosing += FRM_Invoice_FormClosing;
        }

        private void Look_grid_source_EditValueChanged(object sender, EventArgs e)
        {
            if (look_grid_source.EditValue is int sourceID && sourceID > 0)
            {
                using (var db = new Pro_SallesDataContext())
                {
                    ReturnSourceDetails = db.Invoice_Details.Where(x => x.invoice_id == sourceID).ToList();
                    Invoice_Header sourceInvoice = db.Invoice_Headers.Single(x => x.ID == sourceID);
                    spin_discount_ratio.EditValue = sourceInvoice.discount_ratio;
                    spin_tax_ratio.EditValue = sourceInvoice.tax;
                    btn_selectSourceItems.PerformClick();
                }
            }
        }

        private void FRM_Invoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridView1.SaveLayOut(this.Name);
            layoutControl1.SaveLayOut(this.Name);
        }

        AccountBalance accountBalance;
        private void Look_grid_part_id_EditValueChanged(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(look_grid_part_id.EditValue);

            if (id != 0)
            {
                CustomersAndVendor custAcc;
                if (Convert.ToByte(look_part_type.EditValue) == (byte)Master.Part_Type.Vendor)
                {
                    custAcc = Sessions.Vendors.SingleOrDefault(x => x.ID == id);
                }
                else
                {
                    custAcc = Sessions.Customers.SingleOrDefault(x => x.ID == id);
                }

                if (custAcc == null)
                    goto IfEmpty;

                txt_part_address.Text = custAcc.address;
                spin_part_maxCredit.EditValue = Convert.ToDouble(custAcc.max_Credit);
                txt_part_phone.Text = custAcc.phone;
                accountBalance = GetAccountBalance(custAcc.account_id);
                txt_part_balance.Text = accountBalance.Balance;

                //104
                if (Type == Master.Invoice_Type.Salles_Return || Type == Master.Invoice_Type.Purchase_Return)
                {
                    using (var db = new Pro_SallesDataContext())
                    {
                        var sourceInvoices = db.Invoice_Headers.Where(x =>
                        x.invoice_type == ((Type == Master.Invoice_Type.Salles_Return) ? (byte)Master.Invoice_Type.Salles
                        : (byte)Master.Invoice_Type.Purchase)
                        && x.part_id == id
                        && x.part_type == Convert.ToByte(look_part_type.EditValue)).Select(x => new { x.ID, x.code }).ToList();
                        look_grid_source.InitializeData(sourceInvoices, "code", "ID");
                    }
                }
                return;//To don't go to the goto marker after executing the codes
            }
            else
                goto IfEmpty;

            IfEmpty:
            txt_part_address.Text = "";
            spin_part_maxCredit.EditValue = 0;
            txt_part_phone.Text = "";
            txt_part_balance.Text = "";
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
                var row = Rows.FirstOrDefault(x => x.item_id == lastRow.item_id && x.item_unit_id == lastRow.item_unit_id
                && x.store_id == lastRow.store_id && x.price == lastRow.price  //94
                && x != lastRow);
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
                                    var DR = XtraMessageBox.Show("سعر البيع اقل من سعر التكلفه..هل تريد المتابعه؟", "تأكيد عمليه البيع",
                                        buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question);
                                    if (DR == DialogResult.No)
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
                case Master.Invoice_Type.Salles_Return:
                case Master.Invoice_Type.Purchase_Return:
                    //validate that qty isn't more than the qty in the source
                    double? Other_Qty = gridView1.GetRowCellValue(e.RowHandle, "Other_Qty") as double?;
                    double? Source_Qty = gridView1.GetRowCellValue(e.RowHandle, "Source_Qty") as double?;
                    //لو الكميه اللي تم ارجعها من فواتير تانيه مطروحه من الكميه الأساسيه لو ناتج الطرح ده اصغر من الكميه المدخله يبقي تعمل الاتي
                    if (row.item_qty > Convert.ToDouble(((Source_Qty ?? 0) - (Other_Qty ?? 0))))
                    {
                        e.Valid = false;
                        gridView1.SetColumnError(gridView1.Columns[nameof(v.item_qty)], "لا يمكن للكميه المرتجعه ان تكون اكبر من الكميه المتاحه من المصدر");
                    }
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
            if (row.item_id == 0)
                return;

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
                        case Master.Invoice_Type.Purchase_Return://103

                            var returnSourceRow = ReturnSourceDetails.Where(x => x.ID == row.SourceRowID).SingleOrDefault();
                            if (returnSourceRow != null)
                            {
                                row.price = returnSourceRow.price;
                            }

                            break;
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

                    goto case nameof(v.discount_value);


                case nameof(v.discount_value):
                    if (gridView1.FocusedColumn.FieldName == nameof(v.discount_value))
                        row.discount = row.discount_value / (row.item_qty * row.price);
                    row.total_price = (row.item_qty * row.price) - row.discount_value;


                    goto case nameof(v.store_id);


                //////////////////94
                case nameof(v.store_id):
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

                            var returnSourceRow = ReturnSourceDetails.Where(x => x.ID == row.SourceRowID).SingleOrDefault();
                            if (returnSourceRow != null)
                            {
                                row.cost_value = returnSourceRow.cost_value;
                                row.total_cost_value = row.cost_value * row.item_qty;
                            }

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;


                default:
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
                repo.LookUp_DataSource(item.Units, null, null, "Unit_Name", "Unit_ID");
                repo.Columns.Clear();
                repo.Columns.Add(new LookUpColumnInfo("Unit_Name"));
                repo.ShowHeader = false;
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
            else if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control)
            {
                if (view.FocusedRowHandle >= 0)
                    view.DeleteSelectedRows();
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
                var rowIndex = e.ListSourceRowIndex;
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

                //هنا انت بتختار عدد السطور اللي انت عايزه سواء اول خمسه او غيره ...هنا لو واقف عند روو 2 يبقي هياخد اول سطرين
                var Rows = (gridView1.DataSource as Collection<Invoice_Detail>).Take(rowIndex).Where
                    (x => x.item_id == row.item_id && x.store_id == row.store_id);

                //To calculate the balance after any process 
                double otherBalance = 0;

                foreach (var item in Rows)
                {
                    //understand
                    var otherFactor = product.Units.Single(x => x.Unit_ID == item.item_unit_id).Factor;
                    otherBalance += item.item_qty * otherFactor;
                }

                e.Value = (balance - otherBalance) / factor;

            }

            else if (e.Column.FieldName == "Source_Qty")
            {
                var row = e.Row as Invoice_Detail;
                if (row == null) return;

                if (e.IsGetData)
                {
                    var sourceRow = ReturnSourceDetails.SingleOrDefault(x => x.ID == row.SourceRowID);
                    if (sourceRow != null)
                        e.Value = sourceRow.item_qty;
                    else
                        e.Value = 0;
                }
            }
            //103
            else if (e.Column.FieldName == "Other_Qty")
            {
                var row = e.Row as Invoice_Detail;
                if (row == null) return;

                if (e.IsGetData)
                {
                    var db = new Pro_SallesDataContext();
                    var otherReturnRows = db.Invoice_Details.Where(x => x.SourceRowID == row.SourceRowID).Sum(x => (double?)x.item_qty) ?? 0;
                    e.Value = otherReturnRows;
                }
            }
        }

        List<Invoice_Detail> ReturnSourceDetails = new List<Invoice_Detail>();
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

            Spin_tax_value_EditValueChanged(sender, e);
            Spin_discount_value_EditValueChanged(sender, e);

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
                //88
                var isCustomer = Convert.ToInt32(look_part_type.EditValue) == (int)Master.Part_Type.Customer;
                var newId = FRM_Customer_Vendor.AddNew(isCustomer);
                if (newId != 0)
                    look_grid_part_id.EditValue = newId;
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
                    look_grid_part_id.InitializeData(Sessions.Customers);
                else if (parttype == (int)Master.Part_Type.Vendor)
                    look_grid_part_id.InitializeData(Sessions.Vendors);
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

            Part_ID = Invoice.ID;
            Part_Name = Invoice.code + " - " + look_grid_part_id.Text;

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
                    break;
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
                    StoreAccount = store_journal.Inventory_Account_ID;
                    TaxAccount = Sessions.Defaults.Purchase_Tax;
                    DiscountAccount = Sessions.Defaults.Discount_Received_Account;
                    IsPartCredit = true;
                    Is_In = false;
                    InsertCostOsSoldGoodsJournal = false;
                    msg = string.Format("فاتوره مردود مشتروات رقم {0} : {1}", Invoice.ID, look_grid_part_id.Text);
                    break;
                case Master.Invoice_Type.Salles_Return:
                    StoreAccount = store_journal.Salles_Return_Account_ID;
                    TaxAccount = Sessions.Defaults.Salles_Tax;
                    DiscountAccount = Sessions.Defaults.Discount_Allowed_Account;
                    IsPartCredit = true;
                    Is_In = true;
                    InsertCostOsSoldGoodsJournal = true;
                    msg = string.Format("فاتوره مردود مبيعات رقم {0} : {1}", Invoice.ID, look_grid_part_id.Text);
                    break;
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

                //86
                if (items.Where(x => x.store_id != Invoice.branch).Count() > 0)
                {
                    //The word distinct chooses the diffrent stores oly, and remove the repititev
                    var otherStores = items.Where(x => x.store_id != Invoice.branch).Select(s => s.store_id).Distinct();

                    foreach (var otherStore in otherStores)
                    {

                        var Cost = items.Where(x => x.store_id == otherStore).Sum(x => x.total_cost_value);

                        db.Journals.InsertOnSubmit(new Journal() // CostOfSolds
                        {
                            Account_ID = store_journal.Inventory_Account_ID,
                            Code = 54545,
                            Credit = (IsPartCredit) ? Cost : 0,
                            Debit = (!IsPartCredit) ? Cost : 0,
                            Insert_Date = Invoice.date,
                            Notes = msg = " - نقل البضاعه للبيع",
                            Source_ID = Invoice.ID,
                            Source_Type = (byte)Type
                        });
                        db.Journals.InsertOnSubmit(new Journal() // CostOfSolds
                        {
                            Account_ID = db.Stores.Single(x => x.ID == otherStore).Inventory_Account_ID,
                            Code = 54545,
                            Credit = (!IsPartCredit) ? Cost : 0,
                            Debit = (IsPartCredit) ? Cost : 0,
                            Insert_Date = Invoice.date,
                            Notes = msg = " - نقل البضاعه للبيع",
                            Source_ID = Invoice.ID,
                            Source_Type = (byte)Type
                        });
                    }
                }

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

            //100
            DeleteInvoiceStoreLogDetailes(Invoice.ID, (byte)Type);

            generaldb.SubmitChanges();
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

            Part_ID = Invoice.ID;
            Part_Name = Invoice.code + " - " + look_grid_part_id.Text;

            base.Save();

            var forms = Application.OpenForms.Cast<Form>().Where(x => x.Name == this.Name + "_List");
            foreach (var frm in forms)
            {
                if (frm != null && frm is FRM_Master_List)
                    ((FRM_Master_List)frm).Refresh_Data();
            }
        }
        /// <summary>
        /// It deletes the details for spicified store
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <param name="type"></param>
        public static void DeleteInvoiceStoreLogDetailes(int invoiceID, byte type)
        {
            using (var db = new Pro_SallesDataContext())
            {
                //100
                db.Store_Logs.DeleteAllOnSubmit
                    (db.Store_Logs.Where(x => x.source_type == type &&
                db.Invoice_Details.Where(i => i.invoice_id == invoiceID).Select(d => d.ID).Contains(x.source_id)));
                db.SubmitChanges();
            }
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
            Print(Invoice.ID, this.Type, this.Name);
            //base.Print();// Here you disabled the base.print because when you press print button in the invoice it record the event twice
        }
        public static void Print(int id, Master.Invoice_Type Type, string CallerScreenName)
        {
            Print(new List<int> { id }, Type, CallerScreenName);
        }
        public static void Print(Invoice_Header invoice, Master.Invoice_Type Type, string CallerScreenName)
        {
            Print(invoice.ID, Type, CallerScreenName);
        }
        public static void Print(List<int> ids, Master.Invoice_Type Type, string CallerScreenName)
        {
            var name = "";
            switch (Type)
            {
                case Master.Invoice_Type.Purchase:
                    name = Screens.Add_Purchase_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Salles:
                    name = Screens.Add_Sales_Invoice.Screen_Name;
                    break;
                case Master.Invoice_Type.Purchase_Return:
                case Master.Invoice_Type.Salles_Return:
                default:
                    throw new NotImplementedException();
            }

            if (CheckActionAuthorization(name, Master.Actions.Print) == false)
                return;

            /////////49////////
            using (var db = new Pro_SallesDataContext())
            {
                var Order_Invoice = (from inv in db.Invoice_Headers
                                     join str in db.Stores on inv.branch equals str.ID
                                     from part in db.CustomersAndVendors.Where(x => x.ID == inv.part_id).DefaultIfEmpty()
                                     from dr in db.Drowers.Where(x => x.ID == inv.drower).DefaultIfEmpty()
                                     where ids.Contains(inv.ID)
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
                //97
                Order_Invoice.ForEach(x =>
                {
                    FRM_Master.Insert_User_Action(Action_Type.Print, x.ID, x.code + " - " + x.PartName, CallerScreenName);
                });
                RPT_Invoice.Print(Order_Invoice);
            }
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
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void checkbox_posted_to_store_CheckedChanged(object sender, EventArgs e)
        {
            lyc_post_date.Visibility = checkbox_posted_to_store.Checked ?
                DevExpress.XtraLayout.Utils.LayoutVisibility.Never : DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
        }

        private void btn_selectSourceItems_Click(object sender, EventArgs e)
        {
            //106
            XtraForm frm = new XtraForm
            {
                Size = new Size(this.Width - 130, this.Height - 130),
                StartPosition = FormStartPosition.CenterScreen,
                Name = "selectSourceItems",
                Text = "  أختيار أصناف الإرجاع",
                RightToLeft = RightToLeft.Yes,
                RightToLeftLayout = true
            };

            frm.IconOptions.ShowIcon = false;
            frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            frm.MaximizeBox = false;

            GridControl gridControl = new GridControl();
            gridControl.Dock = DockStyle.Fill;

            GridView View = new GridView();
            gridControl.MainView = View;
            //View.OptionsBehavior.Editable = false;
            View.OptionsView.ShowGroupPanel = false;
            View.OptionsCustomization.AllowColumnMoving = false;
            View.OptionsCustomization.AllowQuickHideColumns = false;
            View.OptionsSelection.MultiSelect = true;
            View.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            View.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Center;
            View.Appearance.Row.TextOptions.VAlignment = VertAlignment.Center;
            View.OptionsBehavior.AutoPopulateColumns = false;
            View.OptionsView.ShowIndicator = false;
            View.OptionsCustomization.AllowSort = false;

            var source = gridView1.DataSource as Collection<Invoice_Detail>;
            //107 What will happen after i select or check any row or item  from the customized form
            View.SelectionChanged += (viewSender, changeEvent) =>
            {
                //
                var selectedRow = ReturnSourceDetails[changeEvent.ControllerRow];
                if (source == null)
                    return;

                if (changeEvent.Action == System.ComponentModel.CollectionChangeAction.Add)
                {
                    if (source.Where(x => x.SourceRowID == selectedRow.ID).Count() == 0)
                    {
                        //Here you can't provide the complite row to the source directllay, you should select what you want and provide the selected items to the 
                        //source or InvoiceDetails by add Method
                        source.Add(new Invoice_Detail()
                        {
                            SourceRowID = selectedRow.ID,
                            cost_value = selectedRow.cost_value,
                            item_id = selectedRow.item_id,
                            item_unit_id = selectedRow.item_unit_id,
                            item_qty = selectedRow.item_qty,
                            discount_value = selectedRow.discount_value,
                            price = selectedRow.price,
                            discount = selectedRow.discount,
                            store_id = (look_branch.EditValue is int storeID) ? storeID : selectedRow.store_id,
                            total_cost_value = selectedRow.total_cost_value,
                            total_price = selectedRow.total_price
                        });
                    }
                }
                else if (changeEvent.Action == System.ComponentModel.CollectionChangeAction.Remove)
                {
                    if (source.Where(x => x.SourceRowID == selectedRow.ID).Count() > 0)
                    {
                        source.Remove(source.Single(x => x.SourceRowID == selectedRow.ID));
                    }
                }
            };


            Invoice_Detail ins;
            View.Columns.AddField(nameof(ins.ID));

            gridControl.RepositoryItems.AddRange(new RepositoryItem[] { repoUOM, repoAll });

            var productColumn = View.Columns.AddField(nameof(ins.item_id));
            productColumn.Visible = true;
            productColumn.Caption = "الصنف";

            var unitColumn = View.Columns.AddField(nameof(ins.item_unit_id));
            unitColumn.Visible = true;
            unitColumn.Caption = "الوحده";

            var itemQtyColumn = View.Columns.AddField(nameof(ins.item_qty));
            itemQtyColumn.Caption = "الكميه";
            itemQtyColumn.Visible = true;

            var repoMyItems = new RepositoryItemLookUpEdit();
            var repoMyUMO = new RepositoryItemLookUpEdit();
            repoMyItems.LookUp_DataSource(Sessions.Product_View, productColumn, gridControl, "Name", "ID");
            repoMyUMO.LookUp_DataSource(Sessions.Unit_Names, unitColumn, gridControl);

            gridControl.DataSource = ReturnSourceDetails;
            frm.Controls.Add(gridControl);

            //To force the gridcontrol finish its initialization to take its (propeties || (something else) )
            gridControl.ForceInitialize();
            if (source != null)
            {
                for (int i = 0; i < ReturnSourceDetails.Count(); i++)
                {
                    if (source.Where(x => x.SourceRowID == ReturnSourceDetails[i].ID).Count() > 0)
                        View.SelectRow(i);
                }
            }
            frm.ShowDialog();
        }
    }
}