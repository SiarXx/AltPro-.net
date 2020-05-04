using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class SQLTaskRepository : ITaskRepository
    {
        private readonly AppDBContext context;

        public SQLTaskRepository(AppDBContext context)
        {
            this.context = context;
        }
        public TaskModel Add(TaskModel taskModel)
        {
            context.TaskModels.Add(taskModel);
            context.SaveChanges();
            return taskModel;
        }

        public void Delete(int id)
        {
            TaskModel task = context.TaskModels.Find(id);
            if (task != null)
            {
                context.TaskModels.Remove(task);
                context.SaveChanges();
            }
        }

        public TaskModel Edit(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            return context.TaskModels;
        }

        public TaskModel GetTask(int id)
        {
            TaskModel task = context.TaskModels.Find(id);
            return task;
        }
    }
}
