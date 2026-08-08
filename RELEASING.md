# Releasing

Maintainer notes for publishing this library to NuGet as
[`StocksDeveloper.AutoTraderWeb.Api`](https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/).

Last verified: **2026-08-08** (release 1.4.0).

## What you need

Just the [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or newer). Nothing else — no Visual
Studio, no `nuget.exe`, no .NET Framework Developer Pack.

That works because the project is SDK-style and pulls in
`Microsoft.NETFramework.ReferenceAssemblies`, which supplies the .NET Framework 4.7.2 reference
assemblies as an ordinary build-time package. The library still targets `net472` and still ships as
`lib/net472/StocksDeveloper.AutoTraderWeb.Api.dll` — only the build tooling changed.

> Converted from the old `packages.config` format on 2026-08-08. If you find instructions elsewhere
> mentioning `nuget pack`, `msbuild`, `AssemblyInfo.cs` versions or `csharp-library.nuspec`, they are
> out of date — none of those are used any more.

## Before you start

- Every change is committed and pushed.
- Check what is already published and pick the next version from that. New API calls are a minor
  bump; a fix is a patch bump.

  ```bash
  curl -s https://api.nuget.org/v3-flatcontainer/stocksdeveloper.autotraderweb.api/index.json
  ```

> **A NuGet version cannot be deleted**, only unlisted, and the number is never reusable. Get it
> right before you push.

## Steps

**1. Bump the version.** One place only — `<Version>` in `csharp-library.csproj`. It drives the
package version, the assembly version and the file version together.

While you are there, update `<PackageReleaseNotes>` to describe this release. It is shown on the
package page, and a stale note is worse than none.

**2. Build and pack.**

```bash
dotnet build -c Release
dotnet pack  -c Release
```

The package lands in `bin/Release/StocksDeveloper.AutoTraderWeb.Api.<version>.nupkg`.

**3. Verify before pushing.** A `.nupkg` is a zip. Confirm the assembly is in the right place, the
version is right, and the API you are shipping is actually in the DLL:

```bash
python -c "
import zipfile,sys
z=zipfile.ZipFile(sys.argv[1])
print([n for n in z.namelist() if n.endswith(('.dll','.nuspec'))])
dll=z.read('lib/net472/StocksDeveloper.AutoTraderWeb.Api.dll')
print('PlaceAutoTraderBracketOrder' in str(dll))
" bin/Release/StocksDeveloper.AutoTraderWeb.Api.<version>.nupkg
```

**4. Push.**

```bash
dotnet nuget push bin/Release/StocksDeveloper.AutoTraderWeb.Api.<version>.nupkg \
  --source https://api.nuget.org/v3/index.json --api-key "$NUGET_API_KEY"
```

Keep the key in an environment variable so it never lands in your shell history. Never commit a key
or paste one into an issue, a chat or a document.

**5. Confirm** the new version appears on the package page and restores into a test project.

## Notes

- **Package metadata lives in the `.csproj`**, in the `PackageId`/`Version`/`Description` property
  group. There is no `.nuspec` any more, and `Properties/AssemblyInfo.cs` keeps only the two
  attributes the SDK does not generate (`ComVisible`, `Guid`) — everything else, including the
  version, comes from the project file. This is deliberate: the version used to live in two files
  that could silently disagree.
- **Dependencies are declared once**, as `PackageReference`. Only `System.Text.Json` is listed
  directly; the rest (`System.Buffers`, `System.Memory`, `System.Numerics.Vectors`,
  `System.Runtime.CompilerServices.Unsafe`, `System.Text.Encodings.Web`,
  `System.Threading.Tasks.Extensions`, `System.ValueTuple`, `Microsoft.Bcl.AsyncInterfaces`) come in
  transitively at the same versions the old `packages.config` pinned. Do not re-add them by hand.
- `Microsoft.NETFramework.ReferenceAssemblies` is marked `PrivateAssets="all"`, so it is build-time
  only and never appears in the published package's dependency list.
- The website's C# setup page installs "latest" and does **not** pin a version, so a release needs no
  website change.
