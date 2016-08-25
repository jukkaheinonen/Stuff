using BIV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    /// <summary>
    /// Quick tester for BusinessIdValidator-project
    /// It creates an instance of the BusinessIdValidator class, feeds it the user input and checks for validity.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string bid;
            Console.WriteLine("Please enter BusinessId to validate:");

            BusinessIdValidator bIV = new BusinessIdValidator();
            bid = Console.ReadLine();

            // Check validity
            if (bIV.IsSatisfiedBy(bid))
            {
                Console.WriteLine(string.Format("BusinessId '{0}' is valid.", bid));
            }
            else
            {
                Console.WriteLine(string.Format("BusinessId '{0}' is NOT valid.", bid));

                foreach (var reason in bIV.ReasonsForDissatisfaction)
                    Console.WriteLine(reason);
            }

            // To keep console from closing "too soon"
            Console.ReadLine();
        }
    }
}
