using System;
using System.Collections.Generic;

namespace Tutorial2.Domain
{
    public class Post : EntityBase 
    {
        private IList<Comment> _comments = new List<Comment>();
        private string _title = string.Empty;
        private string _content = string.Empty;
        private Blog _blog = null;
        private DateTime _date = DateTime.Now;

        public IList<Comment> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public Blog Blog
        {
            get { return _blog; }
            set { _blog = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
    }
}
