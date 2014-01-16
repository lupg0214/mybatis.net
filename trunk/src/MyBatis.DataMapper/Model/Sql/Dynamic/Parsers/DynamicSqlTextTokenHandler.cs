using System;
using MyBatis.DataMapper.Model.Sql.Dynamic.Handlers;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// This is used within dynamic sql processing to process bindings and property names.
    /// Please see <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Parsers.TextPropertyProbe"/> for more details.
    /// </summary>
    internal class DynamicSqlTextTokenHandler : TextTokenHandlerBase
    {
        private readonly SqlTagContext _ctx;
        private readonly SqlText _sqlText;

        public DynamicSqlTextTokenHandler(SqlTagContext ctx, SqlText sqlText)
        {
            if (ctx == null)
                throw new ArgumentNullException("ctx");
            if (sqlText == null)
                throw new ArgumentNullException("sqlText");

            _ctx = ctx;

            _sqlText = sqlText;

            KeepSurroundingToken = true;
        }

        public override string PostProcessing(string sqlStatement)
        {
            return _ctx.ReplaceIteratePropertiesAndVariables(_sqlText, sqlStatement);
        }

        public override string ProcessToken(string tokenValue)
        {
            var value = _ctx.ReplaceIterateCurrentProperty(_sqlText, tokenValue);

            return _ctx.ReplaceIteratePropertiesAndVariables(_sqlText, value);
        }
    }
}