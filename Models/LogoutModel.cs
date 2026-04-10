namespace SeleniumNUnitExcelAutomation.Models
{
    public class LogoutModel : ILoginCredentials
    {
        public int LoID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}