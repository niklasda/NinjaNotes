using System;
using System.ComponentModel;

namespace NinjaNotes.App.ViewModels
{
    public class NoteModel : INotifyPropertyChanged
    {
        public NoteModel(string id, string note)
        {
            _id = id;
            TheNote = note;
        }

        private readonly string _id;
        /// <summary>
        /// Note Id; 
        /// </summary>
        /// <returns></returns>
        public string Id
        {
            get
            {
                return _id;
            }
        }

        private int _idx;
        /// <summary>
        /// Note Idx; index in list used temporarily when editing
        /// </summary>
        /// <returns></returns>
        public int Idx
        {
            get
            {
                return _idx;
            }
            set
            {
                _idx = value;
            }
        }

        /// <summary>
        /// First line of note, shown in list.
        /// </summary>
        /// <returns></returns>
        public string FirstLine
        {
            get
            {
                string lineOne;
                int nl = _note.IndexOf(Environment.NewLine, StringComparison.CurrentCultureIgnoreCase);
                if (nl > 0)
                {
                    lineOne = _note.Substring(0, nl);
                }
                else
                {
                    nl = _note.IndexOf("\r", StringComparison.CurrentCultureIgnoreCase);
                    if (nl > 0)
                    {
                        lineOne = _note.Substring(0, nl);
                    }
                    else
                    {
                        lineOne = _note;
                    }
                }

                return lineOne;
            }
        }

        ///// <summary>
        ///// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        ///// </summary>
        ///// <returns></returns>
        //public string RestOfLines
        //{
        //    get
        //    {
        //        string lineTwo;
        //        int nl = _note.IndexOf(Environment.NewLine, StringComparison.CurrentCultureIgnoreCase);
        //        if (nl > 0)
        //        {
        //            lineTwo = _note.Substring(nl + 2);
        //        }
        //        else
        //        {
        //            nl = _note.IndexOf("\r", StringComparison.CurrentCultureIgnoreCase);
        //            if (nl > 0)
        //            {
        //                lineTwo = _note.Substring(nl + 1);
        //            }
        //            else
        //            {
        //                lineTwo = _note;
        //            }
        //        }

        //        return lineTwo;
        //    }
        //}

        private string _note;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string TheNote
        {
            get
            {
                return _note;
            }
            private set
            {
                if (value != _note)
                {
                    _note = value;
                    NotifyPropertyChanged("TheNote");
                }
            }
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