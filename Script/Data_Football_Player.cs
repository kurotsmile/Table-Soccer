using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data_Football_Player : MonoBehaviour
{
    [Header("Obj Main")]
    public Game g;
    
    [Header("Ui")]
    public Sprite icon_change_player;

    private Carrot_Box box;
    List<Football_Player> list_player = null;

    public void add()
    {

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
            Destroy(btn_sel.GetComponent<Button>());
        }
    }

    private void Select_P(int index_p)
    {
        if (box != null) box.close();
        this.g.manager_play.Load_change_player_info(this.list_player[index_p]);
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
