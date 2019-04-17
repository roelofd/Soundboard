using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class StopState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, SplitView splitView, AudioManager audioManager)
        {
            Debug.WriteLine("Start Sound!");
            Sound sound = (Sound)e.ClickedItem;
            audioManager.Play(sound.FileName);
            return new PlayState();
        }

        public StateType getState()
        {
            return StateType.Stopped;
        }
    }
}
