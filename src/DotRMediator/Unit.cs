namespace DotRMediator;

/// <summary>
/// Represents an empty response for requests that return no value.
/// </summary>
/// <remarks>
/// Equivalent to MediatR's <c>Unit</c> type.
/// All instances compare equal; use <see cref="Value"/> as the canonical singleton.
/// </remarks>
public readonly struct Unit : IEquatable<Unit>
{
    /// <summary>
    /// Singleton instance of <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    /// Returned by void-style <see cref="IRequestHandler{TRequest}"/> implementations.
    /// </remarks>
    public static readonly Unit Value = default;

    /// <summary>
    /// Determines whether this instance equals another <see cref="Unit"/> value.
    /// </summary>
    /// <remarks>
    /// Always returns <c>true</c> because all <see cref="Unit"/> values are equivalent.
    /// </remarks>
    /// <param name="other">The other <see cref="Unit"/> value.</param>
    /// <returns><c>true</c> for every comparison.</returns>
    public bool Equals(Unit other) => true;

    /// <summary>
    /// Determines whether this instance equals the specified object.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> only when <paramref name="obj"/> is a <see cref="Unit"/>.
    /// </remarks>
    /// <param name="obj">The object to compare.</param>
    /// <returns><c>true</c> when <paramref name="obj"/> is <see cref="Unit"/>; otherwise <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is Unit;

    /// <summary>
    /// Returns a fixed hash code for all <see cref="Unit"/> values.
    /// </summary>
    /// <remarks>
    /// Always returns <c>0</c> to satisfy equality semantics.
    /// </remarks>
    /// <returns>The hash code <c>0</c>.</returns>
    public override int GetHashCode() => 0;

    /// <summary>
    /// Returns the string representation of <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    /// Always returns <c>()</c> for diagnostic output.
    /// </remarks>
    /// <returns>The literal <c>()</c>.</returns>
    public override string ToString() => "()";

    /// <summary>
    /// Determines whether two <see cref="Unit"/> values are equal.
    /// </summary>
    /// <remarks>
    /// Always returns <c>true</c>.
    /// </remarks>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>true</c> for every comparison.</returns>
    public static bool operator ==(Unit left, Unit right) => true;

    /// <summary>
    /// Determines whether two <see cref="Unit"/> values are unequal.
    /// </summary>
    /// <remarks>
    /// Always returns <c>false</c>.
    /// </remarks>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><c>false</c> for every comparison.</returns>
    public static bool operator !=(Unit left, Unit right) => false;
}
