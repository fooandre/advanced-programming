from datetime import datetime


class Employee:
    def __init__(self, record):
        data = record.split('|')
        self.__nric = data[0]
        self.__full_name = data[1]
        self.__salutation = data[2]
        self.__start_date = datetime(*[int(i) for i in data[3][:11].split("/")[::-1]])
        self.__designation = data[4]
        self.__department = data[5]
        self.__mobile_no = int(data[6])
        self.__hire_type = data[7]
        self.__salary = int(data[8])

    def get_start_year(self): return self.__start_date.year
    def get_salary(self): return self.__salary
    def get_hire_type(self): return self.__hire_type
    def set_salary_by_multiplier(self, multiplier): 
        self.__salary *= multiplier
        print(self)
        return self

    def __str__(self): return f"{self.__full_name} ({self.__nric})\n{self.__designation}\n{self.__hire_type} Payout: ${self.__salary:.0f}\n------------------------"
 