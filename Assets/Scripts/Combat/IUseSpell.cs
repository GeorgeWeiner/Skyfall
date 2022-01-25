using UnityEngine;

namespace Combat
{
    public interface IUseSpell
    {
        void CastSpell(UseSpell useSpell, Transform instantiationLocation);
    }
}