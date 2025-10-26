    using System;
    using System.Collections.Generic;
    using TCPdata;

    namespace TCPextensions
    {
        public static class method
        {
            public static List<T> Filter<T>(this List<T> records, Func<T, bool> func)
            {
                List<T> filteredlist = new List<T>();
                foreach (var record in records)
                {
                    if (func(record))
                        filteredlist.Add(record);
                }
                return filteredlist;
            }
            public static IEnumerable<employee> gethighsalaries(this IEnumerable<employee> employees)
            {
                foreach (var emp in employees)
                {
                    Console.WriteLine($"accessing {emp.firstname} with id = {emp.id}");
                    if (emp.annualsalary > 300000)
                        yield return emp;
                }
            }
        
            }
        }
