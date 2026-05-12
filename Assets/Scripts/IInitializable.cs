using UnityEngine;

public interface IInitializable
{
    bool Initialized { get; }
}

public interface IInitializable<T> : IInitializable
{
    public void Initialize(T first);
}

public interface IInitializable<T, U> : IInitializable
{
    public void Initialize(T first, U second);
}

public interface IInitializable<T, U, V> : IInitializable
{
    public void Initialize(T first, U second, V third);
}

public interface IInitializable<T, U, V, W> : IInitializable
{
    public void Initializable(T first, U second, V third, W fourth);
}
