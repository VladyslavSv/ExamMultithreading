using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace WrongWords
{
    class MainViewModel: DependencyObject
    {
        DependencyProperty counterProperty;
        DependencyProperty lbItemsProp;
        DependencyProperty directoryPath;

        private FileSystemParser parser;
        public SynchronizationContext context;

        public ObservableCollection<string> Items
        {
            get
            {
                return (ObservableCollection<string>) GetValue(lbItemsProp);
            }
            set
            {
                SetValue(lbItemsProp, value);
            }
        }
        public string counterProp
        {
            get
            {
                return (string)GetValue(counterProperty);
            }
            set
            {
                SetValue(counterProperty, value);
               
            }
        }
        public string DirectoryPath
        {
            get
            {
                return (string)GetValue(directoryPath);
            }
            set
            {
                SetValue(directoryPath, value);
            }
        }

        public MyCommand startCommand
        {
            get;set;
        }
        public MyCommand browseCommand
        {
            get;set;
        }
        public MyCommand labelCommand
        {
            get;set;
        }

        public MainViewModel()
        {
            parser = new FileSystemParser();

            startCommand = new MyCommand(onStart, null);
            browseCommand = new MyCommand(onBrowse, null);
            labelCommand = new MyCommand(onLabelClick, null);

            counterProperty = DependencyProperty.Register(
                "counterProp", 
                typeof(string), 
                typeof(MainViewModel));

            lbItemsProp = DependencyProperty.Register(
                "Items",
                typeof(ObservableCollection<string>),
                typeof(MainViewModel));

            directoryPath = DependencyProperty.Register(
                "DirectoryPath",
                typeof(string),
                typeof(MainViewModel));

            this.context = SynchronizationContext.Current;

            Items = new ObservableCollection<string>();

            parser.couterChanged += () => 
            {
                context.Send( ( d ) => 
                { counterProp = "Слов заменено:" + parser.WordsReplaced; } , 0);
            };

            parser.reporter += (fileInfo) =>
            {
                context.Send((d) =>
                {
                    Items.Add(fileInfo.FullName);
                }, 0);
            };


        }   

        public void onStart(object ob)
        {
            if (parser.allWords.Count == 0)
            {
                MessageBox.Show("Не выбраны ключевые слова!");
                return;
            }
            if (DirectoryPath == "" || DirectoryPath == null)
            {
                MessageBox.Show("Не выбрана папка для сохранения результатов.");
                return;
            }
            parser.parseFiles();
        }
        public void onBrowse(object ob)
        {
            using (var dialog = new WinForms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                DirectoryPath = dialog.SelectedPath;
                parser.directoryForCopy = dialog.SelectedPath;
            }
        }
        private void onLabelClick(object ob)
        {
            ReadInstructionFileWindow readInstructionWindow = new ReadInstructionFileWindow();

            readInstructionWindow.ShowDialog();

            if (readInstructionWindow.descriptionTextBox.Text != "" )
            {
                parser.initKeyWords(readInstructionWindow.descriptionTextBox.Text);
                System.Windows.MessageBox.Show( "Файл прочитан." );
            }
        }

    }
}
