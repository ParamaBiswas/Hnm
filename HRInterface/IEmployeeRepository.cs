using HRModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRInterface
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployeeAll();
        string SaveEmployee(Employee objEmployee);
        Employee GetEmployeeByID(string pEmployeeId, int pCompanyCode);
    }
}
