using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using EducationalSystem.DAL.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalSystem.BLL.Repositories.Repositories
{
    public class LessonRepository : GenericReposiory<Lessons> , ILessonRepository
    {
        public LessonRepository(Education_System dbcontext) : base(dbcontext)
        {
        }


    }
}
