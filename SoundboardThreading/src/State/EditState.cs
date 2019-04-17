using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    class EditState : State
    {
        public State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, SplitView splitView, AudioManager audioManager)
        {
            Debug.WriteLine("Time to Edit!");
            splitView.IsPaneOpen = true;
            return new EditState();
        }

        public StateType getState()
        {
            return StateType.Editing;
        }
    }
}
