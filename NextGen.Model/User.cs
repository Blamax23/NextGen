using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Model
{
    public class User
    {
        public int Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string Password { get; set; }
        public virtual string OldPassword { get; set; }
        public virtual bool AccepteNews { get; set; }

        public void Validate()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(this.OldPassword))
                this.OldPassword = this.Password;

            if (string.IsNullOrEmpty(this.Email))
                errors.Add("Email is required");

            if (string.IsNullOrEmpty(this.Password))
                errors.Add("Password is required");

            if (errors.Count > 0)
                throw new Exception(string.Join(", ", errors));
        }

        public string StringToRgb()
        {
            // Générer un hash à partir du string (par ex., le nom de l'utilisateur)
            int hash = this.FirstName.GetHashCode();

            // Utilisation du hash pour générer une couleur RGB
            Random random = new Random(hash);

            // Limiter les valeurs pour obtenir des couleurs plus douces (luminosité élevée, saturation basse)
            int r = (random.Next(128, 256) + 255) / 2; // Rouge entre 128 et 255, plus clair
            int g = (random.Next(128, 256) + 255) / 2; // Vert entre 128 et 255, plus clair
            int b = (random.Next(128, 256) + 255) / 2; // Bleu entre 128 et 255, plus clair

            // Retourner la couleur en format hexadécimal
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
