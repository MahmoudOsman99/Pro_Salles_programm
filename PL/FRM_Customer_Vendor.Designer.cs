namespace Pro_Salles.PL
{
    partial class FRM_Customer_Vendor
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.spin_maxCredit = new DevExpress.XtraEditors.SpinEdit();
            this.txt_account_id = new DevExpress.XtraEditors.TextEdit();
            this.txt_address = new DevExpress.XtraEditors.TextEdit();
            this.txt_mobile = new DevExpress.XtraEditors.TextEdit();
            this.txt_phone = new DevExpress.XtraEditors.TextEdit();
            this.txt_name = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.spin = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spin_maxCredit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_account_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_address.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_mobile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_phone.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.spin_maxCredit);
            this.layoutControl1.Controls.Add(this.txt_account_id);
            this.layoutControl1.Controls.Add(this.txt_address);
            this.layoutControl1.Controls.Add(this.txt_mobile);
            this.layoutControl1.Controls.Add(this.txt_phone);
            this.layoutControl1.Controls.Add(this.txt_name);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(510, 169);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // spin_maxCredit
            // 
            this.spin_maxCredit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spin_maxCredit.Location = new System.Drawing.Point(185, 132);
            this.spin_maxCredit.Name = "spin_maxCredit";
            this.spin_maxCredit.Properties.Appearance.Options.UseTextOptions = true;
            this.spin_maxCredit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.spin_maxCredit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.spin_maxCredit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spin_maxCredit.Size = new System.Drawing.Size(253, 20);
            this.spin_maxCredit.StyleController = this.layoutControl1;
            this.spin_maxCredit.TabIndex = 5;
            // 
            // txt_account_id
            // 
            this.txt_account_id.Location = new System.Drawing.Point(185, 108);
            this.txt_account_id.Name = "txt_account_id";
            this.txt_account_id.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_account_id.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_account_id.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_account_id.Properties.ReadOnly = true;
            this.txt_account_id.Size = new System.Drawing.Size(253, 20);
            this.txt_account_id.StyleController = this.layoutControl1;
            this.txt_account_id.TabIndex = 4;
            // 
            // txt_address
            // 
            this.txt_address.Location = new System.Drawing.Point(12, 84);
            this.txt_address.Name = "txt_address";
            this.txt_address.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_address.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_address.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_address.Size = new System.Drawing.Size(426, 20);
            this.txt_address.StyleController = this.layoutControl1;
            this.txt_address.TabIndex = 4;
            // 
            // txt_mobile
            // 
            this.txt_mobile.Location = new System.Drawing.Point(12, 60);
            this.txt_mobile.Name = "txt_mobile";
            this.txt_mobile.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_mobile.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_mobile.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_mobile.Size = new System.Drawing.Size(426, 20);
            this.txt_mobile.StyleController = this.layoutControl1;
            this.txt_mobile.TabIndex = 4;
            // 
            // txt_phone
            // 
            this.txt_phone.Location = new System.Drawing.Point(12, 36);
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_phone.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_phone.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_phone.Size = new System.Drawing.Size(426, 20);
            this.txt_phone.StyleController = this.layoutControl1;
            this.txt_phone.TabIndex = 4;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(12, 12);
            this.txt_name.Name = "txt_name";
            this.txt_name.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_name.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_name.Size = new System.Drawing.Size(426, 20);
            this.txt_name.StyleController = this.layoutControl1;
            this.txt_name.TabIndex = 4;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.spin,
            this.emptySpaceItem1,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(510, 169);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txt_name;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(490, 24);
            this.layoutControlItem1.Text = "الأسم";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(57, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_phone;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(490, 24);
            this.layoutControlItem2.Text = "الهاتف";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(57, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txt_mobile;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(490, 24);
            this.layoutControlItem3.Text = "الموبايل";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(57, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txt_address;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(490, 24);
            this.layoutControlItem4.Text = "العنوان";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(57, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txt_account_id;
            this.layoutControlItem5.Location = new System.Drawing.Point(173, 96);
            this.layoutControlItem5.MaxSize = new System.Drawing.Size(317, 24);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(317, 24);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(317, 24);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "رقم الحساب";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(57, 13);
            // 
            // spin
            // 
            this.spin.Control = this.spin_maxCredit;
            this.spin.Location = new System.Drawing.Point(173, 120);
            this.spin.MaxSize = new System.Drawing.Size(317, 24);
            this.spin.MinSize = new System.Drawing.Size(317, 24);
            this.spin.Name = "spin";
            this.spin.Size = new System.Drawing.Size(317, 29);
            this.spin.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.spin.Text = "حد الأئتمان";
            this.spin.TextSize = new System.Drawing.Size(57, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 96);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(173, 24);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 120);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(173, 29);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // FRM_Customer_Vendor
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 193);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.LargeImage = global::Pro_Salles.Properties.Resources.customer_32x32;
            this.MaximizeBox = false;
            this.Name = "FRM_Customer_Vendor";
            this.Load += new System.EventHandler(this.FRM_Customer_Vendor_Load);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spin_maxCredit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_account_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_address.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_mobile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_phone.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit txt_name;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.TextEdit txt_account_id;
        private DevExpress.XtraEditors.TextEdit txt_address;
        private DevExpress.XtraEditors.TextEdit txt_mobile;
        private DevExpress.XtraEditors.TextEdit txt_phone;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.SpinEdit spin_maxCredit;
        private DevExpress.XtraLayout.LayoutControlItem spin;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}