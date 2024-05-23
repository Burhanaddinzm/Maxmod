namespace Maxmod.Services.Interfaces;

public interface ILayoutService
{
    Task<string?> GetUserFullNameAsync();
    bool CheckLoggedIn();
}
