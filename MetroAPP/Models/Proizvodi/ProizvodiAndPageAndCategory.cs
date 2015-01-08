using System.Collections.Generic;

namespace MetroAPP.Models.Proizvodi
{
    public class ProizvodiAndPageAndCategory
    {
        public List<Proizvod> ListProizvodi;
        public List<int> Pages;
        public HashSet<string> Category;
        public int Num;
    }
}