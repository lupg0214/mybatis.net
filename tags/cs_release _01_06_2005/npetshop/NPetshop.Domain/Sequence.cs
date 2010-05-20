using System;

namespace NPetshop.Domain
{

	[Serializable]
	public class Sequence 
	{

		#region Private Fields
		private String _name;
		private int _nextId;
		#endregion

		/* Constructors */

		public Sequence() { }

		public Sequence(String name, int nextId) 
		{
			this._name = name;
			this._nextId = nextId;
		}

		#region Properties 

		public string Name 
		{
			set{_name = value;}
			get { return _name; }
		}


		public int NextId
		{
			set{_nextId = value;}
			get { return _nextId; }
		}
		#endregion

	}
}
