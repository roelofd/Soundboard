using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SoundboardThreading.State;

namespace SoundboardThreading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly ObservableCollection<Sound> _sounds;
        private readonly AudioManager _audioManager;
        private readonly YoutubeDownloader _youtubeDownloader;
        private State.State _state;
        private State.State _prevState;

        public MainPage()
        {
            InitializeComponent();

            _state = new StopState();
            _prevState = new StopState();
            _audioManager = new AudioManager();
            _sounds = new ObservableCollection<Sound>();
            _youtubeDownloader = new YoutubeDownloader();

            LoadSounds();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            _state = _state.ListViewBase_OnItemClick(sender, e, SplitView, _audioManager);
            StateBox.Text = _state.getState().ToString();
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() != typeof(EditState))
            {
                _prevState = _state;
                _state = new EditState();
                StateBox.Text = _state.getState().ToString();
                return;
            }

            _state = _prevState;

            StateBox.Text = _state.getState().ToString();
        }

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addDialog = new AddDialog(_youtubeDownloader);
            await addDialog.ShowAsync();

            if (addDialog.Result == DownloadResult.Ok)
            {
                Debug.WriteLine("download success!");
                _sounds.Add(addDialog.Sound);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() == typeof(PauseState))
            {
                _audioManager.Play();
                _state = new PlayState();
                StateBox.Text = _state.getState().ToString();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() == typeof(PlayState))
            {
                _audioManager.Pause();
                _state = new PauseState();
                StateBox.Text = _state.getState().ToString();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _audioManager.Stop();
            _state = new StopState();
            StateBox.Text = _state.getState().ToString();
        }

        private async void LoadSounds()
        {
            var fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".mp3");

            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            var query = ApplicationData.Current.LocalFolder.CreateFileQueryWithOptions(queryOptions);

            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList)
            {
                _sounds.Add(new Sound(file.DisplayName, file.Name));
            }
        }
    }
}
