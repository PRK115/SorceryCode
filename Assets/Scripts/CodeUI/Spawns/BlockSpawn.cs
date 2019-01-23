using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeUI;
using UnityEngine.UI;

public class BlockSpawn : Block, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Block blockPrefab;

    private new void Awake()
    {
        base.Awake();
        //blockListPanel = FindObjectOfType<CommandListBlock>().blockListPanel;
    }

    //private void Update()
    //{
    //    if (name == "ConjureBlockSpawn") 
    //    Debug.Log(spawnedBlock);
    //}

    //private void Update()
    //{
    //    if(gameObject.name == "ConjureBlockSpawn")
    //    Debug.Log(spawnedBlock);
    //}

    protected Block spawnedBlock;

    public new void OnBeginDrag(PointerEventData eventData)
    {
        spawnedBlock = Instantiate(blockPrefab, transform.position, Quaternion.identity);

        spawnedBlock.OnPointerDown(eventData);
        spawnedBlock.OnBeginDrag(eventData);
    }
    public new void OnDrag(PointerEventData eventData)
    {
        spawnedBlock.OnDrag(eventData);

    }

    public new void OnEndDrag(PointerEventData eventData)
    {
        if (!IsMovable)
            spawnedBlock.OnEndDrag(eventData);
        else
            base.OnEndDrag(eventData);
    }
}
