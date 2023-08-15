using Newtonsoft.Json;
using System.IO;

namespace GraficosFullWMS.Dominio.File
{
    public class FileOperations<T> : FileBase
    {
        public FileOperations(string path) : base(path)
        {
            Directory.CreateDirectory(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Files"));
        }

        public T Get()
        {
            string value = this.Load();

            return JsonConvert.DeserializeObject<T>(value);
        }

        public void SaveString(string value)
        {
            this.Save(value);
        }
    }
}
