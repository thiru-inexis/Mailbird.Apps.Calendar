using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mailbird.Apps.Calendar.Engine.Metadata;
using Mailbird.Apps.Calendar.Infrastructure;
using Mailbird.Apps.Calendar.Engine.Enums;
using System.Windows.Media;

namespace Mailbird.Apps.Calendar.UIModels
{
    public class CalenderUI : NotificationObject
    {
        private Engine.Metadata.Calendar _bm;

        public Engine.Metadata.Calendar BaseModel
        {
            get { return _bm; }
        }



        public string Id 
        {
            get { return _bm.Id; }
            set
            {
                _bm.Id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Summary
        {
            get { return _bm.Summary; }
            set
            {
                _bm.Summary = value;
                RaisePropertyChanged(() => Summary);
            }
        }

        public string Description
        {
            get { return _bm.Description; }
            set
            {
                _bm.Description = value;
                RaisePropertyChanged(() => Description);
            }
        }
    
        public string Location
        {
            get { return _bm.Location; }
            set
            {
                _bm.Location = value;
                RaisePropertyChanged(() => Location);
            }
        }
        
        //public string TimeZone
        //{
        //    get { return _bm.TimeZone; }
        //    set
        //    {
        //        _bm.TimeZone = value;
        //        RaisePropertyChanged(() => TimeZone);
        //    }
        //}


        public Access AccessRole
        {
            get { return _bm.CalenderList.AccessRole; }
            set
            {
                _bm.CalenderList.AccessRole = value;
                RaisePropertyChanged(() => AccessRole);
            }
        }

        public string ColorId
        {
            get { return _bm.CalenderList.ColorId; }
            set
            {
                _bm.CalenderList.ColorId = value;
                RaisePropertyChanged(() => ColorId);
            }
        }

        public string BackgroundColor
        {
            get { return _bm.CalenderList.BackgroundColor; }
            set
            {
                _bm.CalenderList.BackgroundColor = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        public string ForegroundColor
        {
            get { return _bm.CalenderList.ForegroundColor; }
            set
            {
                _bm.CalenderList.ForegroundColor = value;
                RaisePropertyChanged(() => ForegroundColor);
            }
        }


        public bool IsDeleted
        {
            get { return _bm.CalenderList.IsDeleted; }
        }

        public bool IsHidden
        {
            get { return _bm.CalenderList.IsHidden; }
            set
            {
                _bm.CalenderList.IsHidden = value;
                RaisePropertyChanged(() => IsHidden);
            }
        }

        public bool IsPrimary
        {
            get { return _bm.CalenderList.IsPrimary; }
        }

        public bool IsSelected
        {
            get { return _bm.CalenderList.IsSelected; }
            set
            {
                _bm.CalenderList.IsSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }



        public CalenderUI()
            :base()
        {
            this._bm = new Engine.Metadata.Calendar();
        }


        public CalenderUI(Engine.Metadata.Calendar basemodel)
            : base()
        {
            this._bm = basemodel;
        }



    }
}
