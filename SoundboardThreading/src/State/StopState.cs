using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class StopState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage)
        {
            Debug.WriteLine("Start Sound!");
            Sound sound = (Sound)e.ClickedItem;
            mainPage.AudioManager.Play(sound.FileName);
            return new PlayState();
        }

        public StateType getState()
        {
            return StateType.Stopped;
        }
    }
}
