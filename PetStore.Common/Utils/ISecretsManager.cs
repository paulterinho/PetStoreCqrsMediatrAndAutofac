namespace PetStore.Common.Utils
{
    public interface ISecretsManager
    {
        string GetDbConnectionString();
    }
}