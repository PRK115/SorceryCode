using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeUI;
using UnityEngine.UI;

public class CommandBlockSpawn : BlockSpawn, IBeginDragHandler {

    [SerializeField] Block commandBlockPrefab;

    private new void Awake()
    {
        base.Awake();
        IsMovable = false;
        //blockListPanel = FindObjectOfType<CommandListBlock>().blockListPanel;
    }

    public new void OnBeginDrag(PointerEventData eventData)
    {
        spawnedBlock = Instantiate(commandBlockPrefab, transform.position, Quaternion.identity);

        spawnedBlock.OnPointerDown(eventData);
        spawnedBlock.OnBeginDrag(eventData);
    }
}
