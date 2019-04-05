using System;
using System.Collections.Generic;
using System.Threading;
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
        TextBox textBox;
        TextBlock textBlock;
        Button playButton;
        Button stopButton;
        Button downloadButton;

        public MainPage()
        {
            this.InitializeComponent();

            var taskNumber = 0;
            for (int Column = 0; Column < 4; Column++)
            {
                for(int Row = 0; Row < 4; Row++)
                {
                    textBox = new TextBox();
                    Square.Children.Add(textBox);
                    textBox.Name = "textBox" + Column + Row;
                    textBox.HorizontalAlignment = HorizontalAlignment.Center;
                    textBox.VerticalAlignment = VerticalAlignment.Center;
                    textBox.Width = 350;
                    textBox.PlaceholderText = "Paste Url here";
                    textBox.Visibility = Visibility.Visible;
                    Grid.SetColumn(textBox, Column);
                    Grid.SetRow(textBox, Row);

                    textBlock = new TextBlock();
                    Square.Children.Add(textBlock);
                    textBlock.Name = "textBlock" + Column + Row;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(textBlock, Column);
                    Grid.SetRow(textBlock, Row);

                    playButton = new Button();
                    Square.Children.Add(playButton);
                    playButton.Name = "playButton" + Column + Row;
                    playButton.Content = "Play";
                    playButton.HorizontalAlignment = HorizontalAlignment.Center;
                    playButton.VerticalAlignment = VerticalAlignment.Top;
                    playButton.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(playButton, Column);
                    Grid.SetRow(playButton, Row);

                    stopButton = new Button();
                    Square.Children.Add(stopButton);
                    stopButton.Name = "stopButton" + Column + Row;
                    stopButton.Content = "Stop";
                    stopButton.HorizontalAlignment = HorizontalAlignment.Center;
                    stopButton.VerticalAlignment = VerticalAlignment.Top;
                    stopButton.Visibility = Visibility.Collapsed;
                    Grid.SetColumn(stopButton, Column);
                    Grid.SetRow(stopButton, Row);

                    downloadButton = new Button();
                    Square.Children.Add(downloadButton);
                    downloadButton.Name = "downloadButton" + Column + Row;
                    downloadButton.Content = "Download";
                    downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
                    downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
                    downloadButton.Visibility = Visibility.Visible;
                    Grid.SetColumn(downloadButton, Column);
                    Grid.SetRow(downloadButton, Row);

                    taskNumber = (Column * 10) + Row;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(createTile), taskNumber);
                }
            }
        }

        private void createTile(Object taskNumber)
        {
            var Column = Convert.ToInt32(taskNumber.ToString()) / 10;
            var Row = Convert.ToInt32(taskNumber.ToString()) % 10;
            
            Tile tile = new Tile(textBox, textBlock, playButton, stopButton, downloadButton, Column, Row);

            tiles[Column, Row] = tile;
        }

        private void builder()
        {
            GridView tilegrid = new GridView();
            tilegrid.Margin = new Thickness(50);

            DataTemplate template1 = new DataTemplate();
            
        }
    }
}
