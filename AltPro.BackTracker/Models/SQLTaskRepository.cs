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

        public FileAttachment Add(FileAttachment attachment)
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

        public IEnumerable<TaskModel> GetAlLUserTasks(string Id)
        {
            return Context.TaskModels.Where(s => s.ReporterID.Equals(Id) || s.AssignedID.Equals(Id));
        }

        public TaskModel GetTask(int id)
        {
            TaskModel task = Context.TaskModels.Find(id);
            return task;
        }

        public IEnumerable<string> GetAllAttachmentsPaths(int Id)
        {
            var paths = Context.Attachments.Where(e => e.TaskId.Equals(Id)).Select(e => $"{e.Path}");
            return paths;
        }

        public IEnumerable<string> GetAllAttachmentsNames(int Id)
        {
            var names = Context.Attachments.Where(e => e.TaskId.Equals(Id)).Select(e => $"{e.Name}");
            return names;
        }

        public Dictionary<string, string> GetAttachmentsStrings(int Id)
        {
            var strings = Context.Attachments.Where(e => e.TaskId.Equals(Id)).Select(e => new {name = e.Name, path = e.Path });
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var obj in strings){
                dict.Add(obj.name, obj.path);
            }
            return dict;
        }
    }
}
