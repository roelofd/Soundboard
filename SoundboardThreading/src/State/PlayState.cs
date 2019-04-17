using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class PlayState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, SplitView splitView, AudioManager audioManager)
        {
            var sound = (Sound) e.ClickedItem;
            if (sound.FileName.Equals(audioManager.CurrentlyPlaying))
            {
                Debug.WriteLine("Pause Sound!");
                audioManager.Pause();
                return new PauseState();
            }

            Debug.WriteLine("Play new one!");
            audioManager.Play(sound.FileName);
            return new PlayState();
        }

        public StateType getState()
        {
            return StateType.Playing;
        }
    }
}
