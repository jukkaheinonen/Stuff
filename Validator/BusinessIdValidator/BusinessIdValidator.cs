using BIV.Interfaces;
using BIV.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIV
{
    /// <summary>
    /// Class for validating given BusinessIds
    /// </summary>
    public class BusinessIdValidator : ISpecification<string>
    {
        /// <summary>
        /// Members
        /// </summary>
        private List<string> _errors;   // List to gather our errors in

        #region Interface implementation
        /// <summary>
        /// Call this to get reasons why the BusinessId failed validation
        /// </summary>
        public IEnumerable<string> ReasonsForDissatisfaction
        {
            get { return _errors; }
        }

        /// <summary>
        /// Checks given BusinessId and returns true if valid
        /// A BusinessId is considered valid if our List of errors is empty.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsSatisfiedBy(string id)
        {
            _errors = new List<string>();

            Validate(id);

            return (_errors.Count == 0);
        }
        #endregion

        /// <summary>
        /// An overridable method for validations, in case you want to do your own
        /// </summary>
        /// <param name="id"></param>
        public virtual void Validate(string id)
        {
            // Various validations we want to perform on our given id before even trying to calculate checksum
            if (!validateSeparator(id))
                _errors.Add(Resources.InvalidSeparator);

            if (!validateLength(id))
                _errors.Add(Resources.InvalidLength);

            if (!validateDigits(id))
                _errors.Add(Resources.InvalidDigits);

            // Early exit:
            if (_errors.Count() > 0)
                return;

            // If format validations pass, finally validate checksum
            validateChecksum(id);
        }

        #region private methods
        /// <summary>
        /// Returns boolean whether given id contains a '-' as separator
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool validateSeparator(string id)
        {
            if (id.Length >= 7) // Ensure our string is of proper length before trying to check the separator index
                return (id[7] == '-');
            return false;
        }

        /// <summary>
        /// Returns boolean whether given id is of proper length as devised in standard
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool validateLength(string id)
        {
            return (id.Length == 9);
        }

        /// <summary>
        /// Returns boolean whether id contains proper digits in the correct places
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool validateDigits(string id)
        {
            int digitcounter = 0;   // Keeps track of which character we are checking in passed string

            foreach (char c in id)
            {
                if (!Char.IsNumber(c) && digitcounter != 7) // Index 7 contains the separator character, so it is allowed to be nonnumeric!
                    return false;

                digitcounter++;
            }
            return true;
        }

        /// <summary>
        /// Returns boolean whether the id checksum is correct. If BusinessId is not in use, also adds this to _errors.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool validateChecksum(string id)
        {
            int[] coefficients = new int[] { 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;
            int checksum;
            int reminder;

            // Calculate sum of digits when coefficients applied
            for (int i = 0; i < 7; i++)
                sum += (int)Char.GetNumericValue(id[i]) * coefficients[i];

            // See the division reminder
            reminder = sum % 11;

            // Reminder used in checksum calculation
            if (reminder == 0)
                checksum = 0;
            else if (reminder == 1)
            {
                _errors.Add(Resources.IdNotInUse);
                return false;
            }
            else if (reminder > 1 && reminder <= 10)
                checksum = 11 - reminder;
            else // If we pass all other ifs somehow, assume the id *has* to invalid
            {
                _errors.Add(Resources.UnknownError);
                return false;   
            }

            if ((int)Char.GetNumericValue(id[8]) == checksum)
                return true;
            else
                _errors.Add(Resources.ChecksumFailed);

            return false;
        }
        #endregion
    }
}
