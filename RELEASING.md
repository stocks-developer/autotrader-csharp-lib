# Releasing

Maintainer notes for publishing this library to NuGet as
[`StocksDeveloper.AutoTraderWeb.Api`](https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/).

Last verified: **2026-08-08** (release 1.4.0).

**Publishing is keyless.** [`.github/workflows/publish.yml`](.github/workflows/publish.yml) does it,
using NuGet [Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing):
the job requests a short-lived GitHub OIDC token, nuget.org validates it against a policy registered
for this repository, and returns a temporary API key valid for about an hour and usable once. **No
API key is stored in this repository or anywhere else**, so there is nothing to rotate and nothing
to leak.

## Releasing

**1. Bump the version.** One place only — `<Version>` in `csharp-library.csproj`. It drives the
package version, the assembly version and the file version together.

While you are there, update `<PackageReleaseNotes>`. It is shown on the package page, and a stale
note is worse than none.

**2. Check locally first** (optional, but it turns a failed release into a failed build):

```bash
dotnet build -c Release
dotnet pack  -c Release
```

**3. Commit, then tag and push the tag.**

```bash
git commit -am "Release 1.4.0"
git push
git tag v1.4.0 && git push origin v1.4.0
```

The tag must match the csproj version — the workflow compares them and fails if they disagree, so a
forgotten version bump cannot publish the wrong number. `workflow_dispatch` also works for a manual
run.

**4. Watch the run** under the repo's Actions tab, then confirm the new version appears on the
package page and restores into a test project.

> **A NuGet version cannot be deleted**, only unlisted, and the number is never reusable. That is why
> the workflow verifies the package before pushing.

## One-time setup

Done once per repository, and needed before the first keyless release.

**On nuget.org** — sign in, click your username, choose **Trusted Publishing**, add a policy:

| Field | Value |
|---|---|
| Repository Owner | `stocks-developer` |
| Repository | `autotrader-csharp-lib` |
| Workflow File | `publish.yml` *(file name only, no `.github/workflows/` path)* |
| Environment | *(leave empty — this workflow does not use a GitHub environment)* |

**In this repository** — add a secret `NUGET_USER` containing the nuget.org **profile name** that
owns the package. Not an email address.

Two things to know: the policy applies to every package owned by the account you pick as its owner,
and a policy on a **private** repo starts out only temporarily active for 7 days until its first
successful publish (this repo is public, so that does not apply).

## Building

The .NET SDK (8.0 or newer) is all you need. No Visual Studio, no `nuget.exe`, no .NET Framework
Developer Pack — the project is SDK-style and pulls in
`Microsoft.NETFramework.ReferenceAssemblies`, which supplies the .NET Framework 4.7.2 reference
assemblies as an ordinary build-time package.

The library still targets `net472` and still ships as
`lib/net472/StocksDeveloper.AutoTraderWeb.Api.dll` — only the build tooling changed.

> Converted from the old `packages.config` format on 2026-08-08. Instructions elsewhere mentioning
> `nuget pack`, `msbuild`, an `AssemblyInfo.cs` version or `csharp-library.nuspec` are out of date —
> none of those are used any more.

## Notes

- **Package metadata lives in the `.csproj`**, in the `PackageId`/`Version`/`Description` property
  group. There is no `.nuspec`, and `Properties/AssemblyInfo.cs` keeps only the two attributes the
  SDK does not generate (`ComVisible`, `Guid`). The version used to live in two files that could
  silently disagree; now it does not.
- **Dependencies are declared once**, as `PackageReference`. Only `System.Text.Json` is listed
  directly; `System.Buffers`, `System.Memory`, `System.Numerics.Vectors`,
  `System.Runtime.CompilerServices.Unsafe`, `System.Text.Encodings.Web`,
  `System.Threading.Tasks.Extensions`, `System.ValueTuple` and `Microsoft.Bcl.AsyncInterfaces` come
  in transitively at the same versions the old `packages.config` pinned. Do not re-add them by hand.
- `Microsoft.NETFramework.ReferenceAssemblies` is `PrivateAssets="all"`, so it is build-time only and
  never appears in the published package's dependency list.
- **Update the website after a release.** The C# setup page installs "latest", so nothing breaks if
  you forget — but the page states the current version, and a stale number there misleads people into
  thinking they are up to date. Page:
  `stocksdev-website/src/content/docs/client-setup/c-library.md`. Bump `lastUpdated` in the same edit.
  A brand-new API call should also get a "needs version X.Y.Z" note on its API reference page.
- **Publishing by hand with a long-lived API key is discouraged** and should not be needed. If you
  ever must, generate a key scoped to this package alone, pass it via an environment variable so it
  stays out of shell history, and revoke it straight afterwards.
