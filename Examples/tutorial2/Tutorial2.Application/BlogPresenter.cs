using System.ComponentModel;
using Tutorial2.Domain;
using Tutorial2.Service;

namespace Tutorial2.Application
{
    public class BlogPresenter
    {
        private readonly IBlogService _service = null;
        private readonly IBlogView _view = null;
         

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="service">The service.</param>
        /// <remarks>
        /// Use "Dependency Injection" to attach the dependencies for the view and service.
        /// </remarks>
        public BlogPresenter(IBlogView view, IBlogService service)
        {
            _view = view;
            _service = service;
        }

        /// <summary>
        /// Check for User.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        public void Verify(string login, string password)
        {
            _view.Author = _service.Verify(login, password);
        }



        /// <summary>
        /// Creates the blog.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public void CreateBlog(string name, string description)
        {
            Blog blog = new Blog();
            blog.Name = name;
            blog.Description = description;

            _service.SaveBlog(blog);
            
            _view.Blog = blog;
        }

        /// <summary>
        /// Creates the post.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        public void CreatePost(string title, string text)
        {
            Post post = new Post();
            post.Title = title;
            post.Content = text;
            post.Blog = _view.Blog;

            _service.SavePost(post);
            
            _view.Blog.Posts.Add(post);
        }
    }
}
