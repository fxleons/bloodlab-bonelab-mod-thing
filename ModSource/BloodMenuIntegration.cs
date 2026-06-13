using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using UnityEngine;

namespace BloodLabMod.Core
{
    public static class BloodMenuIntegration
    {
        private const string CategoryName = "BloodLab";
        private const string SubmenuName = "Blood Settings";

        private static bool initialized;
        private static bool menuCreated;

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;
            TryCreateMenu();
        }

        public static void Update()
        {
            if (menuCreated) return;
            TryCreateMenu();
        }

        private static void TryCreateMenu()
        {
            try
            {
                var rootCategory = CreateCategory(CategoryName);
                if (rootCategory == null) return;

                var settingsMenu = CreateSubMenu(rootCategory, SubmenuName);
                if (settingsMenu == null) return;

                AddSlider(settingsMenu, "NPC Blood Multiplier", BloodConfig.MinNPCBloodMultiplier, BloodConfig.MaxNPCBloodMultiplier, () => BloodConfig.NPCBloodMultiplier, v => BloodConfig.NPCBloodMultiplier = v);
                AddSlider(settingsMenu, "Droplet Size", BloodConfig.MinDropletSize, BloodConfig.MaxDropletSize, () => BloodConfig.DropletSize, v => BloodConfig.DropletSize = v);
                AddSlider(settingsMenu, "Blood Intensity", BloodConfig.MinBloodIntensity, BloodConfig.MaxBloodIntensity, () => BloodConfig.BloodIntensity, v => BloodConfig.BloodIntensity = v);
                AddToggle(settingsMenu, "Enable Dripping", () => BloodConfig.EnableDripping, v => BloodConfig.EnableDripping = v);
                AddToggle(settingsMenu, "Enable Blood Decals", () => BloodConfig.EnableBloodDecals, v => BloodConfig.EnableBloodDecals = v);

                menuCreated = true;
                MelonLogger.Msg("BloodLab BoneMenu integration initialized.");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"BloodLab BoneMenu integration failed: {ex.Message}");
            }
        }

        private static object? CreateCategory(string title)
        {
            var targetMethods = new[] { "CreateCategory", "CreateMenuCategory", "Create", "AddCategory" };
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyName = assembly.FullName;
                if (assemblyName == null || (!assemblyName.Contains("Bone", StringComparison.OrdinalIgnoreCase) && !assemblyName.Contains("Menu", StringComparison.OrdinalIgnoreCase)))
                    continue;

                foreach (var type in GetLoadableTypes(assembly))
                {
                    foreach (var methodName in targetMethods)
                    {
                        var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                        if (method == null) continue;

                        var parameters = method.GetParameters();
                        if (parameters.Length != 1 || parameters[0].ParameterType != typeof(string))
                            continue;

                        var category = method.Invoke(null, new object[] { title });
                        if (category != null) return category;
                    }
                }
            }

            return null;
        }

        private static object? CreateSubMenu(object parent, string title)
        {
            var methodNames = new[] { "AddSubMenu", "CreateSubMenu", "AddMenu", "CreateMenu" };
            return InvokeParentMethod(parent, methodNames, new object?[] { title });
        }

        private static bool AddSlider(object parent, string label, float min, float max, Func<float> getter, Action<float> setter)
        {
            var candidateArguments = new[]
            {
                new object[] { label, min, max, getter, setter },
                new object[] { label, min, max, getter(), setter },
                new object?[] { label, min, max, getter(), setter, (object?)null },
                new object[] { label, getter, setter },
                new object[] { label, getter(), setter },
            };

            var methodNames = new[] { "AddSlider", "CreateSlider", "AddFloat", "CreateFloat", "AddRange", "CreateRange" };
            foreach (var args in candidateArguments)
            {
                if (TryInvokeParentMethod(parent, methodNames, args))
                    return true;
            }

            MelonLogger.Msg($"BloodLab BoneMenu: unable to create slider '{label}'.");
            return false;
        }

        private static bool AddToggle(object parent, string label, Func<bool> getter, Action<bool> setter)
        {
            var candidateArguments = new[]
            {
                new object[] { label, getter, setter },
                new object[] { label, getter(), setter },
                new object?[] { label, getter, setter, (object?)null },
                new object?[] { label, getter(), setter, (object?)null },
            };

            var methodNames = new[] { "AddToggle", "CreateToggle", "AddBool", "CreateBool", "AddCheckbox", "CreateCheckbox", "AddBoolean", "CreateBoolean" };
            foreach (var args in candidateArguments)
            {
                if (TryInvokeParentMethod(parent, methodNames, args))
                    return true;
            }

            MelonLogger.Msg($"BloodLab BoneMenu: unable to create toggle '{label}'.");
            return false;
        }

        private static bool TryInvokeParentMethod(object parent, string[] methods, object?[] args)
        {
            if (parent == null) return false;
            var parentType = parent.GetType();

            foreach (var methodName in methods)
            {
                var methodCandidates = parentType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                foreach (var method in methodCandidates)
                {
                    if (!ParametersMatch(method.GetParameters(), args))
                        continue;

                    try
                    {
                        method.Invoke(parent, args);
                        return true;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return false;
        }

        private static object? InvokeParentMethod(object parent, string[] methods, object?[] args)
        {
            if (parent == null) return null;
            var parentType = parent.GetType();

            foreach (var methodName in methods)
            {
                var methodCandidates = parentType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));

                foreach (var method in methodCandidates)
                {
                    if (!ParametersMatch(method.GetParameters(), args))
                        continue;

                    try
                    {
                        var result = method.Invoke(parent, args);
                        if (result != null) return result;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        private static bool ParametersMatch(ParameterInfo[] parameters, object?[] args)
        {
            if (parameters.Length != args.Length) return false;

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;
                var arg = args[i];

                if (arg == null)
                {
                    if (parameterType.IsValueType && Nullable.GetUnderlyingType(parameterType) == null)
                        return false;
                }
                else
                {
                    var argType = arg.GetType();
                    if (parameterType.IsAssignableFrom(argType))
                        continue;

                    if (parameterType == typeof(float) && argType == typeof(double))
                        continue;

                    if (parameterType == typeof(double) && argType == typeof(float))
                        continue;

                    if (parameterType == typeof(object))
                        continue;

                    return false;
                }
            }

            return true;
        }

        private static Type[] GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).Cast<Type>().ToArray();
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}
