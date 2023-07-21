using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficosFullWMS.Dominio.File
{
    public interface IFile
    {
        void Save(string text);
        string Load();
    }
}
