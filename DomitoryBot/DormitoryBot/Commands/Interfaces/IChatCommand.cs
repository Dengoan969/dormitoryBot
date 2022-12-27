using DormitoryBot.App;

namespace DormitoryBot.Commands.Interfaces
{
    public interface IChatCommand
    {
        DialogState SourceState { get; }
        DialogState DestinationState { get; }
    }
}