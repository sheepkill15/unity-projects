using System;
using System.Collections.Generic;
using System.Reflection;

public class EffectHandler
{
    private Card _owner;

    private readonly Dictionary<EventInfo, List<Action<Command>>> _addedHandlers =
        new Dictionary<EventInfo, List<Action<Command>>>();

    public EffectHandler(Card owner, params Effect[] effects)
    {
        _owner = owner;
        foreach (Effect effect in effects)
        {
            foreach (Effect.CommandInfo commandInfo in effect.commandsToExecute)
            {
                ConstructorInfo[] constructorInfoOfCommand = commandInfo.executeCommand.Type.GetConstructors();
                Command commandToExecute;
                if (constructorInfoOfCommand[0].GetParameters().Length > 1)
                {
                    commandToExecute = (Command)Activator.CreateInstance(commandInfo.executeCommand, 
                        owner, commandInfo.data);
                }
                else if(constructorInfoOfCommand[0].GetParameters().Length == 1)
                {
                    commandToExecute = (Command)Activator.CreateInstance(commandInfo.executeCommand, args: owner);
                }
                else
                {
                    commandToExecute = (Command)Activator.CreateInstance(commandInfo.executeCommand);
                }
                if (commandInfo.triggerOnCommand.Type is null)
                {
                    GameManager.CommandQueue.Enqueue(commandToExecute);
                    continue;
                }

                EventInfo callbacks =
                    commandInfo.triggerOnCommand.Type?.GetEvent("Callbacks",
                        BindingFlags.Public | BindingFlags.Static);
                if (callbacks is null) continue;
                Command execute = commandToExecute;

                void Handler(Command command)
                {
                    GameManager.CommandQueue.Enqueue(execute);
                }

                callbacks.AddEventHandler(null, (Action<Command>) Handler);
                if (!_addedHandlers.ContainsKey(callbacks))
                {
                    _addedHandlers.Add(callbacks, new List<Action<Command>>());
                }
                _addedHandlers[callbacks].Add(Handler);
            }
        }
    }

    public void Cleanup()
    {
        foreach (var addedHandler in _addedHandlers)
        {
            foreach (Action<Command> action in addedHandler.Value) 
                addedHandler.Key.RemoveEventHandler(null, action);
        }
    }
}
