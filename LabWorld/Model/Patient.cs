using System.ComponentModel.DataAnnotations;

namespace LabWorld.Model
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string? ReferredBy { get; set; }
        public string? ReferredbyNumber { get; set; }
        public string? ContactNumber { get; set; }
        public string?  Sex { get; set; }
        public string? Age { get; set; }
        public string? Comments { get; set; }
        public DateTime?  DOB { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? InsertedBy { get; set; }
        public int? LaboratoryID { get; set; }
        public DateTime? created_at { get; set; }

    }
}
