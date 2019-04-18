using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using SoundboardThreading.State;

namespace SoundboardThreading
{
    /// <summary>
    /// The Main class of the program.
    /// </summary>
    public sealed partial class MainPage
    {
        public Sound EditSound { get; set; }
        public AudioManager AudioManager { get; set; }

        private readonly ObservableCollection<Sound> _sounds;
        private readonly YoutubeDownloader _youtubeDownloader;
        private readonly List<SolidColorBrush> _colorOptions;
        private SolidColorBrush _currentBrush;
        private IState _state;
        private IState _prevState;

        public MainPage()
        {
            InitializeComponent();

            _state = new StopState();
            _prevState = new StopState();
            AudioManager = new AudioManager(MediaElement);
            _sounds = new ObservableCollection<Sound>();
            _youtubeDownloader = new YoutubeDownloader();
            _colorOptions = new List<SolidColorBrush>();

            LoadSounds();
            AddColorOptions();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            _state = _state.ListViewBase_OnItemClick(sender, e, this);
            StateBox.Text = _state.GetState().ToString();

            if (_state.GetType() != typeof(EditState)) return;

            EditSound = (Sound) e.ClickedItem;
            Debug.WriteLine($"Edited sound is: {EditSound.Title}");
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() != typeof(EditState))
            {
                _prevState = _state;
                _state = new EditState();
                StateBox.Text = _state.GetState().ToString();
                return;
            }

            _state = _prevState;
            EditSound = null;

            StateBox.Text = _state.GetState().ToString();
        }

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addDialog = new AddDialog(_youtubeDownloader);
            await addDialog.ShowAsync();

            if (addDialog.Result != DownloadResult.Ok) return;

            Debug.WriteLine("Download success!");
            _sounds.Add(addDialog.Sound);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() != typeof(PauseState)) return;

            AudioManager.Play();
            _state = new PlayState();
            StateBox.Text = _state.GetState().ToString();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_state.GetType() != typeof(PlayState)) return;

            AudioManager.Pause();
            _state = new PauseState();
            StateBox.Text = _state.GetState().ToString();
        }

        public void StopButton_Click(object sender, RoutedEventArgs e)
        {
            AudioManager.Stop();
            _state = new StopState();
            StateBox.Text = _state.GetState().ToString();
        }

        private async void LoadSounds()
        {
            var fileTypeFilter = new List<string> {".mp3"};

            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            var query = ApplicationData.Current.LocalFolder.CreateFileQueryWithOptions(queryOptions);

            var fileList = await query.GetFilesAsync();

            foreach (StorageFile file in fileList)
            {
                _sounds.Add(new Sound(file.DisplayName, file.Name));
            }
        }

        public SplitView SetEditPaneContent(string title)
        {
            EditTitleBox.Text = title;

            return SplitView;
        }

        private void EditSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditSound == null) return;

            foreach (var sound in _sounds)
            {
                if (!sound.FileName.Equals(EditSound.FileName)) continue;

                var newSound = sound;
                var index = _sounds.IndexOf(sound);

                newSound.Title = EditTitleBox.Text;
                newSound.Color = _currentBrush;
                var color = System.Drawing.Color.FromArgb(_currentBrush.Color.A, _currentBrush.Color.R,
                    _currentBrush.Color.G, _currentBrush.Color.B);
                        
                newSound.ForeGround = !ContrastIsReadable(color, System.Drawing.Color.Black) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);

                _sounds.RemoveAt(index);
                _sounds.Insert(index, newSound);
                return;
            }
        }

        private static bool ContrastIsReadable(System.Drawing.Color color1, System.Drawing.Color color2)
        {
            // Maximum contrast would be a value of "1.0f" which is the brightness
            // difference between "Color.Black" and "Color.White"
            const float minContrast = 0.55f;

            var brightness1 = color1.GetBrightness();
            var brightness2 = color2.GetBrightness();

            // Contrast readable?
            return (Math.Abs(brightness1 - brightness2) >= minContrast);
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
            _currentBrush = (SolidColorBrush)e.AddedItems[0];
            SelectedColorBorder.Background = _currentBrush;
            ColorFlyout.Hide();
        }
    }
}
