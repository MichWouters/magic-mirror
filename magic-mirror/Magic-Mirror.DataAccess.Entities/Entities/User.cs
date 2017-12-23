using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Entities.Entities
{
    public class User
    {
        private int _userId;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _street;

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        private string _zipCode;

        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        private string _city;

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _workAddress;

        public string WorkAddress
        {
            get { return _workAddress; }
            set { _workAddress = value; }
        }
    }
}