using System;
using System.Linq;

public abstract class Initializable
{
    public bool Initialized { get; private set; }

    public void Initialize(params object[] injected)
    {
        var injectableProps = GetInjectedProps();
        foreach (var prop in injectableProps)
        {
            var injectedValue = injected.First(o => o.GetType() == prop.PropertyType);
            if (injectedValue != null)
            {
                prop.SetValue(this, injectedValue);
            }
            else
            {
                throw new NullReferenceException($"Injected value of type {prop.PropertyType} is missing in initialization.");
            }
        }
        Initialized = true;
    }

    private System.Reflection.PropertyInfo[] GetInjectedProps()
    {
        return GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(InjectAttribute))).ToArray();
    }
}
