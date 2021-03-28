using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace country.back {

    public class Country : BaseClass {

        public Country() { }

        public Country(IConfiguration cfg) : base(cfg) { }

        private CountryModel Load(dynamic d) {
            if (d == null) return null;
            CountryModel country = JsonConvert.DeserializeObject<CountryModel>(Convert.ToString(d.INFO));
            country.Id = d.ID;
            return country;
        }

        public async Task<CountryModel> Get(long id) => await Task.Run(() => {
            string sql = "SELECT c.ID, c.INFO FROM Country c WHERE id = :id";
            dynamic d = BLL.Cnn(cfg).QueryFirst(sql, new { id });
            return Load(d);
        });

        public async Task<IEnumerable<CountryModel>> GetAll() => await Task.Run(() => {
            List<CountryModel> list = new List<CountryModel>();
            string sql = "SELECT c.ID, c.INFO FROM Country c";
            IEnumerable<dynamic> coutries = BLL.Cnn(cfg).Query(sql);
            foreach (dynamic d in coutries) {
                CountryModel country = JsonConvert.DeserializeObject<CountryModel>(Convert.ToString(d.INFO));
                country.Id = d.ID;
                list.Add(country);
            }
            return list;
        });

        public async Task<bool> Save(CountryModel model) => await Task.Run(() => {
            string info = JsonConvert.SerializeObject(model);
            string sql = "SELECT COUNT(*) FROM Country WHERE Id = :id";
            bool exists = BLL.Cnn(cfg).ExecScalar<long>(sql, new { id = model.Id }) == 1;
            if (exists)
                BLL.Cnn(cfg).UpdateSQL(nameof(Country), new { model.Id, info }, "Id");
            else
                BLL.Cnn(cfg).InsertSQL(nameof(Country), new { model.Id, info });
            return true;
        });
    }
}