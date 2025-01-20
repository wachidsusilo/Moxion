namespace Moxion.Presentation.Abstractions.Dto;

public interface IValueDto<TSelf> : IEquatable<TSelf> where TSelf : IValueDto<TSelf>;