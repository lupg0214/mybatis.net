using System;
using System.Collections;
using System.Collections.Specialized;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for DeSerializerFactory.
	/// </summary>
	public class DeSerializerFactory
	{
		private IDictionary _serializerMap = new HybridDictionary();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configScope"></param>
		public DeSerializerFactory(ConfigurationScope configScope)
		{
			_serializerMap.Add("dynamic", new DynamicDeSerializer(configScope));
			_serializerMap.Add("isEqual", new IsEqualDeSerializer(configScope));
			_serializerMap.Add("isNotEqual", new IsNotEqualDeSerializer(configScope));
			_serializerMap.Add("isGreaterEqual", new IsGreaterEqualDeSerializer(configScope));
			_serializerMap.Add("isGreaterThan", new IsGreaterThanDeSerializer(configScope));
			_serializerMap.Add("isLessEqual", new IsLessEqualDeSerializer(configScope));
			_serializerMap.Add("isLessThan", new IsLessThanDeSerializer(configScope));
			_serializerMap.Add("isNotEmpty", new IsNotEmptyDeSerializer(configScope));
			_serializerMap.Add("isEmpty", new IsEmptyDeSerializer(configScope));
			_serializerMap.Add("isNotNull", new IsNotNullDeSerializer(configScope));
			_serializerMap.Add("isNotParameterPresent", new IsNotParameterPresentDeSerializer(configScope));
			_serializerMap.Add("isNotPropertyAvailable", new IsNotPropertyAvailableDeSerializer(configScope));
			_serializerMap.Add("isNull", new IsNullDeSerializer(configScope));
			_serializerMap.Add("isParameterPresent", new IsParameterPresentDeSerializer(configScope));
			_serializerMap.Add("isPropertyAvailable", new IsPropertyAvailableDeSerializer(configScope));
			_serializerMap.Add("iterate", new IterateSerializer(configScope));		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IDeSerializer GetDeSerializer(string name) 
		{
			return (IDeSerializer) _serializerMap[name];
		}

	}
}
