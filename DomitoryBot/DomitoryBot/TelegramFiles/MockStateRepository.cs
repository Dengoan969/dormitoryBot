namespace Telegram;

public class MockStateRepository : IUsersStateRepository
{
    private readonly Dictionary<long, DialogState> db = new();


    public DialogState GetState(long id)
    {
        if (!db.ContainsKey(id)) throw new ArgumentException("User doesn't exists");

        return db[id];
    }

    public bool ContainsKey(long id)
    {
        return db.ContainsKey(id);
    }

    public void SetState(long id, DialogState state)
    {
        db[id] = state;
    }
}