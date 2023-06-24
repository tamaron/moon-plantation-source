#define INPUT_DEBUG
using System.Collections.Generic;
using UnityEngine;
using Project.InputSystem;

namespace U1W.MoonPlant
{
    /// <summary>
    /// GUI描画時にデバッグログを流す
    /// </summary>
    public class GUIDebugger : MonoBehaviour
    {
        public static List<string> Labels = new List<string>();

        private void Update()
        {
            Labels.Clear();
            Labels.Add($"Move: {InputController.Input.Player.Move.ReadValue<UnityEngine.Vector2>().ToString()}");
            Labels.Add($"Fire: {InputController.Input.Player.Fire.phase.ToString()}");
            Labels.Add($"Cancel: {InputController.Input.Player.Cancel.phase.ToString()}");
            Labels.Add($"Sub: {InputController.Input.Player.Sub.phase.ToString()}");
        }

        void OnGUI()
        {
#if INPUT_DEBUG
            foreach (var label in Labels)
            {
                GUILayout.Label(label);
            }
#endif
        }
    }
}
