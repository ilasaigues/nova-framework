using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Character Config")]
public class CharacterConfig : ScriptableObject
{

    public float horizontalSpeed = 5;
    public float horizontalAcceleration = 10;
    public float jumpSpeed = 7;
}