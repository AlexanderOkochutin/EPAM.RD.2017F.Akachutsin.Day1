using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Entities;

namespace UserServiceLibrary.Services
{
    internal class UserEventArgs : EventArgs
    {
        public User User { get; set; }

        public UserEventArgs(User user)
        {
            User = user;
        }
    }
}
