using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SoundboardThreading.State;
using Color = Windows.UI.Color;

namespace SoundboardThreading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public Sound EditSound { get; set; }
        public AudioManager AudioManager { get; set; }

        private readonly ObservableCollection<Sound> _sounds;
        private readonly YoutubeDownloader _youtubeDownloader;
        private readonly List<SolidColorBrush> _colorOptions;
        private SolidColorBrush currentBrush;
        private State.State _state;
        private State.State _prevState;
        private ItemClickEventArgs _editSouundArgs;

        public MainPage()
        {
            InitializeComponent();

            _state = new StopState();
            _prevState = new StopState();
            AudioManager = new AudioManager();
            _sounds = new ObservableCollection<Sound>();
            _youtubeDownloader = new YoutubeDownloader();
            _colorOptions = new List<SolidColorBrush>();

            LoadSounds();
            AddColorOptions();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            _state = _state.ListViewBase_OnItemClick(sender, e, this);
            StateBox.Text = _state.getState().ToString();

            if (_state.GetType() == typeof(EditState))
            {
                EditSound = (Sound) e.ClickedItem;
                _editSouundArgs = e;
                Debug.WriteLine($"Edited sound is: {EditSound.Title}");
            }
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
            EditSound = null;

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
                AudioManager.Play();
                _state = new PlayState();
                StateBox.Text = _state.getState().ToString();
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() == typeof(PlayState))
            {
                AudioManager.Pause();
                _state = new PauseState();
                StateBox.Text = _state.getState().ToString();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            AudioManager.Stop();
            _state = new StopState();
            StateBox.Text = _state.getState().ToString();
        }

        private async void LoadSounds()
        {
            var fileTypeFilter = new List<string> {".mp3"};

            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            var query = ApplicationData.Current.LocalFolder.CreateFileQueryWithOptions(queryOptions);

            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList)
            {
                _sounds.Add(new Sound(file.DisplayName, file.Name));
            }
        }

        public SplitView setEditPaneContent(string title)
        {
            EditTitleBox.Text = title;

            return SplitView;
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditSound != null)
            {
                foreach (var sound in _sounds)
                {
                    if (sound.FileName.Equals(EditSound.FileName))
                    {
                        var newSound = sound;
                        var index = _sounds.IndexOf(sound);

                        newSound.Title = EditTitleBox.Text;
                        newSound.Color = currentBrush;

                        _sounds.RemoveAt(index);
                        _sounds.Insert(index, newSound);
                        return;
                    }
                }
            }
        }

        private void AddColorOptions()
        {
            _colorOptions.Add(new SolidColorBrush(Colors.LightGray));
            _colorOptions.Add(new SolidColorBrush(Colors.Blue));
            _colorOptions.Add(new SolidColorBrush(Colors.Green));
            _colorOptions.Add(new SolidColorBrush(Colors.Orange));
            _colorOptions.Add(new SolidColorBrush(Colors.Red));
        }

        private void ColorSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBrush = (SolidColorBrush)e.AddedItems[0];
            SelectedColorBorder.Background = currentBrush;
            ColorFlyout.Hide();
        }
    }
}
