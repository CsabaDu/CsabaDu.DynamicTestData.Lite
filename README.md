# CsabaDu.DynamicTestData.Core

Core types of **CsabaDu.DynamicTestData**, a robust, flexible, extensible, pure .NET framework to facilitate dynamic data-driven testing.

---

## 📖 Documentation

This README contains the base info and the current version related notes.    
Visit the **[Wiki](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki)** for full documentation, including  
- 📖 [**Introduction**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/00-%F0%9F%93%96-Introduction)
- 🚀 [**Quick Start Guide**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/01-%F0%9F%9A%80-Quick-Start-Guide)  
- 📐 [**Architecture**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/02-%F0%9F%93%90-Architecture)  
- 🔍 [**Types**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/03-%F0%9F%94%8D-Types)  
- 🌍 [**Project Ecosystem**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/04-%F0%9F%8C%8D-Project-Ecosystem)  
- 📚 [**Sample Codes**](https://github.com/CsabaDu/CsabaDu.DynamicTestData/wiki/05-%F0%9F%93%9A-Sample-Codes)  

---

## 📘 Table of Contents

- [**CsabaDu.DynamicTestData — Modular Architecture**](#csabadudynamictestdata--modular-architecture))
- [**Changelog**](#changelog)
- [**Contributing**](#contributing)
- [**License**](#license)
- [**Contact**](#contact)
- [**FAQ**](#faq)
- [**Troubleshooting**](#troubleshooting)

---

## CsabaDu.DynamicTestData — Modular Architecture

### **Overview**  

**CsabaDu.DynamicTestData** has been reorganized from a single monolithic package into a set of focused, aligned modules (NuGet packages) while keeping a clean, consistent namespace hierarchy under `CsabaDu.DynamicTestData.*`. Modules are deployable package boundaries; namespaces are logical organization inside those packages. The new layout reduces transitive dependencies, clarifies responsibilities, and makes it easier for consumers to adopt only what they need.

See the Segregated Architecture Diagram for a visual overview of module boundaries and namespace relationships:

![CsabaDu_DynamicTestData_Segregated_Simplified](https://raw.githubusercontent.com/CsabaDu/CsabaDu.DynamicTestData/refs/heads/master/_Images/CsabaDu_DynamicTestData_Segregated_Simplified.svg)

---

### **Modules and contents**

#### **Core Foundation Module  
  (package: `CsabaDu.DynamicTestData.Core`)**:  

Foundation layer with essential contract surface, DTOs, stateless encoding helpers and strategy definitions.  
See [CsabaDu.DynamicTestData.Core README](https://github.com/CsabaDu/CsabaDu.DynamicTestData.Core/blob/master/README.md).

#### **Lite Implementation Module  
  (package: `CsabaDu.DynamicTestData.Lite`)**  

Lightweight runtime helpers for manual-enumerable-style data sources - depends on Core.

**Namespaces and highlights**:  

---
**`CsabaDu.DynamicTestData.DataStrategyTypes.Interfaces`**  

- *Core strategy contract*:  

  - **IDataStrategy.cs**  

    - `IDataStrategy`: interface  

---
**`CsabaDu.DynamicTestData.DynamicDataSources`**  

- *Basic data sources*:  

  - **DynamicDataSource.cs**  

    - `DynamicDataSource` : abstract class  
    - `DynamicDataSource<TDataHolder>` : abstract class  
  
  - **DynamicObjectArraySource.cs**  

    - `DynamicObjectArraySource` : abstract class  

  - **DynamicExpectedObjectArraySource.cs**  

    - `DynamicExpectedObjectArraySource` : abstract class  

---
**When to use**:  
- You implement `IEnumerable<object[]>` or `IEnumerable<T>` data sources and want minimal transitive dependencies while keeping enriched metadata support
- You need basic thread-safe data source runtime implementations for common scenarios

**Dependencies**: `CsabaDu.DynamicTestData.Core` (>= 2.1.0-beta)

---

## Changelog

### **Version 2.1.0-beta** (2025-10-27)

- Initial release of the `CsabaDu.DynamicTestData.Lite` framework, by segregating the light version of the `CsabaDu.DynamicTestData` framework into a dedicated package.

---

## License

This project is licensed under the MIT License. See the [License](LICENSE.txt) file for details.

---

## Contact

For any questions or inquiries, please contact [CsabaDu](https://github.com/CsabaDu).

---

## FAQ
---

## Troubleshooting
