namespace InteractiveScheduleUad.Api.Services.AuthAndAuthorizationServices.Contracts;

public interface IHashService
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}