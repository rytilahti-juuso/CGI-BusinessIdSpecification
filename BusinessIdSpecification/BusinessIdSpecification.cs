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
        private IEnumerable<string> reasonsForDissatisfaction;

        //getters and setters
        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get => reasonsForDissatisfaction;

            set => reasonsForDissatisfaction = value;
        }



        public bool IsSatisfiedBy(string businessId)
        {
            //List of dissatisfactions in businessId
            List<string> reasonsForDissatisfactionList = new List<string>();
            //checks if businessId is in correct form and if the verification number is right
            if (BusinessIdIsInCorrectForm(businessId))
            {
                if (CalculateandCheckVerificationNumber(businessId))
                {
                    ReasonsForDissatisfaction = Enumerable.Empty<string>();
                    return true;
                }
                else
                {
                    reasonsForDissatisfactionList.Add("Verification number is wrong. Please check that all inputted numbers are right");
                    return false;
                }
            } 
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
        private bool BusinessIdIsInCorrectForm(string businessId)
        {
            if (businessId.Length == 9)
            {
                string regexString = @"(^[0-9]{7})+[-]+[0-9]{1}$";
                //calls method, which checks if verificationnumber on the businessId is correct
                if (Regex.IsMatch(businessId, regexString))
                {
                    return true;
                }
            }
            return false;
        }
        //only called when businessId has been verified to be in form: 1234567-8
        //uses function found here: http://tarkistusmerkit.teppovuori.fi/tarkmerk.htm#y-tunnus2
        private bool CalculateandCheckVerificationNumber(string businessId)
        {
            string firstPart = businessId.Substring(0, businessId.IndexOf('-'));
            
            int firstNumber = (int)char.GetNumericValue(businessId[0])*7;
            int secondNumber = (int)char.GetNumericValue(businessId[1])*9;
            int thirdNumber = (int)char.GetNumericValue(businessId[2])*10;
            int fourthNumber = (int)char.GetNumericValue(businessId[3])*5;
            int fifthNumber = (int)char.GetNumericValue(businessId[4])*8;
            int sixthNumber = (int)char.GetNumericValue(businessId[5])*4;
            int seventhNumber = (int)char.GetNumericValue(businessId[6])*2;
            int total = firstNumber + secondNumber + thirdNumber + fourthNumber + fifthNumber + sixthNumber + seventhNumber;
            Console.WriteLine("Total is: " +total);

            //reduces the modulo from 11 and gets the calculated verification number 
            int calculatedVerificationNumber = 11 - (total % 11);
            Console.WriteLine("Calculated verification number is: " + calculatedVerificationNumber);
            
            int verificationNumberInBusinessId = (int)char.GetNumericValue(businessId[8]);
            Console.WriteLine("VerificationNumber in businessId is: " + verificationNumberInBusinessId);
            
            if (calculatedVerificationNumber == verificationNumberInBusinessId)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
