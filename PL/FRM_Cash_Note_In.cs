using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace Pro_Salles.PL
{
    public class FRM_Cash_Note_In : FRM_Cash_Note
    {
        public FRM_Cash_Note_In() : base(true)
        {
            this.Text = "   سند قبض نقدي";
            lyc_Type.Text = "سند قبض نقدي";
        }
    }
    public class FRM_Cash_Note_Out : FRM_Cash_Note
    {
        public FRM_Cash_Note_Out() : base(false)
        {
            this.Text = "   سند دفع نقدي";
            lyc_Type.Text = "سند دفع نقدي";
        }
    }
}