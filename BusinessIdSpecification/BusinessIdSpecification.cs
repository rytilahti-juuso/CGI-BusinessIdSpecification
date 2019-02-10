using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;
namespace BusinessIdSpecification
{
    class BusinessIdSpecification : ISpecification<string>
    {
        private string businessId;
        private IEnumerable<string> reasonsForDissatisfaction;


        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get => reasonsForDissatisfaction;

            set => reasonsForDissatisfaction = value;
        }



        public bool IsSatisfiedBy(string businessId)
        {
            if (businessId.Length == 9)
            {
                if (IsCorrect(businessId))
                {
                    ReasonsForDissatisfaction = Enumerable.Empty<string>();
                    return true;
                }
            }

            //List of dissatisfactions in businessId
            List<string> reasonsForDissatisfactionList = new List<string>();
            if (businessId.Length > 9)
            {
                reasonsForDissatisfactionList.Add("BusinessId is too long");
            }
            else if (businessId.Length < 9)
            {
                reasonsForDissatisfactionList.Add("BusinessId is too short");
            }
            //if businessId does not contain hyphon
            if (!businessId.Contains('-'))
            {
                reasonsForDissatisfactionList.Add("BusinessId is missing hyphon!");
                //Checks that there are only numbers
                if (!(int.TryParse(businessId, out int c)))
                {
                    reasonsForDissatisfactionList.Add("There is supposed to be only numbers separated by hyphon!");
                }
            }
            // Starts checking left and right side of the hyphon. Also avoids neg. numbers
            else if (businessId.IndexOf('-') != 0)
            {
                string firstPart = businessId.Substring(0, businessId.IndexOf('-'));

                //Check if left side of hyphon can be converted to int and is left side of hyphon correct length
                if (!(int.TryParse(firstPart, out int a)) || firstPart.Length != 7)
                {
                    reasonsForDissatisfactionList.Add("There should be seven numbers on the left side of the hyphon and no other characters");
                }
                //Checks right side of the hyphon, if there are characters then executes
                if (businessId.IndexOf('-') != businessId.Length - 1)
                {
                    string secondPart = businessId.Substring((businessId.IndexOf('-') + 1));
                    Console.WriteLine("Right side of the hyphon is: " + secondPart);
                    //Checks if there are only ints on the right side of hyphon and that there is only one chareacter on the right side of the hyphon
                    if (!(int.TryParse(secondPart, out int b)) || secondPart.Length != 1)
                    {
                        reasonsForDissatisfactionList.Add("There should be only one number on the rigth side of the hyphon and no other characters");
                    }
                }
            }

            ReasonsForDissatisfaction = reasonsForDissatisfactionList.AsEnumerable();
            return false;
        }
        public bool IsCorrect(string businessId)
        {
            string regexString =
           @"(^[0-9]{7})+[-]+[0-9]{1}$";
            RegexStringValidator myRegexValidator =
             new RegexStringValidator(regexString);

            // Determine if the object to validate can be validated.
            Console.WriteLine("CanValidate: {0}",
              myRegexValidator.CanValidate(businessId.GetType()));

            try
            {
                // Attempt validation.
                myRegexValidator.Validate(businessId);
                Console.WriteLine("Validated.");
                return true;
            }
            catch (ArgumentException e)
            {
                // Validation failed.
                Console.WriteLine(" BusinessId is not in correct form. It should be ex.1234567-8 , instead it is {0}", businessId);
                return false;
            }


        }

    }
}
