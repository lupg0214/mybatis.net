using NUnit.Framework;
using Tutorial2.Application;
using Tutorial2.Domain;
using Tutorial2.Service;

namespace Tutorial2.Test
{
    [TestFixture]
    public class TestManualPresenterMock
    {
        protected IBlogService service = null;
        protected BlogPresenter presenter = null;
        protected IBlogView view = null;
        
        [SetUp]
        public void SetUp()
        {
            view = new BlogViewMock();
            service = new BlogServiceMock();
            presenter = new BlogPresenter(view, service);
        }

        [Test]
        public void Verify()
        {
            presenter.Verify("ibatis","rock") ;

            Assert.IsTrue(view.Author.Name == "Gilles");
        }


        [Test]
        public void CreatePost()
        {
            view.Author = new Author("Gilles");
            Blog blog = new Blog();
            blog.Name = "iBATIS Rock";
            blog.Posts.Add(new Post());
            view.Author.Blog = blog;
            view.Blog = blog;

            presenter.CreatePost("MySpace - Powered by iBATIS", "MySpace.com is now running iBATIS for a good portion of its data access abstraction layer.");

            Assert.IsTrue(view.Blog.Posts.Count == 2);
        }
    }
}
