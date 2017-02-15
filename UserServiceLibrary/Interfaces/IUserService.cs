using System;
using System.Collections.Generic;
using UserServiceLibrary.CustomExceptions;
using UserServiceLibrary.Entities;

namespace UserServiceLibrary.Interfaces
{
    /// <summary>
    /// Service class for users CRUD operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Method for adding new user to the service
        /// </summary>
        /// <param name="user">New user which add to the service</param>
        /// <exception cref="NotImplementedException">Tthrow when <paramref name="user"/> has not initialized fields</exception>
        /// <exception cref="AlreadyExistException">Throw when such <paramref name="user"/> already exist</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="user"/> is null</exception>
        void Add(User user);

        /// <summary>
        /// Method for deleting exist user from service
        /// </summary>
        /// <param name="user">Exist user in service</param>
        /// <exception cref="NotExistException">Throw when such <paramref name="user"/> not exist</exception>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="user"/> is null</exception>
        void Delete(User user);

        /// <summary>
        /// Method for searching exist users by predicate
        /// </summary>
        /// <param name="predicate">predicate for users compliance check to the search request</param>
        /// <returns>Enumeration of <see cref="User"/>>></returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="predicate"/> is null</exception>
        IEnumerable<User> Search(Predicate<User> predicate);
    }
}