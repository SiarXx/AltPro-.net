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

        FileAttachment Add(FileAttachment attachment);

        TaskModel Edit(TaskModel taskModel);

        void Delete(int id);

        public IEnumerable<CommentModel> GetAllComments(int taskId);

        public IEnumerable<string> GetAllAttachmentsPaths(int Id);

        public IEnumerable<string> GetAllAttachmentsNames(int Id);

        public IEnumerable<TaskModel> GetAlLUserTasks(string Id);

        public Dictionary<string, string> GetAttachmentsStrings(int Id);

        public CommentModel AddComment(CommentModel comment);
    }
}
