using UnityEngine;
using UnityEngine.Events;


public class SpriteManager: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator anim;
    public UnityEvent startIdleEvent;
    public UnityEvent startWalkEvent;
    public UnityEvent startRunEvent;

    private void Start()
    {
        if (!anim) anim = GetComponent<Animator>();
        startIdleEvent.AddListener(startIdleAnimation);
        startWalkEvent.AddListener(startWalkAnimation);
        startRunEvent.AddListener(startRunAnimation);
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
        anim.SetFloat("rotationAngle", transform.parent.transform.eulerAngles.z);
    }

    private void startIdleAnimation()
    {
        anim.SetTrigger("IdleTrigger");
    }

    private void startWalkAnimation()
    {
        anim.SetTrigger("WalkTrigger");
    }

    private void startRunAnimation()
    {
        anim.SetTrigger("RunTrigger");
    }

}
