using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    // ----------------------------------------
    // プレイヤーのステータス一覧
    // ----------------------------------------

    // 最大HP
    [SerializeField] private int maxHp = 100;

    // 現在のHP
    [SerializeField] private int currentHp = 100;

    // 攻撃力
    [SerializeField] private int attackPower = 10;

    // 防御力
    [SerializeField] private int defensePower = 5;

    // 素早さ
    [SerializeField] private int speed = 10;

    // 経験値
    [SerializeField] private int experience = 0;

    // レベル
    [SerializeField] private int level = 1;

    // ----------------------------------------
    // 外部から値を取得する
    // ----------------------------------------
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int AttackPower => attackPower;
    public int DefensePower => defensePower;
    public int Speed => speed;
    public int Level => level;
    public int Experience => experience;

    // ----------------------------------------
    // HPを変更する関数
    // ----------------------------------------
    public void SetCurrentHp(int value)
    {
        currentHp = Mathf.Clamp(value, 0, maxHp);
    }
}