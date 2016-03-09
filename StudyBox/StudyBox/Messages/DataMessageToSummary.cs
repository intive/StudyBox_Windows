using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyBox.Model;

namespace StudyBox.Messages
{
    public class DataMessageToSummary
    {
        public Exam ExamInstance { get; set; }

        public DataMessageToSummary(Exam exam)
        {
            this.ExamInstance = exam;
        }
    }
}