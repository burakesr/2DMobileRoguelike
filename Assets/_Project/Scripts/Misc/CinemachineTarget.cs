using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;

    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachineTargetGroup();
    }

    private void SetCinemachineTargetGroup()
    {
        CinemachineTargetGroup.Target cinemachineTarget_player = new CinemachineTargetGroup.Target 
        { weight = 1.0f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform };

        //CinemachineTargetGroup.Target cinemachineTarget_cursor = new CinemachineTargetGroup.Target
        //{ weight = 1.0f, radius = 1.0f, target = cursorTarget };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = 
            new CinemachineTargetGroup.Target[] {cinemachineTarget_player, /*cinemachineTarget_cursor*/};

        targetGroup.m_Targets = cinemachineTargetArray; 
    }

    //private void Update()
    //{
    //    cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
    //}
}
