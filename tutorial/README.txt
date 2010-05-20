Tutorial README.txt

* Prerequisite: NUnit 2.1.x

* Download the iBatisNet Tutorial. 

  A zip file containing the Tutorial Visual Studio 2003 solution can be downloaded iBatisNet SourceForge project. 

  * http://ibatisnet.sf.net/

* Extact the Tutorial to a likely location, such as "c:\projects\Apache\iBatisNet\".

* Adjust the database location, if necessary. For simplicity, the application uses an Access database. If you use another path, edit the string: 

Data Source=C:\projects\Apache\iBatisNet\Tutorial\WebView\Resources\iBatisTutorial.mdb"

to match the location of your project. 

* Set the WebView project for web sharing using the alias "iBatisTutorial". 
 
   (Right-click for folder properties, press the Web Sharing tab, click Add, and enter "iBatisTutorial" for the alias.)

* Open Visual Studio, start the application in Debug mode, and take the Tutorial for a spin. 

  * If necessary, set WebView as the StartUp Project and set Default.aspx as the Start Page.

  * If you get an "Operation must use an updateable query" error, you may need to give the ASP.NET machine account Modify permissions for both the folder containing the database as well as the Access database file itself. Modify permissions are required for the folder since Access creates a locking database file (.ldbin the same directory as the Access .mdb file when a client is using the database.