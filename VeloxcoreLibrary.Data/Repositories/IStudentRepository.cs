using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloxcoreLibrary.Data.Infrastructure;
using VeloxcoreLibrary.Model;

namespace VeloxcoreLibrary.Data.Repositories
{
public interface IStudentRepository : IRepository<Student>
{
    PagedResult<Student> GetStudentByName(string name);
}
}
