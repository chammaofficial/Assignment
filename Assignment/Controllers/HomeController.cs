using System.Data;
using System.Data.SqlClient;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Assignment.ViewModels;
using Dapper;
using Task = Assignment.Models.Task;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDbConnection _connection;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connection = new SqlConnection(configuration.GetConnectionString("Development"));
            _connection.Open();
        }

        //Landing Page
        public IActionResult Index()
        {
            return View();
        }


        //List View of Task List
        [HttpGet]
        [Route("/tasks")]
        public IActionResult TaskList()
        {
            //Getting Model Data to Pass View
            if(_connection.State != ConnectionState.Open)
                _connection.Open();

            var Tasklist = _connection.Query<Task>("SELECT * FROM Task").ToList();
            return View("TaskList",Tasklist);
        }


        //List View of Employee List
        [HttpGet]
        [Route("/employees")]
        public IActionResult EmployeeList()
        {
            //Getting Model Data to Pass View
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            var EmpoyeeList = _connection.Query<Employee>("SELECT * FROM Employee").ToList();
            return View("EmployeeList", EmpoyeeList);
        }


        //Assign New Task to Employees
        [HttpGet]
        [Route("/assign-tasks")]
        public IActionResult AssignTasks()
        {
            //Getting Model Data to Pass View
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            var viewModel = new AssignTaskViewModel();
            viewModel.EmployeeTasks = _connection.Query<EmployeeTask>("SELECT * From Employee_Tasks").ToList();
            viewModel.Employees = _connection.Query<Employee>("SELECT * FROM Employee").ToList();
            viewModel.Tasks = _connection.Query<Task>("SELECT * FROM Task").ToList();
            viewModel.TaskAssignment = new TaskAssignment();
            return View("AssignTasks",viewModel);
        }


        //Edit Assigned Tasks
        [HttpGet]
        [Route("/assign-tasks/edit/{Id}")]
        public IActionResult EditTasks(int Id)
        {
            //Getting Model Data to Pass View
            if (_connection.State != ConnectionState.Open)
                _connection.Open();


            var viewModel = new AssignTaskViewModel();
            viewModel.EmployeeTasks = _connection.Query<EmployeeTask>("SELECT * From Employee_Tasks").ToList();
            viewModel.Employees = _connection.Query<Employee>("SELECT * FROM Employee").ToList();
            viewModel.Tasks = _connection.Query<Task>("SELECT * FROM Task").ToList();
            viewModel.TaskAssignment = _connection.Query<TaskAssignment>("SELECT * FROM Employee_Tasks WHERE Id = @id",new{id = Id}).First();
            return View("AssignTasks", viewModel);
        }


        //Delete Assigned Tasks
        [HttpGet]
        [Route("/assign-tasks/delete/{Id}")]
        public IActionResult DeleteAssignedTasks(int Id)
        {
            //Getting Model Data to Pass View
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            _connection.Query("DELETE FROM Employee_Tasks WHERE Id = @Id", new { Id = Id });
            return RedirectToAction("AssignTasks");
        }


        //Save Assigned Tasks
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/assign-tasks")]
        public IActionResult SaveAssignment(ViewModels.AssignTaskViewModel model)
        {
            //Getting Model Data to Pass View
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            //Validate Model
            if (model.TaskAssignment.Employee_Id > 0 && model.TaskAssignment.Task_Id > 0)
            {
                //Check Insert or Update
                if (model.TaskAssignment.Id == 0)
                {
                    _connection.Query("INSERT INTO Employee_Tasks VALUES(@empId , @taskId)",new {empId = model.TaskAssignment.Employee_Id , taskId = model.TaskAssignment.Task_Id});
                    
                }
                else
                {
                    _connection.Query("UPDATE Employee_Tasks SET Employee_Id = @empId , Task_Id = @taskId WHERE Id = @id", new { empId = model.TaskAssignment.Employee_Id, taskId = model.TaskAssignment.Task_Id ,id = model.TaskAssignment.Id});
                }
            }
            return RedirectToAction("AssignTasks");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}