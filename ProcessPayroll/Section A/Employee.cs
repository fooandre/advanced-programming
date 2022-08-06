namespace SectionA;
public class Employee {
  public string Nric { get; set; }
  public string FullName { get; set; }
  public string Salutation { get; set; }
  public DateTime StartDate { get; set; }
  public string Designation { get; set; }
  public string Department { get; set; }
  public string MobileNo { get; set; }
  public string HireType { get; set; }
  public double Salary { get; set; }
  public double MonthlyPayout { get; set; }

  public Employee(string record) {
    string[] data = record.Split('|');
    Nric = data[0];
    FullName = data[1];
    Salutation = data[2];
    StartDate = DateTime.Parse(data[3]);
    Designation = data[4];
    Department = data[5];
    MobileNo = data[6];
    HireType = data[7];
    Salary = double.Parse(data[8]);
  }

  public string FormatInfoForCorpAdmin() => $"{FullName}, {Designation}, {Department}";
  public string FormatInfoForProcurement() => $"{Designation}, {FullName}, {MobileNo}, {Designation}, {Department}";
  public string FormatInfoForITDepartment() => $"{Nric}, {FullName}, {StartDate.ToString(@"dd/MM/yyyy")}, {Department}, {MobileNo}";
  public override string ToString() => $"{Nric}|{FullName}|{Salutation}|{StartDate.ToString(@"dd/MM/yyyy")}|{Department}|{Designation}|{MobileNo}|{HireType}|{Salary}";
}