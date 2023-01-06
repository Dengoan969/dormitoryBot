namespace DormitoryBot.App.Commands.Interfaces
{
    public interface IExecutableCommand : IChatCommand
    {
        public string Name { get; }
        Task Execute(long chatId);
    }
}