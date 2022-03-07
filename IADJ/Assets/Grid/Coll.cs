using System;
using UnityEngine;

namespace Grid
{
    public class Coll:MonoBehaviour
    {
        public bool collisionBool;
        public bool collisionFinihed;
        private void OnCollisionEnter(Collision collision)
        {
            collisionBool = false;
            if (collision.gameObject.CompareTag("Untagged"))
            {
                collisionBool = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                collisionBool = false;
            }
        }
    }
}