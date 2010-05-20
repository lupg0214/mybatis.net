-- Create a new user, grant her rights, and set her password.
grant select, insert, update, delete ON npetshop.* TO npetshop@'localhost' IDENTIFIED BY 'ibatis'  ;