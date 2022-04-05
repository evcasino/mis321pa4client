using MySql.Data.MySqlClient;
using System;
using API.Interfaces;
using API.Models;
using System.Collections.Generic;

namespace API.Models
{
    public class databaseUtil: ISongUtilities
    {
        public List<Song> playlist { get; set; }

        public static List<Song> ReadData()
        {
            CreateSongTable();
            ConnectionString cs = new ConnectionString();
            bool isOpen = cs.OpenConnection();

            if(isOpen){
                MySqlConnection conn = cs.GetConn();
                string stm = "SELECT * FROM Songs";
                MySqlCommand cmd = new MySqlCommand(stm, conn);

                List<Song> playlist = new List<Song>();

                using ( var rdr = cmd.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        Song song = new Song(){SongID = rdr.GetInt32(0), SongTitle = rdr.GetString(1), SongTimestamp = rdr.GetDateTime(2), Deleted = rdr.GetString(3)};   
                        playlist.Add(song);
                    }
                }
                cs.CloseConnection();
                return playlist;
            }
            else{
                return new List<Song>();
            }
            // CreateSongTable();
            // string server = "wb39lt71kvkgdmw0.cbetxkdyhwsb.us-east-1.rds.amazonaws.com";
            // string userName = "u5gzm97ulx1mhtqs";
            // string password = "to05kyk7u7i5a1qu";
            // string port = "3306";
            // string database = "sx459fh0rrhhxjde";

            // string cs = $"server={server};port={port};database={database};user={userName};password={password}";
            // List<Song> playlist = new List<Song>();
            // using var con = new MySqlConnection(cs);
            // con.Open();

            // string stm = "SELECT * FROM Songs";
            // using var cmd = new MySqlCommand(stm,con);

            // using MySqlDataReader rdr = cmd.ExecuteReader();

            // while(rdr.Read())
            // {
               
            //     Song song = new Song(){SongID = rdr.GetInt32(0), SongTitle = rdr.GetString(1), SongTimestamp = rdr.GetDateTime(2), Deleted = rdr.GetString(3)};   
                
            //     playlist.Add(song);
            // }

            // return playlist;
        }
        
        public void PrintPlaylist() 
        { // display all items in the playlist to the console
            Console.Clear();
            playlist.Sort();
            foreach (Song song in playlist) { // for every song in the playlist, write the song's ToString to the console
                if(song.Deleted != "y"){
                    Console.WriteLine(song.ToString());
                }
            }
            Console.WriteLine();
        }
        public static void CreateSongTable(){
            
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;

            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"CREATE TABLE IF NOT EXISTS Songs(id INTEGER PRIMARY KEY AUTO_INCREMENT, SongTitle TEXT, TimeAdded DATE, Deleted Text)"; 

            using var cmd = new MySqlCommand(stm,con);

            cmd.ExecuteNonQuery();
        }

        public void AddSong(){
            CreateSongTable();
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            DateTime temp = DateTime.Now;
            Console.WriteLine("What is the title of your song?");
            string SongTitle = Console.ReadLine();
            string Deleted = "n";

            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Songs(SongTitle, TimeAdded, Deleted) Values(@SongTitle,@TimeAdded,@Deleted)"; 

            using var cmd = new MySqlCommand(stm,con);
            cmd.Parameters.AddWithValue("@SongTitle", SongTitle);
            cmd.Parameters.AddWithValue("@TimeAdded", temp);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);

            cmd.Prepare(); //add value

            string stm2 = "Select * FROM Songs ORDER BY TimeAdded DESC";
            using var cmd2 = new MySqlCommand(stm2,con);
            cmd.ExecuteNonQuery();

        }
        public void DeleteSong(){
            
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            Console.WriteLine("What is the ID of the song you want to delete?");
            int SongID = int.Parse(Console.ReadLine());
            string Deleted = "y"; //use only for soft delete

            using var con = new MySqlConnection(cs);
            con.Open();
            //string stm = @"DELETE FROM Songs WHERE id = @id"; //Hard Delete
            string stm = @"UPDATE Songs SET Deleted = @Deleted WHERE id = @id";
            using var cmd = new MySqlCommand(stm,con);
            cmd.Parameters.AddWithValue("@id", SongID);
            cmd.Parameters.AddWithValue("@Deleted", Deleted); // only for soft delete
            cmd.Prepare(); //add value

            string stm2 = "Select * FROM Songs ORDER BY TimeAdded DESC";
            using var cmd2 = new MySqlCommand(stm2,con);
            cmd.ExecuteNonQuery();

        }

        public void EditSong(){
            CreateSongTable();
            PrintPlaylist();
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            DateTime temp = DateTime.Now;
            string Deleted = "n";
            Console.WriteLine("What is the ID of the song you want to Edit?");
            int SongID = int.Parse(Console.ReadLine());
            Console.WriteLine("What would you like to change the song too?");
            string SongTitle = Console.ReadLine();

            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"UPDATE Songs SET SongTitle = @SongTitle,TimeAdded = @TimeAdded,Deleted = @Deleted WHERE id = @id";

            using var cmd = new MySqlCommand(stm,con);
            cmd.Parameters.AddWithValue("@id", SongID);
            cmd.Parameters.AddWithValue("@SongTitle", SongTitle);
            cmd.Parameters.AddWithValue("@TimeAdded", temp);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);

            cmd.Prepare(); //add value

            string stm2 = "Select * FROM Songs ORDER BY TimeAdded DESC";
             using var cmd2 = new MySqlCommand(stm2,con);
            cmd.ExecuteNonQuery();

        }
    }
}