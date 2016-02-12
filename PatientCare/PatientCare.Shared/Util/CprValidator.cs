using System;
using System.Text.RegularExpressions;

namespace PatientCare.Shared.Util
{
    /// <summary>
    /// Denne klasse håndtere CPR-nr og tjekker om det er gyldigt. 
    /// Sourcekilde: 4. semester I4KPU Løsningsforslag fra Poul Ejner. 
    /// </summary>
    public class CprValidator
    {
        public enum CprError { NoError, FormatError, DateError, Check11Error };
        /// <summary>
        /// This method validates a danish cpr number.
        /// Checks performed:
        ///     Must contain 10 digits.
        ///     First 6 digits must be a valid date.
        ///     The 11-rule (Checksum test)
        /// </summary>
        /// <param name="cprTxt">a string containing a cprnumber.</param>
        /// <param name="error">Indicates type of error if any.</param>
        /// <returns>Returns true if no format errors found</returns>
        public static bool CheckCPR(string cprTxt, out CprError error)
        {
            CprError cprError = FormatTest(cprTxt);
            if (cprError == CprError.NoError)
                cprError = DateTest(cprTxt);
            if (cprError == CprError.NoError)
                cprError = Check11Test(cprTxt);

            error = cprError;
            return (cprError == CprError.NoError);
        }

        private static CprError FormatTest(string cprTxt)
        {
            Regex regExp;
            Match match;
            string cprFormat = "[0-9]{10}";
            CprError cprError = CprError.NoError;

            // Test length
            if (cprTxt.Length != 10)
                cprError = CprError.FormatError;
            else
            {
                // Make sure we have 10 digits
                regExp = new Regex(cprFormat);
                match = regExp.Match(cprTxt);
                if (!match.Success)
                    cprError = CprError.FormatError;
            }
            return cprError;
        }

        private static CprError DateTest(string cprTxt)
        {
            string dayStr = cprTxt.Substring(0, 2);
            string monthStr = cprTxt.Substring(2, 2);
            string yearStr = cprTxt.Substring(4, 2);
            CprError cprError = CprError.NoError;

            int day;
            int month;
            int year;

            try
            {
                day = int.Parse(dayStr);
                month = int.Parse(monthStr);
                year = int.Parse(yearStr);
                DateTime dt = new DateTime(year, month, day);
            }
            catch (Exception)
            {
                cprError = CprError.DateError;
            }
            return cprError;
        }

        /// <summary>
        /// The CPR check sum algorithm is calculated by mulitiplying each digit with a factor 
        /// and then add all results and divide the sum by 11.
        /// Factors: 4327654321
        /// CPR:     0609240121
        /// Sum:     0 + 18 + 0 + 72 + 12 + 20 + 0 + 1 + 4 + 1 = 121 / 11 = 11.0 -> CPR is OK
        /// </summary>
        ///
        private static CprError Check11Test(string cprTxt)
        {
            
            CprError cprError = CprError.NoError;
            try
            {
                int sum = 0;
                for (int i = 0; i < 3; i++)
                    sum += int.Parse(cprTxt.Substring(i, 1)) * (4 - i);
                for (int i = 3; i < 10; i++)
                    sum += int.Parse(cprTxt.Substring(i, 1)) * (10 - i);
                if (sum % 11 != 0)
                    cprError = CprError.Check11Error;
            }
            catch (Exception)
            {
                cprError = CprError.FormatError;
            }

            return cprError;
        }
    }
}
