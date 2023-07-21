using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
