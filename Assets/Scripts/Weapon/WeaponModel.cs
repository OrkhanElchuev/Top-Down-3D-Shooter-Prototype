using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [SerializeField] private Transform gunPoint;

    public WeaponType weaponType;
    public Transform GunPoint => gunPoint;
}
