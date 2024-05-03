using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Transform tr_player_select;
    private Vector3 pos_start;
    private float speed=2f;
    private bool is_move = false;
    private bool is_follow = false;
    private int player_goal_after = 0;
    public Rigidbody2D rig;
    public Animator anim;

    [Header("Arrow")]
    public GameObject ball_arrow;
    public Transform[] tr_arrow;

    void Start()
    {
        this.ball_arrow.SetActive(false);
        this.pos_start = this.rig.transform.position;
    }

    public void on_Start_Game()
    {
        this.on_Restart_play();
    }

    public void on_Play()
    {
        this.anim.Play("ball");
        this.is_move = true;
    }

    void Update()
    {
        if (this.is_move) this.rig.transform.Translate(Vector3.up * (this.speed * Time.deltaTime));
        if (this.is_follow) this.rig.transform.position = this.tr_player_select.position;
    }

    public void on_Restart_play()
    {
        this.gameObject.SetActive(true);
        this.ball_arrow.SetActive(false);
        this.is_move = false;
        this.is_follow = false;
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.rig.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-360f, 360f));
        this.rig.transform.position = this.pos_start;
        this.anim.Play("ball_ready");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.is_move)
        {
            if (collision.gameObject.name == "football_field_limit")
            {
                this.rig.transform.Rotate(0, 0, Random.Range(90f, 180f));
                GameObject.Find("Game").GetComponent<Game>().play_sound(3);
            }

            if (collision.gameObject.tag == "Player")
            {
                Football_Player fp_cur = collision.gameObject.GetComponent<Football_Player>();
                string s_name_p = collision.transform.parent.name;
                this.transform.position = collision.gameObject.transform.position;
                this.tr_player_select = collision.gameObject.transform;
                this.is_move = false;
                this.is_follow = true;
                this.ball_arrow.SetActive(true);
                this.tr_player_select.parent.GetComponent<Control_Player>().enabled = false;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
                this.GetComponent<CircleCollider2D>().enabled = false;
                if (s_name_p == "p_1")
                    this.anim.Play("ball_p_1");
                else
                    this.anim.Play("ball_p_2");

                GameObject.Find("Game").GetComponent<Game>().manager_play.show_player_info(fp_cur);

                this.show_all_arrow();
                fp_cur.hide_block_shoot_arrow();
                if (this.tr_player_select.parent.GetComponent<Control_Player>().is_npc)
                {
                    GameObject.Find("Game").GetComponent<Game>().carrot.delay_function(1f, auto_shoot_ball);
                    this.speed = Random.Range(1, 5);
                }
                else
                {
                    this.speed = (fp_cur.ball_force / 2);
                }

            }

            if (collision.gameObject.name == "football_field_goal_p1")
            {
                football_goal(2);
            }

            if (collision.gameObject.name == "football_field_goal_p2")
            {
                football_goal(1);
            }
        }
    }

    private void football_goal(int index_goal)
    {
        this.player_goal_after = index_goal;
        this.anim.Play("ball_goal");
        this.GetComponent<CircleCollider2D>().enabled = false;
        this.is_move = false;
        GameObject.Find("Game").GetComponent<Game>().manager_play.add_scores_player(index_goal);
        GameObject.Find("Game").GetComponent<Game>().play_sound(4);
    }

    private void auto_shoot_ball()
    {
        List<Transform> tr_arrow_shoot = new List<Transform>();
        for (int i = 0; i < this.tr_arrow.Length; i++)
        {
            if (this.tr_arrow[i].gameObject.activeInHierarchy)
            {
                tr_arrow_shoot.Add(this.tr_arrow[i]);
            }
        }

        int index_r=Random.Range(0,tr_arrow_shoot.Count);
        GameObject.Find("Game").GetComponent<Game>().play_sound(0);
        if (tr_arrow_shoot[index_r] != null)
        {
            this.transform.position = tr_arrow_shoot[index_r].position;
            this.transform.rotation = tr_arrow_shoot[index_r].rotation;
        }
        this.on_end_shoot();
    }


    private void on_end_shoot()
    {
        this.ball_arrow.SetActive(false);
        this.anim.Play("ball");
        this.GetComponent<CircleCollider2D>().enabled = true;
        this.is_follow = false;
        this.is_move = true;
        GameObject.Find("Game").GetComponent<Game>().manager_play.panel_info_player.SetActive(false);
    }

    public void on_shoot(int index)
    {
        if (!this.tr_player_select.parent.GetComponent<Control_Player>().is_npc)
        {
            GameObject.Find("Game").GetComponent<Game>().play_sound(0);
            this.transform.position = this.tr_arrow[index].position;
            this.transform.rotation = this.tr_arrow[index].rotation;
            this.on_end_shoot();
        }
    }

    private void show_all_arrow()
    {
        for (int i = 0; i < this.tr_arrow.Length; i++) this.tr_arrow[i].gameObject.SetActive(true);
    }

    public void on_hide()
    {
        this.gameObject.SetActive(false);
    }

    public void on_show()
    {
        this.is_move = true;
        this.gameObject.SetActive(true);
    }

    public void on_goalkeeper()
    {
        GameObject.Find("Game").GetComponent<Game>().manager_play.show_goalkeeper();
        this.gameObject.SetActive(false);
    }

    public Football_Player get_football_player_ball()
    {
        return this.tr_player_select.GetComponent<Football_Player>();
    }

    public int get_index_goal_after()
    {
        return this.player_goal_after;
    }
}
