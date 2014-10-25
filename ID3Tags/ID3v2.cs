using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ID3Tags
{
    class ID3v2
    {
        private const byte HEADERTAGLENGTH = 3;
        private const byte VERSIONTAGLENGTH = 2;
        private const byte FLAGTAGLENGTH = 1;
        private const byte SIZETAGLENGTH = 4;

        private const byte NAMEFRAMELENGTH = 4;
        private const byte SIZEFRAMELENGTH = 4;
        private const byte FLAGFRAMELENGTH = 2;

        private ID3Tag _id3Tag;
        /// <summary>
        /// конструктор
        /// </summary>
        public ID3v2()
        {
            _id3Tag = new ID3Tag();
        }
        /// <summary>
        /// свойство
        /// </summary>
        public ID3Tag ID3Tag
        {
            get { return _id3Tag; }
        }
        /// <summary>
        /// проверяет наличие у юникод-строки BOM-маркера(первые два байта(FF-FE))
        /// </summary>
        /// <param name="uString">массив байт юникод-строки с BOM-маркером</param>
        /// <returns>массив байт юникод-строки без BOM-маркера</returns>
        private byte[] ParseUnicodeString(byte[] uString)
        {
            return uString.Where((element, index) => !(((element == 255) && (index == 0)) || ((element == 254) && (index == 1)))).ToArray();
        }
        /// <summary>
        /// обрабатывает текстовые фреймы, возвращая значащую информацию
        /// </summary>
        /// <param name="data">массив байт тела фрейма</param>
        /// <returns>строка со значащей информацией</returns>
        private string ParseTextFrames(byte[] data)
        {
            // проверяем кодировку фрейма
            string encoding = BitConverter.ToString(data.Where((element, index) => index == 0).ToArray());
            if (encoding == "01")
                return Encoding.Unicode.GetString(ParseUnicodeString(data.Where((element, index) => index > 0).ToArray()));
            return Encoding.Default.GetString(data.Where((element, index) => index > 0).ToArray());
        }
        /// <summary>
        /// обрабатывает фрейм-комментарий, возвращая значащую информацию
        /// </summary>
        /// <param name="data">массив байт тела фрейма</param>
        /// <returns>строка со значащей информацией</returns>
        private string ParseCommentFrames(byte[] data)
        {
            // проверяем кодировку фрейма
            string encoding = BitConverter.ToString(data.Where((element, index) => index == 0).ToArray());
            // убираем 3 байта, обозначающих язык и 1 байт кодировки фрейма
            data = data.Where((element, index) => index > 3).ToArray();
            if (encoding == "01")
            {
                // убираем BOM-маркер
                data = ParseUnicodeString(data);
                // убираем 2 байта, обозначающие краткое содержание комментария
                data = data.Where((element, index) => index > 1).ToArray();
                return Encoding.Unicode.GetString(ParseUnicodeString(data));
            }
            else
            {
                return Encoding.Default.GetString(data.Where((element, index) => index > 0).ToArray());
            }
        }
        /// <summary>
        /// Возвращает размер тега(без учета заголовка).
        /// Формируется из 4 последних байт заголовка 
        /// без учета старших бит каждого байта
        /// </summary>
        /// <param name="arr">массив 4 последних байт заголовка тега</param>
        /// <returns>целочисленный размер тега без учета заголовка</returns>
        private int GetTagSizeInt(byte[] arr)
        {
            int result = 0;
            int countBits = 0;

            for (int i = 3; i >= 0; i--)
            {
                for (int ii = 0; ii < 7; ii++)
                {
                    if ((arr[i] & (1 << ii)) != 0)
                        result |= (1 << countBits);
                    countBits++;
                }
            }

            return result;
        }
        /// <summary>
        /// переводит размер тега в массив 4 байт,
        /// старшие биты каждого байта не значимые
        /// </summary>
        /// <param name="size">размер тега</param>
        /// <returns>размер тега, представленный массивом 4 байт</returns>
        private byte[] GetTagSizeByteArray(int size)
        {
            byte[] result = new byte[4];
            int countBits = 0;

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

            return result;
        }
        /// <summary>
        /// определяет размер фрейма, учитывая порядок байт архитектуры ком-ра
        /// </summary>
        /// <param name="arr">массив 4 байт размера фрейма</param>
        /// <returns>размер фрейма</returns>
        private int GetFrameSizeInt(byte[] arr)
        {
            if (BitConverter.IsLittleEndian)
                arr = arr.Reverse().ToArray();
            return BitConverter.ToInt32(arr, 0);
        }
        /// <summary>
        /// получает размер фрейма в виде массива байт
        /// </summary>
        /// <param name="size">размер фрейма</param>
        /// <returns>размер фрейма(4 байт)</returns>
        private byte[] GetFrameSizeByteArray(int size)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.GetBytes(size).Reverse().ToArray();
            return BitConverter.GetBytes(size);
        }
        /// <summary>
        /// создает массив байт текстовых фреймов(заголовок + тело)
        /// </summary>
        /// <param name="frameID">ID текстового фрейма</param>
        /// <param name="frameData">значащая информация фрейма</param>
        /// <returns>массив байт</returns>
        private byte[] GetTextFrame(byte[] frameID, byte[] frameData)
        {
            return frameID
                            .Concat(GetFrameSizeByteArray(frameData.Length + 1))
                            .Concat(new byte[] { 0, 0 })
                            .Concat(new byte[] { 1 })
                            .Concat(frameData)
                            .ToArray();
        }
        /// <summary>
        /// создает массив байт фрейма-комментария(заголовок + тело)
        /// </summary>
        /// <param name="frameID">ID фрейма-комментария</param>
        /// <param name="frameData">значащая информация фрейма</param>
        /// <returns>массив байт</returns>
        private byte[] GetCommentFrame(byte[] frameID, byte[] frameData)
        {
            return frameID
                            .Concat(GetFrameSizeByteArray(frameData.Length + 10))
                            .Concat(new byte[] { 0, 0 })
                            .Concat(new byte[] { 1 })
                            .Concat(Encoding.Default.GetBytes("eng"))
                            .Concat(new byte[] { 255, 254 })
                            .Concat(new byte[] { 0, 0 })
                            .Concat(new byte[] { 255, 254 })
                            .Concat(frameData)
                            .ToArray();
        }
        /// <summary>
        /// создает массив байт тега ID3(заголовок + тело)
        /// для дальнейшего сохранения
        /// </summary>
        /// <param name="tagBody">тело тега без заголовка</param>
        /// <returns>тег в виде массива байт</returns>
        private byte[] GetTagID3ByteArray(byte[] tagBody)
        {
            return Encoding.Default.GetBytes("ID3")
                                                    .Concat(new byte[] { 3, 0 })
                                                    .Concat(new byte[] { 0 })
                                                    .Concat(GetTagSizeByteArray(tagBody.Length))
                                                    .Concat(tagBody)
                                                    .ToArray();
        }
        /// <summary>
        /// получает имеющиеся метаданные аудиофайла
        /// </summary>
        /// <param name="mp3FilePath">полный путь к аудиофайлу</param>
        public void GetTag(string mp3FilePath)
        {
            int tagSize = 0;
            int frameSize = 0;
            string frameName = "";
            byte[] frameData;

            if (!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");

            using (BinaryReader binReader = new BinaryReader(File.Open(mp3FilePath, FileMode.Open)))
            {
                // проверяем метки тега ID3v2
                if ((new string(binReader.ReadChars(HEADERTAGLENGTH)) == "ID3") && (BitConverter.ToString(binReader.ReadBytes(VERSIONTAGLENGTH)) == "03-00"))
                {
                    binReader.ReadByte();
                    // определяем размер тега
                    tagSize = this.GetTagSizeInt(binReader.ReadBytes(SIZETAGLENGTH));
                    // читаем необходимые фреймы и сохраняем значащую информацию
                    while (binReader.BaseStream.Position <= tagSize)
                    {
                        frameName = new string(binReader.ReadChars(NAMEFRAMELENGTH));
                        frameSize = GetFrameSizeInt(binReader.ReadBytes(SIZEFRAMELENGTH));
                        binReader.ReadBytes(FLAGFRAMELENGTH);
                        frameData = binReader.ReadBytes(frameSize);
                        if ((frameSize == 0) && (frameName == "\0\0\0\0")) break;

                        switch (frameName)
                        {
                            case "TIT2":
                                this._id3Tag.Title = this.ParseTextFrames(frameData);
                                break;
                            case "TPE1":
                                this._id3Tag.Artist = this.ParseTextFrames(frameData);
                                break;
                            case "TALB":
                                this._id3Tag.Album = this.ParseTextFrames(frameData);
                                break;
                            case "TYER":
                                this._id3Tag.Year = this.ParseTextFrames(frameData);
                                break;
                            case "COMM":
                                this._id3Tag.Comment = this.ParseCommentFrames(frameData);
                                break;
                            case "TCON":
                                this._id3Tag.Genre = this.ParseTextFrames(frameData);
                                break;
                            case "TRCK":
                                this._id3Tag.Track = this.ParseTextFrames(frameData);
                                break;
                        }
                    }
                    this._id3Tag.PositionAudioData = tagSize + 10;
                    this._id3Tag.HasTag = true;

                }
                else
                {
                    this._id3Tag.HasTag = false;
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
            int tagSize = 0;
            byte[] tagBody;

            if (!File.Exists(mp3FilePath))
                throw new Exception("файл не найден");

            using(BinaryReader binReader = new BinaryReader(File.Open(mp3FilePath, FileMode.Open)))
            {
                //проверяем наличие структуры
                if ((new string(binReader.ReadChars(HEADERTAGLENGTH)) == "ID3") && (BitConverter.ToString(binReader.ReadBytes(2)) == "03-00"))
                {
                    binReader.ReadByte();
                    tagSize = this.GetTagSizeInt(binReader.ReadBytes(SIZETAGLENGTH));    
                }

                binReader.BaseStream.Position = tagSize;
                //создаем тело тега из введенных пользовательских данных
                tagBody = GetTextFrame(Encoding.Default.GetBytes("TIT2"), Encoding.Unicode.GetBytes(this.ID3Tag.Title))
                            .Concat(GetTextFrame(Encoding.Default.GetBytes("TPE1"), Encoding.Unicode.GetBytes(this.ID3Tag.Artist)))
                            .Concat(GetTextFrame(Encoding.Default.GetBytes("TALB"), Encoding.Unicode.GetBytes(this.ID3Tag.Album)))
                            .Concat(GetTextFrame(Encoding.Default.GetBytes("TYER"), Encoding.Unicode.GetBytes(this.ID3Tag.Year)))
                            .Concat(GetCommentFrame(Encoding.Default.GetBytes("COMM"), Encoding.Unicode.GetBytes(this.ID3Tag.Comment)))
                            .Concat(GetTextFrame(Encoding.Default.GetBytes("TCON"), Encoding.Unicode.GetBytes(this.ID3Tag.Genre)))
                            .Concat(GetTextFrame(Encoding.Default.GetBytes("TRCK"), Encoding.Unicode.GetBytes(this.ID3Tag.Track)))
                            .ToArray();
                //создаем временный файл
                //записываем тег и аудиоданные
                using (BinaryWriter binWriter = new BinaryWriter(File.Create(@"tmp.mp3")))
                {
                    binWriter.Write(GetTagID3ByteArray(tagBody));
                    while (binReader.BaseStream.Position < binReader.BaseStream.Length)
                    {
                        binWriter.Write(binReader.ReadByte());
                    }
                }

            }
            //удаляем старый mp3-файл
            File.Delete(mp3FilePath);
            File.Move(@"tmp.mp3", mp3FilePath);
            this._id3Tag.PositionAudioData = tagSize + 10;
            this._id3Tag.HasTag = true;
        }
    }
}





//// id фрейма
//string frameID = new string(binReader.ReadChars(4));
//// размер фрейма
//string sizeFrame = (BitConverter.ToString(binReader.ReadBytes(4)));
//// флаги фрейма
//binReader.ReadBytes(2);
//// кодировка - 1 байт
//string encoding = BitConverter.ToString(new byte[1] { binReader.ReadByte() });
//// язык - 3 байта
//string language = new string(binReader.ReadChars(3));
//// UNICODE NULL(FF FE 00 00)
//string unicodeNull = (BitConverter.ToString(binReader.ReadBytes(4)));
////UNICODE BOM(FF FE)
//string unicodeBom = (BitConverter.ToString(binReader.ReadBytes(2)));
//// текст фрейма
//string str = Encoding.Unicode.GetString(binReader.ReadBytes(16));
//Console.WriteLine(str);
////UNICODE BOM(FF FE)
////string unicodeBom2 = (BitConverter.ToString(binReader.ReadBytes()));
//Console.WriteLine();
//// фрейм следующий
//string str2 = new string(binReader.ReadChars(4));//Encoding.Default.GetString(binReader.ReadBytes(4));
//string sizeFrame2 = (BitConverter.ToString(binReader.ReadBytes(4)));
//binReader.ReadBytes(2);
//string enc = BitConverter.ToString(new byte[1] { binReader.ReadByte() });

//string str3 = new string(binReader.ReadChars(4));

//------------------------------------------------------------------test --------------------------------------------


//int sTag = tagSize;

//int sFrame = 0;
//while (binReader.BaseStream.Position <= sTag)
//{



//    Console.Write(new string(binReader.ReadChars(4)) + " - ");
//    sFrame = GetSizeTag(binReader.ReadBytes(4));
//    Console.Write(sFrame + " - ");
//    binReader.ReadBytes(2);
//    Console.WriteLine(new string(binReader.ReadChars(sFrame)));

//    //count += 4 + 4 + 2 + sFrame;
//}


////byte[] arr =  binReader.ReadBytes(650);




//-------------------------------------------------------------







//Console.WriteLine(this._id3Tag.Title);
//                    Console.WriteLine(this._id3Tag.Artist);
//                    Console.WriteLine(this._id3Tag.Album);
//                    Console.WriteLine(this._id3Tag.Year);
//                    Console.WriteLine(this._id3Tag.Comment);
//                    Console.WriteLine(this._id3Tag.Genre);
//                    Console.WriteLine(tagSize);