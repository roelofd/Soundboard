using Windows.UI.Xaml.Controls;

namespace SoundboardThreading.State
{
    public enum StateType
    {
        Stopped,
        Playing,
        Paused,
        Editing
    }
    interface State
    {
        State ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, SplitView splitView, AudioManager audioManager);
        StateType getState();
    }
}
