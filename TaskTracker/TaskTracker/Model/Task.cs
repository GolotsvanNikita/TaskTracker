using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackerProgram.Model
{
    internal class Task
    {
        public string Description { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            string status = IsCompleted ? "[completed]" : "[not completed]";
            return $"{status} {Description} (Priority: {Priority})";
        }
    }
}
