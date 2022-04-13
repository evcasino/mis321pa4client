using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Models;
using MySql.Data.MySqlClient;
using API;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsConController : ControllerBase
    {
        // GET: api/SongsCon
        //[EnableCors("OpenPolicy")]
        [HttpGet]
        public List<Song> Get()
        {
            databaseUtil.CreateSongTable();
            ConnectionString cs = new ConnectionString();
            bool isOpen = cs.OpenConnection();

            if(isOpen){
                MySqlConnection conn = cs.GetConn();
                string stm = "SELECT * FROM Songs WHERE Deleted = 'n'"; //only deleted values
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

        // GET: api/SongsCon/5
        //[EnableCors("OpenPolicy")]
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SongsCon
        [EnableCors("OpenPolicy")]
        [HttpPost]
        public void Post([FromBody] Song value) //add
        {
            value.SongTimestamp = DateTime.Now;
            value.Favorite = "n";
            Console.WriteLine($"adding {value.SongTitle}");
            databaseUtil.AddSong(value);
        }

        // PUT: api/SongsCon/5
       //[EnableCors("OpenPolicy")]
        [HttpPut("{id}")]
        public void Put(int id,[FromBody] Song value) //favorite
        {
            Console.WriteLine($"Current = {value.Favorite}");
            if (value.Favorite == "y")
            {
                value = databaseUtil.SelectOneById(id);
                value.Favorite = "y";
            }
            else if (value.Favorite == "n")
            {
                value = databaseUtil.SelectOneById(id);
                value.Favorite = "n";
            }
            Console.WriteLine($" Song = {value.SongID} {value.SongTitle} {value.SongTimestamp} {value.Favorite}");
            Console.WriteLine($"updating song ID:{id} for {value.SongTitle}");
            databaseUtil.EditSong(value);
            Console.WriteLine("youre here");
        }

        // DELETE: api/SongsCon/5
        //[EnableCors("OpenPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id) //delete
        {
            Song temp = new Song();
            temp = databaseUtil.SelectOneById(id);
            temp.Deleted = "y";
            databaseUtil.DeleteSong(temp);
            Console.WriteLine($"Soft deleting song ID:{id} for {temp.SongTitle}");
        }
    }
}
