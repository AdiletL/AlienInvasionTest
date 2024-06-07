using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackControl : CharacterAttackControl
{
    private PlayerMainController playerMainController;

    [SerializeField] private Transform pullPosition;

    private float pullSpeed;

    public override void Initialize(IController controller)
    {
        base.Initialize(controller);

        playerMainController = controller as PlayerMainController;
        var playerConfig = (SO_PlayerConfig)playerMainController.so_CharacterConfig;
        pullSpeed = playerConfig.pullSpeed;
    }


    public override void ApplyDamage(IHealth health)
    {
        base.ApplyDamage(health);
        if (health.GetHealth() <= 0)
        {
            var enemy = health as EnemyHealthControl;
            StartCoroutine(PullCharacterCoroutine((CharacterMainController)enemy.iController));
            currentHealthTargets.Remove(health);
        }
    }

    private IEnumerator PullCharacterCoroutine(CharacterMainController target)
    {
        var distance = Vector3.Distance(target.transform.position, pullPosition.position);
        while (distance > .15f)
        {
            yield return null;
            target.transform.position = Vector3.MoveTowards(target.transform.position, pullPosition.position, pullSpeed * Time.deltaTime);
            distance = Vector3.Distance(target.transform.position, pullPosition.position);
        }
        
        yield return new WaitForEndOfFrame();
        target.ReturnToPool();
    }
}
