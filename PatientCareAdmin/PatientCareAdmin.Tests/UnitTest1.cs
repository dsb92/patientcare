using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PatientCareAdmin.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SplitDetailsString()
        {
            var test = "sukker,Mælk,";

            char delimiter = ',';

            string[] test2 = test.Split(delimiter);
            List<string> List = new List<string>();
            for (int i = 0; i < test2.Length; i++)
            {
                if (test2[i].Length > 0)
                {
                    //Find detailid by name og smid det på objektet inden det sendes afsted.
                    
                    List.Add(test2[i]);
                }
            }

            Assert.AreEqual(2,List.Count);
        }
    }
}
