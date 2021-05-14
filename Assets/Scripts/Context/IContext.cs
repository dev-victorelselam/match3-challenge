using Controllers;
using Controllers.Game;
using UnityEngine;

namespace Context
{
    public interface IContext
    {
        Environment Environment { get; }
        GameSettings GameSettings { get; }
        GameController GameController { get; }
    }
}