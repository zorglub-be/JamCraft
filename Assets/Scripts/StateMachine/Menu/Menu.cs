using UnityEngine;

public class Menu : IState
{
    private AudioSource _source;

    public Menu(AudioSource musicSource)
    {
        _source = musicSource;
    }

    public void Tick()
    {
         
    }
 
    public void OnEnter()
    {
        if(_source.isPlaying == false)
            _source.Play();
    }
 
    public void OnExit()
    {
         
    }
}