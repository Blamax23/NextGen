using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Dal.Interfaces;
using NextGen.Model;

namespace NextGen.Front.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionSrv _questionSrv;

        public QuestionController(IQuestionSrv questionSrv)
        {
            _questionSrv = questionSrv;
        }
        // GET: QuestionController
        public ActionResult Index()
        {
            List<Question> questions = _questionSrv.GetAllQuestions();
            return View(questions);
        }

        [HttpPost]
        public IActionResult AddQuestion(string question, string reponse)
        {
            Question newQuestion = new Question
            {
                Intitule = question,
                Reponse = reponse
            };

            _questionSrv.AddQuestion(newQuestion);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            _questionSrv.DeleteQuestion(id);

            return RedirectToAction("Index");
        }
    }
}
