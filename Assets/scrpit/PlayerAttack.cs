using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject weapon;

    void Start()
    {
        WeaponSwing swing = weapon.GetComponent<WeaponSwing>();
    }

    void Update()
    {
        FlipSprite();
    }

    void FlipSprite()
    {
        var mouseX = Input.mousePosition.x;
        bool faceRight = mouseX > Screen.width / 2;
        GetComponent<SpriteRenderer>().flipX = !faceRight;
    }
}
