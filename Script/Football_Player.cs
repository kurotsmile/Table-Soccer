using UnityEngine;
using UnityEngine.UI;

public class Football_Player : MonoBehaviour
{
    [Header("Obj Football")]
    public Image img_avatar;
    public string s_id;
    public string s_name;
    public GameObject[] block_shoot_arow;
    public int ball_force;
    public int ball_control;
    public int ball_cutting;
    public int playing_position;

    [Header("Obj Item List")]
    public Text txt_name;
    public Text txt_tip;
    public GameObject button_buy;
    public bool is_free = true;

    public void hide_block_shoot_arrow()
    {
        for (int i = 0; i < block_shoot_arow.Length; i++) this.block_shoot_arow[i].SetActive(false);
    }

    public void click()
    {
        GameObject.Find("Game").GetComponent<Game>().manager_play.show_change_player(this);
    }

    public void mode_two_player()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 180f);
    }

    public void mode_one_player()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0f);
    }

    public void select_change_player()
    {
        GameObject.Find("Game").GetComponent<Game>().play_sound(1);
        GameObject.Find("Game").GetComponent<Game>().manager_play.show_change_player_in(this);
    }
}
