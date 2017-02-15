using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.CustomExceptions;
using UserServiceLibrary.Entities;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Services
{
    public class UserService:IUserService
    {
        private IEqualityComparer<User> equalityComparer;
        private Func<int> idGenerator;
        private ICollection<User> users;

        /// <summary>
        /// Constructor for <see cref="UserService"/>>
        /// </summary>
        /// <param name="idGenerator">Delegate for generating unique id</param>
        /// <param name="equalityComparer">comparator for <see cref="User"/> equality check</param>
        public UserService(Func<int> idGenerator = null, IEqualityComparer<User> equalityComparer = null)
        {
            int counter = 0;
            this.idGenerator = idGenerator??(()=>counter++);
            this.equalityComparer = equalityComparer??EqualityComparer<User>.Default;
            users = new List<User>();
        }

        /// <inheritdoc cref="IUserService.Add"/>
        public void Add(User user)
        {
            if(user == null) throw new ArgumentNullException();
            if(user.BirthDate == null || user.FirstName == null || user.LastName == null) throw new NotInitializedException();
            if(users.Contains(user,equalityComparer)) throw new AlreadyExistException();
            user.Id = idGenerator();
            users.Add(user);
        }

        /// <inheritdoc cref="IUserService.Delete"/>
        public void Delete(User user)
        {
            if(user == null) throw new ArgumentNullException();
            if(!users.Contains(user,equalityComparer)) throw new NotExistException();
            users.Remove(user);
        }

        /// <inheritdoc cref="IUserService.Search"/>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null) throw  new ArgumentNullException();

            return users.Where(u=>predicate(u));
        }
    }
}
