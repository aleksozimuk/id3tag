using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ID3Tags
{
    class ID3v1
    {
        private const byte HEADERTAGLENGTH = 3;
        private const byte TITLETAGLENGTH = 30;
        private const byte ARTISTTAGLENGTH = 30;
        private const byte ALBUMTAGLENGTH = 30;
        private const byte YEARTAGLENGTH = 4;
        private const byte COMMENTTAGLENGTH = 30;
        private const byte GENRETAGLENGTH = 1;


        private ID3Tag _id3v1Tag;

        public ID3v1()
        {
            _id3v1Tag = new ID3Tag();
        }

        public ID3Tag ID3v1Tag
        {
            get { return _id3v1Tag; }
        }

        public void GetTag(string mp3FilePath)
        {
            char[] tag;

            if(!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");


            using (BinaryReader binReader = new BinaryReader(File.Open(mp3FilePath, FileMode.Open)))
            {
                binReader.BaseStream.Position = binReader.BaseStream.Length - 128;
                tag = binReader.ReadChars(128);
                if (new string(new char[]{tag[0], tag[1], tag[2]}) == "TAG")
                {
                    Console.WriteLine("ok");
                }

            }

             

            /*
            using(FileStream fs = new FileStream(mp3FilePath, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length > 128)
                {
                    fs.Seek(-128, SeekOrigin.End);
                    
                    tag = new byte[HEADERTAGLENGTH];
                    fs.Read(tag, 0, HEADERTAGLENGTH);
                    _id3v1Tag.Header = Encoding.Default.GetString(tag);

                    if (_id3v1Tag.Header == "TAG")
                    {
                        tag = new byte[TITLETAGLENGTH];
                        fs.Read(tag, 0, TITLETAGLENGTH);
                        _id3v1Tag.Title = Encoding.Default.GetString(tag);

                        tag = new byte[ARTISTTAGLENGTH];
                        fs.Read(tag, 0, ARTISTTAGLENGTH);
                        _id3v1Tag.Artist = Encoding.Default.GetString(tag);

                        tag = new byte[ALBUMTAGLENGTH];
                        fs.Read(tag, 0, ALBUMTAGLENGTH);
                        _id3v1Tag.Album = Encoding.Default.GetString(tag);

                        tag = new byte[YEARTAGLENGTH];
                        fs.Read(tag, 0, YEARTAGLENGTH);
                        _id3v1Tag.Year = Encoding.Default.GetString(tag);

                        tag = new byte[COMMENTTAGLENGTH];
                        fs.Read(tag, 0, COMMENTTAGLENGTH);
                        _id3v1Tag.Comment = Encoding.Default.GetString(tag);

                        tag = new byte[GENRETAGLENGTH];
                        fs.Read(tag, 0, GENRETAGLENGTH);
                        _id3v1Tag.Genre = Encoding.Default.GetString(tag);
                    }
                    else
                    {
                        throw new Exception("ID3v1 не найден");//переделать
                    }
                }
            }
            */
        }

        public void SetTag(string mp3FilePath)
        {
            byte[] tag;
            
            if (!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");

            using (FileStream fs = new FileStream(mp3FilePath, FileMode.Open, FileAccess.ReadWrite))
            {

                fs.Seek(-128, SeekOrigin.End);
                    
                tag = new byte[HEADERTAGLENGTH];
                fs.Read(tag, 0, HEADERTAGLENGTH);
                if (Encoding.Default.GetString(tag) != "TAG")
                {
                    fs.Seek(0, SeekOrigin.End);
                    Array.Copy(Encoding.Default.GetBytes("TAG"), tag, HEADERTAGLENGTH);
                    fs.Write(tag, 0, HEADERTAGLENGTH);
                }

                tag = new byte[TITLETAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Title), tag, Encoding.Default.GetBytes(_id3v1Tag.Title).Length);
                fs.Write(tag, 0, TITLETAGLENGTH);

                tag = new byte[ARTISTTAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Artist), tag, Encoding.Default.GetBytes(_id3v1Tag.Artist).Length);
                fs.Write(tag, 0, ARTISTTAGLENGTH);
                
                tag = new byte[ALBUMTAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Album), tag, Encoding.Default.GetBytes(_id3v1Tag.Album).Length);
                fs.Write(tag, 0, ALBUMTAGLENGTH);

                tag = new byte[YEARTAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Year), tag, Encoding.Default.GetBytes(_id3v1Tag.Year).Length);
                fs.Write(tag, 0, YEARTAGLENGTH);

                tag = new byte[COMMENTTAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Comment), tag, Encoding.Default.GetBytes(_id3v1Tag.Comment).Length);
                fs.Write(tag, 0, COMMENTTAGLENGTH);

                tag = new byte[GENRETAGLENGTH];
                Array.Copy(Encoding.Default.GetBytes(_id3v1Tag.Genre), tag, Encoding.Default.GetBytes(_id3v1Tag.Genre).Length);
                fs.Write(tag, 0, GENRETAGLENGTH);
            }
        }
    }
}