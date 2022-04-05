using System.Collections.Generic;
using API.Interfaces;
using API.Models;

namespace API.Interfaces
{
    public interface ISongUtilities
    {
        public List<Song> playlist { get; set; }
         public void AddSong();
         public void DeleteSong();
         public void EditSong();
         public void PrintPlaylist();
    }
}