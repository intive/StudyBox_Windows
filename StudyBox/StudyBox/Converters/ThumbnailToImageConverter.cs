using System;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StudyBox.Converters
{
    class ThumbnailToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage image = null;

            if (value != null && value.GetType() == typeof(StorageItemThumbnail) && targetType == typeof(ImageSource))
            {
                StorageItemThumbnail thumbnail = (StorageItemThumbnail)value;
                image = new BitmapImage();
                image.SetSource(thumbnail);
            }
            return (image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
