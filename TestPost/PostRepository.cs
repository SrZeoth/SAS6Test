namespace Posts.Service
{
    public class PostRepository : IPostRepository
    {
        private readonly List<Post> posts = new List<Post>();
        public PostRepository()
        {
            posts.Add(new Post
            {
                Id = 1,
                Title = "Post 1"
            });

            posts.Add(new Post
            {
                Id = 2,
                Title = "Post 2"
            });

            posts.Add(new Post
            {
                Id = 3,
                Title = "Post 3"
            });
        }
        public Task<List<Post>> GetAllPosts()
        {
            return Task.FromResult(posts);
        }
    }
}
