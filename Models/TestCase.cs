using System.Collections.Generic;

namespace SeleniumNUnitExcelAutomation.Models
{
    public class TestCase
    {
        public string TestCaseId { get; set; }
        public List<TestStep> Steps { get; set; } = new List<TestStep>();
    }
}