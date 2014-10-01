using DevExpress.Mvvm;
using Mailbird.Apps.Calendar.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mailbird.Apps.Calendar.UIModels
{
   public class ContactUI : NotificationObject
    {
       public string _email;
       public string _firstName;
       public string _lastname;
       public string _profileImgPath;



        #region Binding Properties

        public string Email 
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        public string LastName
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        public string ProfileImgPath
        {
            get { return _profileImgPath; }
            set
            {
                _profileImgPath = value;
                RaisePropertyChanged(() => ProfileImgPath);
            }
        }


        #endregion



        #region Contructor(s)

        public ContactUI()
            :base()
        {}

        #endregion
    }
}
