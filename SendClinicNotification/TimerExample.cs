using System;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SendClinicNotification
{
    public static class TimerExample
    {
        [Function("TimerExample")]
        public static void Run([TimerTrigger("0 0 * * * *")] MyInfo myTimer, FunctionContext context)
        {
            HttpClient httpClient = new HttpClient();
            var logger = context.GetLogger("TimerExample");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var s = httpClient.GetAsync("http://localhost:52175/api/notify").Result;

            myTimer.ScheduleStatus = new MyScheduleStatus();
            //var s = myTimer.ScheduleStatus.Next;
            //myTimer = new MyInfo();
            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next} with data "+s);
            //var SlotsOfDoctorContent = await SlotsOfDoctor.Content.ReadAsStringAsync();
            //var SlotsOfDoctorDesrialized = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(SlotsOfDoctorContent);
        }
    }

    public class MyInfo
    {

        
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
