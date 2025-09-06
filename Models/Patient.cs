using System.ComponentModel.DataAnnotations;

namespace GenHunter.Models;

public class Patient
{
    [Key]
    public string KID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateOnly DOB { get; set; }
    public Sex Sex { get; set; }
}

public enum Sex
{
    M,
    F,
    Other
}