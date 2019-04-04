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
        TextBox[,] textBoxes = new TextBox[10,10];

        public MainPage()
        {
            this.InitializeComponent();

            Grid grid = new Grid();

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(0, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(0, GridUnitType.Star);
            grid.RowDefinitions.Add(row1);


            for (int i = 1; i <= 4; i++)
            {
                for(int x = 1; x <= 4; x++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + i + x;
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    textBox.VerticalAlignment = VerticalAlignment.Center;
                    textBox.Width = 350;
                    textBox.PlaceholderText = "Paste Url here";
                    textBox.Visibility = Visibility.Visible;
                    grid.Children.Add(textBox);
                    Grid.SetColumn(textBox, i);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Name = "textBlock" + i + x;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Visibility = Visibility.Collapsed;

                    Button playButton = new Button();
                    playButton.Name = "playButton" + i + x;
                    playButton.HorizontalAlignment = HorizontalAlignment.Center;
                    playButton.VerticalAlignment = VerticalAlignment.Bottom;
                    playButton.Visibility = Visibility.Collapsed;

                    Button downloadButton = new Button();
                    downloadButton.Name = "downloadButton" + i + x;
                    downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
                    downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
                    downloadButton.Visibility = Visibility.Visible;

                    Tile tile = new Tile(textBox, textBlock, playButton, downloadButton);

                    textBoxes[i,x] = textBox;
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
