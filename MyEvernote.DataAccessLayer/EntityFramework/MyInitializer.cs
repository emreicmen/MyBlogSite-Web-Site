using MyEvernote.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists <DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Emre",
                Surname = "Icmen",
                Email = "icmenemre@gmail.com",
                ActiviteGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "icmenemre",
                ProfileImageFileName = "userProfileImage.png",
                Password = "12345",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "icmenemre"
            };

            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Testname",
                Surname = "Testsurname",
                Email = "test@gmail.com",
                ActiviteGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "test123",
                ProfileImageFileName = "userProfileImage.png",
                Password = "12345",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "icmenemre"
            };

            context.EvernoteUser.Add(admin);
            context.EvernoteUser.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActiviteGuid = Guid.NewGuid(),
                    ProfileImageFileName = "userProfileImage.png",
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "12345",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.EvernoteUser.Add(user);
            }

            context.SaveChanges();
            List<EvernoteUser> userList = context.EvernoteUser.ToList();
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "icmenemre"
                };
                context.Categories.Add(cat);

                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.PlaceData.GetCity(),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                        NotePicture = "https://dummyimage.com/320x150/0035d4/ff0000.jpg"
                    };

                    cat.Notes.Add(note);

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        EvernoteUser comment_owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username,
                        };
                        note.Comments.Add(comment);
                    }

                    for (int m = 0; m < note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userList[m]
                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
