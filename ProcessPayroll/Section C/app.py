from functools import reduce

from Employee import Employee

if __name__ == "__main__":
    with open("../HRMasterlistB.txt") as file: employees = list(map(lambda line: Employee(line), file.readlines()))
    employees = list(map(lambda e: e.set_salary_by_multiplier(0.85), filter(lambda e: e.get_start_year() > 1995 and e.get_hire_type() == "FullTime", employees)))
    print(f"Total Payroll Amount: ${reduce(lambda total, e: total + e.get_salary(), filter(lambda e: e.get_start_year() > 1995 and e.get_hire_type() == 'FullTime', employees), 0):.0f} to be paid to {len(employees)} employees.")
