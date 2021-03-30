using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace country.back {

    public class Country2 : Country {

        public Country2(IConfiguration cfg) : base(cfg) { }

        private CountryModel2 Load(dynamic d) {
            if (d == null) {
                return null;
            }

            CountryModel2 country = JsonConvert.DeserializeObject<CountryModel2>(Convert.ToString(d.INFO));
            country.Id = d.ID;
            return country;
        }

        public new async Task<CountryModel2> Get(long id) => await Task.Run(() => {
            string sql = "SELECT c.ID, c.INFO FROM Country c WHERE id = :id";
            dynamic d = BLL.Cnn(cfg).QueryFirst(sql, new { id });
            return Load(d);
        });

        public new async Task<IEnumerable<CountryModel2>> GetAll() => await Task.Run(() => {
            List<CountryModel2> list = new List<CountryModel2>();
            string sql = "SELECT c.ID, c.INFO FROM Country c";
            IEnumerable<dynamic> coutries = BLL.Cnn(cfg).Query(sql);
            foreach (dynamic d in coutries) {
                CountryModel2 country = JsonConvert.DeserializeObject<CountryModel2>(Convert.ToString(d.INFO));
                country.Id = d.ID;
                list.Add(country);
            }
            return list;
        });

        public async Task<bool> Save(CountryModel2 model) => await Task.Run(() => {
            if (!IsValid(model.Id)) {
                return false;
            }

            string info = JsonConvert.SerializeObject(model);
            string sql = "SELECT COUNT(*) FROM Country WHERE Id = :id";
            bool exists = BLL.Cnn(cfg).ExecScalar<long>(sql, new { id = model.Id }) == 1;
            if (exists) {
                BLL.Cnn(cfg).UpdateSQL(nameof(Country), new { model.Id, info }, "Id");
            } else {
                BLL.Cnn(cfg).InsertSQL(nameof(Country), new { model.Id, info });
            }

            return true;
        });
    }
}