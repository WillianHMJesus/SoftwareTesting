using System;
using System.Collections.Generic;

namespace Demo
{
    public class Person
    {
        public string Name { get; set; }
        public string Nickname { get; set; }
    }

    public class Employee : Person
    {
        public Employee(string name, double salary)
        {
            Name = string.IsNullOrEmpty(name) ? "Fulano" : name;
            SetSalary(salary);
            SetSkills();
        }
        
        public double Salary { get; set; }
        public ProfessionalLevel ProfessionalLevel { get; set; }
        public IList<string> Skills { get; set; }

        public void SetSalary(double salary)
        {
            if (salary < 500) throw new Exception("Salario inferior ao permitido");

            Salary = salary;

            if (salary < 2000) ProfessionalLevel = ProfessionalLevel.Junior;
            else if (salary >= 2000 && salary < 8000) ProfessionalLevel = ProfessionalLevel.Pleno;
            else ProfessionalLevel = ProfessionalLevel.Senior;
        }

        public void SetSkills()
        {
            var basicSkills = new List<string>()
            {
                "Lógica de Programação",
                "OOP"
            };

            Skills = basicSkills;

            switch (ProfessionalLevel)
            {
                case ProfessionalLevel.Pleno:
                    Skills.Add("Testes");
                    break;
                case ProfessionalLevel.Senior:
                    Skills.Add("Testes");
                    Skills.Add("Microservices");
                    break;
            }
        }
    }

    public enum ProfessionalLevel
    {
        Junior,
        Pleno,
        Senior
    }

    public class EmployeeFactory
    {
        public static Employee Create(string name, double salary)
        {
            return new Employee(name, salary);
        }
    }
}
