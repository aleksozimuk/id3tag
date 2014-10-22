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
                //ID3v1 mp3 = new ID3v1();

                //mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                //Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Comment + "!");

                //Console.Read();

                //mp3.ID3v1Tag.Album = "AlbumTest";
                //mp3.ID3v1Tag.Title = "TitleTest";
                //mp3.ID3v1Tag.Artist = "ArtistTest";
                //mp3.ID3v1Tag.Year = "0000";
                //mp3.ID3v1Tag.Comment = "CommentTest";
                //mp3.ID3v1Tag.Genre = "0";
                //mp3.SetTag(@"D:\Sia - I go to sleep.mp3");

                //Console.Read();

                //mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                //Console.WriteLine(mp3.ID3v1Tag.Artist + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Title + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Album + "!");
                //Console.WriteLine(mp3.ID3v1Tag.Comment + "!");
                #endregion


                //byte[] tag = new byte[1];
                //using (FileStream fs = new FileStream(@"D:\Sia - I go to sleep.mp3", FileMode.Open, FileAccess.Read))
                //{
                //    fs.Seek(5, SeekOrigin.Begin);
                //    fs.Read(tag, 0, 1);
                //}

                ////0-2 - строка "ID3"                            заголовок тега
                ////3-4 - 03 00                                   версия/субверсия
                ////
                //Console.WriteLine(BitConverter.ToString(tag));//3-4

                /*
                ID3v2 mp3 = new ID3v2();
                mp3.GetTag(@"D:\Sia - I go to sleep.mp3");
                //mp3.GetTag(@"D:\maroon5- animals.mp3");
                //mp3.SetTag(@"D:\Sia - I go to sleep.mp3");
                Console.WriteLine(mp3.ID3Tag.Title);
                Console.WriteLine(mp3.ID3Tag.Artist);
                Console.WriteLine(mp3.ID3Tag.Album);
                Console.WriteLine(mp3.ID3Tag.Year);
                Console.WriteLine(mp3.ID3Tag.Comment);
                Console.WriteLine(mp3.ID3Tag.Genre);
                Console.WriteLine(mp3.ID3Tag.Track);
                */



                int size = 530;

                byte[] result = new byte[4];
                int countBits = 0;
                ///int temp = 0;

                for (int i = 3; i >= 0; i--)
                {
                    int temp = 0;
                    for (int ii = 0; ii < 7; ii++)
                    {
                        if ((size & (1 << countBits)) != 0)
                            temp |= (1 << ii);
                        countBits++;
                    }
                    result[i] = (byte)temp;
                }

                {
                }
                //byte[] arr = new byte[] { 1, 2, 3 };
                //arr = arr.Reverse().ToArray();


                //int res = 0;
                //byte[] arr = { 7, 7, 7, 7 };
                //int count = 0;

                //for (int i = 3; i >= 0; i--)
                //{
                //    for (int ii = 0; ii < 7; ii++)
                //    {
                //        if ((arr[i] & (1 << ii)) != 0)
                //            res |= (1 << count);
                //        count++;
                //    }
                //}

                //Console.WriteLine(res);

                //обозреватель решений - панель управления
                //свойства






                //byte[] ar = new byte[] { 255, 254, 3, 4};

                //byte[] ar2;// = ar.Where((elem, ind) => ind > 1).ToArray();

                //ar2 = ar.Where((elem, ind) => !(((elem == 255) && (ind == 0)) || ((elem == 254) && (ind == 1)))).ToArray();


                //Console.WriteLine();

                /*
                string str = "саша";
                string str2 = "маша";

                byte[] arr = Encoding.Unicode.GetBytes(str);
                byte[] arr2 = Encoding.Unicode.GetBytes(str2);
                byte[] res = arr.Concat(arr2).Concat(arr).ToArray();

                //res = res.Concat(new byte[] { 1, 2, 15 }).ToArray();
                res.Concat(arr).
                    ToArray().
                    Concat(arr).
                    Concat(arr).
                    ToArray();

                Console.WriteLine(Encoding.Unicode.GetString(res));
                */
                /*
                string str = @"D:\1.txt";

                using (FileStream fs = new FileStream(str, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream fs2 = new FileStream(@"D:\2.txt", FileMode.Create, FileAccess.Write))
                    {
                        while (fs.Position != fs.Length)
                        {
                            fs2.WriteByte((byte)fs.ReadByte());
                            

                        }

                        
                    }
                }

                File.Delete(str);
                File.Move(@"D:\2.txt", str);
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
