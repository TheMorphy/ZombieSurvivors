using UnityEngine;

public abstract class Tab : MonoBehaviour
{
    public abstract void Initialize();

	public abstract void InitializeWithArgs(object[] args);

	public virtual void Hide() => gameObject.SetActive(false);

	public virtual void Show() => gameObject.SetActive(true);
}
