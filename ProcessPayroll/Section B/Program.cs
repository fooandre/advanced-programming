using SectionA;
using static SectionA.Program;

List<Employee> processPayroll(List<Employee> employees) {
  foreach (Employee e in employees) {
    HireType hireType = (HireType)Enum.Parse(typeof(HireType), e.HireType);
    double multiplier = hireType == HireType.FullTime ? 1 : hireType == HireType.PartTime ? 0.5 : 0.25;
    e.MonthlyPayout = e.Salary * multiplier;
    Console.WriteLine($"{e.FullName} ({e.Nric})\n{e.Designation}\n{e.HireType} Payout: ${e.Salary}\n------------------------");
  };

  Console.WriteLine($"Total Payroll Amount: ${employees.Select(e => e.Salary).Sum()} to be paid to {employees.Count()} employees");
  return employees;
}

async void updateMonthlyPayoutToMasterlist(List<Employee> employees) => await File.WriteAllLinesAsync("../HRMasterlistB.txt", employees.Select(e => $"{e.ToString()}|{e.MonthlyPayout}"));

List<Employee> employees = processPayroll(await readHRMasterList());
updateMonthlyPayoutToMasterlist(employees);

enum HireType { FullTime, PartTime, Hourly };