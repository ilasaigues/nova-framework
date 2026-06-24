using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
public class InjectAttribute : System.Attribute
{
    public InjectAttribute() { }
}