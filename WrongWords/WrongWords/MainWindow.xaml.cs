using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForms = System.Windows.Forms;
using System.Threading;

namespace WrongWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemParser parser;

        public SynchronizationContext uiContext
        {
            get; set;
        }

        public void addLineToListView(string line)
        {
            uiContext.Send((d) => { myListBox.Items.Add(line); }, 0);
        }
        private volatile int fileCounter = 0;

        public int uiCounter
        {
            get
            {
                return fileCounter;
            }
            set
            {
                fileCounter = value;
                uiContext.Send((d) => { repWords.Content = "Слов заменено: " + fileCounter; }, 0);
            }
        }
        public void ClearTypeHintListView()
        {

        }
        public MainWindow()
        {
            parser = new FileSystemParser(this);
            InitializeComponent();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReadInstructionFileWindow readInstructionWindow = new ReadInstructionFileWindow();

            readInstructionWindow.ShowDialog();

            uiContext = SynchronizationContext.Current;

            if (readInstructionWindow.descriptionTextBox.Text != "")
            {
                parser.initKeyWords(readInstructionWindow.descriptionTextBox.Text);
                System.Windows.MessageBox.Show("File parsed");
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new WinForms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                folderTextBox.Text = dialog.SelectedPath;
                parser.directoryForCopy = dialog.SelectedPath;
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            parser.parseFiles();
        }
    }
}
