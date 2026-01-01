using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _3DeanSlicer.Gui.Utils
{
    public static class Images
    {
        public static ImageSource LoadIcon(string name)
        {
            return new BitmapImage(new Uri(
                $"C:\\Users\\muld0324.SKYNET\\source\\repos\\3DeanSlicer\\3DeanSlicer\\Assets\\Icons\\{name}",
                UriKind.Absolute));
        }
    }
}
