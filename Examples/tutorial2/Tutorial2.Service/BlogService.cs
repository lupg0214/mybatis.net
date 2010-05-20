using System;
using System.Collections;
using IBatisNet.DataMapper;
using Tutorial2.Domain;

namespace Tutorial2.Service
{
    public class BlogService : IBlogService
    {
        private ISqlMapper _mapper = null;



        /// <summary>
        /// Initializes a new instance of the <see cref="BlogService"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        public BlogService(ISqlMapper mapper)
        {
            _mapper = mapper;
        }
        
        #region IBlogService Members

        /// <summary>
        /// Check for Author.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public Author Verify(string login, string password)
        {
            Hashtable map = new Hashtable();
            map.Add("Login", login);
            map.Add("Password", password);

            return _mapper.QueryForObject<Author>("Author-Verify", map); ;
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
            _mapper.Insert("Post-Insert", post);
        }
        #endregion
    }
}
