using UnityEngine;

public abstract class SingletonMB<T> : MonoBehaviour where T:SingletonMB<T>
{
    private static T _instance;
    public static T Instance => GetInstance();

    public bool isPersistent;

    private void Awake()
    {
        if (TryInitialize() == false)
        {
            Debug.LogErrorFormat(this, "An instance of <b><color=red>{0}</color></b> already exists on the scene.\r\n" +
                                       "The new instance was automatically destroyed.", typeof(T));
            Destroy(this);
            return;
        }
        if (isPersistent)
            DontDestroyOnLoad(this);

        Initialize();
    }

    protected abstract void Initialize();

    private bool TryInitialize()
    {
        if(!_instance)
        {
            _instance = (T) this;
        }
        return ReferenceEquals(_instance, this);
    }

    private static T GetInstance()
    {
        if(!_instance)
        {
            var type = typeof(T);
            _instance = new GameObject(type.ToString(), type).GetComponent(type) as T;
        }
        return _instance;
    }
/*
    private void OnValidate()
    {
        if (TryInitialize() == false)
        {
            Debug.LogErrorFormat(this, "An instance of <b><color=red>{0}</color></b> named <b><color=blue>{1}</color></b> already exists on scene <b><color=blue>{2}</color></b>.\r\n" +
                                       "Superfluous instance <b><color=blue>{3}</color></b> on scene <b><color=blue>{4}</color></b>\r\n"+
                                       "Please remove the superfluous instance", typeof(T), _instance.name, _instance.gameObject.scene.name, gameObject.name, gameObject.scene.name);
        }
    }
*/

    private void OnDisable()
    {
        Cleanup();
        if (ReferenceEquals(this, _instance))
            _instance = null;
    }
    protected abstract void Cleanup();
}
