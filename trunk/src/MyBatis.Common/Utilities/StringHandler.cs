using System.Text;

namespace MyBatis.Common.Utilities
{
    public class StringHandler
    {
        /// <summary>
        /// This could be made an extension method...
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        public static void Replace(StringBuilder buffer, string find, string replace)
        {
            int start = buffer.ToString().IndexOf(find);
            int length = find.Length;
            while (start > -1)
            {
                buffer = buffer.Replace(find, replace, start, length);
                start = buffer.ToString().IndexOf(find);
            }
        }
    }
}