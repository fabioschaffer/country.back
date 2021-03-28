using Microsoft.Extensions.Configuration;

namespace country.back {
    public class BaseClass {

        protected readonly IConfiguration cfg;

        public BaseClass() { }

        public BaseClass(IConfiguration cfg) => this.cfg = cfg;
    }
}
