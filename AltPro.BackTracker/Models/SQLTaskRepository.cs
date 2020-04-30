using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class SQLTaskRepository : ITaskRepository
    {
        public TaskModel Add(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        public TaskModel Delete(int id)
        {
            throw new NotImplementedException();
        }

        public TaskModel Edit(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public TaskModel GetTask(int id)
        {
            throw new NotImplementedException();
        }
    }
}
