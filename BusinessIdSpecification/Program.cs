using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;

namespace BusinessIdSpecification
{
    class Program
    {
        static void Main(string[] args)
        {
            BusinessIdSpecification business = new BusinessIdSpecification();
            business.IsSatisfiedBy("1234567-1");
            foreach (var item in business.ReasonsForDissatisfaction)
            {
                Console.WriteLine(item);
            }
            // Display and wait
            Console.ReadLine();
        }
    }
}
