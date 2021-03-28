using Microsoft.Extensions.Configuration;

namespace country.back {
    public class BLL {
        public static ConnectionManager Cnn(IConfiguration cfg) => new ConnectionManager(
            cfg.GetSection("ConnectionString").Value
        );
    }
}
