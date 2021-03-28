using System.ComponentModel.DataAnnotations;

namespace country.back {
    public class CountryModel {

        public int Id { get; set; }
        public long Area { get; set; }
        public long Population { get; set; }
        public decimal Density { get; set; }
        public string Capital { get; set; }
    }
}