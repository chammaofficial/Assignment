﻿namespace Assignment.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Task> Tasks { get; set; }

    }
}
