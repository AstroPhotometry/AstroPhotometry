using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AstroPhotometry.View
{
    /// <summary>
    /// Interaction logic for ImageShow.xaml
    /// </summary>
    public partial class ImageShow : UserControl
    {
        private PhotoVM photo;

        private bool mouse_enter;
        private Point start;
        private Point origin;
        private string selected_mode;
        public ImageShow()
        {
            InitializeComponent();
            // watcher:
            var watcher = new ViewModels.FileWatcherVM("./tmp/", "*.png");
            photo = new PhotoVM(watcher);
            DataContext = photo;
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Scale
            ScaleTransform st = scale;
            double zoom = e.Delta > 0 ? .2 : -.2;
            st.ScaleX += zoom;
            st.ScaleY += zoom;

            // Translate to the zoom position
            Point mouse_pos = e.GetPosition(image);
            TranslateTransform tt = translate;
            tt.X -= mouse_pos.X * zoom;
            tt.Y -= mouse_pos.Y * zoom;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            image.CaptureMouse();

            TranslateTransform tt = translate;
            start = e.GetPosition(border);
            origin = new Point(tt.X, tt.Y);
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (image.IsMouseCaptured)
            {
                TranslateTransform tt = translate;
                Vector v = start - e.GetPosition(border);
                tt.X = origin.X - v.X;
                tt.Y = origin.Y - v.Y;
            }
            if (mouse_enter)
            {
                lblCursorPosition.Text = String.Format("mouseX: {0:0.00} mouseY:{1:0.00}", e.GetPosition(image).X, e.GetPosition(image).Y);
            }
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            // Reset zoom
            ScaleTransform st = scale;
            st.ScaleX = 1;
            st.ScaleY = 1;

            // Reset translate
            TranslateTransform tt = translate;
            tt.X = 0;
            tt.Y = 0;
        }

        private void image_MouseEnter(object sender, MouseEventArgs e)
        {
            mouse_enter = true;
        }

        private void image_MouseLeave(object sender, MouseEventArgs e)
        {
            mouse_enter = false;
            lblCursorPosition.Text = "";
        }

        /**
        * selection box function
        */
        private void viewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected_mode = ((ContentControl)view_box.SelectedValue).Content.ToString();
            photo.changeMode(selected_mode);
        }
    }
}
