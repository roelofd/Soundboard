using System.Collections.Generic;
using System.Threading;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundboardThreading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        Tile[,] tiles = new Tile[10,10];
        private List<Thread> _downloadThreads = new List<Thread>();
        private SemaphoreSlim _downloadSlim = new SemaphoreSlim(4);

        public MainPage()
        {

            InitializeComponent();
            for (int column = 0; column < 4; column++)
            {
                for(int row = 0; row < 4; row++)
                {
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

                    Tile tile = new Tile(textBox, textBlock, playButton, stopButton, downloadButton, column, row, _downloadThreads, _downloadSlim);

                    tiles[column,row] = tile;
                }
            }
        }
    }
}
