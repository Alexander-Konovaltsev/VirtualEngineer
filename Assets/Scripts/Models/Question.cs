namespace VirtualEngineer.Models
{
    public class Question
    {
        public int id;
        public string question_text;
        public int quiz_id;
        public int? model_id;
        public int question_type_id;

        public QuestionType question_type;
        public Answer[] answers;
    }
}