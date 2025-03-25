using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol //hereda
{
    // Start is called before the first frame update
    void Start()
{
    //El retraso entre disparos (puede definir el valor que desee)
    cooldown = 0.2f;
    //Esta arma dispara en modo automático; seguirá disparando mientras mantengamos el botón del mouse (no se preocupe: ¡el retraso que definió antes se tendrá en cuenta!
    auto = true;
    ammoCurrent=30;
    ammoMax=30;
    ammoBackPack=60;
}

    
}
