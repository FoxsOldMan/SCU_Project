using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatController : MonoBehaviour
{
    public PlayerStateAndMovement player;

    private AudioSource heartBeat;
    // Start is called before the first frame update
    void Start()
    {
        heartBeat = GetComponent<AudioSource>();
        heartBeat.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        heartBeat.volume = player.SAM / player.MaxSAM;
    }
}
