If you got error
"Operation must use an updateable query." 

This is almost always a permissions issue. Be sure that the MDB file is in a folder 
where <machineName>\ASPNET have read/write access 
(because it needs to create an .LDB file when modifying the database). 
