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

        public Attachment Add(Attachment attachment)
        {
            Context.Attachments.Add(attachment);
            Context.SaveChanges();
            return attachment;
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

        public CommentModel AddComment(CommentModel comment)
        {
            Context.CommentModels.Add(comment);
            Context.SaveChanges();
            return comment;
        }
        public IEnumerable<CommentModel> GetAllComments(int taskId)
        {
            var comments = Context.CommentModels.Where(x => x.TaskId == taskId).OrderBy(x => x.TimePosted);
            return comments;
        }

        public TaskModel Edit(TaskModel taskModelChanges)
        {
            var taskModel = Context.TaskModels.Update(taskModelChanges);
            taskModel.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Context.SaveChanges();
            return taskModelChanges;
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
