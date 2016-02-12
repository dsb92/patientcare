using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientCare.Shared.Managers;
using PatientCare.Shared.Util;

namespace PatientCare.Shared.Test
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public void CheckCPRValid_CheckingCPRIsValid_CPRIsValid()
        {
            // Arrange
            const string cpr = "0105921853";

            CprValidator.CprError cprError;

            CprValidator.CheckCPR(cpr, out cprError);

            // Act
            bool isValid = cprError == CprValidator.CprError.NoError;

            // Assert
            Assert.AreEqual(isValid, true);
        }

        [TestMethod]
        public void IsPatientValid_CheckPatientIsValid_PatientIsValid()
        {
            // Arrange
            var cpr = "0105921853";

            var loginManager = new LoginManager();

            try
            {
                // Act
                var loginCpr = loginManager.GetPatient(cpr);

                // Assert
                Assert.AreEqual(cpr, loginCpr);
            }
            catch (Exception e)
            {
                Assert.Fail("Status Code not OK " + e.Message);
            }

        }
    }
}
