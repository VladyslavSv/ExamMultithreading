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

namespace WrongWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemParser parser = new FileSystemParser();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReadInstructionFileWindow readInstructionWindow = new ReadInstructionFileWindow();

            readInstructionWindow.ShowDialog();

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
            MessageBox.Show("Done");
        }
    }
}
