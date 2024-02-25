using LabWorld.Model;
using LabWorld.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LabWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IEmailService _emailService;
        //private readonly ApplicationDbContext _context;

        public HomeController(IRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }
        [HttpGet("GetPatient")]
        public async Task<List<Patient>> getAllpatient(int LabID)
        {
            return  await _repo.GetAllAsync(LabID);
        }
        [HttpPost("Addpatient")]
        public async Task<IActionResult> Addpatient(Patient patient)
        {
            try
            {

             var result =    await _repo.AddPatient(patient);

                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save data: " + ex.Message);
            }
            
        }
        [HttpPost("AddTest")]
        public async Task<IActionResult> AddTest(Test tests)
        {
            try
            {
                
               var response = await _repo.AddTests(tests);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save data: " + ex.Message);
            }
            
        }

        [HttpGet("GetTests")]
        public async Task<List<Test>> GetTests( int LabID)
        {
           
                return await _repo.GetTest_Details(LabID);

        }
        [HttpGet("GetSinglePatient")]
        public async Task<List<Patient>> GetSinglePatient(int id, int LabID)
        {
            return await _repo.GetPatientAsync(id ,LabID);
        }
        [HttpGet("GetTestforSearch")]

        public async Task<List<Test>> GetTestforSearch(string Queryparams,string LabId)
        {
                
                return await _repo.GetTest_byParameter(Queryparams, LabId);
           
        }
        [HttpPost("GeneratePresciption")]
        public async Task<IActionResult> GeneratePresciption([FromBody] List<Prescriptions> PR)
        {   
            try
            {

               var result = await  _repo.GenertaePresciption(PR);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to save data: " + ex.Message);
            }

        }
        [HttpGet("GetPresciptionsData")]

        public async Task<List<GetPresciptions>> GetPresciptionsData(int PatientID,int LabID)
        {

            return await _repo.GetPresciption(PatientID,LabID);

        }
        [HttpGet("Reports")]

        public async Task<IEnumerable<Reports>> Reports(int PatientID, int LabID)
        {

            return await _repo.PrintReports(PatientID, LabID);

        }
        [HttpGet("Dashboard")]
        public async Task<Dashboard> Dashboard(int Labid)
        {
            return await _repo.getDashboardData(Labid);
        }
        [HttpGet("PatientGloablSearch")]
        public async Task<IEnumerable<Patient>> PatientGloablSearch (int labid,string Name)
        {
            var result = await _repo.GloablSearchPatient(labid, Name);
            return result;
        }
        [HttpPost("UploadLogoImage")]
        public async Task<IActionResult> UploadLogoImage([FromForm] int LabID,[FromForm] string TEMP)
        {
            try
            {
                var file = Request.Form.Files[0];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var imageId = await _repo.UploadImage(LabID,TEMP, fileName, memoryStream.ToArray());

                    return Ok(new { ImageId = imageId });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
        [HttpGet("GetLogoImage")]
        public async Task<List<LogoImage>> getLogoImage(int LabID)
        {
            return await _repo.GetImage(LabID);
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string toEmail, string subject, string body)
        {
            await _emailService.SendEmailAsync(toEmail, subject, body);
            return Ok();
        }
    }
}
