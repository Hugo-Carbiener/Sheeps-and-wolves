using UnityEngine;
using UnityEngine.Events;


public class SpriteManager: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerManager player;
    [SerializeField] private Animator anim;
    public UnityEvent startIdleEvent;
    public UnityEvent startWalkEvent;
    public UnityEvent startRunEvent;

    private void Start()
    {
        if (!player) player = transform.parent.GetComponent<PlayerManager>();
        if (!anim) anim = GetComponent<Animator>();
        startIdleEvent.AddListener(startIdleAnimation);
        startWalkEvent.AddListener(startWalkAnimation);
        startRunEvent.AddListener(startRunAnimation);
    }

    private void Update()
    {
        transform.eulerAngles = Vector3.zero;
        anim.SetFloat("rotationAngle", player.getRotationAngle());
    }

    private void startIdleAnimation()
    {
        Debug.Log("start idle");
        anim.SetTrigger("IdleTrigger");
    }

    private void startWalkAnimation()
    {
        Debug.Log("start walk");
        anim.SetTrigger("WalkTrigger");
    }

    private void startRunAnimation()
    {
        Debug.Log("start run");
        anim.SetTrigger("RunTrigger");
    }

}
