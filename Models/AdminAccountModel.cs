namespace SeleniumNUnitExcelAutomation.Models
{
    public class AdminAccountModel
    {
        public int? AccId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; }
        public int? Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int? IsActive { get; set; }
        public string SearchQuery { get; set; }
        public string SearchType { get; set; }
        public int? PageNumber { get; set; }
    }
}