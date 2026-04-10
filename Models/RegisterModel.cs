namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Model cho trang ??ng ký
    /// </summary>
    public class RegisterModel
    {
        public int ReId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AgreeTerms { get; set; } = true;
    }
}