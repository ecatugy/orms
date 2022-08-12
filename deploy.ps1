
#Deploy the database poster in server sql server
#The project startup will be Orms.Api
#In the package manager choice the project Orms.Persistence, that contain the class DbContext
add-migration initial
update-database
