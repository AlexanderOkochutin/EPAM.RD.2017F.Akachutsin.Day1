using System;

namespace UserServiceLibrary.Entities
{
    /// <summary>
    /// User entity class, contains data about user 
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unic Id for each user
        /// </summary>
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

    }
}
