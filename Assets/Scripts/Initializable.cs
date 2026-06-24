using System;
using System.Linq;

public abstract class Initializable
{
    public bool Initialized { get; private set; }

    public virtual Initializable Initialize(params object[] injected)
    {
        if (Initialized) return this;
        var injectableProps = GetInjectedProps();
        foreach (var prop in injectableProps)
        {
            var injectedValue = injected.FirstOrDefault(o => o != null && prop.PropertyType.IsAssignableFrom(o.GetType()));
            if (injectedValue != null)
            {
                prop.SetValue(this, injectedValue);
            }
            else
            {
                AstralCore.Logger.LogWarning(AstralCore.LogCategory.Initialization, $"Injected value of type {prop.PropertyType} is missing in initialization.");
            }
        }
        Initialized = true;
        return this;
    }

    private System.Reflection.PropertyInfo[] GetInjectedProps()
    {
        return GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute))).ToArray();
    }
}
