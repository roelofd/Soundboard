using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class StopState : IState
    {
        public IState ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage)
        {
            Debug.WriteLine("Start Sound!");
            Sound sound = (Sound)e.ClickedItem;
            mainPage.AudioManager.Play(sound.FileName);
            return new PlayState();
        }

        public StateType GetState()
        {
            return StateType.Stopped;
        }
    }
}
