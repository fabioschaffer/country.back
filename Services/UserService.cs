using System.Threading.Tasks;

namespace country.back {

    public class UserService {

        public async Task<User> Authenticate(string username, string password) {
            User user = await Task.Run(() => {
                User user = null;
                //Em uma aplica��o real, normlamente a valida��o de usu�rio � feita em banco de dados.
                if (username == "adm" && password == "123") {
                    user = new User() { Id = 1, FirstName = "Administrador", LastName = "do Sistema", Username = "adm", Password = "123" };
                }
                return user;
            });
            if (user == null) {
                return null;
            }

            return user.WithoutPassword();
        }
    }
}