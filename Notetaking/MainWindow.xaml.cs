using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Notetaking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static void ShowMessage(string Message)
        {
            MessageBox.Show(Message);
        }

        public void SetNotes(ObservableCollection<Note> Notes)
        {
            NotesList.ItemsSource = Notes;
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Do nothing for now.
        }

        private void CreateNewNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Content = new CreateNewNoteControl();
        }
    }
}
