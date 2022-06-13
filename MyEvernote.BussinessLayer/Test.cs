using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinessLayer
{
    public class Test
    {

        private Repository<EvernoteUser> repo_user = new Repository<EvernoteUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Comment> repo_comment = new Repository<Comment>();
        private Repository<Note> repo_note = new Repository<Note>();
        public Test()
        {
            DatabaseContext db = new DatabaseContext();
            db.Database.CreateIfNotExists();
        }

        public void InsertTest()
        {

        int result = repo_user.Insert(new EvernoteUser()
            {
                Name = "testname2",
                Surname = "testsurname2",
                Email = "test2@gmail.com",
                ActiviteGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "test2123",
                Password = "12345",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "testname2"
            });
        }
        public void UpdateTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "test2123");
            if (user != null)
            {
                user.Username = "TestUserNameUpdated";
                int result = repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            EvernoteUser user = repo_user.Find(x => x.Username == "TestUserNameUpdated");
            if (user != null)
            {
                int result = repo_user.Delete(user);
            }
        }
    }
}
