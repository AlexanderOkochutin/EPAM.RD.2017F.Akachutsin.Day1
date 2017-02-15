using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.CustomExceptions
{
    public class AlreadyExistException : UserServiceException
    {
        public AlreadyExistException()
        {
        }

        public AlreadyExistException(string message) : base(message)
        {
        }
    }
}
