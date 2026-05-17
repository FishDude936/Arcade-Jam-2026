using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions m_Actions;
    private InputSystem_Actions.PlayerActions m_Player;
    private PlayerController playerController;
    Vector2 move;
    void Awake()
    {
        m_Actions = GameManager.instance.m_actions;
        m_Player = m_Actions.Player;
        m_Player.Enable();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        move = m_Player.Move.ReadValue<Vector2>();
        if (m_Player.Jump.IsPressed())
        {
            playerController.Jump();
        }
        if (m_Player.Attack.WasPressedThisFrame())
        {
            playerController.StartAttack();
        }
    }
    void FixedUpdate()
    {
        playerController.Move(move);
    }
    void OnDestroy()
    {
        m_Player.Disable();
    }
}
