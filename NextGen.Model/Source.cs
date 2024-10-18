using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Model
{
    public class Source
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int IdActualite { get; set; }

        //public TypeSource Type
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(TypeString))
        //        {
        //            return (TypeSource)Enum.Parse(typeof(TypeSource), TypeString);
        //        }
        //        else
        //            return TypeSource.Autre;
        //    }
        //    set
        //    {

        //    }
        //}
    }

    public enum TypeSource
    {
        Video,
        Photo,
        Audio,
        Lien,
        Pdf,
        Autre
    }
}
