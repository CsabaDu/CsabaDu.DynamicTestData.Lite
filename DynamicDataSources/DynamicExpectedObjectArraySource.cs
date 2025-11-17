// SPDX-License-Identifier: MIT
// Copyright (c) 2025. Csaba Dudas (CsabaDu)

namespace CsabaDu.DynamicTestData.Lite.DynamicDataSources;

/// <summary>
/// Represents a dynamic object array source, specifically configured to exclude testcase name from
/// the object arrays when using 'ArgsCode.Properties'.
/// </summary>
/// <remarks>This class is an abstract base type that provides functionality for dynamically generating object
/// arrays with a focus on expected properties. It inherits from <see cref="DynamicObjectArraySource"/> and is
/// initialized with a specific <see cref="ArgsCode"/> instance.</remarks>
/// <param name="argsCode">The <see cref="ArgsCode"/> instance used to configure the dynamic object array source.</param>
public abstract class DynamicExpectedObjectArraySource(ArgsCode argsCode)
: DynamicObjectArraySource(argsCode, PropsCode.Expected);