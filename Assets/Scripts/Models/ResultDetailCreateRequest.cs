using System;

namespace VirtualEngineer.Models
{
    public class ResultDetailCreateRequest
    {
        public DateTime created_at;
        public int result_id;
        public int question_id;
        public int answer_id;
    }
}