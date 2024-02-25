using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace LabWorld.Model
{
    public class Dashboard
    {
        [Key]
        public int TotalPatients { get; set; }
        public int TotalTests { get; set; }

    }
}
