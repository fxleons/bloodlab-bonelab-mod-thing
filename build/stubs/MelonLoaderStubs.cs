using System;

namespace MelonLoader
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class MelonInfoAttribute : Attribute
    {
        public MelonInfoAttribute(Type t, string name, string version, string author) { }
    }

    public class MelonMod
    {
        public virtual void OnApplicationStart() { }
        public virtual void OnUpdate() { }
    }

    public static class MelonLogger
    {
        public static void Msg(string s) { Console.WriteLine(s); }
    }
}
