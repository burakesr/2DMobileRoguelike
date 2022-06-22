using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
    [SerializeField] private SoundEffectSO doorOpenCloseSoundEffect;
    [SerializeField] private BoxCollider2D doorCollider;
    [HideInInspector] public bool isBossRoomDoor;

    private BoxCollider2D doorTrigger;
    private Animator animator;

    private bool isOpen;
    private bool isPreviouslyOpened;

    private void Awake()
    {
        doorCollider.enabled = false;

        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Settings.playerTag) || collision.CompareTag(Settings.playerWeapon))
        {
            OpenDoor();
        }
    }

    private void OnEnable()
    {
        // When the parent gameobject is disabled (when the player character move far away enough from the room)
        // the animator state gets reset. Therefore we need to restore the animator state.
        animator.SetBool(Settings.open, isOpen);
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            isPreviouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            // setting open parameter in animator
            animator.SetBool(Settings.open, true);

            // play open close sound effect
            if (doorOpenCloseSoundEffect)
            {
                SoundEffectManager.Instance.PlaySoundEffect(doorOpenCloseSoundEffect);
            }
        }
    }

    public void LockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        // settings open false to close the door
        animator.SetBool(Settings.open, false);
    }

    public void UnlockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if (isPreviouslyOpened)
        {
            isOpen = false;
            OpenDoor();
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
    }
#endif
    #endregion
}