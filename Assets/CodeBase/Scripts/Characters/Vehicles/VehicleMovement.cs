using UnityEngine;

namespace CodeBase.Scripts.Characters.Vehicles
{
    public class VehicleMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float acceleration = 15f;
        [SerializeField] private float maxSpeed = 20f;

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > maxSpeed)
            {
                Vector3 clampedVelocity = horizontalVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
            }
        }
    }
}
