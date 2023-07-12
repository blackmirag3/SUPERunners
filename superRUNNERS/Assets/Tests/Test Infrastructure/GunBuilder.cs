
    public class PlayerGunBuilder
    {
        private int shootForce;
        private int damage;
        private int maxDist;
        private int spread;
        private int magSize;
        private int fireRate;
        private int bulletsPerShot;
        private bool isAuto;

        public PlayerGunBuilder WithShootForce(int val)
        {
            shootForce = val;
            return this;
        }

        public PlayerGunBuilder WithDamage(int val)
        {
            damage = val;
            return this;
        }

        public PlayerGunBuilder WithMaxDist(int val)
        {
            maxDist = val;
            return this;
        }

        public PlayerGunBuilder WithSpread(int val)
        {
            spread = val;
            return this;
        }

        public GunFire Build()
        {
            /*GunFire gun = new GunFire();
            GunFire.shootForce = shootForce;
            GunFire.damage = damage;
            GunFire.maxDist = maxDist;
            GunFire.spread = spread;
            GunFire.magSize = magSize;
            GunFire.fireRate = fireRate;
            GunFire.bulletsPerShot = bulletsPerShot;
            GunFire.isAuto = isAuto;
            return gun;*/
            return null;
        }
    }