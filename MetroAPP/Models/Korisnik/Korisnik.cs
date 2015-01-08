using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MetroAPP.Models.Korisnik
{
    public class Korisnik
    {
        public int KorisnikId;
        public string KorisnikIme;
        public string KorisnikPrezime;
        public DateTime KorisnikRegistracija;
        public string KorisnikSlika;

        [Required]
        [DisplayName("Username: ")]
        public string KorisnikUsername { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(20,MinimumLength = 6)]
        [DisplayName("Password: ")]
        public string KorisnikPassword { get; set; }

        public string KorisnikPasswordSalt { get; set; }

        public Korisnik(int korisnikId, string korisnikIme, string korisnikPrezime, DateTime korisnikRegistracija, string korisnikSlika)
        {
            KorisnikId = korisnikId;
            KorisnikIme = korisnikIme;
            KorisnikPrezime = korisnikPrezime;
            KorisnikRegistracija = korisnikRegistracija;
            KorisnikSlika = korisnikSlika;
        }
    }
}