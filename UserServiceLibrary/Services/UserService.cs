using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.CustomExceptions;
using UserServiceLibrary.Entities;
using UserServiceLibrary.Interfaces;

namespace UserServiceLibrary.Services
{
    public class UserService: MarshalByRefObject,IUserService
    {
        public IAddRemoveNotifier notifier;
        private IEqualityComparer<User> equalityComparer;
        private Func<int> idGenerator;
        public  ICollection<User> users;
        private bool isMaster = false;
        /// <summary>
        /// Constructor for <see cref="UserService"/>>
        /// </summary>
        /// <param name="idGenerator">Delegate for generating unique id</param>
        /// <param name="equalityComparer">comparator for <see cref="User"/> equality check</param>
        public UserService(bool isMaster,HashSet<User> users ,Func<int> idGenerator = null, IEqualityComparer<User> equalityComparer = null)
        {
            this.isMaster = isMaster;
            int counter = 0;
            this.idGenerator = idGenerator??(()=>counter++);
            this.equalityComparer = equalityComparer??EqualityComparer<User>.Default;
            this.users = users;
            notifier = new AddRemoveNotifier();
        }

        /// <inheritdoc cref="IUserService.Add"/>
        public void Add(User user)
        {
            if(!isMaster) throw new NotMasterException();
            if(user == null) throw new ArgumentNullException();
            if(user.BirthDate == null || user.FirstName == null || user.LastName == null) throw new NotInitializedException();
            if(users.Contains(user,equalityComparer)) throw new AlreadyExistException();
            user.Id = idGenerator();
            users.Add(user);
            notifier.AddNotification(this,new UserEventArgs(user));
        }

        /// <inheritdoc cref="IUserService.Delete"/>
        public void Delete(User user)
        {
            if (!isMaster) throw new NotMasterException();
            if(user == null) throw new ArgumentNullException();
            if(!users.Contains(user,equalityComparer)) throw new NotExistException();
            users.Remove(user);
            notifier.RemoveNotification(this,new UserEventArgs(user));
        }

        /// <inheritdoc cref="IUserService.Search"/>
        public IEnumerable<User> Search(Predicate<User> predicate)
        {
            if (predicate == null) throw  new ArgumentNullException();

            return users.Where(u=>predicate(u));
        }

        public void TcpAddToSlave(object o, EventArgs args)
        {
            Console.WriteLine("Users update by TCP");
            TcpClient client = null;
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                NetworkStream stream = client.GetStream();
                new BinaryFormatter().Serialize(stream, this.users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                client?.Close();
            }
            Console.WriteLine("Master update storage");
        }
    }
}
