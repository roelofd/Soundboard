using System;
using System.Collections.Generic;
using Windows.UI.Popups;
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
        Tile[,] tiles = new Tile[10,10];

        public MainPage()
        {
            this.InitializeComponent();
            for (int Column = 0; Column < 4; Column++)
            {
                for(int Row = 0; Row < 4; Row++)
                {
                    TextBox textBox = new TextBox();
                    Square.Children.Add(textBox);

                    TextBlock textBlock = new TextBlock();
                    Square.Children.Add(textBlock);
                    
                    Button playButton = new Button();
                    Square.Children.Add(playButton);

                    Button downloadButton = new Button();                    
                    Square.Children.Add(downloadButton);

                    Tile tile = new Tile(textBox, textBlock, playButton, downloadButton, Column, Row);

                    tiles[Column,Row] = tile;
                }
            }
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //staat in tile.cs

            //var url = new Uri(Urlbox1.Text);
            //var downloader = new YoutubeDownloader();
            //fileLocation = downloader.Download(url.ToString());
            //if (fileLocation != null)
            //{
            //    Download_button1.Visibility = Visibility.Collapsed;
            //    Urlbox1.Visibility = Visibility.Collapsed;
            //    PlayButton1.Visibility = Visibility.Visible;
            //    Name1.Text = fileLocation.Split(".")[0];
            //    Name1.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    var message = new MessageDialog("This video is encrypted!");
            //    await message.ShowAsync();
            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var audioManager = new AudioManager();
            audioManager.Play(fileLocation);
        }
    }
}
