using System;

namespace VirtualEngineer.Models
{
    public class UserAnswer
    {
        public int question_id;

        public int[] selected_answer_ids;

        public DateTime created_at;
    }
}