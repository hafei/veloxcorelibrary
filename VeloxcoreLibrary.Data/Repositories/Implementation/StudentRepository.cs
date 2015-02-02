using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeloxcoreLibrary.Data.Infrastructure;
using VeloxcoreLibrary.Model;

namespace VeloxcoreLibrary.Data.Repositories.Implementation
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DataBaseContext context) : base(context) { }

        protected override object[] GetKey(Student entity)
        {
            return new object[] { entity.ID };
        }

        public PagedResult<Student> GetStudentByName(string name)
        {
            return this.GetPage(1, 20, orderBy: s => s.OrderBy(o => o.FirstName).ThenBy(o => o.LastName),
                filter: s => s.FirstName == name || s.LastName == name);
        }
    }
}
