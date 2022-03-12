public abstract class Command
{
    public abstract bool Validate();
    public abstract void Execute();
    public abstract void Invoke();
}
