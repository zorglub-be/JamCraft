using UnityEngine;

public class DestroyableBanner : MonoBehaviour
{
    [SerializeField] private AudioClip bannerSwish;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void Destroy()
    {
        _source.PlayOneShot(bannerSwish, 1f);
        Destroy(this);
    }
}
