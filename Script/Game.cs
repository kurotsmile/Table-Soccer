using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Carrot.Carrot carrot;

    [Header("Panel Game")]
    public GameObject panel_menu;
    public GameObject panel_play;
    public GameObject panel_setting;

    [Header("Menu Home")]
    public Sprite icon_rank;
    public Sprite[] icon_team_sel;
    public Image img_icon_team1;
    public Image img_icon_team2;
    public Image img_icon_sound_status;
    private int player_sel_team=2;
    public Text txt_menu_total_goals;

    [Header("Obj Game")]
    public Manager_Play manager_play;
    public GameObject Item_list_prefab;
    public GameObject panel_setting_removeAds;
    public GameObject panel_setting_buy_all_player;
    public Sprite[] sp_icon_rank;

    [Header("Sound")]
    public Image img_icon_sound_setting;
    public Sprite[] icon_status_sound;
    public Sprite[] icon_status_sound_setting;
    public AudioSource[] sound;

    private void Start()
    {
        this.carrot.Load_Carrot(this.check_exit_app);
        this.carrot.shop.onCarrotPaySuccess += this.on_success_carrot_pay;
        this.carrot.shop.onCarrotRestoreSuccess += this.on_success_carrot_restore;

        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_setting.SetActive(false);
        this.load_total_goals("");
        this.manager_play.on_start();

        this.player_sel_team = PlayerPrefs.GetInt("player_sel_team");
        this.check_team_select();

        if (PlayerPrefs.GetInt("is_buy_player", 0) == 0)
            this.panel_setting_buy_all_player.SetActive(true);
        else
            this.panel_setting_buy_all_player.SetActive(false);
    }


    private void check_exit_app()
    {
        if (this.manager_play.Panel_change_player.activeInHierarchy)
        {
            this.manager_play.hide_edit_team_football_player();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.manager_play.is_edit_team)
        {
            this.manager_play.close_team_football_player();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_play.activeInHierarchy)
        {
            this.back_home();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_setting.activeInHierarchy)
        {
            this.close_setting();
            this.carrot.set_no_check_exit_app();
        }
    }

    private void load_total_goals(string s_data)
    {
        this.manager_play.scores_total = PlayerPrefs.GetInt("scores_total", 0);
        this.txt_menu_total_goals.text = PlayerPrefs.GetString("your_total_goals", "Your total goal score") + " : " + this.manager_play.scores_total.ToString();
    }

    public void back_home()
    {
        this.panel_menu.SetActive(true);
        this.play_sound(1);
    }

    public void game_play()
    {
        this.manager_play.game_one_player(this.player_sel_team);
    }

    public void game_two_play()
    {
        this.manager_play.game_two_player();
    }

    public void game_reset_play()
    {
        this.manager_play.game_reset_player();
        this.play_sound(1);
    }

    public void play_sound(int index)
    {
        if(this.carrot.get_status_sound()) this.sound[index].Play();
    }


    public void game_rate()
    {
        this.play_sound(1);
        this.carrot.show_rate();
    }

    public void game_more_app()
    {
        this.play_sound(1);
        this.carrot.show_list_carrot_app();
    }

    public void game_login()
    {
        this.play_sound(1);
        this.carrot.show_login();
    }

    public void game_show_lang()
    {
        this.play_sound(1);
        this.carrot.lang.Show_list_lang(this.load_total_goals);
    }

    public void game_share()
    {
        this.play_sound(1);
        this.carrot.show_share();
    }

    public void btn_select_team(int index_team)
    {
        this.player_sel_team = index_team;
        this.play_sound(1);
        PlayerPrefs.SetInt("player_sel_team", index_team);
        this.check_team_select();
    }

    private void check_team_select()
    {
        if (this.player_sel_team == 1)
        {
            this.img_icon_team1.sprite = this.icon_team_sel[1];
            this.img_icon_team2.sprite = this.icon_team_sel[0];
        }
        else
        {
            this.img_icon_team1.sprite = this.icon_team_sel[0];
            this.img_icon_team2.sprite = this.icon_team_sel[1];
        }
    }

    public void game_show_rank()
    {
        this.carrot.game.Show_List_Top_player();
    }


    public void on_change_sound_status()
    {
        this.carrot.Change_status_sound(null);
    }

    public void buy_product(int index_p)
    {
        this.play_sound(1);
        this.carrot.buy_product(index_p);
    }

    private void in_app_unlock_all_player()
    {
        this.panel_setting_buy_all_player.SetActive(false);
        PlayerPrefs.SetInt("is_buy_player", 1);
    }

    public void buy_success(Product product)
    {
        this.on_success_carrot_pay(product.definition.id);
    }

    public void on_success_carrot_pay(string s_id)
    {
        if (s_id == this.carrot.shop.get_id_by_index(1))
        {
            this.carrot.Show_msg(PlayerPrefs.GetString("shop", "Shop"), PlayerPrefs.GetString("buy_player_success", "Buy successful football players!"));
            this.manager_play.unlock_player();
        }

        if (s_id == this.carrot.shop.get_id_by_index(2))
        {
            this.carrot.Show_msg(PlayerPrefs.GetString("shop", "Shop"), PlayerPrefs.GetString("buy_all_player_success", "Buy all successful soccer players. you can use any player in the list"));
            this.in_app_unlock_all_player();
        }
    }

    public void on_success_carrot_restore(string[] arr_id)
    {
        for(int i = 0; i < arr_id.Length; i++)
        {
            string s_id_p = arr_id[i];
            if (s_id_p == this.carrot.shop.get_id_by_index(2)) this.in_app_unlock_all_player();
        }
    }

    public void show_setting()
    {
        this.play_sound(1);
        this.panel_setting.SetActive(true);
    }

    public void close_setting()
    {
        this.play_sound(1);
        this.panel_setting.SetActive(false);
    }

    public void game_in_app_restore()
    {
        this.play_sound(1);
        this.carrot.shop.restore_product();
    }

    public void game_del_all_data()
    {
        this.panel_setting_buy_all_player.SetActive(false);
        this.panel_setting_removeAds.SetActive(true);
        this.play_sound(1);
        this.carrot.Delete_all_data();
    }

    public void check_and_show_ads()
    {
        carrot.ads.show_ads_Interstitial();
    }

}
