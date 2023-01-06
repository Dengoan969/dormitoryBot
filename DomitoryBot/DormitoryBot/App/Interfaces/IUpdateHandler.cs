namespace DormitoryBot.App.Interfaces;

public interface IUpdateHandler<TUpdate>
{
    Task HandleUpdate(TUpdate update);
}