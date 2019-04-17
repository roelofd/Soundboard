using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class EditState : IState
    {
        public IState ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage)
        {
            Debug.WriteLine("Time to Edit!");
            var sound = (Sound) e.ClickedItem;

            var splitView = mainPage.SetEditPaneContent(sound.Title);
            splitView.IsPaneOpen = true;

            return new EditState();
        }

        public StateType GetState()
        {
            return StateType.Editing;
        }
    }
}
