using System;
using UniRx;
using UnityEngine.InputSystem;

namespace Extensions.UniRx
{
    public static class UnityInputSystemExtensions
    {
        public static IObservable<InputAction.CallbackContext> AsObservable(this InputAction action)
        {
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => action.performed += h,
                h => action.performed -= h);
        }
    }
}