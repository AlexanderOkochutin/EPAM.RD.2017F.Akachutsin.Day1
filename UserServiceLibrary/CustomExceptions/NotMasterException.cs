using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceLibrary.CustomExceptions
{
    public class NotMasterException : Exception
    {
        public NotMasterException()
        {
        }

        public NotMasterException(string message) : base(message)
        {
        }

        public NotMasterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
