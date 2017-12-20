using System;

public interface IObserver<TType> {
    void OnCompleted();
    void OnError(Exception exception);
    void OnNext(TType value);
}

public interface IObservable<TType> {
    void Subscribe(IObserver<TType> observer);
}