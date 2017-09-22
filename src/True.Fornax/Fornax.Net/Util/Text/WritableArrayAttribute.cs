using System;

namespace Fornax.Net.Util.Text
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    internal sealed class WritableArrayAttribute : Attribute
    {
        internal WritableArrayAttribute() {
        }
    }
}