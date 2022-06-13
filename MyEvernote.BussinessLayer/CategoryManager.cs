using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            foreach (Note note in category.Notes.ToList()) 
            {
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }
                noteManager.Delete(note);

                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }
                noteManager.Delete(note);
            }
            return base.Delete(category);
        }
    }
}
