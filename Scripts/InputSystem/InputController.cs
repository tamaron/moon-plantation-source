namespace U1W.MoonPlant
{
    public static class InputController
    {
        /// <summary>
        /// InputActionDataを保持します
        /// </summary>
        public static InputActionData Input { get; private set; }
        static InputController()
        {
            Input = new InputActionData();
            Input.Enable();
        }
    }
}
