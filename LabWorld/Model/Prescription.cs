using System.ComponentModel.DataAnnotations;

namespace LabWorld.Model
{
    public class Prescriptions
    {
        [Key]
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }
        public int TestID { get; set; }
        public int userID { get; set; }
        public int LaboratoryID { get; set; }
        public string testResult { get; set;}
        //public DateTime Created_at { get; set; }

    }
    public class GetPresciptions
    {
        [Key]
        public int PrescriptionID { get; set; }
        public string NormalRange { get; set; }
        public string Unit { get; set; }
        public string TestName { get; set; }
        public string testResult { get; set; }
        //public DateTime Created_at { get; set; }

    }
    public class Reports
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string? ReferredBy { get; set; }
        public string? ContactNumber { get; set; }
        public string? Sex { get; set; }
        public string? Age { get; set; }
        public string? Address { get; set; }
        public string NormalRange { get; set; }
        public string Unit { get; set; }
        public string TestName { get; set; }
        public string testResult { get; set; }
        public int TestID { get; set; }
        public string? FileName { get; set; }
        public byte[]? Data { get; set; }
        public string? SelectedTemplate { get; set; }
        public string LaboratoryName { get; set; }
        public string LaboratoryContactNumber { get; set; }
        public string LaboratoryEmail { get; set; }
        public string LaboratoryAddress { get; set; }
    }
}
