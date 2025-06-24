using TestMaster.Models;

namespace TestMaster.Services
{
    public interface IFileService
    {
        void Save(Test test, string path);
        Test Load(string path);
    }
}