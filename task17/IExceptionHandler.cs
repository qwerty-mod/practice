namespace task17
{
    public interface IExceptionHandler
    {
        void Handle(Exception exception, ICommand command);
    }
}
