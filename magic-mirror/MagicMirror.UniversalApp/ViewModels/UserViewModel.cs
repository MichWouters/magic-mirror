using MagicMirror.Business.Models.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class UserViewModel: ViewModelBase
    {
        private static void LoadUser(UserProfileModel userProfile)
        {
            _id = userProfile.Id;
            _personId = userProfile.PersonId;
            _faceIds = new ObservableCollection<Guid>(userProfile.FaceIds);
        }
        private static Guid _id;
        private static Guid _personId;
        private static ObservableCollection<Guid> _faceIds;

        public Guid Id
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public Guid PersonId
        {
            get
            {
                return _personId;
            }
            private set
            {
                _personId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Guid> FaceId
        {
            get
            {
                return _faceIds;
            }
            private set
            {
                _faceIds = value;
                OnPropertyChanged();
            }
        }
    }
}
