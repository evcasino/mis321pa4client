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
                        Song song = new Song(){SongID = rdr.GetInt32(0), SongTitle = rdr.GetString(1), SongTimestamp = rdr.GetDateTime(2), Deleted = rdr.GetString(3), Favorite = rdr.GetString(4)};   
                        playlist.Add(song);
                    }
                }
                cs.CloseConnection();
                return playlist;
            }
            else{
                return new List<Song>();
            }
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

            string stm = @"CREATE TABLE IF NOT EXISTS Songs(id INTEGER PRIMARY KEY AUTO_INCREMENT, SongTitle TEXT, TimeAdded DATE, Deleted Text, Favorite Text)"; 

            using var cmd = new MySqlCommand(stm,con);

            cmd.ExecuteNonQuery();
        }
        public void AddSong(){}
        public static void AddSong(Song x){
            CreateSongTable();
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;

            using var con = new MySqlConnection(cs);
            con.Open();

            string stm = @"INSERT INTO Songs(SongTitle, TimeAdded, Deleted, Favorite) Values(@SongTitle,@TimeAdded,@Deleted,@Favorite)"; 

            using var cmd = new MySqlCommand(stm,con);
            cmd.Parameters.AddWithValue("@SongTitle", x.SongTitle);
            cmd.Parameters.AddWithValue("@TimeAdded", x.SongTimestamp);
            cmd.Parameters.AddWithValue("@Deleted", x.Deleted);
            cmd.Parameters.AddWithValue("@Favorite", x.Favorite);

            cmd.Prepare(); //add value

            string stm2 = "Select * FROM Songs ORDER BY TimeAdded DESC";
            using var cmd2 = new MySqlCommand(stm2,con);
            cmd.ExecuteNonQuery();

        }
        public void DeleteSong(){}
        public static void DeleteSong(Song x){
            
            ConnectionString myConnection = new ConnectionString();
            string cs = myConnection.cs;
            // Console.WriteLine("What is the ID of the song you want to delete?");
            int SongID = x.SongID;
            string Deleted = x.Deleted; //use only for soft delete

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
        public void EditSong(){}
public static Song SelectOneById(int id)
{
     ConnectionString cs = new ConnectionString();
            bool isOpen = cs.OpenConnection();
            Song song = new Song();

            if(isOpen){
                MySqlConnection conn = cs.GetConn();
                string stm = @"SELECT * from Songs WHERE id = " + id + " LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(stm, conn);

                List<Song> playlist = new List<Song>();

                using var rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                     song = new Song(){SongID = rdr.GetInt32(0), SongTitle = rdr.GetString(1), SongTimestamp = rdr.GetDateTime(2), Deleted = rdr.GetString(3), Favorite = rdr.GetString(4)};  
                }
                cs.CloseConnection();
                return song;
            }
            return song;
}
public static void EditSong(Song x)
{
ConnectionString myConnection = new ConnectionString();
string cs = myConnection.cs;
using var con = new MySqlConnection(cs);
con.Open();
string stm = @"UPDATE Songs SET SongTitle = @SongTitle,TimeAdded = @TimeAdded,Favorite = @Favorite, Deleted = @Deleted WHERE id = @id";
using var cmd = new MySqlCommand(stm,con);
cmd.Parameters.AddWithValue("@id", x.SongID);
cmd.Parameters.AddWithValue("@SongTitle", x.SongTitle);
cmd.Parameters.AddWithValue("@TimeAdded", x.SongTimestamp);
cmd.Parameters.AddWithValue("@Deleted", x.Deleted);
cmd.Parameters.AddWithValue("@Favorite", x.Favorite);
cmd.Prepare(); //add value
string stm2 = "Select * FROM Songs ORDER BY TimeAdded DESC";
using var cmd2 = new MySqlCommand(stm2,con);
cmd.ExecuteNonQuery();
        }
    }
}