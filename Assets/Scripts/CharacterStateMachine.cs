public class CharacterStateMachine : IInitializable<CharacterConfig, CharacterMotor>
{
    public bool Initialized => _initialized;
    private bool _initialized;
    public void Initialize(CharacterConfig config, CharacterMotor motor)
    {

    }
}