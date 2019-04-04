using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundboardThreading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void builder()
        {
            GridView tilegrid = new GridView();
            tilegrid.Margin = new Thickness(50);

            DataTemplate template1 = new DataTemplate();
            
        }
        string fileLocation;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var url = new Uri(Urlbox1.Text);
            var downloader = new YoutubeDownloader();
            fileLocation = downloader.Download(url.ToString());
            Download_button1.Visibility = Visibility.Collapsed;
            Urlbox1.Visibility = Visibility.Collapsed;
            PlayButton1.Visibility = Visibility.Visible;
            Name1.Text = fileLocation.Split(".")[0];
            Name1.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var audioManager = new AudioManager();
            audioManager.Play(fileLocation);
        }
    }
}
