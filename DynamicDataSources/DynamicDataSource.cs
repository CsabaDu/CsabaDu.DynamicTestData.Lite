// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.DynamicDataSources;

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

/// <summary>
/// Abstract base class for dynamic test data sources that manage typed data holders.
/// </summary>
/// <typeparam name="TDataHolder">The type of data holder used to store test cases (must be a class).</typeparam>
/// <remarks>
/// <para>
/// Extends <see cref="DynamicDataSource"/> with:
/// <list type="bullet">
///   <item>Typed data holder management</item>
///   <item>Test case registration methods</item>
///   <item>Holder initialization/reset capabilities</item>
/// </list>
/// </para>
/// <para>
/// Provides three categories of test case registration:
/// <list type="bullet">
///   <item>Standard cases with string expectations</item>
///   <item>Value-returning cases with struct expectations</item>
///   <item>Exception-throwing cases</item>
/// </list>
/// </para>
/// </remarks>
public abstract class DynamicDataSource<TDataHolder>(ArgsCode argsCode, PropsCode propsCode)
: DynamicDataSource(argsCode, propsCode)
where TDataHolder : class
{
    /// <summary>
    /// Gets or sets the current data holder instance.
    /// </summary>
    protected TDataHolder? DataHolder { get; set; }

    /// <summary>
    /// Resets the current data holder to its default state.
    /// </summary>
    public virtual void ResetDataHolder()
    => DataHolder = default;

    /// <summary>
    /// Adds the specified test data to the collection.
    /// </summary>
    /// <typeparam name="TTestData">The type of the test data to add. Must implement <see cref="ITestData"/> and cannot be null.</typeparam>
    /// <param name="testData">The test data to add. This parameter cannot be null.</param>
    protected abstract void Add<TTestData>(TTestData testData)
    where TTestData : notnull, ITestData;
}