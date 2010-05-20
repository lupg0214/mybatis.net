namespace Tutorial2.Domain
{
    public abstract class EntityBase
    {
        protected System.Nullable<int> _id = null;

        public System.Nullable<int> Id
        {
            get { return _id; }
        }
    }
}
