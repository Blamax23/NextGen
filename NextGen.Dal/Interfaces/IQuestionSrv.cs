using NextGen.Model;

namespace NextGen.Dal.Interfaces
{
    public interface IQuestionSrv
    {
        void AddQuestion(Question question);

        Question GetQuestion(int id);

        List<Question> GetAllQuestions();
    }
}
