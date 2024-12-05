using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Model;
using NextGen.Dal.Context;
using NextGen.Dal.Interfaces;

namespace NextGen.Back.Services
{
    public class QuestionSrv : IQuestionSrv
    {
        private readonly NextGenDbContext _context;

        public QuestionSrv(NextGenDbContext context)
        {
            _context = context;
        }

        public void AddQuestion(Question question)
        {
            // Code pour ajouter un Actualite
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public Question GetQuestion(int id)
        {
            // Code pour récupérer un Actualite
            var quest = _context.Questions.Find(id);

            return quest;
        }

        public List<Question> GetAllQuestions()
        {
            // Code pour récupérer tous les Actualites
            var questions = _context.Questions.ToList();

            return questions;
        }

        public void DeleteQuestion(int id)
        {
            // Code pour supprimer un Actualite
            var quest = _context.Questions.Find(id);
            _context.Questions.Remove(quest);
            _context.SaveChanges();
        }
    }
}
