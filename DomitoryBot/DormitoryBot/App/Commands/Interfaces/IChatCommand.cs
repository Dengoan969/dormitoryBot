using DormitoryBot.App;

namespace DormitoryBot.App.Commands.Interfaces
{
    public interface IChatCommand
    {
        DialogState SourceState { get; }
        DialogState DestinationState { get; }
    }
}