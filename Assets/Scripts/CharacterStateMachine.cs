public class CharacterStateMachine : Initializable
{
    private bool _initialized;

    [Inject]
    private TimeContext _timeContext;

    [Inject]
    private CharacterConfig _config;

    [Inject]
    private CharacterMotor _motor;

}