using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MyFace.Models.Database;
using MyFace.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MyFace.Repositories
{
    public interface IUsersRepo
    {
        IEnumerable<User> Search(UserSearchRequest search);
        int Count(UserSearchRequest search);
        User GetById(int id);
        User Create(CreateUserRequest newUser);
        User Update(int id, UpdateUserRequest update);
        void Delete(int id);
    }

    public class UsersRepo : IUsersRepo
    {
        private readonly MyFaceDbContext _context;

        public UsersRepo(MyFaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Search(UserSearchRequest search)
        {
            return _context.Users
                .Where(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ))
                .OrderBy(u => u.Username)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(UserSearchRequest search)
        {
            return _context.Users
                .Count(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ));
        }

        public User GetById(int id)
        {
            return _context.Users
                .Single(user => user.Id == id);
        }

        public User Create(CreateUserRequest newUser)
        {
            byte[] salt = GetSalt();
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User> insertResponse = _context.Users.Add(new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Username = newUser.Username,
                ProfileImageUrl = newUser.ProfileImageUrl,
                CoverImageUrl = newUser.CoverImageUrl,
                Salt = salt,
                Password = HashPassword(newUser.Password, salt)
            });
            _context.SaveChanges();

            return insertResponse.Entity;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public User Update(int id, UpdateUserRequest update)
        {
            User user = GetById(id);

            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.Username = update.Username;
            user.Email = update.Email;
            user.ProfileImageUrl = update.ProfileImageUrl;
            user.CoverImageUrl = update.CoverImageUrl;

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            User user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .Single(user => user.Username == username);
        }

        public bool UserHasAccess(string authHeader)
        {
            string[] headerUsernamePassword = DecodeFrom64(authHeader).Split(":");
            User user = GetByUsername(headerUsernamePassword[0]);

            return user != null && user.Password == HashPassword(headerUsernamePassword[1], user.Salt);
        }
    }
}