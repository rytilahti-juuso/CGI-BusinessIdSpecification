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
        //CLASS VARIABLES
        private IEnumerable<string> reasonsForDissatisfaction;
        private List<string> reasonsForDissatisfactionList;

        //CONSTRUCTOR
        public BusinessIdSpecification(){
            
            FormatReasonForDissatisfactionEnumerableAndListCorrectly();
        }
        //GETTERS AND SETTERS
        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get => reasonsForDissatisfaction;

            private set => reasonsForDissatisfaction = value;
        }

        private List<string> ReasonsForDissatisfactionList
        {
            get => reasonsForDissatisfactionList;

            set => reasonsForDissatisfactionList = value;
        }
        //METHODS

        //PUBLIC METHODS

        public static void ThrowIfNullOrEmpty(string businessId)
        {
            if (businessId == null)
            {
                throw new ArgumentNullException(businessId);
            }
            if (businessId == string.Empty)
            {
                throw new ArgumentException("Argument must not be the empty string.",
                                            businessId);
            }
        }
        //Requires: parameter is not null or empty
        // true: sets reasonsForDissatisFaction as empty
        //false: sets reasons to dissatisfactions in to ReasonsForDissatisfactionList.Add
        public bool IsSatisfiedBy(string businessId)
        {
            BusinessIdSpecification.ThrowIfNullOrEmpty(businessId);
            //checks if businessId is in correct form 
            if (IsBusinessIdFullyCorrect(businessId))
            {
                ReasonsForDissatisfactionList.Clear();
                return true;
            }
            HasStringCorrectLength(businessId, "businessId", 9);
            RightAndLeftSideOfHyphon(businessId);
            return false;
        }
        
        //PRIVATE METHODS

        //Used: In constructor
        //Sets: Creates a new list object to ReasonsForDissatisfactionList and sets it empty.
        //  Lastly sets ReasonForDissatisfaction enumarable to reference the created list object 
        private void FormatReasonForDissatisfactionEnumerableAndListCorrectly()
        {
            ReasonsForDissatisfactionList = new List<string>();
            ReasonsForDissatisfaction = Enumerable.Empty<string>();
            ReasonsForDissatisfaction = ReasonsForDissatisfactionList.AsEnumerable();
        }
        
        //Checks if business id is fully correct
        //Requires: string as arg
        //true: ReasonsForDissatisfaction is set to be empty
        //false:if only verification number is incorrects, add its through ReasonsForDissatisfactionList.Add
        private bool IsBusinessIdFullyCorrect (string businessId)
        {
            //checks if businessId is in correct form and Checks if the verification number is right
            if (BusinessIdIsInCorrectForm(businessId) && CalculateandCheckVerificationNumber(businessId))
            {
                ReasonsForDissatisfactionList.Clear();
                return true;
                
            }
            return false;
        }
        //Requires: 
        //  testString, String, which length is tested,
        //  stringName: tested string name
        //  length: wanted length
        //Sets: Adds reason of test failing through ReasonsForDissatisfactionList.Add
        private void HasStringCorrectLength(string testString, string stringName, int length)
        {
            if (testString.Length >length)
            {
                ReasonsForDissatisfactionList.Add(stringName + " is too long") ;
            }
            else if (testString.Length < length)
            {
                ReasonsForDissatisfactionList.Add(stringName + " is too short");
            }
        }

        //Requires:String as parameter, NOTE: The provided string does not require to have hyphon('-')
        //If String has hyphon , calls methods LeftSideOfHyphonIsInCorrectForm(businessId), RightSideOfHyphonIsInCorrectForm(businessId);
        //sets: if string does not have hyphon, adds to ReasonsForDissatisfactionList note about missing hyphon
        private void RightAndLeftSideOfHyphon(string businessId)
        {
            if (businessId.Contains('-'))
            {
                LeftSideOfHyphonIsInCorrectForm(businessId);
                RightSideOfHyphonIsInCorrectForm(businessId);
            }
            else
            {
                ReasonsForDissatisfactionList.Add("BusinessId is missing hyphon!");
            }
                
        }

        //Requires: String contains a hyphon('-')
        //If left side of hyphon is in correct form, do nothing
        //If left side is not in correct form, adds reasons through ReasonsForDissatisfactionList.Add
        private void LeftSideOfHyphonIsInCorrectForm(string businessId)
        {
            string firstPart = businessId.Substring(0, businessId.IndexOf('-'));
            string regexString = @"^[0-9]+$";
            if (!(Regex.IsMatch(firstPart, regexString)))
            {
                ReasonsForDissatisfactionList.Add("Left side should contain only numbers");
            }
            if(firstPart.Length != 7)
            {
                ReasonsForDissatisfactionList.Add("Left side should contain seven characters");
            }
           
        }

        //Requires: String contains a hyphon('-')
        //If right side of hyphon is in correct form, do nothing
        //If right side is not in correct form, adds reasons through ReasonsForDissatisfactionList.Add
        private void RightSideOfHyphonIsInCorrectForm(string businessId)
        {
            string regexString = @"^[0-9]{1}$";
            string secondPart = businessId.Substring((businessId.IndexOf('-') + 1));
            //Console.WriteLine("secondpart is: " + secondPart);
            //Checks right side of the hyphon, if there are characters then executes
            if (!(Regex.IsMatch(secondPart, regexString)))
            {
                ReasonsForDissatisfactionList.Add("Right side of hyphon should contain only one number and no other characters");
            }

        }
        //Requires: parameter is string
        //true: Business id is in correct form, NOTE: Does not check that verification number is right
        //false: returns false
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

        //uses function found here: http://tarkistusmerkit.teppovuori.fi/tarkmerk.htm#y-tunnus2
        //Requires: businessId is in correct form (ex. 1234567-8)
        //true: verification number is right
        //false: verification number is incorrect, and sets it to dissatisfaction through ReasonsForDissatisfactionList.Add
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
            //Console.WriteLine("Total is: " +total);

            //reduces the modulo from 11 and gets the calculated verification number 
            int calculatedVerificationNumber = 11 - (total % 11);
            //Console.WriteLine("Calculated verification number is: " + calculatedVerificationNumber);
            
            int verificationNumberInBusinessId = (int)char.GetNumericValue(businessId[8]);
            //Console.WriteLine("VerificationNumber in businessId is: " + verificationNumberInBusinessId);
            
            if (calculatedVerificationNumber == verificationNumberInBusinessId)
            {
                return true;
            }
            else
            {
                ReasonsForDissatisfactionList.Add("Verification number is wrong. Please check that all inputted numbers are right");
                return false;
            }

        }

    }
}
