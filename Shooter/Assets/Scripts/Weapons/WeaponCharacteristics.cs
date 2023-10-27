using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCharacteristics", menuName = "ScriptableObjects/WeaponCharacteristics")]
public class WeaponCharacteristics : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string weaponName;
    [SerializeField] private bool canSprayed;
    [SerializeField] private int[] damage = new int[3];
    [SerializeField] private float firingSpeed;
    [SerializeField] private Vector3 weaponsRecoil;
    [SerializeField] private float snappiness;

    [HideInInspector] public Sprite Sprite { get => sprite; }
    [HideInInspector] public string WeaponName { get => weaponName; }
    [HideInInspector] public bool CanSprayed { get => canSprayed; }
    [HideInInspector] public int[] Damage { get => damage; } 
    [HideInInspector] public float FiringSpeed { get => firingSpeed; }
    [HideInInspector] public Vector3 WeaponsRecoil { get => weaponsRecoil; }
    [HideInInspector] public float Snappiness { get => snappiness; }
}
