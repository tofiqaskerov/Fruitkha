using Core.DataAccess.EntityFramework;
using Core.Entity.Models;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntitieFramework
{
    public class UserDal : EfEntityRepositoryBase<User, AppDbContext>, IUserDal
    {
    }
}
