using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ID3Tags
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region ID3v1
                ID3v1 mp3 = new ID3v1();

                
                mp3.GetTagString(@"D:\Sia - I go to sleep.mp3");
                Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                Console.WriteLine(mp3.ID3v1Tag.Comment + "!");
                Console.WriteLine(mp3.ID3v1Tag.Year + "!");
                Console.WriteLine(mp3.ID3v1Tag.Genre + "!");
                Console.WriteLine(mp3.ID3v1Tag.Track + "!");
                
                //Console.Read();
                Console.WriteLine("---------------------------");
                
                /*
                mp3.ID3v1Tag.Album = "AlbumTest";
                mp3.ID3v1Tag.Title = "TitleTest";
                mp3.ID3v1Tag.Artist = "ArtistTest";
                mp3.ID3v1Tag.Year = "0000";
                mp3.ID3v1Tag.Comment = "CommentTest";
                mp3.ID3v1Tag.Track = "200";
                mp3.ID3v1Tag.Genre = "111";
                mp3.SetTag(@"D:\Sia - I go to sleep.mp3");
                */
                //Console.Read();

                //mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                //Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Comment + "!");
                #endregion

                ID3v2 mp32 = new ID3v2();
                mp32.GetTag(@"D:\Sia - I go to sleep.mp3");
                ////mp3.GetTag(@"D:\tmp.mp3");
                ////mp3.GetTag(@"D:\maroon5- animals.mp3");
                ////mp3.SetTag(@"D:\Sia - I go to sleep.mp3");
                Console.WriteLine(mp32.ID3Tag.Title);
                Console.WriteLine(mp32.ID3Tag.Artist);
                Console.WriteLine(mp32.ID3Tag.Album);
                Console.WriteLine(mp32.ID3Tag.Year);
                Console.WriteLine(mp32.ID3Tag.Comment);
                Console.WriteLine(mp32.ID3Tag.Genre);
                Console.WriteLine(mp32.ID3Tag.Track);
                /*
                mp3.ID3Tag.Album = "пупсик";
                mp3.ID3Tag.Artist = "2";
                mp3.ID3Tag.Comment = "йцук";
                mp3.ID3Tag.Title = "3";
                mp3.ID3Tag.Year = "202020";
                mp3.ID3Tag.Genre = "1";
                mp3.ID3Tag.Track = "100";
                mp3.SetTag(@"D:\Sia - I go to sleep.mp3");
                //mp3.SetTag(@"D:\maroon5- animals.mp3");
                 */

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            Console.ReadLine();
        }
    
    }
}
