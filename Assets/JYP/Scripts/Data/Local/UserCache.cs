using Unity.VisualScripting;

public class UserCache
{
    private static UserCache instance = null;

    private UserCache()
    {
    }

    public static UserCache GetInstance()
    {
        if (instance == null)
        {
            instance = new UserCache();
        }

        return instance;
    }

    private string id;

    public string Id
    {
        get => id ?? "test";
        set => id = value;
    }

    public int Point { get; set; }
}