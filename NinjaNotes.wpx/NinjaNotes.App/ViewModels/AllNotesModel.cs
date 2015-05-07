using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using NinjaNotes.App.Resources;

namespace NinjaNotes.App.ViewModels
{
    public class AllNotesModel : INotifyPropertyChanged
    {
        public AllNotesModel()
        {
            this.Items = new ObservableCollection<NoteModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<NoteModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            this.Items.Clear();
            foreach (var s in settings)
            {
                if (s.Key.StartsWith("Note_"))
                {
                    this.Items.Add(new NoteModel(s.Key, s.Value.ToString()));
                }
            }

            if (settings.Count == 0)
            {
                string key = string.Format("Note_{0}", Guid.NewGuid());
                string value = AppResources.DemoNote;
                settings.Add(key, value);

                settings.Save();
                LoadData();
            }

            this.IsDataLoaded = true;
        }

        public void LoadData(int idx, string id)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            string note;
            if (settings.TryGetValue(id, out note))
            {
                this.Items[idx] = new NoteModel(id, note);
            }

            this.IsDataLoaded = true;
        }

        public void LoadData(string id)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            string note;
            if (settings.TryGetValue(id, out note))
            {
                this.Items.Add(new NoteModel(id, note));
            }

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}