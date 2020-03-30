using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttrubuteUI : MonoBehaviour
{
    public GameObject PlayerObj;
    private PlayerStateAndMovement player;

    private Slider Energy;
    private Slider HP;
    private Slider SAM;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerObj.GetComponent<PlayerStateAndMovement>();
        Slider[] Sliders = gameObject.GetComponentsInChildren<Slider>();
        Energy = Sliders[0];
        HP = Sliders[1];
        SAM = Sliders[2];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        Energy.value = player.Energy/player.MaxEnergy;
        HP.value = player.HP / player.MaxHP;
        SAM.value = player.SAM / player.MaxSAM;
    }
}
