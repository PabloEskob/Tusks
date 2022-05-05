
class Weapon
{
    private int _damage;
    private int _bullets;

    public int Damage => _damage;
    public int Bullets => _bullets;

    public void TryToShoot(Player player)
    {
        if (_bullets > 0)
        {
            player.TakeDamage(Damage);
            _bullets -= 1;
        }
    }
}

class Player
{
    private int _health;
    private bool _isDead;

    public int Health => _health;

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
    }
}

class Bot
{
    private Weapon _weapon;

    public void OnSeePlayer(Player player)
    {
        _weapon.TryToShoot(player);
    }
}