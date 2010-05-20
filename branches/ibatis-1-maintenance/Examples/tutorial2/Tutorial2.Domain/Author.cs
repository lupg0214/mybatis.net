namespace Tutorial2.Domain
{
    public class Author : EntityBase 
    {
        private string _name = string.Empty;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private Blog _blog = null;

        public Author()
        {
        }
        
        public Author(string name)
        {
            _name = name;
        }
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Blog Blog
        {
            get { return _blog; }
            set { _blog = value; }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
