﻿using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopApplication.Resources
{
    public static class ImageResources
    {
        public static ImageSource Logo => new BitmapImage(new Uri("@pack://application:,,,/DesktopApplication;component/Resources/SOSIEL.png", UriKind.RelativeOrAbsolute));
    }
}
