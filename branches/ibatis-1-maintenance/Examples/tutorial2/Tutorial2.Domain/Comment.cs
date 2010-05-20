namespace Tutorial2.Domain
{
    public class Comment : EntityBase 
    {
        private string _text = string.Empty;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
