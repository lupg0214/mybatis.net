using Tutorial2.Domain;

namespace Tutorial2.Service
{
    public interface IBlogService
    {

        /// <summary>
        /// Check for Author.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Author Verify(string login, string password);

        /// <summary>
        /// Saves the blog.
        /// </summary>
        /// <param name="blog">The blog.</param>
        void SaveBlog(Blog blog);
        
        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="post">The post.</param>
        void SavePost(Post post);
    }
}
