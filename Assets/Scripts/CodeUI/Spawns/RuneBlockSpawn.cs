using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeUI;
using UnityEngine.EventSystems;

public class RuneBlockSawn : BlockSpawn, IBeginDragHandler {

    //[SerializeField] private RuneType runeType;

    [SerializeField] private EntityBlock entityBlockPrefab;
    [SerializeField] private ChangeTypeBlock changeTypeBlockPrefab;
    [SerializeField] private MoveDirBlock moveDirBlockPrefab;

    Image blockListPanel;

    int number;

    private new void Awake()
    {
        base.Awake();
        blockListPanel = FindObjectOfType<RuneListBlock>().blockListPanel;
    }

    public new void OnBeginDrag(PointerEventData eventData)
    {
        Block runeBlock = null;
        if (runeType.type == RuneType.Type.Entity)
        {
            EntityBlock block = Instantiate(entityBlockPrefab, transform.position, Quaternion.identity);
            block.EntityType = runeType.Entity;
            runeBlock = block;
        }
        else if (runeType.type == RuneType.Type.Adjective)
        {
            ChangeTypeBlock block = Instantiate(changeTypeBlockPrefab);
            block.ChangeType = runeType.adjective;
            runeBlock = block;
        }
        else if (runeType.type == RuneType.Type.Direction)
        {
            MoveDirBlock block = Instantiate(moveDirBlockPrefab);
            block.Dir = runeType.direction;
            runeBlock = block;
        }
        runeBlock.transform.SetParent(blockListPanel.transform);
        //Blocks.Add(runeBlock);
        //runeBlock.SetRuneCount(runeCount);
    }
}
