using UnityEngine;      // Unityの基本機能を使うための名前空間

#if ENABLE_INPUT_SYSTEM
// 新しいInput Systemが有効になっている場合だけ読み込む
using UnityEngine.InputSystem;

#endif

// このクラスを持つGameObjectにRigidbody2Dが必ず必要になる
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMove : MonoBehaviour
{
    // ----------------------------------------
    // 移動・ジャンプの設定
    // SerializeFieldを付けるとprivateでもInspectorから数値を変更できる
    // ----------------------------------------

    [SerializeField] private float moveSpeed = 5f;
    // プレイヤーの移動速度

    [SerializeField] private float jumpSpeed = 8f;
    // ジャンプするときに与える上方向の速度

    [SerializeField] private Transform groundCheck;
    // 地面に接触しているか調べるための位置
    // プレイヤーの足元に空のGameObjectを配置して登録する

    [SerializeField] private LayerMask groundLayer;
    // 地面として判定するレイヤーを指定する

    [SerializeField] private float groundCheckRadius = 0.15f;
    // 地面を調べる円の半径

    [SerializeField] private float jumpBufferTime = 0.12f;
    // ジャンプ入力を記憶しておく時間（秒）
    // 着地する少し前にジャンプキーを押してもジャンプできるようにする

    // ----------------------------------------
    // 内部で使用する変数
    // ----------------------------------------

    private Rigidbody2D body;
    // プレイヤーの物理演算を管理するRigidbody2Dを保存する

    private float moveInput;
    // 左右の移動入力を保存する
    // -1 = 左、0 = 停止、1 = 右

    private float jumpBufferRemaining;
    // ジャンプ入力をあと何秒覚えているかを保存する


    // ----------------------------------------
    // 初期化処理
    // ----------------------------------------
    void Start()
    {
        // 自分のGameObjectに付いているRigidbody2Dを取得する
        body = GetComponent<Rigidbody2D>();

    }

    // ----------------------------------------
    // 毎フレーム実行される入力処理
    // ----------------------------------------
    void Update()
    {
        // 毎フレーム最初に移動入力を0に戻す
        // 前のフレームの入力が残らないようにする
        moveInput = 0f;

#if ENABLE_INPUT_SYSTEM

        // 新しいInput Systemを使用する場合の処理

        // 現在接続されているキーボードを取得する
        Keyboard keyboard = Keyboard.current;

        // キーボードが存在しない場合はここで処理を終了する
        if (keyboard == null)
        {

            return;

        }

        // Aキーが押されている間、移動入力を-1にする
        if (keyboard.aKey.isPressed) moveInput -= 1f;

        // Dキーが押されている間、移動入力を+1にする
        if (keyboard.dKey.isPressed) moveInput += 1f;

        // Spaceキーが今のフレームで押されたか調べる
        // wasPressedThisFrameは押した瞬間だけtrueになる
        bool jumpPressed = keyboard.spaceKey.wasPressedThisFrame;

        // 古いInput Managerを使用する場合の処理
#else
        // 古いInput Managerを使用する場合の処理
        if (Input.GetKey(KeyCode.A)) moveInput -= 1f;
        // Dキーが押されている間、移動入力を+1にする
        if (Input.GetKey(KeyCode.D)) moveInput += 1f;
        // Spaceキーを押した瞬間だけtrueになる
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

#endif

        // ----------------------------------------
        // ジャンプ入力の記憶
        // ----------------------------------------

        // Spaceを押したら、着地まで短時間だけ入力を覚えておく。
        // Spaceキーが押された瞬間だった場合
        if (jumpPressed)
        {
            // ジャンプ入力を記憶する時間を設定する
            jumpBufferRemaining = jumpBufferTime;

        }

    }

    // ----------------------------------------
    // 物理演算の更新処理
    // ----------------------------------------

    void FixedUpdate()
    {
        // Rigidbody2Dの現在の速度を取得する

#if UNITY_6000_0_OR_NEWER

        // Unity 6以降ではlinearVelocityを使用する
        Vector2 velocity = body.linearVelocity;

        // Unity 6より前ではvelocityを使用する
#else

        Vector2 velocity = body.velocity;

#endif

        // ----------------------------------------
        // 左右の移動
        // ----------------------------------------

        // X方向の速度を移動入力 × 移動速度にする
        // moveInputが-1なら左、1なら右、0なら停止
        velocity.x = moveInput * moveSpeed;

        // ----------------------------------------
        // ジャンプ処理
        // ----------------------------------------

        // ジャンプ入力の記憶時間が残っているか確認する
        if (jumpBufferRemaining > 0f)
        {
            // 地面に接触しているか調べる
            // groundCheckが設定されていることも確認する
            bool isGrounded = groundCheck != null &&

                // groundCheckの位置を中心に円形の判定を行う
                Physics2D.OverlapCircle(
                    // 円の中心位置,円の半径,地面として判定するレイヤー
                    groundCheck.position, groundCheckRadius, groundLayer) != null;

            // 地面に接触していて、下向きに落下していない場合
            if (isGrounded && velocity.y <= 0.1f)
            {
                // Y方向の速度をジャンプ速度に変更する
                velocity.y = jumpSpeed;
                // ジャンプ入力の記憶をリセットする
                jumpBufferRemaining = 0f;
            }
            else
            {
                // まだジャンプできない場合は記憶時間を減らす
                // Time.fixedDeltaTimeは物理演算1回分の経過時間
                jumpBufferRemaining -= Time.fixedDeltaTime;

            }

        }

        // ----------------------------------------
        // 速度の反映
        // ----------------------------------------
#if UNITY_6000_0_OR_NEWER

        // Unity 6以降ではlinearVelocityに速度を設定する
        body.linearVelocity = velocity;

        // Unity 6より前ではvelocityに速度を設定する
#else

        body.velocity = velocity;

#endif

    }

}
