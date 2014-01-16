namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// Abstract base class used to handle sql text token parsing.
    /// </summary>
    internal abstract class TextTokenHandlerBase
    {
        public bool KeepSurroundingToken;

        public abstract string PostProcessing(string sqlStatement);

        public abstract string ProcessToken(string tokenValue);
    }
}