namespace Parser;

public interface IParser
{
    public Task<List<PageData>?> Handling(string url);
}