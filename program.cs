using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Transactions;
using TCPdata;     
using TCPextensions;

namespace linqproject
{
    internal class Program
    {
        public class EmployeeComparer : IEqualityComparer<employee>
        {
            public bool Equals(employee emp1, employee emp2)
            {
                if (emp1 == null && emp2 == null)
                    return true;
                if (emp1 == null || emp2 == null)
                    return false;
                return emp1.id == emp2.id &&
                       emp1.firstname == emp2.firstname &&
                       emp1.lastname == emp2.lastname &&
                       emp1.annualsalary == emp2.annualsalary &&
                       emp1.ismanager == emp2.ismanager &&
                       emp1.departmentid == emp2.departmentid;
            }
            public int GetHashCode(employee emp)
            {
                if (emp == null)
                    return 0;
                int hashId = emp.id.GetHashCode();
                int hashFirstName = emp.firstname?.GetHashCode() ?? 0;
                int hashLastName = emp.lastname?.GetHashCode() ?? 0;
                int hashAnnualSalary = emp.annualsalary.GetHashCode();
                int hashIsManager = emp.ismanager.GetHashCode();
                int hashDepartmentId = emp.departmentid.GetHashCode();
                return hashId ^ hashFirstName ^ hashLastName ^ hashAnnualSalary ^ hashIsManager ^ hashDepartmentId;
            }
        }
        static void Main(string[] args)
        {
            List<employee> employees = data.GetEmployees();
            List<department> departments = data.GetDepartments();

            // traditional way by extension method filter and labda expression


            List<employee> employeeslist = data.GetEmployees(); // Now matches the class and method name
            var employeees = employeeslist.Filter(emp => emp.ismanager == false);

            foreach (var x in employees)
            {
                Console.WriteLine(x.firstname + " " + x.lastname);
            }
            List<department> departmentss = data.GetDepartments();
            var deps = departmentss.Filter(dep => dep.shortname.Contains("IT"));


            // method syntax


            //var emps = employees.Select(emp => new
            //{
            //    fullname = emp.firstname + " " + emp.lastname
            //    ,
            //    salary = emp.annualsalary

            //}).Where(emp=> emp.salary>300000);
            //foreach (var emp in emps)
            //{
            //    Console.WriteLine(emp.fullname + " " + emp.salary);
            //}

            // query syntax

            //var results = from emp in employees
            //              select new
            //              {
            //                  fullname = emp.firstname + " " + emp.lastname
            //                  ,
            //                  salary = emp.annualsalary
            //              };
            //foreach (var emp in emps)
            //{
            //    Console.WriteLine(emp.fullname + " " + emp.salary);
            //}

            // using deferred execution with yield return

            var highsal = from empl in employees.gethighsalaries()
                          select new
                          {
                              fullname = empl.firstname + " " + empl.lastname
                ,
                              salary = empl.annualsalary

                          };
            employees.Add(new employee
            {
                id = 4,
                firstname = "ahmed",
                lastname = "ali",
                annualsalary = 50000000,
                ismanager = false,
                departmentid = 2
            });

            foreach (var y in highsal)
            {
                Console.WriteLine(y.fullname + " " + y.salary);
            }

            //using immediate execution with ToList()

            var highsali = (from emplo in employees.gethighsalaries()
                            select new
                            {
                                fullname = emplo.firstname + " " + emplo.lastname
                  ,
                                salary = emplo.annualsalary

                            }).ToList(); // make it immediate execution
            employees.Add(new employee
            {
                id = 5,
                firstname = "sara",
                lastname = "mohamed",
                annualsalary = 70000000,
                ismanager = false,
                departmentid = 2
            });


            // inner join with query syntax

            var joinresult = from dept in departments
                             join empo in employees
                             on dept.id equals empo.departmentid
                             select new
                             {
                                 fullname = empo.firstname + " " + empo.lastname,
                                 departmentname = dept.longname
                             };

            foreach (var p in joinresult)
            {
                Console.WriteLine(p.fullname + " " + p.departmentname);
            }

            // outer join with query syntax


            var outerjoin = from dept in departments
                            join emplx in employees
                            on dept.id equals emplx.departmentid
                            into empgroup

                            select new
                            {
                                employeess = empgroup,
                                departmentname = dept.longname
                            };





            // sorting operators by query syntax
            // there is thenby if using method syntax


            var result = from empu in employees
                         join dept in departments
                         on empu.departmentid equals dept.id
                         orderby dept.id, empu.annualsalary descending
                         select new
                         {
                             employeename = empu.firstname + " " + empu.lastname,
                             departmentname = dept.longname,
                             salary = empu.annualsalary
                         };
            //foreach (var emp in result)
            //{
            //    Console.WriteLine(emp.departmentname + " - " + emp.employeename + " - " + emp.salary);
            //}

            // there is grouping by method syntax using groupby() or ToLookup()
            // the diff between them is that ToLookup()=immediate execution
            // while groupby()=deferred execution


            // grouping operators by query syntax


            var resultyy = from empyy in employees
                         join dept in departments
                         on empyy.departmentid equals dept.id
                         group empyy by dept.id into empgroup
                         select new
                         {
                             fullname = empgroup,
                             departmentid = empgroup.Key

                         };

            //foreach (var emp in result)
            //{
            //    Console.WriteLine("Department ID: " + emp.departmentid);
            //    foreach (var e in emp.fullname)
            //    {
            //        Console.WriteLine(" - " + e.firstname + " " + e.lastname);
            //    }
            //}

            // the result is stored in IGrouping interface 



            // all , any

            var resultany = employees.Any(emp => emp.annualsalary > 5000000);

            if (resultany)
            {
                Console.WriteLine("Yes, there is at least one employee with salary greater than 5,000,000");
            }
            else
            {
                Console.WriteLine("No, there is no employee with salary greater than 5,000,000");
            }
            var result2 = employees.All(emp => emp.annualsalary > 200000);
            if (result2)
            {
                Console.WriteLine("Yes, all employees have salary greater than 200,000");
            }
            else
            {
                Console.WriteLine("No, not all employees have salary greater than 200,000");
            }

            //contain
            // must implement IEqualityComparer interface for custom class
            // 
            var empToFind = new employee
            {
                id = 2,
                firstname = "mahmoud",
                lastname = "wagdy",
                annualsalary = 300000,
                ismanager = false,
                departmentid = 1
            };

            //var result = employees.Contains(empToFind, new EmployeeComparer());
            //if (result)
            //{
            //    Console.WriteLine("Employee found in the list.");
            //}
            //else
            //{
            //    Console.WriteLine("Employee not found in the list.");
            //}

            //oftype


            //// arraylist of mixed types
            var mixedList = new List<object>
            {
                new employee
                {
                    id = 1,
                    firstname = "karim",
                    lastname = "sliman",
                    annualsalary = 600000,
                    ismanager = true,
                    departmentid = 1
                },
                "This is a string",
                42,
                new employee
                {
                    id = 2,
                    firstname = "mahmoud",
                    lastname = "wagdy",
                    annualsalary = 300000,
                    ismanager = false,
                    departmentid = 1
                },
                3.14,
                new department
                {
                    id = 1,
                    shortname = "IT",
                    longname = "Information Technology"
                }
            };

            //// using the OfType<T>() method to filter only employee objects
            var employeesOnly = mixedList.OfType<employee>();

            foreach (var empt in employeesOnly)
            {
                Console.WriteLine($"{empt.firstname} {empt.lastname} - Salary: {empt.annualsalary}");
            }


            //element operators
            // element at , first , last , single
            // there these versions too but with default value if not found

            var emp = employees.ElementAtOrDefault(10);
            var emp2 = employees.FirstOrDefault(e => e.annualsalary > 10000000);
            var emp3 = employees.LastOrDefault(e => e.annualsalary > 10000000);
            //var emp4 = employees.SingleOrDefault(e => e.id == 2);

            // then print as you want 

            //sequence_equal operator


            //var employeeslist1 = data.GetEmployees();
            // must be equal in value and ORDER
            //if its an user defined class must implement IEqualityComparer interface

            //var areEqual = employees.SequenceEqual(employeeslist1, new EmployeeComparer());


            // concat operator

            var moreEmployees = new List<employee>
            {
                new employee
                {
                    id = 4,
                    firstname = "ahmed",
                    lastname = "ali",
                    annualsalary = 500000,
                    ismanager = false,
                    departmentid = 2
                },
                new employee
                {
                    id = 5,
                    firstname = "sara",
                    lastname = "mohamed",
                    annualsalary = 700000,
                    ismanager = false,
                    departmentid = 2
                }
            };
            var allEmployees = employees.Concat(moreEmployees);

            foreach (var empz in allEmployees)
            {
                Console.WriteLine(empz.firstname + " " + empz.lastname);
            }


            //aggregate operators

            // the aggregate operator = It keeps combining elements one by one into a running result.
            var totalSalary = employees.Aggregate(0m, (total, emp) => total + emp.annualsalary);
            Console.WriteLine("Total Annual Salary of all Employees: " + totalSalary);

            // other aggregate operators: sum , min , max , average , count

            var averagesalary = employees.Average(emp => emp.annualsalary);
            var maxsalary = employees.Max(emp => emp.annualsalary);
            var minsalary = employees.Min(emp => emp.annualsalary);
            var countemployees = employees.Count();




            // generation operators (empty , range , repeat)
            var emptylist = Enumerable.Empty<employee>();
            var numberrange = Enumerable.Range(1, 10); // from 1 to 10
            var repeatedvalues = Enumerable.Repeat("Hello", 5); // repeat Hello 5 times




            // set operators (distinct , union , intersect , except)


            // distinct
            var duplicateEmployees = new List<employee>
            {
                new employee
                {
                    id = 1,
                    firstname = "karim",
                    lastname = "sliman",
                    annualsalary = 600000,
                    ismanager = true,
                    departmentid = 1
                },
                new employee
                {
                    id = 2,
                    firstname = "mahmoud",
                    lastname = "wagdy",
                    annualsalary = 300000,
                    ismanager = false,
                    departmentid = 1
                },
                new employee
                {
                    id = 1,
                    firstname = "karim",
                    lastname = "sliman",
                    annualsalary = 600000,
                    ismanager = true,
                    departmentid = 1
                }
            };
            var distinctEmployees = duplicateEmployees.Distinct(new EmployeeComparer());


            // union
            var moreEmployeees = new List<employee>
            {
                new employee
                {
                    id = 2,
                    firstname = "mahmoud",
                    lastname = "wagdy",
                    annualsalary = 300000,
                    ismanager = false,
                    departmentid = 1
                },
                new employee
                {
                    id = 3,
                    firstname = "mazen",
                    lastname = "yasser",
                    annualsalary = 400000,
                    ismanager = false,
                    departmentid = 2
                }
            };
            var allUniqueEmployees = employees.Union(moreEmployeees, new EmployeeComparer());

            // intersect
            var commonEmployees = employees.Intersect(moreEmployeees, new EmployeeComparer());

            // except
            var exclusiveEmployees = employees.Except(moreEmployeees, new EmployeeComparer());




            //partitioning operators (skip , take , skipwhile , takewhile)
            var skippedEmployees = employees.Skip(1); // skip first employee
            var takenEmployees = employees.Take(2); // take first two employees
            //skip until salary > 300000
            var skipWhileEmployees = employees.SkipWhile(emp => emp.annualsalary <= 300000);
            //take until salary > 300000
            var takeWhileEmployees = employees.TakeWhile(emp => emp.annualsalary <= 300000);

            //conversion operators (tolist , toarray , todictionary)
            var employeesList = employees.ToList();
            var employeesArray = employees.ToArray();
            //var employeesDictionary = employees.ToDictionary(emp => emp.id, emp => emp);


            // let and into keywords in query syntax
            // let lets you create a temporary variable inside a LINQ query.
            var letResult = from empz in employees
                            let fullname = empz.firstname + " " + empz.lastname
                            where empz.annualsalary > 300000
                            select new
                            {
                                fullname,
                                empz.annualsalary
                            };
            // into allows you to continue a query after a projection or grouping.
            var intoResult = from empx in employees
                             select new
                             {
                                 empx,
                                 fullname = empx.firstname + " " + empx.lastname
                             } into empWithFullname
                             where empWithFullname.empx.annualsalary > 300000
                             select new
                             {
                                 empWithFullname.fullname,
                                 empWithFullname.empx.annualsalary
                             };

            //select many
            // the diff between it and select is that select many flattens the resulting collections into a single collection.
            // especially in the case of one to many relationship
            var selectManyResult = from dept in departments
                                   join empr in employees
                                   on dept.id equals empr.departmentid
                                   select new
                                   {
                                       dept.longname,
                                       empr.firstname,
                                       empr.lastname
                                   };
            // we used only one loop to print the result
            // because it flattens the result ( makes it one collection not collection of collections)
            foreach (var item in selectManyResult)
            {
                Console.WriteLine($"{item.firstname} {item.lastname} works in {item.longname}");
            }

        }
    }
}
