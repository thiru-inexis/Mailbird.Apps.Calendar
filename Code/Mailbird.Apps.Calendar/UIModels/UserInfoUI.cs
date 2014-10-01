using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Infrastructure;
using Mailbird.Apps.Calendar.Engine.Enums;
using System.Collections.ObjectModel;

namespace Mailbird.Apps.Calendar.UIModels
{
    public class UserInfoUI : NotificationObject
    {
        private Engine.Metadata.UserInfo _bm;

        public Engine.Metadata.UserInfo BaseModel
        {
            get { return _bm; }
        }


        #region Binding Properties

        public string Id 
        {
            get { return _bm.Id; }
            private set
            {
                _bm.Id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Username
        {
            get { return _bm.Username; }
            set
            {
                _bm.Username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        public string BackgroundColor
        {
            get { return _bm.Color.Background; }
            set
            {
                _bm.Color.Background = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        public string ForegroundColor
        {
            get { return _bm.Color.Foreground; }
            set
            {
                _bm.Color.Foreground = value;
                RaisePropertyChanged(() => ForegroundColor);
            }
        }

        public bool IsSelected
        {
            get { return _bm.IsSelected; }
            set
            {
                _bm.IsSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }


        public ObservableCollection<CalenderUI> Calendars { get; set; }

        #endregion



        #region Contructor(s)

        public UserInfoUI()
            :base()
        {
            _bm = new UserInfo();
            Calendars = new ObservableCollection<CalenderUI>();
        }


        public UserInfoUI(Engine.Metadata.UserInfo basemodel)
            : base()
        {
            this._bm = basemodel;
            Calendars = new ObservableCollection<CalenderUI>();
        }


        #endregion
    }
}
