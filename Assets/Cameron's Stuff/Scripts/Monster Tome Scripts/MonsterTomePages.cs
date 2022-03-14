using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterTomePages : MonoBehaviour
{
    [Header("Left Page")]
    [SerializeField]
    private string LeftMonsterName, LeftMonsterDesc;

    [SerializeField]
    private Sprite LeftMonsterImg;

    [Header("Right Page")]
    [SerializeField]
    private string RightMonsterName, RightMonsterDesc;

    [SerializeField]
    private Sprite RightMonsterImg;

    [Header("Drag Components Here")]
    [SerializeField]
    private TextMeshProUGUI leftMonsterNameTMP, leftMonsterDescTMP, rightMonsterNameTMP, rightMonsterDescTMP;

    [SerializeField]
    private Image leftMonsterPic, rightMonsterPic;

    // Start is called before the first frame update
    void Start()
    {
        //sets the monsters names
        leftMonsterNameTMP.text = LeftMonsterName;
        rightMonsterNameTMP.text = RightMonsterName;

        //sets the monsters desc
        leftMonsterDescTMP.text = LeftMonsterDesc;
        rightMonsterDescTMP.text = RightMonsterDesc;

        leftMonsterPic.sprite = LeftMonsterImg;
        rightMonsterPic.sprite = RightMonsterImg;
    }
}
