namespace Kymeta.Cloud.Services.Toolbox.Tools;

public class Deferred<T>
{
    private T _value = default!;
    private Func<T> _getValue;

    /// <summary>
    /// Construct with lambda to return value
    /// </summary>
    /// <param name="getValue"></param>
    public Deferred(Func<T> getValue)
    {
        _getValue = () =>
        {
            Interlocked.Exchange(ref _getValue, () => _value);
            return _value = getValue();
        };
    }

    /// <summary>
    /// Return value (lazy)
    /// </summary>
    public T Value => _getValue();
}
