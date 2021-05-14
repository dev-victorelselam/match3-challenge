using UnityEngine;

namespace Context
{
    public static class ContextProvider
    {
        public static IContext Context { get; private set; }

        public static void Subscribe(IContext context)
        {
            if (Context != null)
            {
                Debug.LogError($"{context.GetType()} trying to override current Context!");
                return;
            }
            
            Context = context;
        }
    }
}