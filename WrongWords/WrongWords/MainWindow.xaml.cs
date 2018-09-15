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

            parser.initKeyWords(readInstructionWindow.descriptionTextBox.Text);

            MessageBox.Show("File parsed");
        }
    }
}
