// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DynamicDataSources;

public abstract class DynamicNamedDataSource<TRow>(ArgsCode argsCode, PropsCode propsCode)
: DynamicDataSource<TRow>(argsCode, propsCode)
{
    protected override TRow Convert<TTestData>(TTestData testData)
    => Convert(testData, null);

    protected abstract TRow Convert<TTestData>(TTestData testData, string? testMethodName)
    where TTestData : notnull, ITestData;
}

