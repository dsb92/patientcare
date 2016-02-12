using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PatientCare.Shared.Model;

namespace PatientCare.Shared.Managers
{
    /// <summary>
    /// Klasse for håndtering af valgmuligheder tilknyttet en afdeling på et sygehus.
    /// </summary>
    public class CategoryManager
    {
        /// <summary>
        /// Henter alle Categorier tilknyttet Typer og tilbehør på en afdeling
        /// </summary>
        /// <returns></returns>
        public List<CategoryEntity> GetCategoriesWithAll()
        {

            var httpHandler = new HttpHandler();

            // Det som modtages fra Web
            var categoryJson = httpHandler.GetData(HttpHandler.API.Category);
            var choiceJson = httpHandler.GetData(HttpHandler.API.Choice);

            var categories = JsonConvert.DeserializeObject<CategoryEntity[]>(categoryJson);
            var choices = JsonConvert.DeserializeObject<ChoiceEntity[]>(choiceJson);

            var categoryList = new List<CategoryEntity>();

            foreach (var category in categories)
            {
                var choiceList = new List<ChoiceEntity>();
                var detailList = new List<DetailEntity>();

                foreach (var choice in choices.Where(choice => choice.CategoryId == category.CategoryId))
                {
                    if (choice.Details != null)
                    {
                        foreach (var detail in choice.Details)
                        {
                            // Tjek hvis en detail har den samme choice.
                            var detailWithSameChoiceList = (from detailWithSameChoice in choice.Details where detailWithSameChoice.DetailId == detail.DetailId select choice).ToList();

                            // Hvis en detail har mere end en choice tilknyttet.
                            detail.Choices = detailWithSameChoiceList.Count > 1 ? detailWithSameChoiceList : new List<ChoiceEntity> { choice };

                            //detail.Choices = new List<ChoiceEntity>(choices);
                            detailList.Add(detail);
                        }
                    }
                    

                    if (detailList.Count > 0)
                    {
                        choice.Details = detailList;
                    }

                    choiceList.Add(choice);

                    detailList = new List<DetailEntity>();
                }

                // Hvis pågældende kategori har nogle choices tilføj til dens liste.
                if (choiceList.Count > 0)
                {
                    category.Choices = choiceList;
                }

                categoryList.Add(category);
            }


            return categoryList;
        }

        public List<CategoryEntity> GetCategoriesTESTDATA()
        {
            // TEST DATA 

           var categoryEntities = new List<CategoryEntity>();

            // CATEGORY -> CHOICES -> DETAILS
           var d1 = new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Mælk",
           };

           var d2 = new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Sukker"
           };

           var d3 = new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Sort kaffe"
           };

           var d4 = new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Sukkerfri"
           };

           var d5 = new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Tun og æg"
           };

           var d6= new DetailEntity()
           {
               DetailId = Guid.NewGuid().ToString(),
               Name = "Skinke og ost"
           };

           var ch1 = new ChoiceEntity()
           {
               ChoiceId = Guid.NewGuid().ToString(),
               Name = "Kaffe",
               Details = new List<DetailEntity> { d1, d2, d3 } // 1-M
           };

           var ch2 = new ChoiceEntity()
           {
               ChoiceId = Guid.NewGuid().ToString(),
               Name = "Cola",
               Details = new List<DetailEntity> { d2, d4 } // 1-M
           };

           d1.Choices = new List<ChoiceEntity> {ch1};
           d2.Choices = new List<ChoiceEntity> { ch1, ch2 }; // M-M

           d3.Choices = new List<ChoiceEntity> { ch1 };

           d4.Choices = new List<ChoiceEntity> { ch2 };

           var ch3 = new ChoiceEntity()
           {
               ChoiceId = Guid.NewGuid().ToString(),
               Name = "Sandwich",
               Details = new List<DetailEntity> { d5, d6 }
           };

           d5.Choices = new List<ChoiceEntity> { ch3 };
           d6.Choices = new List<ChoiceEntity> { ch3 };

           var cat1 = new CategoryEntity()
           {
               CategoryId = Guid.NewGuid().ToString(),
               Name = "Mad/Drikke",
               //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
               Choices = new List<ChoiceEntity> {ch1, ch2, ch3}
           };

           ch1.CategoryId = cat1.CategoryId;
           ch2.CategoryId = cat1.CategoryId;

           categoryEntities.Add(cat1);

           // CATEGORY -> CHOICES
           var ch4 = new ChoiceEntity()
           {
               ChoiceId = Guid.NewGuid().ToString(),
               Name = "Morfin"
           };

           var ch5 = new ChoiceEntity()
           {
               ChoiceId = Guid.NewGuid().ToString(),
               Name = "Panodil"
           };

           var cat2 = new CategoryEntity()
           {
               CategoryId = Guid.NewGuid().ToString(),
               Name = "Smertestillende",
               //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
               Choices = new List<ChoiceEntity> { ch4, ch5 }
           };

           ch2.CategoryId = cat2.CategoryId;

           categoryEntities.Add(cat2);

           // CATEGORY -> OBS EMPTY CHOICE
           var cat3 = new CategoryEntity()
           {
               CategoryId = Guid.NewGuid().ToString(),
               Name = "Toilet",
               //Picture = "http://multimedia.pol.dk/archive/00537/ITALY_CLONED_CHAMPI_537998a.jpg",
           };

           ch3.CategoryId = cat3.CategoryId;

           categoryEntities.Add(cat3);

           return categoryEntities;
          
        } 
    }
}
