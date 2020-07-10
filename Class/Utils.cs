using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace Pro_Salles.Class
{
    public static class Utils
    {
        //RepositoryItemButtonEdit ButtonEdit = new RepositoryItemButtonEdit();
        ////gridControl1.RepositoryItems.Add(ButtonEdit);
        //    ButtonEdit.Buttons.Clear();
        //    ButtonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
        //    ButtonEdit.ButtonClick += ButtonEdit_ButtonClick;

        //GridColumn CLM_Delete = new GridColumn()
        //{
        //    Name = "CLM_Delete",
        //    Caption = "",
        //    FieldName = "Delete",
        //    VisibleIndex = 13,
        //    ColumnEdit = ButtonEdit,
        //    Width = 7,
        //    MaxWidth = 7,
        //};
        //private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        //{
        //    GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
        //    if (view.FocusedRowHandle >= 0)
        //    {
        //        view.DeleteSelectedRows();
        //    }
        //}

        public static void AddButtonToGroupHeader(this GridView view, SvgImage img, EventHandler eventHandler)
        {
            var customClass = new CustomHeaderButtonClass(view, img, eventHandler);
        }

        public static void AddClearValueButton(this PopupBaseEdit edit)
        {
            edit.Properties.Buttons.AddRange(new EditorButton[]
            { new EditorButton(ButtonPredefines.Clear) });

            //79
            ///if i want to declare particular( void | event )inside another void, to make the code short, you can user this structure to make the event
            ///and don't forget to give it its arguments just in case it has parameters and lamba expression =>
            edit.ButtonPressed += (sender, e) => {
                if (e.Button.Kind == ButtonPredefines.Clear)                
                    ((PopupBaseEdit)sender).EditValue = null;                
            };
        }

        class CustomHeaderButtonClass
        {
            GridView View;
            SvgImage svgImage;
            EventHandler eventHandler;
            int svgSize = 16;

            public CustomHeaderButtonClass(GridView _view, SvgImage _img, EventHandler _eventHandler)
            {
                View = _view;
                svgImage = _img;
                eventHandler = _eventHandler;
                View.CustomDrawGroupPanel += View_CustomDrawGroupPanel;
                View.MouseMove += View_MouseMove;
                View.Click += View_Click;
            }

            private void View_Click(object sender, EventArgs e)
            {
                DXMouseEventArgs ea = e as DXMouseEventArgs;
                if (r.Contains(ea.Location))
                    eventHandler(sender, e);
            }

            private void View_MouseMove(object sender, MouseEventArgs e)
            {
                IsInRectangle = r.Contains(e.Location);
                View.Invalidate();
            }
            bool IsInRectangle;
            Rectangle r;
            ////////////77
            private void View_CustomDrawGroupPanel(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
            {
                Brush _brush2 = e.Cache.GetGradientBrush(e.Bounds, Color.LightYellow, Color.WhiteSmoke,
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Cache.FillRectangle(_brush2, e.Bounds);

                SvgBitmap bitmap = SvgBitmap.Create(svgImage);
                r = new Rectangle(
                    e.Bounds.X + e.Bounds.Width - (svgSize * 3),
                    e.Bounds.Y + ((e.Bounds.Height - svgSize) / 2),
                    svgSize, svgSize
                    );
                var palette = SvgPaletteHelper.GetSvgPalette(UserLookAndFeel.Default,
                    DevExpress.Utils.Drawing.ObjectState.Normal);
                e.Cache.DrawImage(bitmap.Render(palette), r);
                int thikness = IsInRectangle ? 2 : 1;
                int offset = thikness + 1;
                e.Cache.DrawRectangle(r.X - offset, r.Y - offset, r.Width + (offset * 2), r.Height + (offset * 2), Color.Black, thikness);

                e.Handled = true;
            }
        }
    }
}