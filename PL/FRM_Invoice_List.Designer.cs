namespace Pro_Salles.PL
{
    partial class FRM_Invoice_List
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions3 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions buttonImageOptions4 = new DevExpress.XtraEditors.ButtonsPanelControl.ButtonImageOptions();
            this.flyoutPanel1 = new DevExpress.Utils.FlyoutPanel();
            this.flyoutPanelControl1 = new DevExpress.Utils.FlyoutPanelControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.look_drawer = new DevExpress.XtraEditors.LookUpEdit();
            this.look_branch = new DevExpress.XtraEditors.LookUpEdit();
            this.look_grid_part_id = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.look_part_type = new DevExpress.XtraEditors.LookUpEdit();
            this.date_from = new DevExpress.XtraEditors.DateEdit();
            this.date_to = new DevExpress.XtraEditors.DateEdit();
            this.btn_apply = new DevExpress.XtraEditors.SimpleButton();
            this.btn_clear_filters = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.item = new DevExpress.XtraLayout.LayoutControlItem();
            this.lyc_part = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).BeginInit();
            this.flyoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanelControl1)).BeginInit();
            this.flyoutPanelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.look_drawer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_branch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_grid_part_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_part_type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_from.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_from.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_to.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_to.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_part)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // flyoutPanel1
            // 
            this.flyoutPanel1.Controls.Add(this.flyoutPanelControl1);
            this.flyoutPanel1.Location = new System.Drawing.Point(390, 167);
            this.flyoutPanel1.Name = "flyoutPanel1";
            this.flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            this.flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            this.flyoutPanel1.Options.CloseOnOuterClick = true;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.flyoutPanel1.OptionsButtonPanel.ButtonPanelLocation = DevExpress.Utils.FlyoutPanelButtonPanelLocation.Bottom;
            buttonImageOptions3.Image = global::Pro_Salles.Properties.Resources.apply_32x32;
            buttonImageOptions4.Image = global::Pro_Salles.Properties.Resources.delete_16x16;
            this.flyoutPanel1.OptionsButtonPanel.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.Utils.PeekFormButton("تطبيق", true, buttonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, "Apply", -1, false),
            new DevExpress.Utils.PeekFormButton("مسح الفلاتر", true, buttonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, "Clear", -1, false)});
            this.flyoutPanel1.OwnerControl = this.gridControl1;
            this.flyoutPanel1.Size = new System.Drawing.Size(325, 243);
            this.flyoutPanel1.TabIndex = 5;
            // 
            // flyoutPanelControl1
            // 
            this.flyoutPanelControl1.Controls.Add(this.layoutControl1);
            this.flyoutPanelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flyoutPanelControl1.FlyoutPanel = this.flyoutPanel1;
            this.flyoutPanelControl1.Location = new System.Drawing.Point(0, 0);
            this.flyoutPanelControl1.Name = "flyoutPanelControl1";
            this.flyoutPanelControl1.Size = new System.Drawing.Size(325, 243);
            this.flyoutPanelControl1.TabIndex = 0;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.look_drawer);
            this.layoutControl1.Controls.Add(this.look_branch);
            this.layoutControl1.Controls.Add(this.look_grid_part_id);
            this.layoutControl1.Controls.Add(this.look_part_type);
            this.layoutControl1.Controls.Add(this.date_from);
            this.layoutControl1.Controls.Add(this.date_to);
            this.layoutControl1.Controls.Add(this.btn_apply);
            this.layoutControl1.Controls.Add(this.btn_clear_filters);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(321, 239);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // look_drawer
            // 
            this.look_drawer.Location = new System.Drawing.Point(13, 154);
            this.look_drawer.Name = "look_drawer";
            this.look_drawer.Properties.Appearance.Options.UseTextOptions = true;
            this.look_drawer.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_drawer.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_drawer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_drawer.Properties.NullText = "";
            this.look_drawer.Size = new System.Drawing.Size(226, 20);
            this.look_drawer.StyleController = this.layoutControl1;
            this.look_drawer.TabIndex = 11;
            // 
            // look_branch
            // 
            this.look_branch.Location = new System.Drawing.Point(13, 130);
            this.look_branch.Name = "look_branch";
            this.look_branch.Properties.Appearance.Options.UseTextOptions = true;
            this.look_branch.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_branch.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_branch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_branch.Properties.NullText = "";
            this.look_branch.Size = new System.Drawing.Size(226, 20);
            this.look_branch.StyleController = this.layoutControl1;
            this.look_branch.TabIndex = 10;
            // 
            // look_grid_part_id
            // 
            this.look_grid_part_id.Location = new System.Drawing.Point(13, 106);
            this.look_grid_part_id.Name = "look_grid_part_id";
            this.look_grid_part_id.Properties.Appearance.Options.UseTextOptions = true;
            this.look_grid_part_id.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_grid_part_id.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_grid_part_id.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_grid_part_id.Properties.NullText = "";
            this.look_grid_part_id.Properties.PopupView = this.gridLookUpEdit1View;
            this.look_grid_part_id.Size = new System.Drawing.Size(226, 20);
            this.look_grid_part_id.StyleController = this.layoutControl1;
            this.look_grid_part_id.TabIndex = 9;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // look_part_type
            // 
            this.look_part_type.Location = new System.Drawing.Point(13, 82);
            this.look_part_type.Name = "look_part_type";
            this.look_part_type.Properties.Appearance.Options.UseTextOptions = true;
            this.look_part_type.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_part_type.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_part_type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_part_type.Properties.NullText = "";
            this.look_part_type.Size = new System.Drawing.Size(226, 20);
            this.look_part_type.StyleController = this.layoutControl1;
            this.look_part_type.TabIndex = 8;
            // 
            // date_from
            // 
            this.date_from.EditValue = null;
            this.date_from.Location = new System.Drawing.Point(13, 34);
            this.date_from.Name = "date_from";
            this.date_from.Properties.Appearance.Options.UseTextOptions = true;
            this.date_from.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.date_from.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.date_from.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_from.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_from.Size = new System.Drawing.Size(226, 20);
            this.date_from.StyleController = this.layoutControl1;
            this.date_from.TabIndex = 4;
            // 
            // date_to
            // 
            this.date_to.EditValue = null;
            this.date_to.Location = new System.Drawing.Point(13, 58);
            this.date_to.Name = "date_to";
            this.date_to.Properties.Appearance.Options.UseTextOptions = true;
            this.date_to.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.date_to.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.date_to.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_to.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.date_to.Size = new System.Drawing.Size(226, 20);
            this.date_to.StyleController = this.layoutControl1;
            this.date_to.TabIndex = 5;
            // 
            // btn_apply
            // 
            this.btn_apply.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btn_apply.Appearance.Options.UseFont = true;
            this.btn_apply.ImageOptions.Image = global::Pro_Salles.Properties.Resources.apply_16x16;
            this.btn_apply.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_apply.Location = new System.Drawing.Point(162, 210);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(152, 22);
            this.btn_apply.StyleController = this.layoutControl1;
            this.btn_apply.TabIndex = 6;
            this.btn_apply.Text = "تطبيق  ";
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_clear_filters
            // 
            this.btn_clear_filters.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btn_clear_filters.Appearance.Options.UseFont = true;
            this.btn_clear_filters.ImageOptions.Image = global::Pro_Salles.Properties.Resources.cancel_16x16;
            this.btn_clear_filters.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btn_clear_filters.Location = new System.Drawing.Point(7, 210);
            this.btn_clear_filters.Name = "btn_clear_filters";
            this.btn_clear_filters.Size = new System.Drawing.Size(151, 22);
            this.btn_clear_filters.StyleController = this.layoutControl1;
            this.btn_clear_filters.TabIndex = 7;
            this.btn_clear_filters.Text = "مسح الفلاتر  ";
            this.btn_clear_filters.Click += new System.EventHandler(this.btn_clear_filters_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem4,
            this.layoutControlItem3,
            this.simpleSeparator1,
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.Root.Size = new System.Drawing.Size(321, 239);
            this.Root.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 177);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(311, 25);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_clear_filters;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 203);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(155, 26);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btn_apply;
            this.layoutControlItem3.Location = new System.Drawing.Point(155, 203);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(156, 26);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.Location = new System.Drawing.Point(0, 202);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(311, 1);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            this.layoutControlGroup1.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.item,
            this.lyc_part,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup1.Size = new System.Drawing.Size(311, 177);
            this.layoutControlGroup1.Text = "الفلاتر";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.date_from;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(299, 24);
            this.layoutControlItem1.Text = "من تاريخ";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(66, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.date_to;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(299, 24);
            this.layoutControlItem2.Text = "حتي تاريخ";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(66, 13);
            // 
            // item
            // 
            this.item.Control = this.look_part_type;
            this.item.Location = new System.Drawing.Point(0, 48);
            this.item.Name = "item";
            this.item.Size = new System.Drawing.Size(299, 24);
            this.item.Text = "طرف التعامل";
            this.item.TextSize = new System.Drawing.Size(66, 13);
            // 
            // lyc_part
            // 
            this.lyc_part.Control = this.look_grid_part_id;
            this.lyc_part.Location = new System.Drawing.Point(0, 72);
            this.lyc_part.Name = "lyc_part";
            this.lyc_part.Size = new System.Drawing.Size(299, 24);
            this.lyc_part.Text = "---";
            this.lyc_part.TextSize = new System.Drawing.Size(66, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.look_branch;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(299, 24);
            this.layoutControlItem5.Text = "الفرغ | المخزن";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(66, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.look_drawer;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(299, 24);
            this.layoutControlItem6.Text = "خزنه الدفع";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(66, 13);
            // 
            // FRM_Invoice_List
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 628);
            this.Controls.Add(this.flyoutPanel1);
            this.IconOptions.SvgImage = global::Pro_Salles.Properties.Resources.listviewappointmentpattern;
            this.Name = "FRM_Invoice_List";
            this.Load += new System.EventHandler(this.FRM_Invoice_List_Load);
            this.Controls.SetChildIndex(this.flyoutPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanel1)).EndInit();
            this.flyoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flyoutPanelControl1)).EndInit();
            this.flyoutPanelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.look_drawer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_branch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_grid_part_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_part_type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_from.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_from.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_to.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.date_to.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lyc_part)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.FlyoutPanel flyoutPanel1;
        private DevExpress.Utils.FlyoutPanelControl flyoutPanelControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit date_from;
        private DevExpress.XtraEditors.DateEdit date_to;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton btn_apply;
        private DevExpress.XtraEditors.SimpleButton btn_clear_filters;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.SimpleSeparator simpleSeparator1;
        private DevExpress.XtraEditors.LookUpEdit look_part_type;
        private DevExpress.XtraLayout.LayoutControlItem item;
        private DevExpress.XtraEditors.GridLookUpEdit look_grid_part_id;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lyc_part;
        private DevExpress.XtraEditors.LookUpEdit look_branch;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.LookUpEdit look_drawer;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
    }
}