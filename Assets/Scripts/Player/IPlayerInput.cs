using System;

public interface IPlayerInput
{
    float Vertical { get; }
    float Horizontal { get; }
    
    bool Attack1 { get; }
    bool Attack2 { get; }
    bool Attack3 { get; }
    
    bool Interact { get; }
    bool PausePressed { get; }

}