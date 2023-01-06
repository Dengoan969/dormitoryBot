using DormitoryBot.App.Interfaces;

namespace DormitoryBot.App;

public class MockStateRepository : IUsersStateRepository
{
    private readonly Dictionary<long, DialogState> db;

    public MockStateRepository(Dictionary<long, DialogState> db)
    {
        this.db = db;
    }

    public MockStateRepository() : this(new Dictionary<long, DialogState>()) { }

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