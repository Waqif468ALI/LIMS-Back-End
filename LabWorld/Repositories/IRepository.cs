using LabWorld.Model;

namespace LabWorld.Repositories
{
    public interface IRepository
    {
            
            
        public  Task<List<Patient>> GetAllAsync( int LabID);
        public Task<bool> AddPatient(Patient patient);
        public Task<List<Patient>> GetPatientAsync(int Id,int LabID);
        public Task<bool> AddTests(Test test);
        public  Task< List<Test>> GetTest_Details(int LabID);
        public Task<List<Test>> GetTest_byParameter(string param, string LabId);
        public Task<bool>   GenertaePresciption(List<Prescriptions> PR);
        public Task <List<GetPresciptions>> GetPresciption(int patientID,int LabID);
        public Task<IEnumerable<Reports>> PrintReports(int patientID, int LabID);
        public Task<Dashboard> getDashboardData(int Labid);

        public Task<IEnumerable<Patient>> GloablSearchPatient(int Labid,string Name);
        public Task<int> UploadImage(int LabID ,string TEMP,  string fileName, byte[] data);
        public Task<List<LogoImage>> GetImage(int LabID);



    }
}
