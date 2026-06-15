#!/usr/bin/env bash
set -euo pipefail

# build/build.sh
# Usage: set UNITY_DLL and MELON_DLL env vars to point to UnityEngine.dll and MelonLoader.dll
# Example:
# UNITY_DLL="/path/to/UnityEngine.dll" MELON_DLL="/path/to/MelonLoader.dll" ./build/build.sh

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BUILD_OUT_DIR="$SCRIPT_DIR/build_out"
ROOT_OUT_DIR="$SCRIPT_DIR/../build_out"
mkdir -p "$BUILD_OUT_DIR"
mkdir -p "$ROOT_OUT_DIR"

if command -v dotnet >/dev/null 2>&1; then
  echo "Building with dotnet"
  pushd "$SCRIPT_DIR" >/dev/null

  if [ -z "${UNITY_DLL:-}" ] && [ -n "${UNITY_DIR:-}" ]; then
    if [ -f "$UNITY_DIR/UnityEngine.dll" ]; then
      UNITY_DLL="$UNITY_DIR/UnityEngine.dll"
    elif [ -f "$UNITY_DIR/net6/UnityEngine.dll" ]; then
      UNITY_DLL="$UNITY_DIR/net6/UnityEngine.dll"
    elif [ -f "$UNITY_DIR/Il2CppAssemblies/UnityEngine.dll" ]; then
      UNITY_DLL="$UNITY_DIR/Il2CppAssemblies/UnityEngine.dll"
    elif [ -f "$UNITY_DIR/Dependencies/Il2CppAssemblyGenerator/Cpp2IL/UnityEngine.dll" ]; then
      UNITY_DLL="$UNITY_DIR/Dependencies/Il2CppAssemblyGenerator/Cpp2IL/UnityEngine.dll"
    elif [ -f "$UNITY_DIR/Dependencies/Il2CppAssemblyGenerator/UnityDependencies/UnityEngine.dll" ]; then
      UNITY_DLL="$UNITY_DIR/Dependencies/Il2CppAssemblyGenerator/UnityDependencies/UnityEngine.dll"
    fi
  fi

  if [ -z "${MELON_DLL:-}" ] && [ -n "${MELON_DIR:-}" ]; then
    if [ -f "$MELON_DIR/net6/MelonLoader.dll" ]; then
      MELON_DLL="$MELON_DIR/net6/MelonLoader.dll"
    elif [ -f "$MELON_DIR/MelonLoader.dll" ]; then
      MELON_DLL="$MELON_DIR/MelonLoader.dll"
    fi
  fi

  if [ -z "${UNITY_REF_DIR:-}" ] && [ -n "${UNITY_DLL:-}" ]; then
    UNITY_REF_DIR="$(cd "$(dirname "$UNITY_DLL")" && pwd)"
  fi

  if [ -z "${UNITY_DLL:-}" ] || [ -z "${MELON_DLL:-}" ]; then
    echo "ERROR: UNITY_DLL and MELON_DLL must be provided for a valid build."
    echo "Usage: UNITY_DLL=\"/path/to/UnityEngine.dll\" MELON_DLL=\"/path/to/MelonLoader.dll\" ./build/build.sh"
    echo "   or UNITY_DIR=\"/path/to/unity/parent\" MELON_DIR=\"/path/to/melonloader\" ./build/build.sh"
    echo "   (MelonLoader.net6/MelonLoader.dll will be used if available.)"
    exit 1
  fi

  if [ -n "${UNITY_REF_DIR:-}" ]; then
    dotnet build -c Release /p:UNITY_DLL="$UNITY_DLL" /p:MELON_DLL="$MELON_DLL" /p:UNITY_REF_DIR="$UNITY_REF_DIR"
  else
    dotnet build -c Release /p:UNITY_DLL="$UNITY_DLL" /p:MELON_DLL="$MELON_DLL"
  fi
  popd >/dev/null

  cp "$BUILD_OUT_DIR/BloodLabMod.dll" "$ROOT_OUT_DIR/BloodLabMod.dll"
  echo "Build complete: $BUILD_OUT_DIR/BloodLabMod.dll"
  echo "Copied build artifact to root: $ROOT_OUT_DIR/BloodLabMod.dll"
  exit 0
fi

echo "No dotnet found; please install .NET SDK or provide UNITY_DLL and MELON_DLL and use an alternative compiler."
exit 1
