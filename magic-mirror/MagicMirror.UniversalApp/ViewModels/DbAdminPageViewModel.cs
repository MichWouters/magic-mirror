using MagicMirror.Business.Models.User;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using MagicMirror.DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.ViewModels
{
    public class DbAdminPageViewModel: ViewModelBase
    {
        private ObservableCollection<UserProfileModel> _users;
        private UserService _userService;

        public DbAdminPageViewModel()
        {
            //_userService = App.GetSingleton<UserService>();
        }

        public ObservableCollection<UserProfileModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public void GetUsersAsync()
        {
            _users = new ObservableCollection<UserProfileModel>(_userService.GetAllUsers());
        }

    }
}
