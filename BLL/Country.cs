using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
            if (!IsValid(model.Id)) return false;
            string info = JsonConvert.SerializeObject(model);
            string sql = "SELECT COUNT(*) FROM Country WHERE Id = :id";
            bool exists = BLL.Cnn(cfg).ExecScalar<long>(sql, new { id = model.Id }) == 1;
            if (exists)
                BLL.Cnn(cfg).UpdateSQL(nameof(Country), new { model.Id, info }, "Id");
            else
                BLL.Cnn(cfg).InsertSQL(nameof(Country), new { model.Id, info });
            return true;
        });

        private bool IsValid(int id) {
            try {
                using HttpClient httpClient = new HttpClient {
                    BaseAddress = new Uri(cfg.GetSection("ApiGraphCountries").Value)
                };
                var query = new {
                    query = @"query {
                    Country (_id:  """ + id + @""") 
                            { _id } 
                    }"
                };
                using HttpRequestMessage request = new HttpRequestMessage {
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
                };
                using Task<HttpResponseMessage> response = httpClient.SendAsync(request);
                response.Wait();
                using HttpResponseMessage result = response.Result;
                result.EnsureSuccessStatusCode();
                Task<string> content = result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<dynamic>(content.Result).data.Country.Count == 1;
            } catch {
                return false;
            }
        }
    }
}