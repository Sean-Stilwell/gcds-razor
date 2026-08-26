# Repository Guidelines

## Project Structure & Module Organization

This repository contains a .NET 10 Razor class library under `GcdsWrapper.Blazor/` and a minimal interactive host under `GcdsWrapper.Demo/`. Specialized form wrappers live in `Gcds*.razor`; attribute-oriented wrappers and their shared renderer are in `GcdsComponents.cs` and `GcdsComponentBase.cs`. Shared enums and version configuration are in `GcdsEnums.cs` and `GcdsVersion.cs`. Browser-facing JavaScript interop belongs in `GcdsWrapper.Blazor/wwwroot/`. `RawGcdsDemo.razor` is a lightweight manual integration example. Build outputs in `bin/` and `obj/` are generated and must not be committed.

Automated bUnit and xUnit tests live in `GcdsWrapper.Blazor.Tests/`.

## Build, Test, and Development Commands

- `dotnet restore GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj` restores NuGet dependencies.
- `dotnet build GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj` compiles the library and Razor components.
- `dotnet build GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj -c Release` verifies release packaging inputs.
- `dotnet run --project GcdsWrapper.Demo/GcdsWrapper.Demo.csproj` runs the bilingual WebAssembly documentation site.
- `dotnet publish GcdsWrapper.Demo/GcdsWrapper.Demo.csproj -c Release` creates the GitHub Pages static output.
- `dotnet test GcdsWrapper.Blazor.Tests/GcdsWrapper.Blazor.Tests.csproj` runs the component tests with Microsoft Testing Platform.
- `dotnet format GcdsWrapper.Blazor/GcdsWrapper.Blazor.csproj --verify-no-changes` checks standard .NET formatting.

The library is not directly runnable. Exercise changes from a consuming Blazor app; render `GcdsAssets` once and use `RawGcdsDemo` to verify upstream GCDS assets.

## Coding Style & Naming Conventions

Follow standard C# conventions: four-space indentation, file-scoped namespaces, nullable annotations, PascalCase for public members and component parameters, and camelCase for private fields. Name wrapper components `Gcds<Component>.razor`. Keep wrappers thin: map typed parameters to official GCDS attributes, pass unmatched attributes through, and place reusable browser interop in `wwwroot/gcdsInterop.js`. Update the pinned upstream version only in `GcdsVersion.Components`.

## Testing Guidelines

Prefer bUnit for component behavior and xUnit for C# helpers. Name test files `<Component>Tests.cs` and tests by observable behavior, for example `Input_UpdatesBoundValue_OnGcdsChange`. Cover parameter rendering, event forwarding, validation integration, JS subscription disposal, and asset-loading idempotency.

## Commit & Pull Request Guidelines

Because the repository has no commit history yet, use short imperative subjects such as `Add textarea wrapper` or `Fix input event disposal`. Keep each commit focused. Pull requests should explain behavior changes, list validation commands, link related issues, and include screenshots or a small consuming-app example for visible UI changes. Call out any GCDS version update and relevant upstream changelog notes.
