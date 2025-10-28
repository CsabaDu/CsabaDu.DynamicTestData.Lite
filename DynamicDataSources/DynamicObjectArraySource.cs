// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.DynamicDataSources;

/// <summary>
/// Abstract base class for dynamic test data sources that generate parameter arrays for test execution.
/// </summary>
/// <remarks>
/// <para>
/// Specializes <see cref="DynamicDataSource"/> to provide:
/// <list type="bullet">
///   <item>Parameter array generation for standard test cases</item>
///   <item>Specialized methods for return value and exception test cases</item>
///   <item>Support for tests with 1-9 parameters</item>
/// </list>
/// </para>
/// <para>
/// Uses the configured <see cref="DynamicDataSource.ArgsCode"/> and <see cref="DynamicDataSource.PropsCode"/>
/// to control parameter generation.
/// </para>
/// </remarks>
public abstract class DynamicObjectArraySource(ArgsCode argsCode, PropsCode propsCode)
    : DynamicDataSource(argsCode, propsCode)
{
    #region Methods
    private object?[] TestDataToParams(ITestData testData)
    => testData.ToParams(ArgsCode, PropsCode);

    #region TestDataToParams
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
    protected object?[] TestDataToParams<T1>(
        string definition,
        string expected,
        T1? arg1)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1));

    /// <summary>
    /// Creates a parameter array for a standard test case with two arguments.
    /// </summary>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <typeparam name="T2">Type of the second test argument.</typeparam>
    /// <inheritdoc cref="TestDataToParams{T1}"/>
    protected object?[] TestDataToParams<T1, T2>(
        string definition,
        string expected,
        T1? arg1, T2? arg2)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2));

    /// <inheritdoc cref="TestDataToParams{T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4, T5>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4, T5, T6>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    => TestDataToParams(CreateTestData(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataToParams{T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataToParams<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        string expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    => TestDataToParams(CreateTestData(
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
    protected object?[] TestDataReturnsToParams<TStruct, T1>(
        string definition,
        TStruct expected,
        T1? arg1)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
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
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    /// <returns>An array of arguments.</returns>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? args6)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, args6));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataReturnsToParams{TStruct, T1, T2, T3, T4, T5, T6, T7, t8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    protected object?[] TestDataReturnsToParams<TStruct, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TStruct expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TStruct : struct
    => TestDataToParams(CreateTestDataReturns(
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
    protected object?[] TestDataThrowsToParams<TException, T1>(
        string definition,
        TException expected,
        T1? arg1)
        where TException : Exception
        => TestDataToParams(CreateTestDataThrows(definition, expected, arg1));

    /// <summary>
    /// Creates a parameter array for an exception test case with two arguments.
    /// </summary>
    /// <typeparam name="TException">Type of expected exception.</typeparam>
    /// <typeparam name="T1">Type of the first test argument.</typeparam>
    /// <typeparam name="T2">Type of the second test argument.</typeparam>
    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1}"/>
    protected object?[] TestDataThrowsToParams<TException, T1, T2>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2)
        where TException : Exception
        => TestDataToParams(CreateTestDataThrows(
            definition,
            expected,
            arg1, arg2));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2}" />
    /// <typeparam name="T3">The type of the third argument.</typeparam>
    /// <param name="arg3">The third argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3}" />
    /// <typeparam name="T4">The type of the fourth argument.</typeparam>
    /// <param name="arg4">The fourth argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4}" />
    /// <typeparam name="T5">The type of the fifth argument.</typeparam>
    /// <param name="arg5">The fifth argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4, T5>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5}" />
    /// <typeparam name="T6">The type of the sixth argument.</typeparam>
    /// <param name="arg6">The sixth argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6}" />
    /// <typeparam name="T7">The type of the seventh argument.</typeparam>
    /// <param name="arg7">The seventh argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6, T7}" />
    /// <typeparam name="T8">The type of the eighth argument.</typeparam>
    /// <param name="arg8">The eighth argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7, T8>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

    /// <inheritdoc cref="TestDataThrowsToParams{TException, T1, T2, T3, T4, T5, T6, T7, T8}" />
    /// <typeparam name="T9">The type of the ninth argument.</typeparam>
    /// <param name="arg9">The ninth argument.</param>
    protected object?[] TestDataThrowsToParams<TException, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        string definition,
        TException expected,
        T1? arg1, T2? arg2, T3? arg3, T4? arg4, T5? arg5, T6? arg6, T7? arg7, T8? arg8, T9? arg9)
    where TException : Exception
    => TestDataToParams(CreateTestDataThrows(
        definition,
        expected,
        arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    #endregion
    #endregion
}

