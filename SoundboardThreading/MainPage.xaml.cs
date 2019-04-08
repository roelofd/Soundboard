using System.Collections.Generic;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly List<Thread> _downloadThreads = new List<Thread>(); 
        private readonly SemaphoreSlim _downloadSlim = new SemaphoreSlim(4);

        public MainPage()
        {
            InitializeComponent();

            //This for loop creates all the tiles.
            for (int column = 0; column < 4; column++)
            {
                for(int row = 0; row < 4; row++)
                {
                    //Adds the UI elements to the grid.
                    TextBox textBox = new TextBox();
                    Square.Children.Add(textBox);

                    TextBlock textBlock = new TextBlock();
                    Square.Children.Add(textBlock);
                    
                    Button playButton = new Button();
                    Square.Children.Add(playButton);

                    Button stopButton = new Button();
                    Square.Children.Add(stopButton);

                    Button downloadButton = new Button();                    
                    Square.Children.Add(downloadButton);
                    
                    //Creates the Tile and adds the UI elements to it.
                    new Tile(textBox, textBlock, playButton, stopButton, downloadButton, column, row, _downloadThreads, _downloadSlim);
                }
            }
        }
    }
}
