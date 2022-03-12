using System;
using TypeReferences;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect", fileName = "Effect", order = 0)]
public class Effect : ScriptableObject
{
    [Serializable]
    public struct CommandInfo
    {
        public int data;
        [Inherits(typeof(Command))]
        public TypeReference executeCommand;
        [Inherits(typeof(Command))]
        public TypeReference triggerOnCommand;
    }
    public string displayText = "Basic effect";
    public CommandInfo[] commandsToExecute;
}
