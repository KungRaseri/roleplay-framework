﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenRP.Framework.Common.Classes
{
    internal sealed class ServerCommand
    {
        public MulticastDelegate Command { get; internal set; }
        string _help;
        List<string> _args;

        internal ServerCommand(MulticastDelegate command, string help, List<string> args)
        {
            Command = command;
            _help = help;
            _args = args;
        }

        internal static 

        string GenerateEventName(Enum eventName)
        {
            var assembly = eventName.GetType().Assembly;
            var caller = assembly.FullName.Split(',')[0];
            var name = $"{caller}:{eventName}";
            return name;
        }
    }
}
