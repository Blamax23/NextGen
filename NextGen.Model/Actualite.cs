using System.ComponentModel.DataAnnotations;

namespace NextGen.Model
{
    public class Actualite
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Contenu { get; set; }
        public int IdUtilisateur { get; set; }

        public DateTime? DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(Contenu))
                this.Contenu = " ";

            if (string.IsNullOrWhiteSpace(Titre))
                errors.Add("Le titre de l'Actualite est obligatoire.");

            if (Titre.Length > 150)
                errors.Add("Le titre contient trop de caractères. (150 maximum)");

            if (!this.DateCreation.HasValue)
                this.DateCreation = DateTime.Now;

            this.DateModification = DateTime.Now;

            return errors;
        }

        public string ReturnDateModification()
        {
            string date = "";
            if (this.DateModification.HasValue)
            {
                date = this.DateModification.Value.Day + " ";
                switch(this.DateModification.Value.Month)
                {
                    case 1:
                        date += "janvier";
                        break;
                    case 2:
                        date += "février";
                        break;
                    case 3:
                        date += "mars";
                        break;
                    case 4:
                        date += "avril";
                        break;
                    case 5:
                        date += "mai";
                        break;
                    case 6:
                        date += "juin";
                        break;
                    case 7:
                        date += "juillet";
                        break;
                    case 8:
                        date += "août";
                        break;
                    case 9:
                        date += "septembre";
                        break;
                    case 10:
                        date += "octobre";
                        break;
                    case 11:
                        date += "novembre";
                        break;
                    case 12:
                        date += "décembre";
                        break;
                }
                date += " " + this.DateModification.Value.Year;

                return date;
            }
            else
                return DateTime.Now.ToLongDateString();

        }
    }
}
