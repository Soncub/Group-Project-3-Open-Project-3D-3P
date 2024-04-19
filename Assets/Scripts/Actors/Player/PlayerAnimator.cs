using UnityEngine;
using Vector3Helper;

public class PlayerAnimator : MonoBehaviour
{
    //Config



    //Components
    private Animator animator;
    private PlayerMove movement;
    private PlayerShooter shooter;
    private PlayerMelee melee;
    private PlayerCooking cooking;
    private Transform movementTransform;

    //Data

    //Animation Parameters
    private float p_walkX { set => animator.SetFloat("walkX", value); }
    private float p_walkZ { set => animator.SetFloat("walkZ", value); }
    public bool p_inAir { set => animator.SetBool("InAir", value); }

    public void Dodge() => animator.SetTrigger("Dodge");
    public void Melee() => animator.SetTrigger("Melee");
    public void Throw() => animator.SetTrigger("Throw");
    public void Jump() => animator.SetTrigger("Jump");
    public void Land() => animator.SetTrigger("Land");
    public void Cook() => animator.SetTrigger("Cook");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMove>();
        shooter = GetComponentInParent<PlayerShooter>();
        melee = GetComponentInParent<PlayerMelee>();
        cooking = GetComponentInParent<PlayerCooking>();
        movementTransform = movement.transform;
    }

    private void Update() => SetDirection();

    private void SetDirection()
    {

        Vector3 result = movementTransform.worldToLocalMatrix * (Vector3)(movement.velocity * Direction.XZ / (movement.speed));

        p_walkX = (result.x.Abs() > 0.2) ? result.x : 0;
        p_walkZ = (result.z.Abs() > 0.2) ? result.z : 0;

        //Neither of these worked and I honestly have no clue why
        //Vector3 result = ((Direction)moveDirection).RotateTo(aimDirection, Direction.front);
        //Direction result = Quaternion.LookRotation(aimDirection, transform.up) * moveDirection;
    }

    public void ThrowCallback() => shooter.KnifeCallback();
    public void CookCallback() => cooking.HealFinishCallback();
    public void JumpCallback() => movement.JumpCallback();

}
