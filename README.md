# AV.Direction

![Header](documentation_header.svg)

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-000000.svg?style=flat-square&logo=unity)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE.md)

High-performance, juice-focused direction and range visualizer for Unity Scene View.

## ✨ Features

- **Direction Visualization**: Draw vectors or angles in the Scene view with `[Direction]`.
- **Range Visualization**: Draw detection radii or interaction ranges with `[RangeCircle]`.
- **Line Ranges**: Visualize linear distances with `[LineRange]`.
- **Zero Runtime Cost**: Attributes are stripped or ignored in build (Editor-only visualization).

## 📦 Installation

Install via Unity Package Manager (git URL).

### Dependencies
- **Unity.Mathematics**

## 🚀 Usage

Apply attributes to your fields to see them in the Scene view.

```csharp
using AV.Direction.Runtime.Attributes;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [RangeCircle] public float scanRadius = 10f;
    [Direction] public Vector3 scanDirection;
}
```

## ⚠️ Status

- 🧪 **Tests**: Missing.
- 📘 **Samples**: None.
