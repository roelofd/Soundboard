using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class EditState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage)
        {
            Debug.WriteLine("Time to Edit!");
            var sound = (Sound) e.ClickedItem;

            var splitView = mainPage.setEditPaneContent(sound.Title);
            splitView.IsPaneOpen = true;

            return new EditState();
        }

        public StateType getState()
        {
            return StateType.Editing;
        }
    }
}
