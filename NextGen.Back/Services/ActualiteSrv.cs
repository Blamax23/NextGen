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
    public class ActualiteSrv : IActualiteSrv
    {
        private readonly NextGenDbContext _context;

        public ActualiteSrv(NextGenDbContext context)
        {
            _context = context;
        }

        public void AddActualite(Actualite actualite)
        {
            List<string> errors = actualite.Validate();

            if (errors.Count > 0)
            {
                throw new Exception(string.Join("\n", errors));
            }
            // Code pour ajouter un Actualite
            _context.Actualites.Add(actualite);
            _context.SaveChanges();
        }

        public Actualite GetActualite(int id)
        {
            // Code pour récupérer un Actualite
            var art = _context.Actualites.Find(id);

            return art;
        }

        public List<Actualite> GetAllActualites()
        {
            // Code pour récupérer tous les Actualites
            var actualites = _context.Actualites.OrderByDescending(a => a.DateModification).ToList();

            return actualites;
        }

        public void UpdateActualite(Actualite actualite)
        {
            List<string> errors = actualite.Validate();

            if (errors.Count > 0)
            {
                throw new Exception(string.Join("\n", errors));
            }
            // Code pour mettre à jour un Actualite
            _context.Actualites.Update(actualite);
            _context.SaveChanges();
        }

        public void DeleteActualite(int id)
        {
            // Code pour supprimer un Actualite
            var actualite = _context.Actualites.Find(id);
            _context.Actualites.Remove(actualite);
            _context.SaveChanges();
        }
    }
}
