using TestMaster.Models.DB;

namespace TestMaster.Services
{
    public interface IFileService
    {
        void Save(TestDB test, string path);
        TestDB Load(string path);
    }
}