using System.ComponentModel.DataAnnotations;

namespace Assignment.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public int Employee_Id { get; set; }
        public int Task_Id { get; set; }

        public TaskAssignment()
        {
            Id = 0;
        }
    }
}
