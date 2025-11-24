
using BancaEnLinea.Models;

namespace BancaEnLinea.Services;

public class AuthService(InMemoryStore store)
{
    private readonly InMemoryStore _store = store;

    public User? Login(string username, string password)
    {
        return _store.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
