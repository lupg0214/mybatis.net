using System.ComponentModel;
using Tutorial2.Application;
using Tutorial2.Domain;

namespace Tutorial2.Test
{
    public class BlogViewMock : IBlogView
    {
        private Author _author = null;
        private Blog _blog= null;
        private BindingList<Post> _posts = new BindingList<Post>();
       
        #region IBlogView Members

        public Author Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public Blog Blog
        {
            get { return _blog; }
            set { _blog = value; }
        }

        public BindingList<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        #endregion
    }
}
