// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DynamicDataSources;

/// <summary>
/// Provides a thread-safe base for dynamic test data sources.
/// Implements <see cref="IDataStrategy"/> and serves as strategy controller
/// for test data generation with temporary strategy overrides.
/// </summary>
/// <remarks>
/// <para>
/// Implements a dsm pattern for temporarily modifying data strategy parameters (ArgsCode and PropsCode)
/// within a specific scope. Changes automatically revert when the scope exits.
/// </para>
/// <para>
/// Key features:
/// <list type="bullet">
///   <item>Thread-safe operation using <see cref="AsyncLocal{T}"/> for async/await support</item>
///   <item>Scoped strategy modifications via disposable mementos</item>
///   <item>Default strategy fallback behavior</item>
///   <item>Value equality based on strategy configuration</item>
/// </list>
/// </para>
/// </remarks>
public abstract class DynamicDataSource : IDataStrategy
{
    #region Embedded DataStrategyMemento Class
    /// <summary>
    /// Represents a snapshot of strategy state that can be temporarily applied and reverted.
    /// </summary>
    private sealed class DataStrategyMemento : IDisposable
    {
        private readonly DynamicDataSource _dataSource;
        private readonly ArgsCode? _tempArgsCodeValue;
        private readonly PropsCode? _tempPropsCodeValue;
        private bool _disposed = false;

        /// <summary>
        /// Captures current state and applies new strategy values.
        /// </summary>
        internal DataStrategyMemento(
            DynamicDataSource dataSource,
            ArgsCode argsCode,
            PropsCode propsCode)
        {
            _dataSource = dataSource
                ?? throw new ArgumentNullException(nameof(dataSource));

            _tempArgsCodeValue = _dataSource._tempArgsCode.Value;
            _dataSource._tempArgsCode.Value = argsCode.Defined(nameof(argsCode));

            _tempPropsCodeValue = _dataSource._tempPropsCode.Value;
            _dataSource._tempPropsCode.Value = propsCode.Defined(nameof(propsCode));
        }

        /// <summary>
        /// Restores the previous strategy values.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _dataSource._tempArgsCode.Value = _tempArgsCodeValue;
            _dataSource._tempPropsCode.Value = _tempPropsCodeValue;
            _disposed = true;
        }
    }
    #endregion

    #region Fields
    private readonly ArgsCode _argsCode;
    private readonly PropsCode _propsCode;
    private readonly AsyncLocal<ArgsCode?> _tempArgsCode = new();
    private readonly AsyncLocal<PropsCode?> _tempPropsCode = new();
    #endregion

    #region Properties
    /// <summary>
    /// Gets the currently active <see cref="ArgsCode"/>, preferring any temporary override.
    /// </summary>
    /// <value>
    /// The temporary <see cref="ArgsCode"/> set, otherwise the default <see cref="Statics.ArgsCode"/>.
    /// </value>
    public ArgsCode ArgsCode
    => _tempArgsCode.Value ?? _argsCode;

    /// <summary>
    /// Gets the currently active <see cref="PropsCode"/>, preferring any temporary override.
    /// </summary>
    /// <value>
    /// The temporary <see cref="PropsCode"/> if set, otherwise the default <see cref="Statics.PropsCode"/>.
    /// </value>
    public PropsCode PropsCode
    => _tempPropsCode.Value ?? _propsCode;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance with default strategy values.
    /// </summary>
    /// <param name="argsCode">The default argument processing mode.</param>
    /// <param name="propsCode">The default currentValue inclusion mode.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if either parameter is null.
    /// </exception>
    protected DynamicDataSource(ArgsCode argsCode, PropsCode propsCode)
    {
        _argsCode = argsCode.Defined(nameof(argsCode));
        _propsCode = propsCode.Defined(nameof(propsCode));
        _tempArgsCode.Value = null;
        _tempPropsCode.Value = null;
    }
    #endregion

    #region Methods
    #region Protected methods
    /// <summary>
    /// Executes a generator function with optional temporary strategy overrides, allowing dynamic data customization.  
    /// Designed for use in derivatives of <see cref="DynamicObjectArraySource"/>..
    /// </summary>
    /// <typeparam name="T">The type of data to generate.</typeparam>
    /// <param name="dataGenerator">The function to execute.</param>
    /// <param name="paramName">Parameter name for error reporting.</param>
    /// <param name="argsCode">Optional temporary ArgsCode override.</param>
    /// <param name="propsCode">Optional temporary PropsCode override.</param>
    /// <returns>The generated data.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if dataGenerator is null.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Only applies temporary overrides when they differ from current values.
    /// Minimizes dsm creation when possible.
    /// </para>
    /// </remarks>
    protected T WithOptionalDataStrategy<T>(
        [NotNull] Func<T> dataGenerator,
        string paramName,
        ArgsCode? argsCode,
        PropsCode? propsCode)
    {
        ArgumentNullException.ThrowIfNull(dataGenerator, paramName);

        if (isUnchanged(argsCode, ArgsCode) &&
            isUnchanged(propsCode, PropsCode))
        {
            return dataGenerator();
        }

        using DataStrategyMemento dsm = new(
            this,
            argsCode ?? ArgsCode,
            propsCode ?? PropsCode);

        return dataGenerator();

        #region Local methods
        static bool isUnchanged<TCode>(
            TCode? nullableNewValue,
            TCode currentValue)
        where TCode : struct, Enum
        => nullableNewValue?.Equals(currentValue) != false;
        #endregion
    }
    #endregion

    #region Public methods
    /// <inheritdoc/>
    public bool Equals(IDataStrategy? other)
        => other is not null
            && ArgsCode == other.ArgsCode
            && PropsCode == other.PropsCode;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
        => obj is IDataStrategy other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode()
        => HashCode.Combine(ArgsCode, PropsCode);
    #endregion
    #endregion
}

public abstract class DynamicDataSource<TRow>(ArgsCode argsCode, PropsCode propsCode)
: DynamicDataSource(argsCode, propsCode)
{
    protected string? TestMethodName {  get; set; } = null;

    public void ResetTestMethodName()
    => TestMethodName = null;

    protected abstract TRow TestDataToParams<TTestData>(string? testMethodName, TTestData testData)
    where TTestData : notnull, ITestData;

    #region TestDataToParms methods
    #region TestDataToParams (Standard test cases)
    /// <summary>
    /// Creates a parameter array for a standard test case with one argument.
    /// </summary>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Description of the expected result.</param>
    /// <param name="arg1">First argument value.</param>
    /// <returns>
    /// Array of test parameters formatted according to the current strategy.
    /// </returns>
    protected TRow TestDataToParams<T1>(
        string definition,
        string expected,
        T1? arg1)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1));

    /// <summary>
    /// Creates a parameter array for a standard test case with two arguments.
    /// </summary>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <typeparam name="T2">Type of the second test argument.</typeparam>
    /// <inheritdoc cref="TestDataToParams{T1}"/>
    protected TRow TestDataToParams<T1, T2>(
        string definition,
        string expected,
        T1? arg1, T2? arg2)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2));

    /// <inheritdoc cref="TestDataToParams{T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4, T5>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4, T5, T6>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataToParams<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    => TestDataToParams(
        TestMethodName,
        CreateTestData(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region TestDataReturnsToParams
    /// <summary>
    /// Creates a parameter array for a test case expecting a value type return.
    /// </summary>
    /// <typeparam name="TStruct">Type of expected return value (non-nullable struct).</typeparam>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Expected return value.</param>
    /// <param name="arg1">First argument value.</param>
    /// <returns>
    /// Array of test parameters including the expected return value.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when current ArgsCode configuration is invalid.
    /// </exception>
    protected TRow TestDataReturnsToParams<TStruct, T1>(
        string definition,
        TStruct expected,
        T1? arg1)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1));

    /// <summary>
    /// Creates a parameter array for a value type return test case with two arguments.
    /// </summary>
    /// <typeparam name="TStruct">Type of expected return value.</typeparam>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <typeparam name="T2">Type of the second test argument.</typeparam>
    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1}"/>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? args6)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, args6));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6, T7, t8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    protected TRow TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TStruct : struct
    => TestDataToParams(
        TestMethodName,
        CreateTestDataReturns(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region TestDataThrowsToParams
    /// <summary>
    /// Creates a parameter array for a test case expecting an exception.
    /// </summary>
    /// <typeparam name="TException">Type of expected exception.</typeparam>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Expected exception instance.</param>
    /// <param name="arg1">First argument value.</param>
    /// <returns>
    /// Array of test parameters including the expected exception.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when current ArgsCode configuration is invalid.
    /// </exception>
    protected TRow TestDataThrowsToParams<TException, T1>(
        string definition,
        TException expected,
        T1? arg1)
        where TException : Exception
        => TestDataToParams(TestMethodName, CreateTestDataThrows(definition, expected, arg1));

    /// <summary>
    /// Creates a parameter array for an exception test case with two arguments.
    /// </summary>
    /// <typeparam name="TException">Type of expected exception.</typeparam>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <typeparam name="T2">Type of the second test argument.</typeparam>
    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1}"/>
    protected TRow TestDataThrowsToParams<TException, T1, T2>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2)
        where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4, T5>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    protected TRow TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TException : Exception
    => TestDataToParams(
        TestMethodName,
        CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion
    #endregion
}


