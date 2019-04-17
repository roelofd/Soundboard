using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class PauseState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, SplitView splitView, AudioManager audioManager)
        {
            var sound = (Sound) e.ClickedItem;

            if (sound.FileName.Equals(audioManager.CurrentlyPlaying))
            {
                Debug.WriteLine("Continue Sound!");
                audioManager.Play();
            }
            else
            {
                Debug.WriteLine("Play new one!");
                audioManager.Play(sound.FileName);
            }

            return new PlayState();
        }

        public StateType getState()
        {
            return StateType.Paused;
        }
    }
}
