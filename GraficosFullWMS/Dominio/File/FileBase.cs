using System;
using System.IO;

namespace GraficosFullWMS.Dominio.File
{
    public class FileBase : IFile
    {
        protected string Path;

        public FileBase(string path)
        {
            this.Path = path;
        }

        public string Load()
        {
            if (!System.IO.File.Exists(this.Path))
            {
                throw new Exception("Arquivo não encontrado!");
            }

            string value = string.Empty;
            using (StreamReader sr = new StreamReader(this.Path))
            {
                value = sr.ReadToEnd();
            }

            return value;
        }

        public void Save(string text)
        {
            using (StreamWriter sw = new StreamWriter(this.Path))
            {
                sw.Write(text);
            }

            return;
        }

        public void Override(string text)
        {
            using (StreamWriter sw = new StreamWriter(this.Path))
            {
                sw.Write(text);
                sw.Flush();
            }
        }
    }
}
