using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferenceFromGameManager : MonoBehaviour
{
    [SerializeField] DropdownUnityEventGameObject onGameObjectSend;
    [SerializeField] DropdownUnityEventTransform onTransformSend;
    [SerializeField] DropdownUnityEventVector3 onPositionSend;

    void LateUpdate()
    {
        var player = GameManager.Instance.player;
        if (player == null) return;

        Transform playerTransform = player.transform;

        onGameObjectSend.Invoke(playerTransform.gameObject);
        onTransformSend.Invoke(playerTransform);
        onPositionSend.Invoke(playerTransform.position);
    }
}
