using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientCare.Shared.Managers;
using PatientCare.Shared.DTO;

namespace PatientCare.Shared.Test
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void CreateTask_TaskContainsMandatoryProperties_TaskIsFormattedToString()
        {
            var taskMgr = new TaskManager();
            string testString = "Test";
            //Arrange
            var taskdto = new TaskDTO
            {
                CreatedTime = 1234,
                LastChanged = 1337,
                NoOfWorkersRequired = 1,
                SourceSystem = "TaskManagement",
                TaskRequester = new TaskRequester
                {
                    Name = "Anders",
                    OrganizationUserId = "Ham Selv!",
                    Phone = "12345678"
                },
                TaskStatus = "UNAS",
                Type = "PT",
                UniqueId = new Guid().ToString(),
                Urgency = "DFLT"
            };
            //Act
            var createdTask = taskMgr.CreateTask(taskdto);

            //Assert
            Assert.IsTrue(ReferenceEquals(createdTask.GetType(), testString.GetType()));
        }
    }
}