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
using System.Threading;

namespace WrongWords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile int fileCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            uiContext = SynchronizationContext.Current;
        }

        public SynchronizationContext uiContext
        {
            get; set;
        }
        public void addLineToListView(string line)
        {
            uiContext.Send((d) => { myListBox.Items.Add(line); }, 0);
        }
        
        public int uiCounter
        {
            get
            {
                return fileCounter;
            }
            set
            {
                fileCounter = value;
                uiContext.Send((d) => { repWords.Text = "Слов заменено: " + fileCounter; }, 0);
            }
        }

        public void ClearListView()
        {

        }
    }
}
