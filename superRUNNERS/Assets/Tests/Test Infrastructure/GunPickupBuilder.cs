using UnityEngine;

//Builds a game object with GunPickup and relevant scripts for pickup/throw

    public class GunPickupBuilder
    {
        private bool isGunFireEnabled;
        private bool isGunBreakEnabled;
        private bool isRbEnabled;
        private bool isColEnabled;
        private bool isDamageColEnabled;
        private bool isDamageRbEnabled;

        public GunPickupBuilder WithGunBreakEnabled(bool isEnabled)
        {
            isGunFireEnabled = isEnabled;
            return this;
        }
        //lol

        public GunPickupBuilder WithGunFireEnabled(bool isEnabled)
        {
            isGunBreakEnabled = isEnabled;
            return this;
        }

        public GunPickupBuilder WithDamageColEnabled(bool isEnabled)
        {
            isDamageColEnabled = isEnabled;
            return this;
        }

        public GunPickupBuilder WithDamageRbEnabled(bool isEnabled)
        {
            isDamageRbEnabled = isEnabled;
            return this;
        }

        public GunPickupBuilder WithColEnabled(bool isEnabled)
        {
            isColEnabled = isEnabled;
            return this;
        }

        public GunPickupBuilder WithRbEnabled(bool isEnabled)
        {
            isRbEnabled = isEnabled;
            return this;
        }

        public GameObject Build()
        {
            GameObject gun = new GameObject();
            GunPickup gunPickup = gun.AddComponent<GunPickup>();
            GunFire gunFire = gun.AddComponent<GunFire>();
            GunBreak gunBreak = gun.AddComponent<GunBreak>();

            Rigidbody rb = gun.AddComponent<Rigidbody>();
            BoxCollider col = gun.AddComponent<BoxCollider>();

            gunFire.enabled = isGunFireEnabled;
            gunBreak.enabled = isGunBreakEnabled;
            rb.isKinematic = isRbEnabled;
            col.isTrigger = isColEnabled;
/*
            damageRb.enabled = isdamageRbEnabled;
            damageCol.enabled = isDamageColEnabled;
            
            */
            
            return gun;
        }
    }
