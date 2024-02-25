using Azure;
using LabWorld.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace LabWorld.Repositories
{
    public class PRepository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public PRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPatient(Patient patient)
        {
            Patient pt = new Patient();
            pt.PatientName = patient.PatientName;
            pt.ReferredBy = patient.ReferredBy;
            pt.ReferredbyNumber = "1";
            pt.ContactNumber = patient.ContactNumber;
            pt.Comments = patient.Comments;
            pt.DOB = DateTime.Now;
            pt.Address = patient.Address;
            pt.Age = patient.Age;
            pt.Sex = patient.Sex;
            pt.IsActive = true;
            pt.IsDeleted = false;
            pt.InsertedBy = "Admin";
            pt.LaboratoryID  = patient.LaboratoryID;

            _context.Patients.Add(pt);
         var response=   await _context.SaveChangesAsync();

            if (response > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetAllAsync(int LabID)
        {
            var data = await _context.Patients.Where(x => x.IsDeleted != true && x.LaboratoryID == LabID).ToListAsync();
            return data;
        }


        public async Task<List<Patient>> GetPatientAsync(int id, int LabID)
        {
            return await _context.Patients.Where(x => x.PatientID == id && x.LaboratoryID == LabID).ToListAsync();
        }


        public async Task<bool> AddTests(Test test)
        {
            try
            {
                Test details = new Test();
                details.NormalRange = test.NormalRange;
                details.Unit = test.Unit;
                details.IsActive = true;
                details.IsChecked = true;
                details.TestName = test.TestName;
                details.TestCategoryName = test.TestCategoryName;
                details.TestResult = "";
                details.TestPrice = test.TestPrice;
                details.Comments = test.Comments;
                details.LaboratoryID = test.LaboratoryID;

                _context.Tests.Add(details);
              var response =   await _context.SaveChangesAsync();
                if (response > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public async Task<List<Test>> GetTest_Details(int LabID)
        {

            return await _context.Tests.Where(x => x.LaboratoryID == LabID).ToListAsync();
        }

        public async Task<List<Test>> GetTest_byParameter(string? param, string LabId)
        {
            var parameter = new SqlParameter("@param", param);
            var parameter1 = new SqlParameter("@LabId", LabId);

            var response = await _context.Tests.FromSqlRaw("EXEC [dbo].[GetLabOn_Search] @param,@LabId", parameter,parameter1).ToListAsync();
            return response;

        }

        public async Task<bool> GenertaePresciption(List<Prescriptions> PR)
        {
            try
            {
                foreach (var PRs in PR)
                {
                    _context.Prescription.Add(PRs);
                }

               var response =   await _context.SaveChangesAsync();
                if(response > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }

        }

        public async Task<List<GetPresciptions>> GetPresciption(int patientID, int LabID)
        {
            var parameterOne = new SqlParameter("@patientID", patientID);
            var parameterTwo = new SqlParameter("@LabID", LabID);
            var response = await _context.GetPresciption.FromSqlRaw("EXEC [dbo].[GetPresciption] @patientID,@LabID", parameterOne, parameterTwo).ToListAsync();
            return response;
        }

        //public async Task<List<Reports>> PrintReports(int PatientID, int LabID)
        // {
        //          var PID = new SqlParameter("@PatientID", PatientID);
        //          var LID = new SqlParameter("@LabID", LabID);
        //          Console.WriteLine($"PatientID: {PatientID}, LabID: {LabID}");
        //          var response = await _context.Reports.FromSqlRaw("EXEC [dbo].[GetReports] @PatientID,@LabID", PID, LID).ToListAsync();
        //          return response;
        // }

        public async Task<IEnumerable<Reports>> PrintReports(int patientID, int labID)
        {
                var response = await (
                from p in _context.Patients
                join ps in _context.Prescription on p.PatientID equals ps.PatientID
                join t in _context.Tests on ps.TestID equals t.TestID
                join I in _context.LogoImages on p.LaboratoryID equals I.LaboratoryID
                join L in _context.LabDetails on p.LaboratoryID equals L.LaboratoryID
                where p.PatientID == patientID && p.LaboratoryID == labID
                select new Reports
                {
                    PatientID = p.PatientID,
                    PatientName = p.PatientName,
                    ReferredBy = p.ReferredBy,
                    ContactNumber = p.ContactNumber,
                    Sex = p.Sex,
                    Age = p.Age,
                    Address = p.Address,
                    testResult = ps.testResult,
                    NormalRange = t.NormalRange,
                    Unit = t.Unit,
                    TestName = t.TestName,
                    TestID = t.TestID,
                    FileName =  I.FileName,
                    Data = I.Data,
                    SelectedTemplate = I.SelectedTemplate,
                    LaboratoryName = L.LaboratoryName,
                    LaboratoryContactNumber = L.LaboratoryContactNumber,
                    LaboratoryEmail = L.LaboratoryEmail,
                    LaboratoryAddress = L.LaboratoryAddress,
                }
            )
            .ToListAsync();

            return response;
        }

        public async Task<Dashboard>   getDashboardData(int Labid)
        {
            var totalPatients = _context.Patients.Count(p => p.LaboratoryID == Labid);
            var totalTests = _context.Tests.Count(t => t.LaboratoryID == Labid);
            var result = new Dashboard
            {
                TotalPatients = totalPatients,
                TotalTests = totalTests
            };
             return  result;
        }

        public async Task<IEnumerable<Patient>> GloablSearchPatient (int labid,string Name)
        {
            var parameterone = new SqlParameter("@LABID", labid);
            var parameterTwo = new SqlParameter("@PARAMETER", Name);
            var result = await _context.Patients.FromSqlRaw("EXEC [dbo].[SEARCHPATIENTGLOBAL] @LABID,@PARAMETER", parameterTwo, parameterone).ToListAsync();
            return result;

        }
        public async Task<int> UploadImage(int LabID, string TEMP, string fileName, byte[] data)
        {
            // Check if an image already exists for the LabID
            var existingImage = await _context.LogoImages.FirstOrDefaultAsync(img => img.LaboratoryID == LabID);

            if (existingImage != null)
            {
                // Update the existing image
                existingImage.FileName = fileName;
                existingImage.Data = data;
                existingImage.SelectedTemplate = TEMP;
            }
            else
            {
                // Create a new image
                var newImage = new LogoImage
                {
                    FileName = fileName,
                    Data = data,
                    LaboratoryID = LabID,
                    SelectedTemplate = TEMP
                };

                _context.LogoImages.Add(newImage);
            }

            await _context.SaveChangesAsync();

            return LabID;
        }

        public async Task<List<LogoImage>> GetImage(int LabID)
        {
            return await _context.LogoImages.Where(x => x.LaboratoryID == LabID).ToListAsync();
        }
    }
}
