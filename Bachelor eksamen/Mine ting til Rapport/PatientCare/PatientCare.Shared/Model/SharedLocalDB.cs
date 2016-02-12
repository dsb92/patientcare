using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientCare.Shared.Model.Utils;
using SQLite.Net;

namespace PatientCare.Shared.Model
{
    public class SharedLocalDB
    {
        protected SQLiteConnection db;

        public void CreateTables()
        {
            // Create the tables
            db.CreateTable<CategoryEntity>();
            db.CreateTable<ChoiceEntity>();
            db.CreateTable<DetailEntity>();
            db.CreateTable<ChoiceDetailEntity>();
            db.CreateTable<CallEntity>();
        }

        public void SaveCategories(List<CategoryEntity> categories)
        {
            // Insert the object in the database
            //db.InsertWithChildren(services, true);

            try
            {
                db.InsertOrReplaceAllWithChildren(categories, recursive: true);

            }
            catch (Exception e)
            {
                db.UpdateAll(categories);
            }
        }

        public void SaveMyCalls(List<CallEntity> callEntities)
        {
            try
            {
                db.InsertOrReplaceAll(callEntities);

            }
            catch (Exception e)
            {
                //db.UpdateAll(callEntities);
            }
        }

        public void UpdateMyCall(CallEntity callEntity)
        {
            try
            {
                db.Update(callEntity);

            }
            catch (Exception e)
            {
                
            }
        }

        public List<CategoryEntity> LoadCategories()
        {
            try
            {
                var categories = db.GetAllWithChildren<CategoryEntity>(recursive: true);
                return categories;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<CallEntity> LoadMyCalls()
        {
            try
            {
                var myCalls = db.GetAllWithChildren<CallEntity>(recursive: true);
                return myCalls;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void DeleteCategories()
        {
            try
            {
                db.DeleteAll<CategoryEntity>();
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
