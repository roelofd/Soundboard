using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class PauseState : IState
    {
        public IState ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage)
        {
            var sound = (Sound) e.ClickedItem;

            if (sound.FileName.Equals(mainPage.AudioManager.CurrentlyPlaying))
            {
                Debug.WriteLine("Continue Sound!");
                mainPage.AudioManager.Play();
            }
            else
            {
                Debug.WriteLine("Play new one!");
                mainPage.AudioManager.Play(sound.FileName);
            }

            return new PlayState();
        }

        public StateType GetState()
        {
            return StateType.Paused;
        }
    }
}
