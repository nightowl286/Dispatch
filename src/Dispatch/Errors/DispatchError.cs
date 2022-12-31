using TNO.Dispatch.Abstractions;

namespace TNO.Dispatch.Errors;

/// <inheritdoc/>
public class DispatchError : IDispatchError
{
    #region Properties
    /// <inheritdoc/>
    public string Description { get; }

    /// <inheritdoc/>
    public IDispatchError? InnerError { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initialises a new instance of the <see cref="DispatchError"/> 
    /// class with the given <paramref name="description"/>.
    /// </summary>
    /// <param name="description">The description of the error.</param>
    public DispatchError(string description) => Description = description;

    /// <summary>
    /// Initialises a new instance of the <see cref="DispatchError"/> class with
    /// the given <paramref name="description"/> and <paramref name="innerError"/>.
    /// </summary>
    /// <param name="description">The description of the error.</param>
    /// <param name="innerError">The inner error that caused this one.</param>
    public DispatchError(string description, IDispatchError? innerError) : this(description) => InnerError = innerError;
    #endregion

    #region Methods
    /// <inheritdoc/>
    public Exception GetException()
    {
        if (InnerError is not null)
            return new DispatchException(Description, InnerError.GetException());

        return new DispatchException(Description);
    }
    #endregion
}
