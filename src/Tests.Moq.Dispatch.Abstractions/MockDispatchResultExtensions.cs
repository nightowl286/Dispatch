using Moq;
using TNO.Dispatch.Abstractions;
using TNO.Dispatch.Abstractions.Results;

namespace TNO.Tests.Moq.Dispatch.Abstractions;

/// <summary>
/// Contains useful extension methods for the <see cref="Mock"/>&lt;<see cref="IDispatchResult{T}"/>&gt; class.
/// </summary>
public static class MockDispatchResultExtensions
{
    #region Methods
    /// <summary>Sets up the given <paramref name="mockResult"/> with the given <paramref name="successful"/> state.</summary>
    /// <typeparam name="TOutput">The output type of the result.</typeparam>
    /// <param name="mockResult">The mock result to setup.</param>
    /// <param name="successful">Whether the result should be successful.</param>
    /// <returns>The <paramref name="mockResult"/> instance that was given.</returns>
    public static Mock<IDispatchResult<TOutput>> WithSuccess<TOutput>(this Mock<IDispatchResult<TOutput>> mockResult, bool successful = true)
       where TOutput : notnull
    {
        mockResult.SetupGet(r => r.Successful).Returns(successful);
        return mockResult;
    }

    /// <summary>
    /// Sets up the given <paramref name="mockResult"/> to be unsuccessful, have a
    /// <see langword="default"/> output, and the given <paramref name="errors"/>.
    /// </summary>
    /// <param name="mockResult">The mock result to setup.</param>
    /// <param name="errors">The errors that the result should have.</param>
    /// <inheritdoc cref="WithSuccess{TOutput}(Mock{IDispatchResult{TOutput}}, bool)"/>
    public static Mock<IDispatchResult<TOutput>> UnsuccessfulErrors<TOutput>(this Mock<IDispatchResult<TOutput>> mockResult, IReadOnlyCollection<IDispatchError> errors)
       where TOutput : notnull
    {
        mockResult.SetupGet(r => r.Successful).Returns(false);
        mockResult.SetupGet(r => r.Output).Returns(default(TOutput));
        mockResult.SetupGet(r => r.Errors).Returns(errors);
        return mockResult;
    }

    /// <summary>
    /// Sets up the given <paramref name="mockResult"/> to have no errors.
    /// </summary>
    /// <inheritdoc cref="UnsuccessfulErrors{TOutput}(Mock{IDispatchResult{TOutput}}, IReadOnlyCollection{IDispatchError})"/>
    public static Mock<IDispatchResult<TOutput>> WithNoErrors<TOutput>(this Mock<IDispatchResult<TOutput>> mockResult)
       where TOutput : notnull
    {
        mockResult.SetupGet(r => r.Errors).Returns(Array.Empty<IDispatchError>());
        return mockResult;
    }

    /// <summary>Sets up the given <paramref name="mockResult"/> to have the given <paramref name="output"/>.</summary>
    /// <param name="mockResult">The mock result to setup.</param>
    /// <param name="output">The output that the result should have.</param>
    /// <inheritdoc cref="WithSuccess{TOutput}(Mock{IDispatchResult{TOutput}}, bool)"/>
    public static Mock<IDispatchResult<TOutput>> WithOutput<TOutput>(this Mock<IDispatchResult<TOutput>> mockResult, TOutput output)
       where TOutput : notnull
    {
        mockResult.SetupGet(r => r.Output).Returns(output);
        return mockResult;
    }

    /// <summary>
    /// Sets up the given <paramref name="mockResult"/> to be successful,
    /// have no errors, and have the given <paramref name="output"/>.
    /// </summary>
    /// <param name="mockResult">The mock result to setup.</param>
    /// <param name="output">The output the result should have.</param>
    /// <inheritdoc cref="WithSuccess{TOutput}(Mock{IDispatchResult{TOutput}}, bool)"/>
    public static Mock<IDispatchResult<TOutput>> SuccessfulOutput<TOutput>(this Mock<IDispatchResult<TOutput>> mockResult, TOutput output)
       where TOutput : notnull
    {
        mockResult.WithSuccess();
        mockResult.WithNoErrors();
        mockResult.WithOutput(output);

        return mockResult;
    }
    #endregion
}
