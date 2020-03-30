using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public HumanBossAnimEvents boss;
    private Slider HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HP.value = boss.bossHP / boss.bossMAXHP;
    }
}
