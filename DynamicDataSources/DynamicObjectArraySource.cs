// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DynamicDataSources;

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
: DynamicDataSource<object?[]>(argsCode, propsCode)
{
    #region Methods
    protected override object?[] Convert<TTestData>(TTestData testData)
    => testData.ToParams(ArgsCode, PropsCode);
    #endregion
}
