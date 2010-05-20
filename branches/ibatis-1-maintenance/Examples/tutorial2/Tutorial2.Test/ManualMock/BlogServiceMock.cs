using Tutorial2.Domain;
using Tutorial2.Service;

namespace Tutorial2.Test
{
    public class BlogServiceMock : IBlogService
    {
        #region IBlogService Members

        /// <summary>
        /// Check for Author.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public Author Verify(string login, string password)
        {
            Author author = new Author("Gilles");
            author.Login = "ibatis";
            author.Password = "rock";
            
            Blog blog = new Blog();
            blog.Name = "iBATIS Blog";
            
            Post post = new Post();
            post.Title = "iBATSI Drives MySpace :-)";
            post.Content = "...";
            
            blog.Posts.Add(post);

            author.Blog = blog;

            return author;
        }

        /// <summary>
        /// Saves the blog.
        /// </summary>
        /// <param name="blog">The blog.</param>
        public void SaveBlog(Blog blog)
        {
        }
        
        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="post">The post.</param>
        public void SavePost(Post post)
        {
            
        }

        #endregion
    }
}
