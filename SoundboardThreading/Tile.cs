using System;
using System.Collections.Generic;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        private List<Thread> _threads;
        private SemaphoreSlim _semaphore;
        private string _fileLocation;
        private int _column;
        private int _row;


        public TextBox TextBox { get; set; }
        public TextBlock TextBlock { get; set; }
        public Button PlayButton { get; set; }
        public Button StopButton { get; set; }
        public Button DownloadButton { get; set; }
        public ProgressBar ProgressBar { get; set; }
        AudioManager audioManager;

        public Tile(TextBox textBox, TextBlock textBlock, Button playButton, Button stopButton, Button downloadButton, int column, int row, List<Thread> threads, SemaphoreSlim semaphore)
        {
            _column = column;
            _row = row;
            _threads = threads;
            _semaphore = semaphore;

            CreateTextBox(textBox);
            CreateTextBlock(textBlock);
            CreatePlayButton(playButton);
            CreateDownloadButton(downloadButton);
            CreateStopButton(stopButton);

            audioManager = new AudioManager();
        }
        
        /*
         * Button listener which will download a youtube video.
         */
        private void Download_Button(object sender, RoutedEventArgs e)
        {
            // Get url from text box before starting threads.
            var url = new Uri(TextBox.Text);
            //Create a downloader.
            var downloader = new YoutubeDownloader();

            // Create a new thread which will download the youtube video.
            _threads.Add(new Thread(() =>
            {
                _semaphore.Wait();

                #region
                _fileLocation = downloader.Download(url.ToString());
                #endregion

                _semaphore.Release();
            }));

            // Start the thread.
            _threads[_threads.Count - 1].Start();
            // Join thread with main thread.
            _threads[_threads.Count - 1].Join();

            // Update the ui to show audio controls
            if (_fileLocation != null)
            {
                DownloadButton.Visibility = Visibility.Collapsed;
                TextBox.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                TextBlock.Text = _fileLocation.Split(".")[0];
                TextBlock.Visibility = Visibility.Visible;
            }
        }

        /*
         * Event listener to the play button which will play the audio from the mp2 file.
         */
        private void Play_Button(object sender, RoutedEventArgs e)
        {
            audioManager.Play(_fileLocation);

            PlayButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Visible;
        }

        /*
         * Event listener to the stop button.
         */
        private void Stop_Button(object sender, RoutedEventArgs e)
        {
            audioManager.Stop();

            PlayButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
        }

        //--------Generate UI--------\\

        private void CreateTextBox(TextBox textBox)
        {
            textBox.Name = "textBox" + _column + _row;
            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Width = 350;
            textBox.PlaceholderText = "Paste Url here";
            textBox.Visibility = Visibility.Visible;
            Grid.SetColumn(textBox, _column);
            Grid.SetRow(textBox, _row);
            TextBox = textBox;
        }

        private void CreateTextBlock(TextBlock textBlock)
        {
            textBlock.Name = "textBlock" + _column + _row;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Visibility = Visibility.Collapsed;
            Grid.SetColumn(textBlock, _column);
            Grid.SetRow(textBlock, _row);
            TextBlock = textBlock;
        }

        private void CreatePlayButton(Button playButton)
        {
            playButton.Name = "playButton" + _column + _row;
            playButton.Content = "Play";
            playButton.HorizontalAlignment = HorizontalAlignment.Center;
            playButton.VerticalAlignment = VerticalAlignment.Top;
            playButton.Visibility = Visibility.Collapsed;
            Grid.SetColumn(playButton, _column);
            Grid.SetRow(playButton, _row);
            playButton.Click += Play_Button;
            PlayButton = playButton;
        }
        private void CreateStopButton(Button stopButton)
        {
            stopButton.Name = "stopButton" + _column + _row;
            stopButton.Content = "Stop";
            stopButton.HorizontalAlignment = HorizontalAlignment.Center;
            stopButton.VerticalAlignment = VerticalAlignment.Top;
            stopButton.Visibility = Visibility.Collapsed;
            Grid.SetColumn(stopButton, _column);
            Grid.SetRow(stopButton, _row);
            stopButton.Click += Stop_Button;
            StopButton = stopButton;
        }
        private void CreateDownloadButton(Button downloadButton)
        {
            downloadButton.Name = "downloadButton" + _column + _row;
            downloadButton.Content = "Download";
            downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
            downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
            downloadButton.Visibility = Visibility.Visible;
            Grid.SetColumn(downloadButton, _column);
            Grid.SetRow(downloadButton, _row);
            downloadButton.Click += Download_Button;
            DownloadButton = downloadButton;
        }

        
    }
}
