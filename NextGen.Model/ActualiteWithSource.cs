using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Model
{
    public class ActualiteWithSource { 
        public int Id { get; set; }
        public Actualite Actualite { get; set; }

        public List<Source> Source { get; set; }

        public User User { get; set; }


        public void InsertLinks()
        {
            if(Source.Count() <= 0)
            {
                return;
            }
            for (var i = 1; i <= Source.Count(); i++)
            {
                if (Source[i - 1].Type == "link")
                {
                    Actualite.Contenu = Actualite.Contenu.Replace("{{" + i + "}}", "<a href='" + Source[i - 1].Path + "'>" + Source[i - 1].Name + "</a>");
                }
            }
        }


    }
}
