using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentSetting", menuName = "Setting")]
public class EnvironmentSettings : ScriptableObject
{
    public AddressableEnvironment Environment;
}

public enum AddressableEnvironment
{
    Develop,
    Production
}