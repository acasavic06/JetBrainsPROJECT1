# Structural Duplication IDE feature

This is a small C# project demonstrating the use of the **Microsoft Roslyn SDK** to manipulate C# source code.

## Description

The program reads a C# source file (`input.cs`), finds all **methods** (and **local functions**) with exactly **one parameter**, duplicates the parameter with a new name (original name + "1"), and writes the modified source code to `output.cs`.

### Example

Original method:

```csharp
void Print(int x) { ... }
```
After running
```csharp
void Print(int x, int x1) { ... }
```


# How to use:

1) Create or place a C# file named input.cs in the project folder.

2) Build the project in JetBrains Rider or Visual Studio.

3) Run the program.

4) Check output.cs for the modified source code.

Make sure the project has the required NuGet packages:
- Microsoft.CodeAnalysis.CSharp
- Microsoft.CodeAnalysis.Common

**Author**: Aleksandar Savić  
**Github**: @acasavic06
