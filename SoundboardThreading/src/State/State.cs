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
    interface IState
    {
        IState ListViewBase_OnItemClick(object sender, ItemClickEventArgs e, MainPage mainPage);
        StateType GetState();
    }
}
