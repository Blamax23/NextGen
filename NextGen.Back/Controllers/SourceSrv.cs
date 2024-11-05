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
    public class SourceSrv : ISourceSrv
    {
        private readonly NextGenDbContext _context;

        public SourceSrv(NextGenDbContext context)
        {
            _context = context;
        }
        public void AddSource(Source source)
        {
            _context.Sources.Add(source);
            _context.SaveChanges();
        }

        public Source GetSource(int id)
        {
            var source = _context.Sources.Find(id);

            return source;
        }

        public List<Source> GetAllSources()
        {
            var sources = _context.Sources.ToList();

            return sources;
        }

        public void UpdateSource(Source source)
        {
            _context.Sources.Update(source);
            _context.SaveChanges();
        }

        public void DeleteSource(int id)
        {
            var source = _context.Sources.Find(id);
            _context.Sources.Remove(source);
            _context.SaveChanges();
        }

        public List<Source> GetSourcesByActualite(int idActualite)
        {
            var sources = _context.Sources.Where(s => s.IdActualite == idActualite)?.ToList();

            return sources;
        }
    }
}
