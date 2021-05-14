using System;
using Context;
using Environment = Context.Environment;

namespace Controllers.Input
{
    public static class InputFactoring
    {
        public static IInputProvider CreateInstance()
        {
            return ContextProvider.Context.Environment switch
            {
                Environment.Game => new UnityInputProvider(),
                Environment.Test => new MockInputProvider(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}