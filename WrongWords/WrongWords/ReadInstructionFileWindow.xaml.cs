using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;


namespace WrongWords
{
    /// <summary>
    /// Interaction logic for ReadInstructionFileWindow.xaml
    /// </summary>
    public partial class ReadInstructionFileWindow : Window
    {
        public ReadInstructionFileWindow()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();

            fdlg.Title = "Instruction file";
            fdlg.InitialDirectory = @"d:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;

            Nullable<bool> result = fdlg.ShowDialog();

            if (result == true)
            {
                // Open document
                string filename = fdlg.FileName;
                descriptionTextBox.Text = filename;
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
