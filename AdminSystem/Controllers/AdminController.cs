using AdminSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace AdminSystem.Controllers
{
    public class AdminController : ApiController
    {

        // POST api/register
        // 1. As a teacher, I want to register one or more students to a specified teacher.
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterStudents regStudent)
        {
            try
            {
                if (regStudent == null || regStudent.students == null || regStudent.teacher == null)
                {
                    var errorMessage = new
                    {
                        message = "No input.",
                    };
                    var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
                    errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                    return errorResponse;
                }

                using (var db = new AdminSystemEntities())
                {
                    int teacherID, studentID;
                    var existTeacher = (from t in db.Teachers
                                         where t.EmailAddress == regStudent.teacher
                                         select t).FirstOrDefault();
                    if (existTeacher == null)
                    {
                        Teacher newTeacher = new Teacher();
                        newTeacher.EmailAddress = regStudent.teacher;
                        db.Teachers.Add(newTeacher);
                        db.SaveChanges();
                        teacherID = newTeacher.TeacherID;
                    }
                    else
                    {
                        teacherID = existTeacher.TeacherID;
                    }

                    foreach (var student in regStudent.students)
                    {
                        var existStudent = (from s in db.Students
                                            where s.EmailAddress == student
                                            select s).FirstOrDefault();
                        if (existStudent == null)
                        {
                            Student newStudent = new Student();
                            newStudent.EmailAddress = student;
                            db.Students.Add(newStudent);
                            db.SaveChanges();
                            studentID = newStudent.StudentID;
                        }
                        else
                        {
                            studentID = existStudent.StudentID;
                        }

                        db.TeacherStudents.Add(new TeacherStudent { TeacherID = teacherID, StudentID = studentID });
                        db.SaveChanges();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (SqlException)
            {
                var errorMessage = new
                {
                    message = "Database Error",
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    message = ex.Message.ToString(),
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
        }

        // GET api/commonstudents
        // 2. As a teacher, I want to retrieve a list of students common to a given list of teachers 
        // (i.e. retrieve students who are registered to ALL of the given teachers).
        [HttpGet]
        public HttpResponseMessage CommonStudents([FromUri] string[] teacher)
        {
            try
            {
                if (teacher == null)
                {
                    var errorMessage = new
                    {
                        message = "No input.",
                    };
                    var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
                    errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                    return errorResponse;
                }

                List<string> result = new List<string>();
                using (var db = new AdminSystemEntities())
                {
                    for (int i = 0; i < teacher.Length; i++) 
                    {

                        string emailAddress = teacher[i];
                        var students = (from ts in db.TeacherStudents
                                        join t in db.Teachers on ts.TeacherID equals t.TeacherID
                                        join s in db.Students on ts.StudentID equals s.StudentID
                                        where emailAddress == t.EmailAddress
                                        select s.EmailAddress).ToList();
                        if (i == 0)
                        {
                            result = students;
                        }
                        else
                        {
                            result = result.Intersect(students).ToList();
                        }
                    }
                }
                CommonStudents common = new CommonStudents()
                {
                    students = result.ToArray()
                };
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(common), Encoding.UTF8, "application/json");
                return response;
            }
            catch (SqlException)
            {
                var errorMessage = new
                {
                    message = "Database Error",
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    message = ex.Message.ToString(),
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
        }

        // POST api/suspend
        // 3. As a teacher, I want to suspend a specified student.
        [HttpPost]
        public HttpResponseMessage Suspend([FromBody] SuspendStudent suspendStudent)
        {
            try
            {
                if (suspendStudent == null)
                {
                    var errorMessage = new
                    {
                        message = "No input.",
                    };
                    var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
                    errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                    return errorResponse;
                }

                using (var db = new AdminSystemEntities())
                {
                    var existStudent = (from s in db.Students
                                        where s.EmailAddress == suspendStudent.student
                                        && s.IsSuspended == false
                                        select s).FirstOrDefault();
                    if (existStudent != null)
                    {
                        existStudent.IsSuspended = true;
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.NoContent);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Specified student has already been suspended.");
                    }
                }
            }
            catch (SqlException)
            {
                var errorMessage = new
                {
                    message = "Database Error",
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    message = ex.Message.ToString(),
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
        }

        // POST api/retrievenotifications
        // 4. As a teacher, I want to retrieve a list of students who can receive a given notification.
        [HttpPost]
        public HttpResponseMessage RetrieveNotifications([FromBody] Retrieve retrieve)
        {
            try
            {
                if (retrieve == null || retrieve.teacher == null || retrieve.notification == null)
                {
                    var errorMessage = new
                    {
                        message = "No input.",
                    };
                    var errorResponse = Request.CreateResponse(HttpStatusCode.BadRequest);
                    errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                    return errorResponse;
                }

                using (var db = new AdminSystemEntities())
                {
                    var registeredStudents = (from ts in db.TeacherStudents
                                              join t in db.Teachers on ts.TeacherID equals t.TeacherID
                                              join s in db.Students on ts.StudentID equals s.StudentID
                                              where t.EmailAddress == retrieve.teacher
                                              && s.IsSuspended == false
                                              select s.EmailAddress).ToList();

                    // Any substring starts with @ and some characters in between another @ and end with .com
                    string pattern = @"@([\w]+)@([\w]+)(\.(\w){3})";
                    Regex rg = new Regex(pattern);
                    List<string> extractEmailAddress = new List<string>();
                    MatchCollection matchCollection = rg.Matches(retrieve.notification);
                    foreach (var match in matchCollection)
                    {
                        extractEmailAddress.Add(match.ToString().Substring(1));
                    }

                    var mentionedStudents = (from s in db.Students
                                             where extractEmailAddress.Contains(s.EmailAddress)
                                             && s.IsSuspended == false
                                             select s.EmailAddress).ToList();
                    ReceiveNotifications result = new ReceiveNotifications
                    {
                        recipients = mentionedStudents.Union(registeredStudents).ToArray()
                    };
                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
                    return response;
                }
            }
            catch (SqlException)
            {
                var errorMessage = new
                {
                    message = "Database Error",
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorMessage = new
                {
                    message = ex.Message.ToString(),
                };
                var errorResponse = Request.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent(JsonConvert.SerializeObject(errorMessage), Encoding.UTF8, "application/json");
                return errorResponse;
            }
        }
    }
}