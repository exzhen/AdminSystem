using AdminSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdminClient
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.ReadLine();
            client.BaseAddress = new Uri("https://localhost:44304/api/");
            // Uncomment the function to test the specific endpoints

            await TestRegisterStudents();
            //await TestCommonStudents();
            //await TestSuspendStudent();
            //await TestRetrieveNotification();
            Console.ReadLine();
        }

        public static async Task TestRegisterStudents()
        {
            RegisterStudents register = new RegisterStudents();
            register.teacher = "teacherjoe@gmail.com";
            register.students = new string[] { "commonstudent1@gmail.com", "commonstudent2@gmail.com" };
            var response = await client.PostAsync("register", new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.IsSuccessStatusCode);
            Console.WriteLine(result);

        }

        public static async Task TestCommonStudents()
        {
            var response = await client.GetAsync("commonstudents?teacher=teacherken%40gmail.com&teacher=teacherjoe%40gmail.com&teacher=asda@gmail.com");
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }

        public static async Task TestSuspendStudent()
        {
            SuspendStudent suspendStudent = new SuspendStudent();
            suspendStudent.student = "studentjon@gmail.com";
            var response = await client.PostAsync("suspend", new StringContent(JsonConvert.SerializeObject(suspendStudent), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.IsSuccessStatusCode);
            Console.WriteLine(result);
        }

        public static async Task TestRetrieveNotification()
        {
            Retrieve retrieve = new Retrieve();
            retrieve.teacher = "teacherjoe@gmail.com";
            retrieve.notification = "Hello students! @studenthon@gmail.com @commonstudent1@gmail.com";
            var response = await client.PostAsync("retrievenotifications", new StringContent(JsonConvert.SerializeObject(retrieve), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(response.IsSuccessStatusCode);
            Console.WriteLine(result);
        }
    }
}
