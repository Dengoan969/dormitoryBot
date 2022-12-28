namespace DormitoryBot.App;

public interface IUpdateHandler<TUpdate>
{
    Task HandleUpdate(TUpdate update);
}