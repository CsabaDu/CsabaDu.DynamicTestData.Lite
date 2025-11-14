// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DynamicDataSources;

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
public abstract class DynamicDataHolderSource<TDataHolder>(ArgsCode argsCode, PropsCode propsCode)
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

    #region Add methods
    #region Add (Standard test cases)
    /// <summary>
    /// Adds a standard test case with string expected result and one argument.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Description of the expected result.</param>
    /// <param name="arg1">First argument value.</param>
    protected void Add<T1>(
        string definition,
        string expected,
        T1? arg1)
        => Add(CreateTestData(definition, expected, arg1));

    /// <summary>
    /// Adds a standard test case with string expected result and two arguments.
    /// </summary>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <typeparam name="T2">Type of the second argument.</typeparam>
    /// <inheritdoc cref="Add{T1}"/>
    protected void Add<T1, T2>(
        string definition,
        string expected,
        T1? arg1, T2? arg2)
        => Add(CreateTestData(definition, expected, arg1, arg2));

    protected void Add<T1, T2, T3>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3));

    protected void Add<T1, T2, T3, T4>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    protected void Add<T1, T2, T3, T4, T5>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    protected void Add<T1, T2, T3, T4, T5, T6>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    protected void Add<T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    protected void Add<T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    protected void Add<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    => Add(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region AddReturns (Value-returning test cases)
    /// <summary>
    /// Adds a test case expecting a value type return with one argument.
    /// </summary>
    /// <typeparam name="TStruct">Type of expected return value (non-nullable struct).</typeparam>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Expected return value.</param>
    /// <param name="arg1">First argument value.</param>
    protected void AddReturns<TStruct, T1>(
        string definition,
        TStruct expected,
        T1? arg1)
        where TStruct : struct
        => Add(CreateTestDataReturns(definition, expected, arg1));

    /// <summary>
    /// Adds a test case expecting a value type return with two arguments.
    /// </summary>
    /// <typeparam name="TStruct">Type of expected return value.</typeparam>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <typeparam name="T2">Type of the second argument.</typeparam>
    /// <inheritdoc cref="AddReturns{TStruct, T1}"/>
    protected void AddReturns<TStruct, T1, T2>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2)
        where TStruct : struct
        => Add(CreateTestDataReturns(definition, expected, arg1, arg2));

    protected void AddReturns<TStruct, T1, T2, T3>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3));

    protected void AddReturns<TStruct, T1, T2, T3, T4>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    protected void AddReturns<TStruct, T1, T2, T3, T4, T5>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    protected void AddReturns<TStruct, T1, T2, T3, T4, T5, T6>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    protected void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    protected void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    protected void AddReturns<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TStruct : struct
    => Add(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion

    #region AddThrows (Exception test cases)
    /// <summary>
    /// Adds a test case expecting an exception with one argument.
    /// </summary>
    /// <typeparam name="TException">Type of expected exception.</typeparam>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <param name="definition">Description of the test scenario.</param>
    /// <param name="expected">Expected exception instance.</param>
    /// <param name="arg1">First argument value.</param>
    protected void AddThrows<TException, T1>(
        string definition,
        TException expected,
        T1? arg1)
        where TException : Exception
        => Add(CreateTestDataThrows(definition, expected, arg1));

    /// <summary>
    /// Adds a test case expecting an exception with two arguments.
    /// </summary>
    /// <typeparam name="TException">Type of expected exception.</typeparam>
    /// <typeparam name="T1">Type of the first argument.</typeparam>
    /// <typeparam name="T2">Type of the second argument.</typeparam>
    /// <inheritdoc cref="AddThrows{TException, T1}"/>
    protected void AddThrows<TException, T1, T2>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2)
        where TException : Exception
        => Add(CreateTestDataThrows(definition, expected, arg1, arg2));

    protected void AddThrows<TException, T1, T2, T3>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3));

    protected void AddThrows<TException, T1, T2, T3, T4>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    protected void AddThrows<TException, T1, T2, T3, T4, T5>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    protected void AddThrows<TException, T1, T2, T3, T4, T5, T6>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    protected void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    protected void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    protected void AddThrows<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TException : Exception
    => Add(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion
    #endregion
}