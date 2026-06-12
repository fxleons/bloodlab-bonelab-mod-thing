#!/usr/bin/env bash
set -e

# build/build.sh
# Usage: set UNITY_DLL and MELON_DLL env vars to point to UnityEngine.dll and MelonLoader.dll
# Example:
# UNITY_DLL="/path/to/UnityEngine.dll" MELON_DLL="/path/to/MelonLoader.dll" ./build/build.sh

OUT_DIR="build_out"
mkdir -p "$OUT_DIR"

if command -v dotnet >/dev/null 2>&1; then
  echo "Building with dotnet"
  pushd build >/dev/null
  dotnet build -c Release
  popd >/dev/null
  echo "Build complete: build_out/BloodLabMod.dll"
  exit 0
fi

echo "No dotnet found; please install .NET SDK or provide UNITY_DLL and MELON_DLL and use an alternative compiler."
exit 1
