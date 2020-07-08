namespace Pro_Salles.PL
{
    partial class FRM_User
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
            this.look_screen_profile_ID = new DevExpress.XtraEditors.LookUpEdit();
            this.toggle_is_active = new DevExpress.XtraEditors.ToggleSwitch();
            this.look_settings_profile_ID = new DevExpress.XtraEditors.LookUpEdit();
            this.look_user_type = new DevExpress.XtraEditors.LookUpEdit();
            this.txt_name = new DevExpress.XtraEditors.TextEdit();
            this.txt_user_name = new DevExpress.XtraEditors.TextEdit();
            this.txt_password = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.item = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.look_screen_profile_ID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggle_is_active.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_settings_profile_ID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_user_type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_user_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.item)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.look_screen_profile_ID);
            this.layoutControl1.Controls.Add(this.toggle_is_active);
            this.layoutControl1.Controls.Add(this.look_settings_profile_ID);
            this.layoutControl1.Controls.Add(this.look_user_type);
            this.layoutControl1.Controls.Add(this.txt_name);
            this.layoutControl1.Controls.Add(this.txt_user_name);
            this.layoutControl1.Controls.Add(this.txt_password);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 24);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(373, 170);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // look_screen_profile_ID
            // 
            this.look_screen_profile_ID.Location = new System.Drawing.Point(7, 127);
            this.look_screen_profile_ID.Name = "look_screen_profile_ID";
            this.look_screen_profile_ID.Properties.Appearance.Options.UseTextOptions = true;
            this.look_screen_profile_ID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_screen_profile_ID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_screen_profile_ID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_screen_profile_ID.Properties.NullText = "";
            this.look_screen_profile_ID.Size = new System.Drawing.Size(251, 20);
            this.look_screen_profile_ID.StyleController = this.layoutControl1;
            this.look_screen_profile_ID.TabIndex = 5;
            // 
            // toggle_is_active
            // 
            this.toggle_is_active.Location = new System.Drawing.Point(7, 7);
            this.toggle_is_active.Name = "toggle_is_active";
            this.toggle_is_active.Properties.OffText = "غير نشط";
            this.toggle_is_active.Properties.OnText = "نشط";
            this.toggle_is_active.Size = new System.Drawing.Size(95, 18);
            this.toggle_is_active.StyleController = this.layoutControl1;
            this.toggle_is_active.TabIndex = 6;
            // 
            // look_settings_profile_ID
            // 
            this.look_settings_profile_ID.Location = new System.Drawing.Point(7, 103);
            this.look_settings_profile_ID.Name = "look_settings_profile_ID";
            this.look_settings_profile_ID.Properties.Appearance.Options.UseTextOptions = true;
            this.look_settings_profile_ID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_settings_profile_ID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_settings_profile_ID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_settings_profile_ID.Properties.NullText = "";
            this.look_settings_profile_ID.Size = new System.Drawing.Size(251, 20);
            this.look_settings_profile_ID.StyleController = this.layoutControl1;
            this.look_settings_profile_ID.TabIndex = 4;
            // 
            // look_user_type
            // 
            this.look_user_type.Location = new System.Drawing.Point(7, 79);
            this.look_user_type.Name = "look_user_type";
            this.look_user_type.Properties.Appearance.Options.UseTextOptions = true;
            this.look_user_type.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.look_user_type.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.look_user_type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.look_user_type.Properties.NullText = "";
            this.look_user_type.Size = new System.Drawing.Size(251, 20);
            this.look_user_type.StyleController = this.layoutControl1;
            this.look_user_type.TabIndex = 3;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(106, 7);
            this.txt_name.Name = "txt_name";
            this.txt_name.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_name.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_name.Size = new System.Drawing.Size(152, 20);
            this.txt_name.StyleController = this.layoutControl1;
            this.txt_name.TabIndex = 0;
            // 
            // txt_user_name
            // 
            this.txt_user_name.Location = new System.Drawing.Point(7, 31);
            this.txt_user_name.Name = "txt_user_name";
            this.txt_user_name.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_user_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_user_name.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_user_name.Size = new System.Drawing.Size(251, 20);
            this.txt_user_name.StyleController = this.layoutControl1;
            this.txt_user_name.TabIndex = 1;
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(7, 55);
            this.txt_password.Name = "txt_password";
            this.txt_password.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_password.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txt_password.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txt_password.Properties.UseSystemPasswordChar = true;
            this.txt_password.Size = new System.Drawing.Size(251, 20);
            this.txt_password.StyleController = this.layoutControl1;
            this.txt_password.TabIndex = 2;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.item});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.Root.Size = new System.Drawing.Size(373, 170);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txt_name;
            this.layoutControlItem1.Location = new System.Drawing.Point(99, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(264, 24);
            this.layoutControlItem1.Text = "الأسم";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_user_name;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(363, 24);
            this.layoutControlItem2.Text = "أسم الدخول";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txt_password;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(363, 24);
            this.layoutControlItem3.Text = "الرقم السري";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.look_settings_profile_ID;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(363, 24);
            this.layoutControlItem5.Text = "نموذج الاعدادات";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.toggle_is_active;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(99, 24);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.look_screen_profile_ID;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(363, 40);
            this.layoutControlItem7.Text = "نموذج صلاحيات الوصول";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(105, 13);
            // 
            // item
            // 
            this.item.Control = this.look_user_type;
            this.item.Location = new System.Drawing.Point(0, 72);
            this.item.Name = "item";
            this.item.Size = new System.Drawing.Size(363, 24);
            this.item.Text = "نوع المستخدم";
            this.item.TextSize = new System.Drawing.Size(105, 13);
            // 
            // FRM_User
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 194);
            this.Controls.Add(this.layoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = global::Pro_Salles.Properties.Resources.bouser_32x32;
            this.MaximizeBox = false;
            this.Name = "FRM_User";
            this.Text = "  مستخدم";
            this.Load += new System.EventHandler(this.FRM_User_Load);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.look_screen_profile_ID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggle_is_active.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_settings_profile_ID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.look_user_type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_user_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.item)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.ToggleSwitch toggle_is_active;
        private DevExpress.XtraEditors.LookUpEdit look_settings_profile_ID;
        private DevExpress.XtraEditors.LookUpEdit look_user_type;
        private DevExpress.XtraEditors.TextEdit txt_name;
        private DevExpress.XtraEditors.TextEdit txt_user_name;
        private DevExpress.XtraEditors.TextEdit txt_password;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem item;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.LookUpEdit look_screen_profile_ID;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}