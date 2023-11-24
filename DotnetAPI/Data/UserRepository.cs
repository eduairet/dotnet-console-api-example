namespace DotnetAPI.Data;

public class UserRepository(IConfiguration config) : IUserRepository
{
    private readonly DataContextEF _data = new(config);

    public bool SaveChanges() => _data.SaveChanges() > 0;

    public void AddEntity<T>(T entity)
    {
        if (entity != null) _data.Add(entity);
    }

    public void RemoveEntity<T>(T entity)
    {
        if (entity != null) _data.Remove(entity);
    }
}