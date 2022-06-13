using MyEvernote.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinessLayer
{
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        protected static object _lockSync = new object();
        protected RepositoryBase()
        {
            CreateContext();
        }
        public static DatabaseContext CreateContext()
        {
            if (context == null)
            {

                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                }
            }
            return context;
        }
    }
}
