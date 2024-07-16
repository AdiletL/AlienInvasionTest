using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackControl : CharacterAttackControl
{
    public event Action<bool> OnAttack;

    private PlayerController playerMainController;

    [SerializeField] private Transform pullPosition;

    protected IList<IHealth> currentHealthTargets = new List<IHealth>(50);

    private int totalTarget;

    private float pullSpeed;
    private float cooldown;
    private float countCooldown;

    private bool isAttackEnabled;

    protected override Damage CreateDamage()
    {
        return new NormalDamage(DamageType.normal, DamageCalculationType.Fixed, playerMainController.so_CharacterConfig.damage);
    }

    public override void Initialize()
    {
        base.Initialize();

        var playerConfig = (SO_PlayerConfig)playerMainController.so_CharacterConfig;
        pullSpeed = playerConfig.pullSpeed;
        cooldown = playerConfig.attackCooldown;
        totalTarget = playerConfig.totalTarget;
    }
    protected override void SetController()
    {
        base.SetController();
        playerMainController = (PlayerController)iController;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerMainController.GetControl<PlayerFOVControl>().onFOVEnter += OnFOVEnter;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        playerMainController.GetControl<PlayerFOVControl>().onFOVEnter -= OnFOVEnter;
    }

    protected override void OnSwitchController(CharacterStateType state)
    {
        isAttackEnabled = state.HasFlag(CharacterStateType.attack) ? true : false;
    }

    public override void Attack()
    {
        if (!isAttackEnabled || currentHealthTargets.Count == 0)
        {
            countCooldown = 0;
            return;
        }

        countCooldown += Time.deltaTime;

        if (cooldown <= countCooldown)
        {
            for (int i = currentHealthTargets.Count - 1; i >= 0; i--)
            {
                if (currentHealthTargets[i] == null || currentHealthTargets[i].GetCurrentHealth() <= 0) continue;
                ApplyDamage(currentHealthTargets[i]);
            }

            OnAttack?.Invoke(true);
            countCooldown = 0;
        }
    }

    public override void ApplyDamage(IHealth health)
    {
        health.TakeDamage(damage);
        if (health.GetCurrentHealth() <= 0)
        {
            var enemy = health as EnemyHealthControl;
            StartCoroutine(PullCharacterCoroutine((EnemyController)enemy.iController));
            currentHealthTargets.Remove(health);
        }
    }
    private void OnFOVEnter(List<IGameObjectController> controller)
    {
        var healthes = new List<IHealth>();
        for (int i = controller.Count - 1; i >= 0; i--)
        {
            if (controller[i] == null) continue;
            healthes.Add(controller[i].GetControl<IHealth>());
        }

        currentHealthTargets = currentHealthTargets.Intersect(healthes).ToList();
        currentHealthTargets = currentHealthTargets.Take(totalTarget).ToList();
        int countController = currentHealthTargets.Count;
        if (countController >= totalTarget) return;

        for (int i = controller.Count - 1; i >= 0; i--)
        {
            if (countController > totalTarget) return;
            else countController++;

            if (controller[i] == null) continue;
            currentHealthTargets.Add(controller[i].GetControl<IHealth>());
        }

        if (currentHealthTargets.Count == 0) OnAttack?.Invoke(false);
    }
    private IEnumerator PullCharacterCoroutine(EnemyController target)
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
