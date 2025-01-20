namespace Moxion.Abstractions;

public interface IValueObject<TSelf> : IEquatable<TSelf> where TSelf : IValueObject<TSelf>;