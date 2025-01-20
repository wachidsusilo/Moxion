namespace Moxion.Presentation.Abstractions.Dto;

public interface IValueObjectDto<TSelf> : IEquatable<TSelf> where TSelf : IValueObjectDto<TSelf>;