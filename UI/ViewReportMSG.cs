using DevExpress.XtraEditors;
using System.Drawing;

namespace Pro_Salles.UI
{
    /// <summary>
    /// 112
    /// </summary>
    public static class ViewActionReport
    {
        public static void View_MSG(string msg)
        {
            XtraForm frm = new XtraForm()
            {
                Size = new Size(350, 300),
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };
            frm.IconOptions.ShowIcon = false;

            MemoEdit memo = new MemoEdit()
            {
                Text = msg,
                Dock = System.Windows.Forms.DockStyle.Fill
            };
            memo.AllowDrop = false;
            memo.Properties.ReadOnly = true;

            frm.Controls.Add(memo);
            frm.ShowDialog();
        }
    }
}