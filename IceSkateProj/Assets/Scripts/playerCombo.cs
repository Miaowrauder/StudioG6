using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombo : MonoBehaviour
{
    private GameObject player;
    private PlayerCombat plC;
    private playerMovement plM;
    private SpriteRenderer mySprite;
    public int comboCount;
    public GameObject[] comboVisuals;
    public Sprite trickSprite;
    private Sprite lastSprite;
    public float trickDur, trickCooldown;
    public bool isTricking;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        plC = player.GetComponent<PlayerCombat>();
        plM = player.GetComponent<playerMovement>();
        mySprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTricking && Input.GetKeyDown(KeyCode.Mouse2) && (plM.isTrailing == false))
        {
            ComboSpend(1);
            isTricking = true;
            ComboVisual();
            Invoke("TrickCD", trickDur + trickCooldown);
        }
    }
    private void TrickCD()
    {
        isTricking = false;
    }

    public void ComboSpend(int amountChanged)
    {
        comboCount += amountChanged;

        if(comboCount < 0)
        {
            comboCount = 0;
        }
        else if(comboCount > 5)
        {
            comboCount = 5;
        }

        Instantiate(comboVisuals[comboCount], transform.position, Quaternion.identity);
    }

    private void ComboVisual()
    {
        lastSprite = mySprite.sprite;
        mySprite.sprite = trickSprite;
        plM.maxSpeed += 5f;
        plM.diagMaxSpeed += 3.4f;
        plM.moveSpeed += 2f;

        Invoke("RevertVisual", trickDur);
    }

    private void RevertVisual()
    {
        plM.maxSpeed -= 5f;
        plM.diagMaxSpeed -= 3.4f;
        plM.moveSpeed -= 2f;
        mySprite.sprite = lastSprite;
    }
}
