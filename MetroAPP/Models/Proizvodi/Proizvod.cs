namespace MetroAPP.Models.Proizvodi
{
    public class Proizvod
    {
        public int ProizvodId;
        public string ProizvodImgSrc;
        public string ProizvodNaziv;
        public decimal ProizvodCijena;
        public int ProizvodPage;
        public string ProizvodCategory;
        public string ProizvodOpis;

        public Proizvod(int proizvodId, string proizvodImgSrc, string proizvodNaziv, decimal proizvodCijena, int proizvodPage, string proizvodCategory, string proizvodOpis)
        {
            ProizvodId = proizvodId;
            ProizvodImgSrc = proizvodImgSrc;
            ProizvodNaziv = proizvodNaziv;
            ProizvodCijena = proizvodCijena;
            ProizvodPage = proizvodPage;
            ProizvodCategory = proizvodCategory;
            ProizvodOpis = proizvodOpis;
        }
    }
}