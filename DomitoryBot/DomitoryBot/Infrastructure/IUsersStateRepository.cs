using DomitoryBot.App;

namespace DomitoryBot.Infrastructure;

public interface IUsersStateRepository
{
    public DialogState GetState(long id);
    public bool ContainsKey(long id);
    public void SetState(long id, DialogState state);
}