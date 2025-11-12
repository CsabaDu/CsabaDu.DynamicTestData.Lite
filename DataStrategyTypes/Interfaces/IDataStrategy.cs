// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DataStrategyTypes.Interfaces;

/// <summary>
/// Represents a strategy for processing test data, defining how an <see cref="ITestData"/> instance should be turned into test data row.
/// </summary>
public interface IDataStrategy : IEquatable<IDataStrategy>
{
    /// <summary>
    /// Gets the <see cref="ITestData"/> instance processing strategy code.
    /// </summary>
    /// <value>
    /// An <see cref="Statics.ArgsCode"/> value specifying how method arguments should be processed.
    /// </value>
    ArgsCode ArgsCode { get; }

    /// <summary>
    /// Gets the property inclusion strategy code.
    /// </summary>
    /// <value>
    /// A <see cref="Statics.PropsCode"/> value specifying which properties should be included.
    /// </value>
    PropsCode PropsCode { get; }
}