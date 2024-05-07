using Carrot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Football_Player : MonoBehaviour
{
    [Header("Obj Main")]
    public Game g;
    public GameObject p_data_playerfootball;
    
    [Header("Ui")]
    public Sprite icon_change_player;

    private Carrot_Box box;
    List<Football_Player> list_player = null;
    private string[] s_data_player_cache = new string[4];

    public void Onload()
    {
        if (g.carrot.is_offline())
        {
            for (int i = 0; i < s_data_player_cache.Length; i++)
            {
                this.s_data_player_cache[i] = PlayerPrefs.GetString("data_p_" + i,"");
            }
        }
        else
        {
            for(int i=0;i<s_data_player_cache.Length;i++)
            {
                this.s_data_player_cache[i] = "";
            }
        }
    }

    public string Get_data_cache(int index_player_position)
    {
        return this.s_data_player_cache[index_player_position];
    }

    public void Save_data_cache(string s_data,int index_player_position)
    {
        PlayerPrefs.SetString("data_p_" + index_player_position, s_data);
        this.s_data_player_cache[index_player_position] = s_data;
    }

    public void Change_player_by_id(string id_player)
    {
        g.carrot.show_loading();
        StructuredQuery q = new("football");
        q.Set_where("id",Query_OP.EQUAL,id_player);
        q.Set_limit(1);
        g.carrot.server.Get_doc(q.ToJson(),(datas) =>
        {
            g.carrot.hide_loading();
            Fire_Collection fc = new Fire_Collection(datas);
            if (!fc.is_null)
            {
                IDictionary data_p = fc.fire_document[0].Get_IDictionary();
                Debug.Log(datas);
                Football_Player p = p_data_playerfootball.GetComponent<Football_Player>();
                p.s_name = data_p["name"].ToString();
                p.txt_name.text = data_p["name"].ToString();
                p.ball_force = int.Parse(data_p["ball_force"].ToString());
                p.ball_control = int.Parse(data_p["ball_control"].ToString());
                p.ball_cutting = int.Parse(data_p["ball_cutting"].ToString());
                if (g.manager_play.get_status_buy_all())
                {
                    p.is_free = true;
                    p.button_buy.SetActive(false);
                }
                else
                {
                    if (data_p["buy"] != null)
                    {
                        if (data_p["buy"].ToString() == "0")
                        {
                            p.is_free = true;
                            p.button_buy.SetActive(false);
                        }
                        else
                        {
                            p.is_free = false;
                            p.button_buy.SetActive(true);
                        }
                    }
                    else
                    {
                        p.is_free = false;
                        p.button_buy.SetActive(true);
                    }
                }

                this.Show_change_player_random();
                g.manager_play.show_change_player_in(p);

                string id_icon_p = "icon_p_" + data_p["id"].ToString();
                Sprite icon_player = this.g.carrot.get_tool().get_sprite_to_playerPrefs(id_icon_p);
                if (icon_player != null)
                {
                    g.manager_play.info_change_avatar_draft.sprite= icon_player;
                }
                else
                {
                    this.g.carrot.get_img_and_save_playerPrefs(data_p["icon"].ToString(), g.manager_play.info_change_avatar_draft, id_icon_p);
                }
            }
            else
            {
                g.carrot.Show_msg(this.g.carrot.L("change_player", "Change football player"), g.carrot.L("no_player", "No player found"));
                g.carrot.play_vibrate();
            }
        }, (error) =>
        {
            g.carrot.Show_msg(this.g.carrot.L("change_player", "Change football player"), g.carrot.L("no_player", "No player found"));
            g.carrot.play_vibrate();
        });
    }

    public void Show_change_player_random()
    {
        g.carrot.ads.Destroy_Banner_Ad();
        this.list_player = this.get_all_player(this.g.Get_team_select());
        int index_random = UnityEngine.Random.Range(0,this.list_player.Count);
        this.g.manager_play.show_change_player(this.list_player[index_random]);
    }

    public void Select_player_main_change()
    {
        this.list_player = this.get_all_player(this.g.Get_team_select());
        box = this.g.carrot.Create_Box();
        box.set_icon(this.icon_change_player);
        box.set_title(this.g.carrot.L("change_player", "Change football player"));

        for(int i = 0; i < this.list_player.Count; i++)
        {
            var index_p = i;
            Carrot_Box_Item item_p = box.create_item("Item_p_" + i);
            item_p.set_icon_white(this.list_player[i].img_avatar.sprite);
            item_p.set_title(this.list_player[i].s_name);
            item_p.set_tip(g.carrot.L("playing_position_" + this.list_player[i].playing_position, "Change football player"));
            item_p.set_act(() => this.Select_P(index_p));

            Carrot_Box_Btn_Item btn_sel = item_p.create_item();
            btn_sel.set_icon(this.g.carrot.icon_carrot_add);
            btn_sel.set_color(this.g.carrot.color_highlight);
            Destroy(btn_sel.GetComponent<Button>());
        }
    }

    private void Select_P(int index_p)
    {
        if (box != null) box.close();
        this.g.manager_play.Load_change_player_info(this.list_player[index_p]);
        this.g.manager_play.get_list_player();
    }

    private List<Football_Player> get_all_player(int index_team)
    {
        List<Football_Player> list_p = new();
        Control_Player[] list_control_p;

        if (this.g.Get_team_select() == index_team)
            list_control_p = g.manager_play.player_1;
        else
            list_control_p = g.manager_play.player_2;

        for(int i = 0; i < list_control_p.Length; i++)
        {
            for(int y=0;y< list_control_p[i].football_Player.Length; y++)
            {
                list_p.Add(list_control_p[i].football_Player[y]);
            }
        }
        return list_p;
    }
}
