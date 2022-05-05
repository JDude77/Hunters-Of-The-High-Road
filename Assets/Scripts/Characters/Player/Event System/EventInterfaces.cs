interface IDamageable
{
    public void TakeDamage(float damage);
}//End IDamageable

interface IDamageableWithMelee : IDamageable
{

}//End IDamageableWithMelee

interface IDamageableWithRanged : IDamageable
{

}//End IDamageableWithRanged