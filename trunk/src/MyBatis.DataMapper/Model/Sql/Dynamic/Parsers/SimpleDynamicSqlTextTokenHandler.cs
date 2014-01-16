using System;
using MyBatis.Common.Utilities.Objects;
using MyBatis.DataMapper.DataExchange;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// This is used within simple dynamic sql processing to process bindings and property names.
    /// Please see <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Parsers.TextPropertyProbe"/> for more details.
    /// </summary>
    internal class SimpleDynamicSqlTextTokenHandler : TextTokenHandlerBase
    {
        private readonly DataExchangeFactory _dataExchangeFactory;
        private readonly object _parameterObject;

        public SimpleDynamicSqlTextTokenHandler(DataExchangeFactory dataExchangeFactory, object parameterObject)
        {
            if (dataExchangeFactory == null)
                throw new ArgumentNullException("dataExchangeFactory");
            if (parameterObject == null)
                throw new ArgumentNullException("parameterObject");

            _dataExchangeFactory = dataExchangeFactory;

            _parameterObject = parameterObject;
            
            KeepSurroundingToken = false;
        }

        public override string PostProcessing(string sqlStatement)
        {
            return sqlStatement;
        }

        public override string ProcessToken(string tokenValue)
        {
            object value = null;

            if (_parameterObject != null)
            {
                if (_dataExchangeFactory.TypeHandlerFactory.IsSimpleType(_parameterObject.GetType()))
                {
                    value = _parameterObject;
                }
                else
                {
                    value = ObjectProbe.GetMemberValue(_parameterObject, tokenValue, _dataExchangeFactory.AccessorFactory);
                }
            }

            if (value != null)
            {
                return value.ToString();
            }

            return null;
        }
    }
}