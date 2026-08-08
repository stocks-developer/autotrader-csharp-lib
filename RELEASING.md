# Releasing

Maintainer notes for publishing this library to NuGet as
[`StocksDeveloper.AutoTraderWeb.Api`](https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/).

Last verified: **2026-08-08**.

## Toolchain required

This is an **old-style (non-SDK) `.csproj`**, `ToolsVersion 15.0`, targeting `net472`, with its
dependencies in `packages.config` resolved through `HintPath` into `packages\`. Packaging it needs all
three of:

1. **`nuget.exe`** — the standalone CLI, for `restore` and `pack`
   ([download](https://www.nuget.org/downloads))
2. **MSBuild 15 or newer** — Visual Studio 2017+, or the standalone
   [Build Tools for Visual Studio](https://visualstudio.microsoft.com/downloads/).
   The `MSBuild.exe` that ships under `C:\Windows\Microsoft.NET\Framework64\v4.0.30319` is MSBuild 4.0
   and **cannot** build a ToolsVersion 15 project.
3. **.NET Framework 4.7.2 Developer Pack** (the reference assemblies)

`dotnet build` alone will not work — it does not support `packages.config` projects.

## Before you start

- Every change is committed and pushed.
- Check what is already published and pick the next version from that:

  ```bash
  curl -s https://api.nuget.org/v3-flatcontainer/stocksdeveloper.autotraderweb.api/index.json
  ```

  New API calls are a minor bump; a fix is a patch bump.

> **A NuGet version cannot be deleted**, only unlisted — and the number is never reusable. Get it
> right before you push.

## Steps

**1. Bump the version in BOTH places.** They are separate files and they must agree:

- `Properties/AssemblyInfo.cs` — `AssemblyVersion` **and** `AssemblyFileVersion`
  (four-part, e.g. `1.4.0.0`)
- `csharp-library.nuspec` — `<version>` (three-part, e.g. `1.4.0`)

While you are in the `.nuspec`, update `<releaseNotes>` to describe this release. It is shown on the
package page, and a stale note is worse than none.

**2. Restore, clean, build.**

```bash
nuget restore csharp-library.sln
msbuild csharp-library.sln /t:Clean /p:Configuration=Release
msbuild csharp-library.sln /t:Build /p:Configuration=Release
```

**3. Pack.** Pack the **project**, not the bare `.nuspec` — the `.nuspec` here carries metadata only
and has no `<files>` section, so packing it alone produces a package with no assembly in it.

```bash
nuget pack csharp-library.csproj -Prop Configuration=Release
```

**4. Verify before pushing.** A `.nupkg` is a zip — confirm the assembly is inside and the version is
what you expect:

```bash
python -c "import zipfile,sys;print('\n'.join(n for n in zipfile.ZipFile(sys.argv[1]).namelist() if n.endswith(('.dll','.nuspec'))))" StocksDeveloper.AutoTraderWeb.Api.1.4.0.nupkg
```

**5. Push.**

```bash
nuget push StocksDeveloper.AutoTraderWeb.Api.<version>.nupkg -Source https://api.nuget.org/v3/index.json -ApiKey %NUGET_API_KEY%
```

Keep the key in an environment variable so it never enters your shell history. Never commit a key or
paste one into an issue, a chat or a document.

**6. Confirm** the new version appears on the package page and installs into a test project.

## Gotchas

- **Two version numbers, two files.** Bumping the `.nuspec` but not `AssemblyInfo.cs` produces a
  package whose assembly reports the old version — it installs fine and is confusing for years.
- **`nuget pack` with no argument** in this directory is ambiguous (there is both a `.csproj` and a
  `.nuspec`). Always name the `.csproj`.
- The website's C# setup page installs "latest" and does **not** pin a version, so a release needs no
  website change.

## Worth doing

Migrating to an **SDK-style `.csproj`** targeting `net472` would let `dotnet pack` build and package
this on any machine with the .NET SDK — no Visual Studio, no `nuget.exe`, no separate developer pack —
and would fold the `.nuspec` into the project file so the version lives in exactly one place.
