using UnityEngine;
using UnityEngine.InputSystem;
using SlimeRPG.Gameplay.Character;


namespace SlimeRPG.Manager
{
    public class InputManager : MonoBehaviour
    {
        private PlayerCharacter player;
        private PlayerInput playerInput;

        private void Start()
        {
            player = GameManager.Instance.GetPlayerCharacter(0);

        }
    }
}
