using System;
using System.Reflection;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
#if dotnet2
[assembly: CLSCompliant(true)]


[assembly: AssemblyCompany("http://ibatis.apache.org/")]
[assembly: AssemblyProduct("iBATIS.NET")]
[assembly: AssemblyCopyright("Copyright 2007,2005 The Apache Software Foundation")]
[assembly: AssemblyTrademark("Licensed under the Apache License, Version 2.0")]
[assembly: AssemblyCulture("")]

#if DEBUG
#else
#if dotnet2
[assembly: AssemblyConfiguration("net-2.0.win32; Release")]
#else
[assembly: AssemblyConfiguration("net-1.1.win32; Release")]
#endif
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFileAttribute("..\\..\\..\\AssemblyKey.snk")]
#endif
#endif

[assembly: AssemblyTitle("iBATIS.DataAccess")]
[assembly: AssemblyDescription("Data Access Object (DAO) design pattern implementation.")]


[assembly: AssemblyVersion("1.9.2")]
