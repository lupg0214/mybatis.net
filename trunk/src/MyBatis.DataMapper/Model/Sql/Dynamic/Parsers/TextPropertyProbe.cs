using System;
using System.Collections;
using System.Text;
using MyBatis.Common.Utilities;
using MyBatis.DataMapper.Exceptions;

namespace MyBatis.DataMapper.Model.Sql.Dynamic.Parsers
{
    /// <summary>
    /// This is called within DynamicSql and SimpleDynamicSql. It is responsible for finding all occurrences of
    /// content surrounded with dollar symbols.
    /// Each "token" found is then passed of to an appropriate implementation of TextTokenHandlerBase which is responsible
    /// for performing some action against the token. 
    /// These actions are the handling of bind (variable) usage and property name conversions to a full property reflection name.
    /// Property reflection works by converting a relative reflection path denoted by "[]." to a 
    /// full reflection that indicates the current item within a list, or list of lists.
    /// This is done by working out the parent tag for each tag. If the parent tag is an iterate item, then the current iterate
    /// item is used in building up the property name.
    /// The logic associated with binding all happens within the <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.DynamicSql"/>.ProcessBodyChildren method.
    /// For a description on how the "bind" element can be used, please see: <see cref="MyBatis.DataMapper.Model.Sql.Dynamic.Elements.Bind"/>.
    /// Deserialization of the "bind" element takes place by class: <see cref="MyBatis.DataMapper.Configuration.Serializers.BindDeSerializer"/>.
    /// </summary>
    internal class TextPropertyProbe
    {
        private const string ELEMENT_TOKEN = "$";
        private readonly string _sqlStatement;

        public TextPropertyProbe(string sqlStatement)
        {
            if (sqlStatement == null)
                throw new ArgumentNullException("sqlStatement");

            _sqlStatement = sqlStatement;
        }

        public string Process(TextTokenHandlerBase behaviour)
        {
            if (behaviour == null)
                throw new ArgumentNullException("behaviour");

            // define which character is separating fields

            var parser = new StringTokenizer(_sqlStatement, ELEMENT_TOKEN, true);

            var newSql = new StringBuilder();
            var keepSurroundingToken = behaviour.KeepSurroundingToken;
            string lastToken = null;

            IEnumerator enumerator = parser.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string token = ((string)enumerator.Current);

                if (ELEMENT_TOKEN.Equals(lastToken))
                {
                    if (ELEMENT_TOKEN.Equals(token))
                    {
                        newSql.Append(ELEMENT_TOKEN);
                        token = null;
                    }
                    else
                    {
                        var value = behaviour.ProcessToken(token);

                        if (value != null)
                        {
                            if (keepSurroundingToken)
                                newSql.Append(ELEMENT_TOKEN + value + ELEMENT_TOKEN);
                            else
                                newSql.Append(value);
                        }

                        enumerator.MoveNext();

                        token = ((string)enumerator.Current);

                        if (!ELEMENT_TOKEN.Equals(token))
                        {
                            throw new DataMapperException("Unterminated dynamic element in sql (" + _sqlStatement + ").");
                        }
                        token = null;
                    }
                }
                else
                {
                    if (!ELEMENT_TOKEN.Equals(token))
                    {
                        newSql.Append(token);
                    }
                }

                lastToken = token;
            }

            return behaviour.PostProcessing(newSql.ToString());
        }
    }
}