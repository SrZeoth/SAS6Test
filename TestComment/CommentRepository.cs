namespace Comments.Service
{
    public class CommentRepository : ICommentRepository
    {
        private readonly List<Comment> comments = new List<Comment>();
        public CommentRepository()
        {
            comments.Add(new Comment()
            {
                Id = 1,
                Body = "Test message 1"
            });

            comments.Add(new Comment()
            {
                Id = 2,
                Body = "Test message 2"
            });
        }
        public Task<List<Comment>> GetAllComments()
        {
            return Task.FromResult(comments);
        }
    }
}
