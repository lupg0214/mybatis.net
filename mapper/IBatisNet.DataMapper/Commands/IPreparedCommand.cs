
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: $
 * $Date: $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - Gilles Bayon
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

#region Imports
using System.Data;

using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;

using log4net;
#endregion

namespace IBatisNet.DataMapper.Commands
{
	/// <summary>
	/// Summary description for IPreparedCommand.
	/// </summary>
	public interface IPreparedCommand
	{
		/// <summary>
		/// Create an IDbCommand for the IDalSession and the current SQL Statement
		/// and fill IDbCommand IDataParameter's with the parameterObject.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="session">The IDalSession</param>
		/// <param name="statement">The IStatement</param>
		/// <param name="parameterObject">
		/// The parameter object that will fill the sql parameter
		/// </param>
		/// <returns>An IDbCommand with all the IDataParameter filled.</returns>
		IDbCommand Create (RequestScope request, IDalSession session, IStatement statement, object parameterObject );
	}
}
