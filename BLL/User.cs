using System.Threading.Tasks;

namespace country.back {
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public async Task<User> Authenticate(string username, string password) {
            User user = await Task.Run(() => {
                User user = null;
                //Em uma aplicação real, normlamente a validação de usuário é feita em banco de dados.
                if (username == "adm" && password == "123") {
                    user = new User() { Id = 1, FirstName = "Administrador", LastName = "do Sistema", Username = "adm"};
                }
                return user;
            });
            return user;
        }
    }
}