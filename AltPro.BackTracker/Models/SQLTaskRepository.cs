using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltPro.BackTracker.Models
{
    public class SQLTaskRepository : ITaskRepository
    {
        private readonly AppDBContext Context;

        public SQLTaskRepository(AppDBContext context)
        {
            this.Context = context;
        }
        public TaskModel Add(TaskModel taskModel)
        {
            Context.TaskModels.Add(taskModel);
            Context.SaveChanges();
            return taskModel;
        }

        public void Delete(int id)
        {
            TaskModel task = Context.TaskModels.Find(id);
            if (task != null)
            {
                Context.TaskModels.Remove(task);
                Context.SaveChanges();
            }
        }

        public TaskModel Edit(TaskModel taskModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            return Context.TaskModels;
        }

        public TaskModel GetTask(int id)
        {
            TaskModel task = Context.TaskModels.Find(id);
            return task;
        }
    }
}
