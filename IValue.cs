namespace Knx
{
    public interface IValue
    {
        object Current { get; set; }
    }
    
    public interface IValue<T> : IValue
    {
        new T Current { get; set; }
    }
}