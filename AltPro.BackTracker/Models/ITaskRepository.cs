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

        Attachment Add(Attachment attachment);

        TaskModel Edit(TaskModel taskModel);

        void Delete(int id);

        public IEnumerable<CommentModel> GetAllComments(int taskId);

        public IEnumerable<string> GetAllAttachments(int Id);

        public CommentModel AddComment(CommentModel comment);
    }
}
