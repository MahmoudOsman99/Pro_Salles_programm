using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Pro_Salles.Class;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraLayout;
using Pro_Salles.DAL;
using DevExpress.Utils.Animation;

namespace Pro_Salles.PL
{
    public partial class FRM_User_Settings_Profile : FRM_Master
    {
        UserSettingsProfile profile;
        List<BaseEdit> editors;
        public FRM_User_Settings_Profile()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
            New();
            Get_Data();
        }
        public FRM_User_Settings_Profile(int id)
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
            using (var db = new Pro_SallesDataContext())
            {
                profile = db.UserSettingsProfiles.Single(x => x.ID == id);
                txt_name.Text = profile.Name;
                Get_Data();
            }
        }

        public override void New()
        {
            profile = new UserSettingsProfile();
            txt_name.Text = profile.Name;
        }
        public override void Get_Data()
        {
            editors = new List<BaseEdit>();
            User_Settings_Template settings = new User_Settings_Template(profile.ID);
            accordionControl1.Elements.Clear();
            xtraTabControl1.TabPages.Clear();
            accordionControl1.AllowItemSelection = true;

            var catalog = settings.GetType().GetProperties();
            foreach (var item in catalog)
            {
                accordionControl1.Elements.Add(new AccordionControlElement()
                {
                    Name = nameof(item),
                    Text = User_Settings_Template.Get_Prop_Caption(item.Name),
                    Style = ElementStyle.Item
                }
                );

                var page = new DevExpress.XtraTab.XtraTabPage()
                {
                    Name = nameof(item),////////There is an edit from item.name
                    Text = User_Settings_Template.Get_Prop_Caption(item.Name),
                };

                xtraTabControl1.TabPages.Add(page);
                LayoutControl lc = new LayoutControl();
                EmptySpaceItem empty1 = new EmptySpaceItem();
                EmptySpaceItem empty2 = new EmptySpaceItem();
                //empty2.SizeConstraintsType = SizeConstraintsType.Custom;
                //empty2.MaxSize = new Size(500, 0);
                //empty2.MinSize = new Size(250, 0);

                lc.AddItem(empty1);
                lc.AddItem(empty2, empty1, DevExpress.XtraLayout.Utils.InsertType.Left);


                var props = item.GetValue(settings).GetType().GetProperties();
                foreach (var prop in props)
                {
                    BaseEdit edit = User_Settings_Template.Get_Property_Control(prop.Name, prop.GetValue(item.GetValue(settings)));
                    if (edit != null)
                    {
                        var layoutItem = lc.AddItem("", edit, empty2, DevExpress.XtraLayout.Utils.InsertType.Top);
                        layoutItem.TextVisible = true;
                        layoutItem.Text = User_Settings_Template.Get_Prop_Caption(prop.Name);
                        layoutItem.SizeConstraintsType = SizeConstraintsType.Custom;
                        layoutItem.MaxSize = new Size(700, 25);
                        layoutItem.MinSize = new Size(250, 25);
                        editors.Add(edit);
                    }
                }

                lc.Dock = DockStyle.Fill;
                page.Controls.Add(lc);
            }

            Part_ID = profile.ID;
            Part_Name = profile.Name;
        }
        public override bool Is_Data_Valide()
        {
            int flag = 0;
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = FRM_Master.Error_Text;
                flag++;
            }
            editors.ForEach(e =>
            {
                if (e.GetType() == typeof(LookUpEdit) && ((LookUpEdit)e).Properties.DataSource.GetType() != typeof(List<Master.Value_And_ID>))
                    flag += ((LookUpEdit)e).IsEditValueValidAndNotZero() ? 0 : 1;
                else if (e.GetType() == typeof(LookUpEdit) && ((LookUpEdit)e).Properties.DataSource.GetType() != typeof(Master.Value_And_ID))
                {

                }
            });
            return (flag == 0);
        }
        public override void Save()
        {
            var db = new Pro_SallesDataContext();
            if (profile.ID == 0)
                db.UserSettingsProfiles.InsertOnSubmit(profile);
            else
                db.UserSettingsProfiles.Attach(profile);

            Set_Data();
            db.SubmitChanges();
            db.UserSettingsProfileProperties.DeleteAllOnSubmit(db.UserSettingsProfileProperties.Where(x => x.Profile_ID == profile.ID));
            db.SubmitChanges();

            editors.ForEach(e =>
            {
                db.UserSettingsProfileProperties.InsertOnSubmit(new UserSettingsProfileProperty()
                {
                    Profile_ID = profile.ID,
                    Property_Name = e.Name,
                    Property_Value = Master.To_Byte_Array<object>(e.EditValue)
                });
            });
            db.SubmitChanges();

            Part_ID = profile.ID;
            Part_Name = profile.Name;

            base.Save();
        }
        public override void Set_Data()
        {
            profile.Name = txt_name.Text;
            base.Set_Data();
        }
        private void AccordionControl1_ElementClick(object sender, ElementClickEventArgs e)
        {
            var index = accordionControl1.Elements.IndexOf(e.Element);
            xtraTabControl1.SelectedTabPageIndex = index;
        }

        private void FRM_User_Settings_Profile_Load(object sender, EventArgs e)
        {
            accordionControl1.AnimationType = AnimationType.Simple;
            accordionControl1.Dock = DockStyle.Left;
            xtraTabControl1.Dock = DockStyle.Fill;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            xtraTabControl1.Transition.AllowTransition = DevExpress.Utils.DefaultBoolean.True;
            xtraTabControl1.Transition.EasingMode = DevExpress.Data.Utils.EasingMode.EaseInOut;
            SlideFadeTransition trans = new SlideFadeTransition();
            trans.Parameters.EffectOptions = PushEffectOptions.FromBottom;
            xtraTabControl1.Transition.TransitionType = trans;
            xtraTabControl1.SelectedPageChanging += XtraTabControl1_SelectedPageChanging;
        }

        private void XtraTabControl1_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            SlideFadeTransition trans = new SlideFadeTransition();
            //trans.s
            var currentPage = xtraTabControl1.TabPages.IndexOf(e.Page);
            var prevPage = xtraTabControl1.TabPages.IndexOf(e.PrevPage);
            if (currentPage > prevPage)
                trans.Parameters.EffectOptions = PushEffectOptions.FromTop;
            else trans.Parameters.EffectOptions = PushEffectOptions.FromBottom;
            xtraTabControl1.Transition.TransitionType = trans;
        }
    }
}