using AdminSystem.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        public HttpClient client = new HttpClient();

        public UnitTest1()
        {
            client.BaseAddress = new Uri("https://localhost:44304/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestMethod]
        public async Task TestRegisterStudents1()
        {
            RegisterStudents register = new RegisterStudents();
            register.teacher = "teacherken@gmail.com";
            register.students = new string[] { "studentjon@gmail.com", "studenthon@gmail.com", "commonstudent1@gmail.com", "commonstudent2@gmail.com" };
            var response = await client.PostAsync("register", new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task TestRegisterStudents2()
        {
            RegisterStudents register = new RegisterStudents();
            register.teacher = "teacherjoe@gmail.com";
            register.students = new string[] { "studentmary@gmail.com", "commonstudent1@gmail.com", "commonstudent2@gmail.com"  };
            var response = await client.PostAsync("register", new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task TestCommonStudents1()
        {
            var response = await client.GetAsync("commonstudents?teacher=teacherken%40gmail.com");
            var result = await response.Content.ReadAsStringAsync();

            CommonStudents commonStudents = new CommonStudents
            {
                students = new string[]
                {
                    "studentjon@gmail.com",
                    "studenthon@gmail.com",
                    "commonstudent1@gmail.com", 
                    "commonstudent2@gmail.com"
                }
            };
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            JToken.DeepEquals(JsonConvert.SerializeObject(commonStudents), result);
        }

        [TestMethod]
        public async Task TestCommonStudents2()
        {
            var response = await client.GetAsync("commonstudents?teacher=teacherken%40gmail.com&teacher=teacherjoe%40gmail.com");
            var result = await response.Content.ReadAsStringAsync();

            CommonStudents commonStudents = new CommonStudents
            {
                students = new string[]
                {
                    "commonstudent1@gmail.com",
                    "commonstudent2@gmail.com"
                }
            };
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            JToken.DeepEquals(JsonConvert.SerializeObject(commonStudents), result);
        }

        [TestMethod]
        public async Task TestSuspendStudent()
        {
            SuspendStudent suspendStudent = new SuspendStudent();
            suspendStudent.student = "studentjon@gmail.com";
            var response = await client.PostAsync("suspend", new StringContent(JsonConvert.SerializeObject(suspendStudent), Encoding.UTF8, "application/json"));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task TestRetrieveNotification()
        {
            Retrieve retrieve = new Retrieve();
            retrieve.teacher = "teacherjoe@gmail.com";
            retrieve.notification = "Hello students! @studentmary@gmail.com @commonstudent2@gmail.com";
            var response = await client.PostAsync("retrievenotifications", new StringContent(JsonConvert.SerializeObject(retrieve), Encoding.UTF8, "application/json"));
            ReceiveNotifications receive = new ReceiveNotifications
            {
                recipients = new string[]
                {
                    "studenthon@gmail.com",
                    "commonstudent1@gmail.com",
                    "commonstudent2@gmail.com",
                    "studentmary@gmail.com"
                }
            };

            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            JToken.DeepEquals(JsonConvert.SerializeObject(receive), result);
        }
    }
}
