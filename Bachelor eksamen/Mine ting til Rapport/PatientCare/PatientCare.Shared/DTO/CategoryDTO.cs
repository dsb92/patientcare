using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Model;

namespace PatientCare.Shared.DTO
{

    public class CategoryDTO
    {
        public ChoiceEntity[] Choices { get; set; }
        public CategoryEntity Category { get; set; }

        public CategoryEntity[] Categories
        {
            get
            {
                var callManager = new CallManager();
                return callManager.GetCategories();
            }
            set
            {
                
            }
        }
    }
}
