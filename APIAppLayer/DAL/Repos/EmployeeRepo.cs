using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class EmployeeRepo
    {
        static EmpContext db;
        static EmployeeRepo() { db = new EmpContext(); }

        public static object Get()
        {
            return db.Employees.ToList();
        }
        public static Employee Get(int id)
        {
            return db.Employees.Find(id);
        }
        public static bool Create(Employee employee)
        {
            db.Employees.Add(employee);
            return db.SaveChanges()>0;
        }
        public static bool Update(Employee employee)
        {
            var exemp = Get(employee.Id);
            db.Entry(exemp).CurrentValues.SetValues(employee);
            return db.SaveChanges() > 0;

        }
        public static bool Delete(int id) 
        {
            var exemp = Get(id);
            db.Employees.Remove(exemp);
            return db.SaveChanges() > 0;
        }
    }
}
