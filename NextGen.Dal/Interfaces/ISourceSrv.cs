using NextGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Dal.Interfaces
{
    public interface ISourceSrv
    {
        void AddSource(Source source);

        Source GetSource(int id);

        List<Source> GetAllSources();

        void UpdateSource(Source source);

        void DeleteSource(int id);

        List<Source> GetSourcesByActualite(int idActualite);
    }
}
