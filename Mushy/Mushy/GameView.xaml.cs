using MushyExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MushyExtensionMethods
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : TabItem
    {

        public GameView()
        {
            InitializeComponent();
        }

        public GameController Controller { get; set; }

        
        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                char c = (char)10146;
                EchoTextBox(c + " " + _input.Text);
                
                try
                {
                    Controller.HandleInput(_input.Text);
                }
                catch
                {
                    UpdateText("Attempt to send text failed: No connection detected.");
                }
                _input.Text = "";
            }
        }

        
        //a parser/decoder for ANSI control sequences, to give text color and potentially other styling
        ANSIColorParser ansiColorParser = new ANSIColorParser();

        public void UpdateText(string data)
        {
            Dispatcher.BeginInvoke(
                (Action)delegate()
                {
                    Paragraph myParagraph = new Paragraph();

                    //pass the run to the AnsiParser to parse any ANSI control sequences (colors!)
                    List<AnsiTextRun> runs = this.ansiColorParser.Parse(data);
                    //add 'runs' to the output
                    foreach (var r in runs)
                    {
                        var rtf = new Run(r.Content);
                        rtf.Foreground = r.ForegroundColor;
                        rtf.Background = r.BackgroundColor;
                        myParagraph.Inlines.Add(rtf);
                        myParagraph.FontFamily = new FontFamily("Courier New");
                        myParagraph.FontSize = 14.0;
                        myParagraph.LineHeight = 1;
                    }
                        
                        _output.Document.Blocks.Add(myParagraph);
                        myParagraph.Loaded += new RoutedEventHandler(paragraph_Loaded);     
                    //ScrollViewer.ScrollToBottom();
                });

        }

        public void EchoTextBox(string data)
        {
            Dispatcher.BeginInvoke(
                (Action)delegate()
                {
                    
                    Paragraph myParagraph = new Paragraph();
                    myParagraph.Inlines.Add(new Run(data));
                    myParagraph.Foreground = Brushes.Orange;
                    myParagraph.FontSize = 14.0;
                    myParagraph.FontFamily = new FontFamily("Courier New");
                    myParagraph.LineHeight = 1;
                    _output.Document.Blocks.Add(myParagraph);
                    myParagraph.Loaded += new RoutedEventHandler(paragraph_Loaded); 
                   
                });
        }


        ///// <summary>
        ///// Backing store for the <see cref="ScrollViewer"/> property.
        ///// </summary>
        //private ScrollViewer scrollViewer;

        /// <summary>
        /// Attempt to bring each new paragraph into view, but only if the scroll viewer is at the bottom.
        /// </summary>
        void paragraph_Loaded(object sender, RoutedEventArgs e)
        {
            Paragraph paragraph = (Paragraph)sender;
            paragraph.Loaded -= paragraph_Loaded;
            
            //if (scrollViewer.VerticalOffset > 0)
            //{
                paragraph.BringIntoView();
            //}

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            _output.Find();
        }

        public void SetFocus()
        {
            Dispatcher.BeginInvoke(new Action(() => { _input.Focus(); }));
        }

        /// <summary>
        /// Backing store for the <see cref="ScrollViewer"/> property.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// Gets the scroll viewer contained within the FlowDocumentScrollViewer control
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if (this.scrollViewer == null)
                {
                    DependencyObject obj = this;

                    do
                    {
                        if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                            obj = VisualTreeHelper.GetChild(obj as Visual, 0);
                        else
                            return null;
                    }
                    while (!(obj is ScrollViewer));

                    this.scrollViewer = obj as ScrollViewer;
                }

                return this.scrollViewer;
            }
        }

    }
}
