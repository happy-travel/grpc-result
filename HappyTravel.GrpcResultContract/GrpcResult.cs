using System.Runtime.Serialization;

namespace HappyTravel.GrpcResultContract;

[DataContract]
public readonly struct GrpcResult<T, E>
{
    private GrpcResult(bool isFailure, E error, T value)
    {
        IsFailure = isFailure;
        _error = error;
        _value = value;
    }
    
    
    public E Error 
        => IsFailure 
            ? _error 
            : throw new NullReferenceException("You attempted to access the Error property for a successful result");
    
    
    public T Value 
        => !IsFailure 
            ? _value 
            : throw new NullReferenceException("You attempted to access the Value property for a failed result");
    

    public static GrpcResult<T, E> Failure(E error) 
        => new GrpcResult<T, E>(true, error, default);

    
    public static GrpcResult<T, E> Success(T value)
        => new GrpcResult<T, E>(false, default, value);


    [DataMember(Order = 1)]
    public bool IsFailure { get; }
    
    [DataMember(Order = 2)]
    private readonly E _error;
    
    [DataMember(Order = 3)]
    private readonly T _value;
}