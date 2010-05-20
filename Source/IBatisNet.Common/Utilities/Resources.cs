
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

#region Imports
using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;
using System.Reflection;

using log4net;

using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities.TypesResolver;
#endregion

namespace IBatisNet.Common.Utilities
{
	/// <summary>
	/// A class to simplify access to resources.
	/// 
	/// The file can be loaded from the application root directory 
	/// (use the resource attribute) 
	/// or from any valid URL (use the url attribute). 
	/// For example,to load a fixed path file, use:
	/// &lt;properties url=”file:///c:/config/my.properties” /&gt;
	/// </summary>
	public class Resources
	{

		#region Fields
		private static string _rootDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin","").Replace(@"\Debug","").Replace(@"\Release","");
		private static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		private static CachedTypeResolver _cachedTypeResolver = null;
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		#endregion

		#region Properties
		/// <summary>
		/// Get the root directory of the application
		/// </summary>
		public static string RootDirectory
		{
			get
			{
				return _rootDirectory;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		static Resources()
		{
			_cachedTypeResolver = new CachedTypeResolver();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get config file from from the base directory that the assembler
		/// used for probe assemblies
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static XmlDocument GetConfigAsXmlDocument(string fileName)
		{
			XmlDocument config = new XmlDocument();

			try 
			{
				XmlTextReader reader = new XmlTextReader(_baseDirectory + Path.DirectorySeparatorChar + fileName);
				config.Load(reader);
				reader.Close();
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load config file \"{0}\". Cause : ",
					fileName, 
					e.Message  ) ,e);
			}

			return config;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static XmlDocument GetAsXmlDocument(XmlNode node)
		{
			XmlDocument xmlDocument = null;

			if (node.Attributes["resource"] != null)
			{
				xmlDocument = Resources.GetResourceAsXmlDocument(node.Attributes["resource"].Value);
			}
			else if (node.Attributes["url"] != null)
			{
				xmlDocument = Resources.GetUrlAsXmlDocument(node.Attributes["url"].Value);
			}
			else if (node.Attributes["embedded"] != null)
			{
				xmlDocument = Resources.GetEmbeddedResourceAsXmlDocument(node.Attributes["embedded"].Value);
			}

			return xmlDocument;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static string GetValueOfNodeResourceUrl(XmlNode node)
		{
			string path = null;

			if (node.Attributes["resource"] != null)
			{
				path = _rootDirectory + Path.DirectorySeparatorChar + node.Attributes["resource"].Value;
			}
			else if (node.Attributes["url"] != null)
			{
				path = node.Attributes["url"].Value;
			}

			return path;
		}

		/// <summary>
		/// Load resource from the root directory of the application
		/// </summary>
		/// <param name="resource"></param>
		/// <returns></returns>
		public static XmlDocument GetResourceAsXmlDocument(string resource)
		{
			string file = string.Empty;
			XmlDocument config = new XmlDocument();
			XmlTextReader reader = null;

			try 
			{
				reader = new XmlTextReader(_rootDirectory + Path.DirectorySeparatorChar + resource);
				config.Load(reader);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load file via resource \"{0}\" as resource. Cause : ",
					resource, 
					e.Message  ) ,e);
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}

			return config;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static XmlDocument GetUrlAsXmlDocument(string url)
		{
			string file = string.Empty;
			XmlDocument config = new XmlDocument();
			XmlTextReader reader = null;

			try 
			{
				reader = new XmlTextReader(url);
				config.Load(reader);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(
					string.Format("Unable to load file via url \"{0}\" as url. Cause : ",
					url, 
					e.Message  ) ,e);
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}

			return config;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileResource"></param>
		/// <returns></returns>
		public static XmlDocument GetEmbeddedResourceAsXmlDocument(string fileResource)
		{
			XmlDocument config = new XmlDocument();
			bool isLoad = false;
			XmlTextReader reader = null;

			FileAssemblyInfo fileInfo = new FileAssemblyInfo (fileResource);
			if (fileInfo.IsAssemblyQualified)
			{
				Assembly assembly = Assembly.LoadWithPartialName (fileInfo.AssemblyName);
//				foreach(string fileName in assembly.GetManifestResourceNames() ) 
//				{
//					Console.WriteLine(fileName);
//				}

				Stream resource = assembly.GetManifestResourceStream(fileInfo.ResourceFileName);
				if (resource != null)
				{
					try
					{
						reader = new XmlTextReader(resource);
						config.Load(reader);
						isLoad = true;
					}
					catch(Exception e)
					{
						throw new ConfigurationException(
							string.Format("Unable to load file \"{0}\" in embedded resource. Cause : ",
							fileResource, 
							e.Message  ) ,e);
					}
					finally
					{
						if (reader != null)
						{
							reader.Close();
						}
					}
				}
			} 
			else
			{
				// bare type name... loop thru all loaded assemblies
				Assembly [] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
				foreach (Assembly assembly in assemblies)
				{
					Stream resource = assembly.GetManifestResourceStream(fileInfo.FileName);
					if (resource != null)
					{
						try
						{
							reader = new XmlTextReader(resource);
							config.Load(reader);
							isLoad = true;
						}
						catch(Exception e)
						{
							throw new ConfigurationException(
								string.Format("Unable to load file \"{0}\" in embedded resource. Cause : ",
								fileResource, 
								e.Message  ) ,e);
						}
						finally
						{
							if (reader != null)
							{
								reader.Close();
							}
						}
						break;
					}
				}
			}

			if (isLoad == false) 
			{
				_logger.Error("Could not load embedded resource from assembly");
				throw new ConfigurationException(
					string.Format("Unable to load embedded resource from assembly \"{0}\".",
					fileInfo.OriginalFileName));
			}

			return config;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static FileInfo GetFileInfo(string path)
		{
			string file = string.Empty;

			if (!File.Exists(path)) 
			{
				file = _rootDirectory + Path.DirectorySeparatorChar + path;
			}
			else
			{
				file = path;
			}

			return new FileInfo(file);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="properties"></param>
		/// <returns></returns>
		public static string ParsePropertyTokens(string str, NameValueCollection  properties) 
		{
			string OPEN = "${";
			string CLOSE = "}";

			string newString = str;
			if (newString != null && properties != null) 
			{
				int start = newString.IndexOf(OPEN);
				int end = newString.IndexOf(CLOSE);

				while (start > -1 && end > start) 
				{
					string prepend = newString.Substring(0, start);
					string append = newString.Substring(end + CLOSE.Length);

					int index = start + OPEN.Length;
					string propName = newString.Substring(index, end-index);
					string propValue = properties.Get(propName);
					if (propValue == null) 
					{
						newString = prepend + propName + append;
					} 
					else 
					{
						newString = prepend + propValue + append;
					}
					start = newString.IndexOf(OPEN);
					end = newString.IndexOf(CLOSE);
				}
			}
			return newString;
		}


		/// <summary>
		/// Find a type by his class name
		/// </summary>
		/// <param name="className">The className ot the type to find.</param>
		public static Type TypeForName(string className)
		{
			return _cachedTypeResolver.Resolve(className);
		}

		#endregion

		
		#region Inner Class : FileAssemblyInfo
		/// <summary>
		/// Holds data about a <see cref="System.Type"/> and it's
		/// attendant <see cref="System.Reflection.Assembly"/>.
		/// </summary>
		internal class FileAssemblyInfo
		{
			#region Constants
			/// <summary>
			/// The string that separates file name
			/// from their attendant <see cref="System.Reflection.Assembly"/>
			/// names in an assembly qualified type name.
			/// </summary>
			public const string FileAssemblySeparator = ",";
			#endregion

			#region Fields
			private string _unresolvedAssemblyName= string.Empty;
			private string _unresolvedFileName= string.Empty;
			private string _originalFileName= string.Empty;
			#endregion

			#region Properties

			/// <summary>
			/// The resource file name .
			/// </summary>
			public string ResourceFileName
			{
				get
				{
					return AssemblyName+"."+FileName;
				}
			}

			/// <summary>
			/// The original name.
			/// </summary>
			public string OriginalFileName
			{
				get
				{
					return _originalFileName;
				}
			}

			/// <summary>
			/// The file name portion.
			/// </summary>
			public string FileName
			{
				get
				{
					return _unresolvedFileName;
				}
			}

			/// <summary>
			/// The (unresolved, possibly partial) name of the attandant assembly.
			/// </summary>
			public string AssemblyName
			{
				get
				{
					return _unresolvedAssemblyName;
				}
			}

			/// <summary>
			/// Is the type name being resolved assembly qualified?
			/// </summary>
			public bool IsAssemblyQualified
			{
				get
				{
					if (AssemblyName ==  null || AssemblyName.Trim().Length==0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}

			#endregion

			#region Constructor (s) / Destructor
			/// <summary>
			/// Creates a new instance of the FileAssemblyInfo class.
			/// </summary>
			/// <param name="unresolvedFileName">
			/// The unresolved name of a <see cref="System.Type"/>.
			/// </param>
			public FileAssemblyInfo (string unresolvedFileName)
			{
				SplitFileAndAssemblyNames (unresolvedFileName);
			}
			#endregion

			#region Methods
			/// <summary>
			/// 
			/// </summary>
			/// <param name="originalFileName"></param>
			private void SplitFileAndAssemblyNames (string originalFileName) 
			{
				_originalFileName = originalFileName;
				int separatorIndex = originalFileName.IndexOf (
					FileAssemblyInfo.FileAssemblySeparator);
				if (separatorIndex < 0)
				{
					throw new ConfigurationException(
						string.Format("Unable to find assembly part to load embedded resource in string \"{0}\". Cause : ",
						originalFileName));
				} 
				else
				{
					_unresolvedFileName = originalFileName.Substring (
						0, separatorIndex).Trim ();
					_unresolvedAssemblyName = originalFileName.Substring (
						separatorIndex + 1).Trim ();
				}
			}
			#endregion

		}
		#endregion

	}
}
