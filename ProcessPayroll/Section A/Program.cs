namespace SectionA;
public class Program {
    private delegate Task GenerateInfo(List<Employee> employees);
    private static void Main() {
        List<Employee> employees = readHRMasterList().Result;

        GenerateInfo generateInfo = generateInfoForCorpAdmin;
        generateInfo += generateInfoForProcurement;
        generateInfo += generateInfoForITDepartment;

        generateInfo(employees).Wait();
    }

    public static async Task<List<Employee>> readHRMasterList() {
        List<Employee> employees = new();
        string[] records = await File.ReadAllLinesAsync("../HRMasterlist.txt");
        foreach (string record in records) employees.Add(new Employee(record));
        return employees;
    }

    private static async Task generateInfoForCorpAdmin(List<Employee> employees) {
        List<string> toWrite = new();
        foreach (Employee employee in employees) toWrite.Add(employee.FormatInfoForCorpAdmin());
        await File.AppendAllLinesAsync("./output/CorporateAdmin.txt", toWrite);
    }

    private static async Task generateInfoForProcurement(List<Employee> employees) {
        List<string> toWrite = new();
        foreach (Employee employee in employees) toWrite.Add(employee.FormatInfoForProcurement());
        await File.AppendAllLinesAsync("./output/Procurement.txt", toWrite);
    }

    private static async Task generateInfoForITDepartment(List<Employee> employees) {
        List<string> toWrite = new();
        foreach (Employee employee in employees) toWrite.Add(employee.FormatInfoForITDepartment());
        await File.AppendAllLinesAsync("./output/ITDepartment.txt", toWrite);
    }
}