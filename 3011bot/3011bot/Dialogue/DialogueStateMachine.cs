﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace bot
{
    internal class DialogueStateMachine : IDialogue
    {
        public DialogueStatus Update(SocketMessage msg)
        {
            // This is here in case the dialogue does not get terminated
            // right after finishing/encountering an error.
            if (_currentState == "error" || _currentState == "final") return DialogueStatus.Error;

            _currentState = _transitionFunc[_currentState](_currentState, msg);

            if (_currentState == "final") return DialogueStatus.Continue;
            if (_currentState == "error") return DialogueStatus.Error;

            return DialogueStatus.Continue;
        }

        public void AddTransition(string startState, Func<string, SocketMessage, string> transition)
        {
            _transitionFunc[startState] = transition;
        }

        string _currentState = "start";
        Dictionary<string, Func<string, SocketMessage, string>> _transitionFunc = new();
    }
}
