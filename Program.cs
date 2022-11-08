using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace AddFileToZip
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fileToZip = new Dictionary<string, Stream>(); 

            Console.WriteLine("Hello World!");

            //Ler arquivo e converter em Stream
            Stream streamFile = GetStream("Aqui caminho do arquivo");
            //Add stream com nome do arquivo e extenção
            fileToZip["arquivo.extenção"] = streamFile;
            //Add stream para zip
            Stream addZip = ZipFileHelper(fileToZip, "NomeArquivo.zip");
            //adicionando ao MemoryStream
            MemoryStream ms = new MemoryStream();
            addZip.CopyTo(ms);
            //Salvar localmente
            string pathToSave = @"";
            FileStream objFileStrm = File.Create(pathToSave);
            objFileStrm.Close();
            File.WriteAllBytes(pathToSave, ms.ToArray());

        }
        public static Stream GetStream(string filePath)
        {
            byte[] readBytes = File.ReadAllBytes(filePath);
            MemoryStream writeBlockBytes = new MemoryStream();
            writeBlockBytes.Write(readBytes, 0, readBytes.Length);
            return writeBlockBytes;
        }
        public static Stream ZipFileHelper(Dictionary<string, Stream> files, string zipName)
        {
            try
            {
                var compressedFileStream = new MemoryStream();

                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        string fileName = file.Key;
                        Stream fileStream = file.Value;

                        if (fileName != null && fileStream != null)
                        {
                            var zipEntry = zipArchive.CreateEntry(fileName);

                            using (var zipEntryStream = zipEntry.Open())
                            {
                                fileStream.Position = 0;
                                fileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }
                compressedFileStream.Position = 0;
                return compressedFileStream;
            }
            catch (Exception exception)
            {
                throw new Exception($"Erro ao Compactar Arquivo Zip de {zipName}", exception);
            }
        }
    }
}
}
