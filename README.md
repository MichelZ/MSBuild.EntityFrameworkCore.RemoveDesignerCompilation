# MSBuild.EntityFrameworkCore.RemoveDesignerCompilation

This MSBuild task is designed to remove the compilation of `*.Designer.cs` files of Entity Framework Core migrations, except for the last N files. It is a simple way to keep your project clean and minimize build times.

To achieve this, the task needs to move the `[Migration]` and `[DBContext]` properties from the Designer file to the main file. 
This is done by using regular expressions, so it is not guaranteed to work in all cases. If you find a case where it does not work, please open an issue and I will try to fix it.

It also needs to add the namespace for your DBContext, so be sure to set that if it's not the same namespace as your project's root namespace.

## Getting Started

To start using this package, simply add it as a NuGet package to your project.

### Prerequisites

- .NET Core SDK 3.1 or higher
- Entity Framework Core 3.1 or higher

### Installation

1. Add the NuGet package to your project:

```bash
dotnet add package MSBuild.EntityFrameworkCore.RemoveDesignerCompilation
```

## Configuration

You can customize the behavior of this task by setting the following properties in your project file:

```xml
<PropertyGroup>
    <RDC_Enabled>true</RDC_Enabled>
    <RDC_MigrationFilesPath>$(ProjectDir)\Migrations</RDC_MigrationFilesPath>
    <RDC_DBContextNamespace>$(RootNamespace)</RDC_DBContextNamespace>
    <RDC_DesignerFileCountToKeep>2</RDC_DesignerFileCountToKeep>
</PropertyGroup>
```

- `RDC_Enabled`: Whether to enable the MSBuild task. Default is `true`.
- `RDC_MigrationFilesPath`: The path where your Entity Framework Core migration files are located. Default is `$(ProjectDir)\Migrations`.
- `RDC_DBContextNamespace`: The namespace used for your DbContext. Default is the project's root namespace.
- `RDC_DesignerFileCountToKeep`: The number of most recent `*.Designer.cs` files to keep compiling. Default is `2`.

## Usage

Once you have configured the properties, simply build your project as usual. The MSBuild task will automatically remove the compilation of older `*.Designer.cs` files in your migration folder, keeping only the last N files as specified in the `RDC_DesignerFileCountToKeep` property.
The MSBuild task will also modify your main migration files to include the `[Migration]` and `[DBContext]` attributes that were previously in the `*.Designer.cs` files, so be sure to commit those changes.

## Known Issues

The following known issues exist:
- These files are needed for actual migrations
  - It should work with a "going forward" approach, when you don't need to create additional databases, and you don't need to revert older migrations
  - It has issues when you need to create additional databases (i.e. newly created database needs to be migrated to latest state)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.