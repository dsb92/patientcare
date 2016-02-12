using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PatientCare.Shared.DTO;

namespace PatientCare.Shared.Managers
{
    /// <summary>
    /// Klasse til opgavehåndtering for en portør
    /// </summary>
    public class TaskManager
    {
       private string task_id;

        private readonly string taskManagement = "TaskManagement";
        private readonly HttpHandler _httpHandler;

        public TaskManager()
        {
            _httpHandler = new HttpHandler();
        }
        /// <summary>
        /// Henter en liste af alle tasks 
        /// </summary>
        /// <param name="request"></param>
        public string GetTasks()
        {
            //dynamic data = JsonConvert.DeserializeObject(_DAL.GetTasks());

            var data = _httpHandler.GetData(HttpHandler.API.Call);

            return data;
        }

        /// <summary>
        /// Opretter en Task
        /// </summary>
        /// <returns></returns>
        public string CreateTask(TaskDTO data)
        {
           string jsonData = JsonConvert.SerializeObject(data);

           return jsonData;
        }

        /// <summary>
        /// Omdanner et kald til Json og sender et Kald
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        public void PostTask(string content)
        {
           StringContent httpContent = new StringContent(content,Encoding.UTF8,"application/json");

           _httpHandler.PostData(httpContent, HttpHandler.API.Task);
        }

        /// <summary>
        /// Omdanner en TaskDTO til Json og sender en Task
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        public void PutTask(string content)
        {
            StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            _httpHandler.PutData(httpContent, HttpHandler.API.Task);
        }
    }
}
