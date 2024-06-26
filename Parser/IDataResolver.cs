namespace Parser;

public interface IDataResolver
{
    public Task Resolve(IParser parser);
}