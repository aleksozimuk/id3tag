using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tags
{
    class ID3Tag
    {
        private string _header;
        private string _title;
        private string _artist;
        private string _album;
        private string _year;
        private string _comment;
        private string _genre;
        private string _track;

        public ID3Tag()
        {
            _header = "";
            _title = "";
            _artist = "";
            _album = "";
            _year = "";
            _comment = "";
            _genre = "";
            _track = "";
        }

        public string Header
        {
            get { return _header.TrimEnd('\0', ' ', '\a'); }
            set { _header = value; }
        }
        public string Title
        {
            get { return _title.TrimEnd('\0', ' ', '\a'); }
            set { _title = value; }
        }
    
        public string Artist
        {
            get { return _artist.TrimEnd('\0', ' ', '\a'); }
            set { _artist = value; }
        }
        public string Album
        {
            get { return _album.TrimEnd('\0', ' ', '\a'); }
            set { _album = value; }
        }
        public string Year
        {
            get { return _year.TrimEnd('\0', ' ', '\a'); }
            set { _year = value; } 
        }
        public string Comment
        {
            get { return _comment.TrimEnd('\0', ' ','\a'); }
            set { _comment = value; }
        }
        public string Genre
        {
            get { return _genre.TrimEnd('\0', ' ', '\a'); }
            set { _genre = value; }
        }
        public string Track
        {
            get { return _track.TrimEnd('\0', ' ', '\a'); }
            set { _track = value; }
        }
    }                                          
}                            
