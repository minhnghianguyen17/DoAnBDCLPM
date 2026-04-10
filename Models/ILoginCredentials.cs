namespace SeleniumNUnitExcelAutomation.Models
{
    /// <summary>
    /// Interface chứa các thông tin cần thiết để đăng nhập
    /// </summary>
    public interface ILoginCredentials
    {
        string Email { get; }
        string Password { get; }
    }
}