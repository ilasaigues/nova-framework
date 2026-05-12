using UnityEngine;


/// <summary>
/// Scriptable Object used to control separate time contexts that can run at different time scales and be paused separately.
/// This can be used to make the gameplay, ui, menus, and background processes run at different speeds, independent from a global timescale.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Time Context")]
public class TimeContext : ScriptableObject
{
    public float TimeScale
    {
        get
        {
            return _paused ? 0 : _timeScale;
        }
        set
        {
            _timeScale = value;
        }
    }
    [SerializeField]
    private float _timeScale;

    public bool Paused
    {
        get
        {
            return _timeScale == 0 ? true : _paused;
        }
        set
        {
            _paused = value;
        }
    }

    [SerializeField]
    private bool _paused;
    public float DeltaTime => TimeScale * Time.deltaTime;
    public float FixedDeltaTime => TimeScale * Time.fixedDeltaTime;
}
