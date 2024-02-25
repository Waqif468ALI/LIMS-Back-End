using System.ComponentModel.DataAnnotations;

namespace LabWorld.Model
{
    public class Test
    {
        [Key]
        public int TestID { get; set; }
        public string NormalRange { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }
        public string TestName { get; set; }
        public string TestCategoryName { get; set; }
        public string? TestResult { get; set; }
        public string TestPrice { get; set; }

        public string Comments { get; set; }
        public int? LaboratoryID { get; set; }
        public DateTime? created_at { get; set; }
    }
    public class LogoImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        public byte[]? Data { get; set; }
        public int? LaboratoryID { get; set; }
        public string? SelectedTemplate { get; set; }
    }
}
