namespace Posts.Service
{
    public interface IPostRepository
    {
        public Task<List<Post>> GetAllPosts();
    }
}
