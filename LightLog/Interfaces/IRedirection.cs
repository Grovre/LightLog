namespace LightLog.Interfaces;

public interface IRedirection<T>
{
    T Redirect(T o);
}