using System.Collections.Generic;
using API.Interfaces;
using API.Models;

namespace API.Interfaces
{
    public interface IReadSongs
    {
        public List<Song> GetAll();
        public Song GetOne(int id);
         
    }
}