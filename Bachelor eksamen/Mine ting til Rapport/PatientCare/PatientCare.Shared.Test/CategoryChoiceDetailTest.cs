using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientCare.Shared.DAL;
using PatientCare.Shared.Model;

namespace PatientCare.Shared.Test
{
    [TestClass]
    public class CategoryChoiceDetailTest
    {
        //[Ignore]
        [TestMethod]
        public void GetCategory_TryingToGetJsonCategoryFromWebservice_JsonObjectReturnedFromWebService()
        {
            // Arrange
            var httpHandler = new HttpHandler();
            string testString = "test";

            try
            {
                
                // Det som modtages fra Web
                // Act
                var categoryJson = httpHandler.GetData(HttpHandler.API.Category);

                var categories = JsonConvert.DeserializeObject<CategoryEntity[]>(categoryJson);

                // Assert
                Assert.IsTrue(ReferenceEquals(categoryJson.GetType(), testString.GetType()));
                Assert.IsTrue(categories.ToList().Count > 0);
                
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetCategoryChoice_TryingToGetJsonCategoryChoiceFromWebservice_JsonObjectReturnedFromWebService()
        {
            // Arrange
            var httpHandler = new HttpHandler();
            var testString = "test";

            try
            {
                
                // Det som modtages fra Web
                // Act
                var categoryJson = httpHandler.GetData(HttpHandler.API.Category);
                var choiceJson = httpHandler.GetData(HttpHandler.API.Choice);
                

                var categories = JsonConvert.DeserializeObject<CategoryEntity[]>(categoryJson);
                var choices = JsonConvert.DeserializeObject<ChoiceEntity[]>(choiceJson);

                
                var categoryList = new List<CategoryEntity>();
                foreach (var category in categories)
                {
                    var choiceList = choices.Where(choice => choice.CategoryId == category.CategoryId).ToList();

                    // Hvis pågældende kategori har nogle choices tilføj til dens liste.
                    if (choiceList.Count > 0)
                    {
                        category.Choices = choiceList;
                    }

                    categoryList.Add(category);
                }


                // Assert
                Assert.IsTrue(ReferenceEquals(categoryJson.GetType(), testString.GetType()));
                Assert.IsTrue(ReferenceEquals(choiceJson.GetType(), testString.GetType()));
                Assert.IsTrue(categoryList.Count > 0);
          
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void GetCategoryChoiceDetail_TryingToGetJsonCategoryChoiceDetailFromWebservice_JsonObjectReturnedFromWebService()
        {
            try
            {
                // Arrange
                var categoryManager = new CategoryManager();
                
                // Act
                var categoryList = categoryManager.GetCategoriesWithAll();

                // Assert
                Assert.IsTrue(categoryList.Count > 0);
      
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        // Test not avaible
        /*
        [TestMethod]
        public void GetCategoryChoiceDetail_TryingToGetFromLocalDatabase_GotCategoriesFromLocalDatabase()
        {
            // Arrange
            var categoryManager = new CategoryManager();
            var categoryList = categoryManager.GetCategoriesWithAll();

            // Act
            var sharedDb = new SharedLocalDB();
            sharedDb.CreateTables();
            DataHandler.SaveCategoriesToLocalDatabase(sharedDb, categoryList.ToArray());

            var categoryFromLocalDb = DataHandler.LoadCategoriesFromLocalDatabase(sharedDb);

            // Assert
            Assert.AreEqual(categoryList, categoryFromLocalDb);

        }
         */

    }
}
