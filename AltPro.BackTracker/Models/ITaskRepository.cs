using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public interface ITaskRepository
    {
        TaskModel GetTask(int id);

        IEnumerable<TaskModel> GetAllTasks();

        TaskModel Add(TaskModel taskModel);

        TaskModel Edit(TaskModel taskModel);

        void Delete(int id);
    }
}
