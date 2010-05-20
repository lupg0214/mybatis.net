using System;

namespace iBatisTutorial.Model
{

	/// <summary>
	/// Business object representing a Person 
	/// entity in our problem domain. 
	/// </summary>	
	public class Person
	{
		/*
		private string _Property;
		public string Property
		{
			get { return _Property; }
			set { _Property = value; }
		}
		*/

		private int _Id;
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _FirstName;
		public string FirstName
		{
			get { return _FirstName; }
			set { _FirstName = value; }
		}

		private string _LastName;
		public string LastName
		{
			get { return _LastName; }
			set { _LastName = value; }
		}

		private DateTime _BirthDate;
		public DateTime BirthDate
		{
			get { return _BirthDate; }
			set { _BirthDate = value; }
		}

		private double _WeightInKilograms;
		public double WeightInKilograms
		{
			get { return _WeightInKilograms; }
			set { _WeightInKilograms = value; }
		}

		private double _HeightInMeters;
		public double HeightInMeters
		{
			get { return _HeightInMeters; }
			set { _HeightInMeters = value; }
		}

	}
}