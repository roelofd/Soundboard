using System;
using System.Collections.Generic;
using System.Threading;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        private readonly List<Thread> _threads;
        private readonly SemaphoreSlim _semaphore;
        private readonly AudioManager _audioManager;
        private readonly int _column;
        private readonly int _row;

        //The location where the .mp3 is stored.
        private string _fileLocation;
        
        // Ui elements
        private TextBox _textBox;
        private TextBlock _textBlock;
        private Button _playButton;
        private Button _stopButton;
        private Button _downloadButton;


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

            _audioManager = new AudioManager();
        }

        /*
         * Button listener which will download a youtube video.
         */
        /*
         * Button listener which will download a youtube video.
         */
        private void Download_Button(object sender, RoutedEventArgs e)
        {
            // Get url from text box before starting threads.
            var url = new Uri(_textBox.Text);
            //Create a downloader.
            var downloader = new YoutubeDownloader(url.ToString());

            // Check if video is encrypted
            if (downloader.IsEncrypted())
            {
                string errorMessage = $"{downloader.GetName()} is encrypted!";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                var message = new MessageDialog(errorMessage);
                message.ShowAsync();
                return;
            }

            // Create a new thread which will download the youtube video.
            _threads.Add(new Thread(() =>
            {
                _semaphore.Wait();

                _fileLocation = downloader.Download();

                _semaphore.Release();
            }));

            // Start the thread.
            _threads[_threads.Count - 1].Start();
            // Join thread with main thread.
            _threads[_threads.Count - 1].Join();

            // Update the ui to show audio controls
            if (_fileLocation != null)
            {
                _downloadButton.Visibility = Visibility.Collapsed;
                _textBox.Visibility = Visibility.Collapsed;
                _playButton.Visibility = Visibility.Visible;
                _textBlock.Text = _fileLocation.Split(".")[0];
                _textBlock.Visibility = Visibility.Visible;
            }
        }

        /*
         * Event listener to the play button which will play the audio from the mp2 file.
         */
        private void Play_Button(object sender, RoutedEventArgs e)
        {
            _audioManager.Play(_fileLocation);

            _playButton.Visibility = Visibility.Collapsed;
            _stopButton.Visibility = Visibility.Visible;
        }

        /*
         * Event listener to the stop button.
         */
        private void Stop_Button(object sender, RoutedEventArgs e)
        {
            _audioManager.Stop();

            _playButton.Visibility = Visibility.Visible;
            _stopButton.Visibility = Visibility.Collapsed;
        }

        #region Generate UI
        private void CreateTextBox(TextBox textBox)
        {
            textBox.Name = "textBox" + _column + _row;
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.PlaceholderText = "Paste Url here";
            textBox.Visibility = Visibility.Visible;
            textBox.Margin = new Thickness(10,0,10,0);
            Grid.SetColumn(textBox, _column);
            Grid.SetRow(textBox, _row);
            _textBox = textBox;
        }

        private void CreateTextBlock(TextBlock textBlock)
        {
            textBlock.Name = "textBlock" + _column + _row;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Visibility = Visibility.Collapsed;
            textBlock.Margin = new Thickness(10, 0, 10, 0);
            Grid.SetColumn(textBlock, _column);
            Grid.SetRow(textBlock, _row);
            _textBlock = textBlock;
        }

        private void CreatePlayButton(Button playButton)
        {
            playButton.Name = "playButton" + _column + _row;
            playButton.Content = "Play";
            playButton.HorizontalAlignment = HorizontalAlignment.Center;
            playButton.VerticalAlignment = VerticalAlignment.Top;
            playButton.Visibility = Visibility.Collapsed;
            playButton.Margin = new Thickness(0, 15, 0, 15);
            Grid.SetColumn(playButton, _column);
            Grid.SetRow(playButton, _row);
            playButton.Click += Play_Button;
            _playButton = playButton;
        }
        private void CreateStopButton(Button stopButton)
        {
            stopButton.Name = "stopButton" + _column + _row;
            stopButton.Content = "Stop";
            stopButton.HorizontalAlignment = HorizontalAlignment.Center;
            stopButton.VerticalAlignment = VerticalAlignment.Top;
            stopButton.Visibility = Visibility.Collapsed;
            stopButton.Margin = new Thickness(0,15,0,15);
            Grid.SetColumn(stopButton, _column);
            Grid.SetRow(stopButton, _row);
            stopButton.Click += Stop_Button;
            _stopButton = stopButton;
        }
        private void CreateDownloadButton(Button downloadButton)
        {
            downloadButton.Name = "downloadButton" + _column + _row;
            downloadButton.Content = "Download";
            downloadButton.HorizontalAlignment = HorizontalAlignment.Center;
            downloadButton.VerticalAlignment = VerticalAlignment.Bottom;
            downloadButton.Visibility = Visibility.Visible;
            downloadButton.Margin = new Thickness(0, 15, 0, 15);
            Grid.SetColumn(downloadButton, _column);
            Grid.SetRow(downloadButton, _row);
            downloadButton.Click += Download_Button;
            _downloadButton = downloadButton;
        }
        #endregion
    }
}
