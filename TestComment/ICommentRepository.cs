namespace Comments.Service
{
    public interface ICommentRepository
    {
        public Task<List<Comment>> GetAllComments();
    }
}
