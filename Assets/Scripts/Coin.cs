using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public AudioClip coinClip;
    private TextMeshProUGUI coinText;            // since this is a prefab, we must use private instead of public
    public int coinsToGive = 1;

    private void Start()
    {
        coinText = GameObject.FindWithTag("CoinText").GetComponent<TextMeshProUGUI>();     
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.coins += coinsToGive;
            
            player.PlaySFX(coinClip, 0.3f);
            
            coinText.text = player.coins.ToString();    // coins é do tipo int, e por isso, deve ser convertido pra tipo string. 
            
            Destroy(gameObject);
        }
    }
}
