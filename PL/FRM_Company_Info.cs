﻿using System;
using System.Linq;
using DevExpress.XtraEditors;
using Pro_Salles.DAL;
using Pro_Salles.Class;

namespace Pro_Salles.PL
{
    public partial class FRM_Company_Info : XtraForm
    {
        public FRM_Company_Info()
        {
            InitializeComponent();
            this.Load += FRM_Company_Info_Load;
        }

        private void FRM_Company_Info_Load(object sender, EventArgs e)
        {
            Get_Data();
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txt_com_name.Text.Trim() == string.Empty)
            {
                txt_com_name.ErrorText = "برجاء ادخال اسم الشركه";
                return;
            }
            if (txt_com_mobile.Text.Trim() == string.Empty)
            {
                txt_com_mobile.Text = "برجاء ادخال رقم الشركه";
                return;
            }
            Save();
        }
        void Get_Data()
        {
            var db = new Pro_SallesDataContext();
            var info = db.Company_Infos.FirstOrDefault();
            if (info == null) 
                return;
            txt_com_name.Text = info.name;
            txt_com_phone.Text = info.phone;
            txt_com_mobile.Text = info.mobile;
            txt_com_address.Text = info.address;
            if (info.Logo != null)
                company_pic.Image = Utils.Get_Image_FromByte_Array(Sessions.Company_Info.Logo.ToArray());
        }
        void Save()
        {
            var db = new Pro_SallesDataContext();
            Company_Info info = db.Company_Infos.FirstOrDefault();
            if (info == null)
            {
                info = new Company_Info();
                db.Company_Infos.InsertOnSubmit(info);
            }
            info.name = txt_com_name.Text;
            info.phone = txt_com_phone.Text;
            info.mobile = txt_com_mobile.Text;
            info.address = txt_com_address.Text;
            info.Logo = Utils.Get_Byte_From_Image(company_pic.Image);

            db.SubmitChanges();
            Sessions.Reset_Company_Info();
            XtraMessageBox.Show("تم الحفظ بنجاح", "تم");
        }
    }
}