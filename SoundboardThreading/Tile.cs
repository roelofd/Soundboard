using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SoundboardThreading
{
    class Tile
    {
        public TextBox textBox { get; set; }
        public TextBlock textBlock { get; set; }
        public Button playButton { get; set; }
        public Button downloadButton { get; set; }

        public Tile(TextBox textBox, TextBlock textBlock, Button playButton, Button downloadButton)
        {
            textBox = this.textBox;
            textBlock = this.textBlock;
            playButton = this.playButton;
            downloadButton = this.downloadButton;
        }
    }
}
