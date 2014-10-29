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
        private const byte COMMENTTAGLENGTH = 28;
        private const byte GENRETAGLENGTH = 1;
        private const byte TRACKTAGLENGTH = 1;

        private ID3Tag _id3v1Tag;
        /// <summary>
        /// конструктор
        /// </summary>
        public ID3v1()
        {
            _id3v1Tag = new ID3Tag();
        }
        /// <summary>
        /// свойство
        /// </summary>
        public ID3Tag ID3v1Tag
        {
            get { return _id3v1Tag; }
        }
        /// <summary>
        /// переводит метаданные в массив байт
        /// </summary>
        /// <param name="metaData">метаданные-строка</param>
        /// <param name="size">размер метаданных</param>
        /// <returns>массив байт</returns>
        private byte[] GetMetaDataByteArray(string metaData, byte size)
        {
            byte[] arr = new byte[size];
            Encoding.Default.GetBytes(metaData).Where((element, index) => index < size).ToArray().CopyTo(arr, 0);
            return arr;
        }
        /// <summary>
        /// получает имеющиеся метаданные аудиофайла
        /// </summary>
        /// <param name="mp3FilePath">полный путь к аудиофайлу</param>
        public void GetTag(string mp3FilePath)
        {     
            if(!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");

            using (BinaryReader binReader = new BinaryReader(File.Open(mp3FilePath, FileMode.Open)))
            {
                binReader.BaseStream.Position = binReader.BaseStream.Length - 128;
                //проверяем метку тега ID3v1
                if (new string(binReader.ReadChars(HEADERTAGLENGTH)) == "TAG")
                {
                    //заполняем все метаданные
                    this.ID3v1Tag.Title = Encoding.Default.GetString(binReader.ReadBytes(TITLETAGLENGTH));
                    this.ID3v1Tag.Artist = Encoding.Default.GetString(binReader.ReadBytes(ARTISTTAGLENGTH));
                    this.ID3v1Tag.Album = Encoding.Default.GetString(binReader.ReadBytes(ALBUMTAGLENGTH));
                    this.ID3v1Tag.Year = Encoding.Default.GetString(binReader.ReadBytes(YEARTAGLENGTH));
                    this.ID3v1Tag.Comment = Encoding.Default.GetString(binReader.ReadBytes(COMMENTTAGLENGTH));
                    if (binReader.ReadByte() == 0) this.ID3v1Tag.Track = Convert.ToString(binReader.ReadByte());
                    this.ID3v1Tag.Genre = Convert.ToString(binReader.ReadByte());
                    this.ID3v1Tag.HasTag = true;
                }
                else
                {
                    this.ID3v1Tag.HasTag = false;
                }
            }
        }
        /// <summary>
        /// заполняет тег пользовательскими данными.
        /// Если тег отсутствует - создает структуру
        /// </summary>
        /// <param name="mp3FilePath"></param>
        public void SetTag(string mp3FilePath)
        {
            long position = 0;
            
            if (!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");
            //проверяем метку тега ID3v1 и сохраняем позицию для начала записи
            using (BinaryReader binReader = new BinaryReader(File.Open(mp3FilePath, FileMode.Open)))
            {
                binReader.BaseStream.Position = binReader.BaseStream.Length - 128;
                if (new string(binReader.ReadChars(3)) == "TAG") 
                {
                    position = binReader.BaseStream.Position - 3; 
                }
                else
                {
                    position = binReader.BaseStream.Length;
                }
            }
            //переводим все метаданные в массивы байт и пишем в файл
            using(BinaryWriter binWriter = new BinaryWriter(File.Open(mp3FilePath, FileMode.Open)))
            {
                binWriter.BaseStream.Position = position;

                binWriter.Write(this.GetMetaDataByteArray("TAG", HEADERTAGLENGTH));
                binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Title, TITLETAGLENGTH));
                binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Artist, ARTISTTAGLENGTH));
                binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Album, ALBUMTAGLENGTH));
                binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Year, YEARTAGLENGTH));
                binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Comment, COMMENTTAGLENGTH));
                binWriter.Write((byte)0);
                //????????????????????????????????????????????????????????
                binWriter.Write(byte.Parse(this.ID3v1Tag.Track));
                binWriter.Write(byte.Parse(this.ID3v1Tag.Genre));
                //binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Track, TRACKTAGLENGTH));
                //binWriter.Write(this.GetMetaDataByteArray(this.ID3v1Tag.Genre, GENRETAGLENGTH));
                //???????????????????????????????????????????????????????
            }

            this.ID3v1Tag.HasTag = true;
        }
    }
}