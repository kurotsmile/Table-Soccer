using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_Player : MonoBehaviour
{
    private Plane dragPlane;
    private Vector3 offset;
    private bool is_move = false;
    private Vector3 pos_start;
    private float pos_limit = 0.6f;
    public bool is_npc = false;
    public Football_Player[] football_Player;
    private int index_team;
    public Animator anim;

    public void on_Strart_Play(bool set_npc,int set_index_team)
    {
        this.is_npc = set_npc;
        this.index_team = set_index_team;
        this.pos_start = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.is_move = false;
        for (int i = 0; i < this.football_Player.Length; i++)
        {
            this.football_Player[i].gameObject.SetActive(true);
            this.football_Player[i].GetComponent<Button>().enabled = false;
        }
        if (set_npc)
        {
            this.anim.enabled = true;
            this.npc_change_status();
        }
        else
        {
            this.anim.enabled = false;
        }
    }

    public void on_Reset_Play()
    {
        this.transform.position = this.pos_start;
        this.is_move = false;
    }

    public void onStartDrag()
    {
        if (!this.is_npc)
        {
            this.is_move = true;
            dragPlane = new Plane(Camera.main.transform.forward, transform.position);
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = transform.position - camRay.GetPoint(planeDist);
        }
    }

    public void onEnDrag()
    {
        this.is_move = false;
    }

    public void onDrag()
    {
        if (this.is_move)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            Vector3 p_new = camRay.GetPoint(planeDist) + offset;

            if (p_new.y < -this.pos_limit)
            {
                this.transform.position = new Vector3(this.transform.position.x, -(this.pos_limit-0.02f), this.transform.position.z);
            }
            else if (p_new.y > this.pos_limit)
            {
                this.transform.position = new Vector3(this.transform.position.x, (this.pos_limit - 0.02f), this.transform.position.z);
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, p_new.y, this.transform.position.z);
            }
        }
    }


    public void on_edit_team(int index_team_edit)
    {
        for(int i = 0; i < this.football_Player.Length; i++)
        {
            if (index_team_edit == this.index_team)
            {
                this.football_Player[i].gameObject.SetActive(true);
                this.football_Player[i].GetComponent<Button>().enabled = true;
            }
            else
                this.football_Player[i].gameObject.SetActive(false);
        }
        this.transform.position = this.pos_start;
        this.anim.enabled = false;
    }

    public void on_edit_close()
    {
        for (int i = 0; i < this.football_Player.Length; i++)
        {
            this.football_Player[i].gameObject.SetActive(true);
            this.football_Player[i].GetComponent<Button>().enabled = false;
        }
        if (is_npc) this.anim.enabled = true;
    }

    public void on_player_two_mode()
    {
        for (int i = 0; i < this.football_Player.Length; i++) this.football_Player[i].mode_two_player();
    }

    public void on_player_one_mode()
    {
        for (int i = 0; i < this.football_Player.Length; i++) this.football_Player[i].mode_one_player();
    }

    public void npc_change_status()
    {
        int rand_status = Random.Range(1, 4);
        this.anim.Play("status_" + rand_status);
    }
}
