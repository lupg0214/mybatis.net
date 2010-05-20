using System;
using System.Collections.Generic;

namespace Tutorial2.Domain
{
    public class Blog : EntityBase 
    {
        private string _name = string.Empty;
        protected string _description = string.Empty;
        private Author _author = null;
        private IList<Post> _posts = new List<Post>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Author Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public IList<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }
    }
}
