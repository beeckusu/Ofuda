using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    public int strength;
    public Slider hpSlider;
    public Slider slowSlider;
    public Movement charMovement;
    public CharacterSpriteController sc;
    public ToggleColliders toggleColliders;
    public Attack attack;

    public int maxHP;
    protected int hp;
    private float spirit;
    protected CHARSTATE charState;

    public AudioSource hurtSound;
    public AudioSource shootSound;


    private void Awake()
    {
        sc.sa.onAnimationComplete += OnAnimationComplete;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHP;
        spirit = 100;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
        SetState(CHARSTATE.IDLE);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (charState == CHARSTATE.ATTACK || charState == CHARSTATE.DIE || charState == CHARSTATE.CAST)
            return;
        Action();
    }

    protected virtual void Action()
    {
    }

    public void Attack()
    {
        SetState(CHARSTATE.ATTACK);
        shootSound.Play();
    }

    public void Attack(Character target)
    {
        target.TakeDamage(strength);
        SetState(CHARSTATE.ATTACK);
    }

    public void Attack(List<Character> targets)
    {
        foreach (Character target in targets)
            target.TakeDamage(strength);
        SetState(CHARSTATE.ATTACK);
    }

    public void Attack(Structure target)
    {
        target.TakeDamage(strength);
        SetState(CHARSTATE.ATTACK);
    }

    public void TakeDamage(int strength)
    {
        hurtSound.Play();
        hp -= strength;
        hpSlider.value = hp;

        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    protected virtual IEnumerator Die()
    {
        SetState(CHARSTATE.DIE);
        charMovement.enabled = false;
        toggleColliders.Disable();
        yield return null;
        if (tag == "Player")
            GameController.PlayerDeath();

    }

    public void PushBack(Character enemy, float strength)
    {
        Vector3 distance = enemy.transform.position - transform.position;
        enemy.transform.position += distance * strength;
    }

    public bool IsAlive()
    {
        return hp > 0;
    }


    public CHARSTATE GetState()
    {
        return charState;
    }

    public void SetState(CHARSTATE newState)
    {
        charState = newState;
        sc.SetState(newState);

    }

    public void Reset()
    {
        SetState(CHARSTATE.IDLE);
        charMovement.Reset();
        toggleColliders.Enable();
        sc.enabled = true;

        hp = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = maxHP;
    }

    public int GetHP()
    {
        return hp;
    }

    private void OnAnimationComplete(SpriteAnimator sa)
    {
        if (charState != CHARSTATE.DIE)
            SetState(CHARSTATE.IDLE);
    }

    public void StartSlow(float slowStrength, float slowDuration)
    {
        StartCoroutine(Slow(slowStrength, slowDuration));
    }
    public void EndSlow()
    {
        slowSlider.value = 0;
    }
    private IEnumerator Slow(float slowStrength, float slowDuration)
    {
        charMovement.MultiplySpeed(slowStrength);
        slowSlider.gameObject.SetActive(true);
        slowSlider.maxValue = slowDuration;
        slowSlider.value = slowSlider.maxValue;

        while(slowSlider.value > 0)
        {

            slowSlider.value -= Time.deltaTime;
            yield return null;
            if (hp <= 0)
                break;
        }

        charMovement.MultiplySpeed(1 / slowStrength);
        slowSlider.gameObject.SetActive(false);
    }

}
