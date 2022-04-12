using MySql.Data.MySqlClient;
using System;
using MySql.Data;

namespace API
{
    public class ConnectionString
     {
        public string cs {get; set;}

        private MySqlConnection connection;

        private string server;
        private string userName;
        private string password;
        private string port;
        private string database;

        public ConnectionString(){
            Intialize();
        }

        private void Intialize()
        {
            server = "wb39lt71kvkgdmw0.cbetxkdyhwsb.us-east-1.rds.amazonaws.com";
            userName = "u5gzm97ulx1mhtqs";
            password = "to05kyk7u7i5a1qu";
            port = "3306";
            database = "sx459fh0rrhhxjde";

            cs = $"server={server};port={port};database={database};user={userName};password={password}";
            connection = new MySqlConnection(cs);
        }

        public bool OpenConnection(){
            try{
                connection.Open();
                return true;
            }
            catch(MySqlException ex){
                if(ex.Number ==0){
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Can't Connect");
                }
                else{
                    if(ex.Number == 1045){
                        Console.WriteLine("Invalid username/password");
                    }
                }
            }
            return false;
        }

        public bool CloseConnection(){
            try{
                connection.Close();
                return true;
            }
            catch(MySqlException ex){
                    Console.WriteLine(ex.Message);
            }
            return false;
        }
        public MySqlConnection GetConn(){
            return connection;
        }
    }
}