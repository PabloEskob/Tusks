
    class Weapon
    {
        public int Damage;
        public int Bullets;

        public void Fire(Player player)
        {
            player.Health -= Damage;
            Bullets -= 1;
        }
    }

    class Player
    {
        public int Health;
    }

    class Bot
    {
        public Weapon Weapon;

        public void OnSeePlayer(Player player)
        {
            Weapon.Fire(player);
        }
    }