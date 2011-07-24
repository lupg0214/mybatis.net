using MyBatis.DataMapper;
using MyBatis.DataMapper.MappedStatements;

namespace MyBatis.DataMapper.SqlClient.Test.Domain
{
    /// <summary>
    /// Sample implemantation of <see cref="ISqlSource"/>
    /// </summary>
    public class SqlSourceWithInlineParameter : ISqlSource
    {
        #region ISqlSource Members

        /// <summary>
        /// Gets the SQL text.
        /// </summary>
        /// <param name="mappedStatement">The mapped statement.</param>
        /// <param name="parameterObject">The parameter object.</param>
        /// <returns></returns>
        public string GetSql(IMappedStatement mappedStatement, object parameterObject)
        {
            return "select * from Accounts where Account_ID = @{Id}";
        }

        #endregion
    }
}
