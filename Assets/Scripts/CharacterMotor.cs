public class CharacterMotor : IInitializable<TimeContext>
{
    public bool Initialized => _initialized;
    private bool _initialized;

    private TimeContext _timeContext;

    public void Initialize(TimeContext timeContext)
    {
        _timeContext = timeContext;
        _initialized = true;
    }
}