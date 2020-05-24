using UnityEngine;
using System.Net.Http;

public class Battles
{
    private static readonly HttpClient client = new HttpClient();
    public static async void Fight(Troop attacker, Troop defender)
    {
        var responseString = await client.GetStringAsync("https://jsonplaceholder.typicode.com/todos/1");
        Debug.Log(responseString);
        if (Random.Range(0, 6) < 3)
        {
            Debug.Log("Defender damaged!");
            defender.ApplyDamage();
        }
        if (defender.InControlZone(attacker.GetPosition()) && Random.Range(0, 6) < 3)
        {
            Debug.Log("Attacker damaged!");
            attacker.ApplyDamage();
        }
    }
    public static void Collide(Troop troop1, Troop troop2)
    {
        if (Random.Range(0,6) + Random.Range(0,6) == 10)
        {
            Debug.Log("Collision!");
            troop1.ApplyDamage();
            troop2.ApplyDamage();
        }
    }
}
