
using System.Collections;

namespace NPetshop.Presentation.Core
{
	public interface IWebAction
	{
		Hashtable Data {get;}
		string NextViewToDisplay{get;}
	}
}
