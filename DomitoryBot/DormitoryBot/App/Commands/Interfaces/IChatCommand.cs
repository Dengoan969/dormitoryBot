using DormitoryBot.App;

namespace DomitoryBot.App.Commands.Interfaces
{
    public interface IChatCommand
    {
        DialogState SourceState { get; }
        DialogState DestinationState { get; }
    }
}