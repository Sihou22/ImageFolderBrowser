using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FolderChoose.Models;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FolderChoose
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<StorageFile> itemsList;
        private ObservableCollection<Picture> Pictures;
        Windows.Storage.StorageFolder folder;
        public MainPage()
        {
            this.InitializeComponent();
            itemsList = new ObservableCollection<StorageFile>();
            Pictures = new ObservableCollection<Picture>();
        }

        private async void FolderPick_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                this.FolderPath.Text = folder.Name;
            }
            else
            {
                this.FolderPath.Text = "Operation cancelled.";
            }
        }

        private async void FolderScan_Click(object sender, RoutedEventArgs e)
        {
            itemsList.Clear();
            StorageFolder userFolder = folder;
            foreach (var item in await folder.GetFilesAsync())
            {
                itemsList.Add(item);
               /* ImageProperties picProperties = await item.Properties.GetImagePropertiesAsync();
                var pic = new Picture();
                pic.PicFile = item;
                pic.Name = picProperties.Title;
                pic.Path = item.Path;
                Pictures.Add(pic);*/
            }
        }

        private void FolderItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private async void FolderItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            var SelPic = (StorageFile)e.ClickedItem;
            if (SelPic != null)

            {
                using (IRandomAccessStream fileStream = await SelPic.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelWidth = 600; //match the target Image.Width, not shown
                    await bitmapImage.SetSourceAsync(fileStream);
                    ChoosedImage.Source = bitmapImage;
                }
            }
        }
    }
}
