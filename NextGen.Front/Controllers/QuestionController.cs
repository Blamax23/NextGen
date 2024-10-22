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


        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
