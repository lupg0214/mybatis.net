

namespace MyBatis.Common.Test.Domain
{
    public interface IUser : IBaseDomain
    {
        IAddress Address { get; set; }
    } 
}
