namespace GraficosFullWMS.Dominio.File
{
    public interface IFile
    {
        void Save(string text);
        string Load();
    }
}
