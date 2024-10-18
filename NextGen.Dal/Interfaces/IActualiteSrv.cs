using NextGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Dal.Interfaces
{
    public interface IActualiteSrv
    {
        void AddActualite(Actualite Actualite);

        Actualite GetActualite(int id);

        List<Actualite> GetAllActualites();

        void UpdateActualite(Actualite Actualite);

        void DeleteActualite(int id);
    }
}
