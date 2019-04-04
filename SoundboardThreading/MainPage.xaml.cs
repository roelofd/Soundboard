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
            for (int i = 0; i < 4; i++)
            {
                for(int x = 0; x < 4; x++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + i + x;
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    textBox.VerticalAlignment = VerticalAlignment.Center;
                    textBox.Width = 350;
                    textBox.PlaceholderText = "Paste Url here";
                    textBox.Visibility = Visibility.Visible;
                    Square.Children.Add(textBox);
                    Grid.SetColumn(textBox, i);
                    Grid.SetRow(textBox, x);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Name = "textBlock" + i + x;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Visibility = Visibility.Collapsed;
                    Square.Children.Add(textBlock);
                    Grid.SetColumn(textBlock, i);
                    Grid.SetRow(textBlock, x);

                    Button playButton = new Button();
                    playButton.Name = "playButton" + i + x;
                    playButton.Content = "Play";
                    playButton.HorizontalAlignment = HorizontalAlignment.Center;
                    playButton.VerticalAlignment = VerticalAlignment.Top;
                    playButton.Visibility = Visibility.Collapsed;
                    Square.Children.Add(playButton);
                    Grid.SetColumn(playButton, i);
                    Grid.SetRow(playButton, x);

                    Button downloadButton = new Button();
                    downloadButton.Name = "downloadButton" + i + x;
                    downloadButton.Content = "Download";
                    downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
                    downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
                    downloadButton.Visibility = Visibility.Visible;
                    Square.Children.Add(downloadButton);
                    Grid.SetColumn(downloadButton, i);
                    Grid.SetRow(downloadButton, x);

                    Tile tile = new Tile(textBox, textBlock, playButton, downloadButton);

                    tiles[i,x] = tile;
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
