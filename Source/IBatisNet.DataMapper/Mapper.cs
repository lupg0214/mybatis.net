#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: $
 * $Date: $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;

namespace IBatisNet.DataMapper
{
	/// <summary>
	/// A singleton class to access the default SqlMapper defined by the SqlMap.Config
	/// </summary>
	public class Mapper
	{
		#region Fields
		private static volatile SqlMapper _mapper = null;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		protected static void Configure (object obj)
		{
			_mapper = (SqlMapper) obj;
		}

		/// <summary>
		/// Init the 'default' SqlMapper defined by the SqlMap.Config file.
		/// </summary>
		protected static void InitMapper()
		{
			ConfigureHandler handler = new ConfigureHandler (Configure);
			_mapper = SqlMapper.ConfigureAndWatch (handler);
		}

		/// <summary>
		/// Get the instance of the SqlMapper defined by the SqlMap.Config file.
		/// </summary>
		/// <returns>A SqlMapper initalized via the SqlMap.Config file.</returns>
		public static SqlMapper Instance()
		{
			if (_mapper == null)
			{
				lock (typeof (SqlMapper))
				{
					if (_mapper == null) // double-check
						InitMapper();
				}
			}
			return _mapper;
		}
		
		/// <summary>
		/// Get the instance of the SqlMapper defined by the SqlMap.Config file. (Convenience form of Instance method.)
		/// </summary>
		/// <returns>A SqlMapper initalized via the SqlMap.Config file.</returns>
		public static SqlMapper Get()
		{
			return Instance();
		}
	}
}
