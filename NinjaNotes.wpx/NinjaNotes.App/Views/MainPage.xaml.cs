using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

using Microsoft.Phone.Controls;
using NinjaNotes.App.Resources;
using NinjaNotes.App.ViewModels;

namespace NinjaNotes.App.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        private readonly string ClickToAdd = AppResources.ClickToAdd;
        private readonly string NoteSaved = AppResources.NoteSaved;

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
                Note.Text = ClickToAdd;
            }
        }

        // Save data for the ViewModel Items when closing down
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Notorama.SelectedIndex == 1)
            {
                Notorama.DefaultItem = Notorama.Items[0];
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // should be 1 since it's not multiselect
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as NoteModel;

                if (item != null)
                {
                    item.Idx = App.ViewModel.Items.IndexOf(item);
                    Note.Text = item.TheNote;
                    Note.Tag = item;
                    Notorama.DefaultItem = Notorama.Items[1];
                }
            }
        }

        private void Note_LostFocus(object sender, RoutedEventArgs e)
        {
            var item = Note.Tag as NoteModel;
            SaveItem(item);

            if (Notorama.SelectedIndex == 1)
            {
                Notorama.DefaultItem = Notorama.Items[0];
            }
        }

        private void SaveItem(NoteModel item)
        {
            string newNote = Note.Text;

            Note.Text = NoteSaved;
            Note.Tag = null;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (item != null)
            {
                if (settings.Contains(item.Id))
                {
                    settings[item.Id] = newNote;
                }
                else
                {
                    settings.Add(item.Id, newNote);
                }

                settings.Save();

                App.ViewModel.LoadData(item.Idx, item.Id);
            }
            else
            {
                if (!newNote.Equals(ClickToAdd) && !newNote.Equals(NoteSaved))
                {
                    string key = string.Format("Note_{0}", Guid.NewGuid());

                    settings.Add(key, newNote);
                    settings.Save();

                    App.ViewModel.LoadData(key);
                }
            }
        }

        private void Notorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.RemovedItems.Count == 1)
            {
                var from = Notorama.Items.IndexOf(e.RemovedItems[0]);
                
                if (from == 1)
                {
                    var item = Note.Tag as NoteModel;
                    SaveItem(item);
                }
            }
        }

        private void Note_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Note.Text.Equals(NoteSaved) ||
                Note.Text.Equals(ClickToAdd))
            {
                Note.Text = string.Empty;
            }
        }

        private void RemoveAll_OnTap(object sender, GestureEventArgs e)
        {
            popItUp(AppResources.ApplicationTitle, AppResources.DeleteConfirmation);
           
        }

        private Popup _p;

        private void popItUp(string header, string text)
        {
            _p = new Popup();

            var border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x6A, 0xF1));
            border.BorderThickness = new Thickness(5.0);

            var panel1 = new StackPanel();
            panel1.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x8A, 0xFF));

            var buttonOk = new Button();
            buttonOk.Content = "Yes";
            buttonOk.Margin = new Thickness(4.0);
            buttonOk.FontSize = 24;
            buttonOk.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x6A, 0xF1));
            buttonOk.Click += buttonOk_Click;

            var buttonNo = new Button();
            buttonNo.Content = "No";
            buttonNo.Margin = new Thickness(4.0);
            buttonNo.FontSize = 24;
            buttonNo.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x6A, 0xF1));
            buttonNo.Click += buttonNo_Click;

            var textblockH = new TextBlock();
            textblockH.Text = header;
            textblockH.FontSize = 30;
            textblockH.Margin = new Thickness(6.0);
            textblockH.Foreground = new SolidColorBrush(Colors.White);

            var textblockT = new TextBlock();
            textblockT.Text = string.Concat(string.Format(text, Environment.NewLine), Environment.NewLine);
            textblockT.FontSize = 26;
            textblockT.Margin = new Thickness(6.0);
            textblockT.Foreground = new SolidColorBrush(Colors.White);


            panel1.Children.Add(textblockH);
            panel1.Children.Add(textblockT);
            panel1.Children.Add(buttonOk);
            panel1.Children.Add(buttonNo);
            border.Child = panel1;

            // Set the Child property of Popup to the border
            // which contains a stackpanel, textblock and button.
            _p.Child = border;

            // Set where the popup will show up on the screen.
            _p.VerticalOffset = 150;
            _p.HorizontalOffset = 50;

            // Open the popup.
            _p.IsOpen = true;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Count > 0)
            {
                settings.Clear();
                settings.Save();
                App.ViewModel.LoadData();
            }

            _p.IsOpen = false;
        }

        private void buttonNo_Click(object sender, RoutedEventArgs e)
        {
            _p.IsOpen = false;
        }
    }
}