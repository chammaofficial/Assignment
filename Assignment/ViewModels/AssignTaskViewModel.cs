using Assignment.Models;
using Task = Assignment.Models.Task;


namespace Assignment.ViewModels
{
    public class AssignTaskViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<EmployeeTask> EmployeeTasks { get; set; }
        public TaskAssignment TaskAssignment { get; set; }

    }
}
