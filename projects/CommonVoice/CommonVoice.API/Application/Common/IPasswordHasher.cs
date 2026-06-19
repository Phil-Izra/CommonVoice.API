namespace CommonVoice.API.Application.Common;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
