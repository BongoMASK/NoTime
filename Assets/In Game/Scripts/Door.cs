using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] Transform _player;
    [SerializeField] float _openDoorDistance;
    void Update()
    {
        Automatic_Door_open();
    }
    void Automatic_Door_open()
    {
        float _distance = Vector3.Distance(_player.position, transform.position);
        {
            if (_distance <= _openDoorDistance)
            {
                _anim.SetBool("_openDoor", true);
            }
            else
            {
                _anim.SetBool("_openDoor", false);
            }

        }
    }
    public void openeDoor()
    {
        _anim.SetBool("_openDoor", true);
    }
    public void closeDoor()
    {
        _anim.SetBool("_openDoor", false);
    }
}
