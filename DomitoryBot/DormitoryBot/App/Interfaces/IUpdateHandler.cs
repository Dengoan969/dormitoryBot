namespace DomitoryBot.App.Interfaces;

public interface IUpdateHandler<TUpdate>
{
    Task HandleUpdate(TUpdate update);
}