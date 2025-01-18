using Carrot;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public IronSourceAds ads;

    [Header("Panel Game")]
    public GameObject panel_menu;
    public GameObject panel_play;

    [Header("Menu Home")]
    public Sprite[] icon_team_sel;
    public Image img_icon_team1;
    public Image img_icon_team2;
    private int player_sel_team=2;
    public Text txt_menu_total_goals;

    [Header("Obj Game")]
    public Manager_Play manager_play;
    public Data_Football_Player data_football_player;
    public GameObject Item_list_prefab;

    [Header("Asset icon")]
    public Sprite icon_all_player;

    [Header("Sound")]
    public AudioSource[] sound;
    private string link_deep_app = "";

    private void Start()
    {
        this.link_deep_app = Application.absoluteURL;
        Application.deepLinkActivated += onDeepLinkActivated;

        this.carrot.Load_Carrot(this.check_exit_app);
        this.ads.On_Load();

        this.carrot.shop.onCarrotPaySuccess += this.on_success_carrot_pay;
        this.carrot.shop.onCarrotRestoreSuccess += this.on_success_carrot_restore;
        
        this.carrot.game.load_bk_music(this.sound[5]);
        this.carrot.change_sound_click(this.sound[1].clip);

        this.carrot.game.act_click_watch_ads_in_music_bk=this.ads.ShowRewardedVideo;
        this.carrot.act_buy_ads_success=this.ads.RemoveAds;
        this.ads.onRewardedSuccess=this.carrot.game.OnRewardedSuccess;

        this.panel_menu.SetActive(true);
        this.panel_play.SetActive(false);
        this.load_total_goals("");
        this.manager_play.on_start();
        this.data_football_player.Onload();

        this.player_sel_team = PlayerPrefs.GetInt("player_sel_team");
        this.check_team_select();
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
        }
    }

    private void onDeepLinkActivated(string url)
    {
        this.link_deep_app = url;
        if (this.carrot != null) this.carrot.delay_function(1f, this.Check_link_deep_app);
    }

    private void Check_link_deep_app()
    {
        if (this.link_deep_app.Trim() != "")
        {
            if (this.carrot.is_online())
            {
                if (this.link_deep_app.Contains("tablesoccer:"))
                {
                    string id_project = this.link_deep_app.Replace("tablesoccer://show/", "");
                    Debug.Log("Get player football id:" + id_project);
                    this.data_football_player.Change_player_by_id(id_project);
                    this.link_deep_app = "";
                }
            }
        }
    }

    [ContextMenu("Test Deep Link")]
    public void Test_deep_link()
    {
        this.onDeepLinkActivated("tablesoccer://show/player1714458121648");
    }

    private void load_total_goals(string s_data)
    {
        this.manager_play.scores_total = PlayerPrefs.GetInt("scores_total", 0);
        this.txt_menu_total_goals.text = carrot.L("your_total_goals", "Your total goal score") + " : " + this.manager_play.scores_total.ToString();
    }

    public void back_home()
    {
        this.ads.ShowBannerAd();
        this.panel_menu.SetActive(true);
        this.carrot.play_sound_click();
    }

    public void game_play()
    {
        this.ads.HideBannerAd();
        this.manager_play.game_one_player(this.player_sel_team);
    }

    public void game_two_play()
    {
        this.ads.HideBannerAd();
        this.manager_play.game_two_player();
    }

    public void game_reset_play()
    {
        this.manager_play.game_reset_player();
        this.carrot.play_sound_click();
    }

    public void play_sound(int index)
    {
        if(this.carrot.get_status_sound()) this.sound[index].Play();
    }


    public void game_rate()
    {
        this.carrot.play_sound_click();
        this.carrot.show_rate();
    }

    public void game_more_app()
    {
        this.play_sound(1);
        this.carrot.show_list_carrot_app();
    }

    public void game_login()
    {
        this.carrot.play_sound_click();
        this.carrot.show_login();
    }

    public void game_show_lang()
    {
        this.carrot.play_sound_click();
        this.carrot.lang.Show_list_lang(this.load_total_goals);
    }

    public void game_share()
    {
        this.carrot.play_sound_click();
        this.carrot.show_share();
    }

    public void btn_select_team(int index_team)
    {
        this.player_sel_team = index_team;
        this.carrot.play_sound_click();
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

    public void buy_product(int index_p)
    {
        this.play_sound(1);
        this.carrot.buy_product(index_p);
    }

    private void in_app_unlock_all_player()
    {
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
            this.carrot.Show_msg(carrot.L("shop", "Shop"),carrot.L("buy_player_success", "Buy successful football players!"));
            this.manager_play.unlock_player();
        }

        if (s_id == this.carrot.shop.get_id_by_index(2))
        {
            this.carrot.Show_msg(carrot.L ("shop", "Shop"), carrot.L("buy_all_player_success", "Buy all successful soccer players. you can use any player in the list"));
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
        Carrot_Box box=this.carrot.Create_Setting();

        if (PlayerPrefs.GetInt("is_buy_player", 0) == 0)
        {
            Carrot_Box_Item item_unlook_all_player = box.create_item_of_index("item_unlook_all_player");
            item_unlook_all_player.set_icon(this.icon_all_player);
            item_unlook_all_player.set_title(carrot.L("unlock_all_p","Unlock all soccer players"));
            item_unlook_all_player.set_tip(carrot.L("unlock_all_p_tip","Buy and use all soccer players"));
            item_unlook_all_player.set_type(Box_Item_Type.box_nomal);
            item_unlook_all_player.check_type();
            item_unlook_all_player.set_act(() => this.buy_product(2));

            Carrot_Box_Btn_Item btn_buy = item_unlook_all_player.create_item();
            btn_buy.set_icon(this.carrot.icon_carrot_buy);
            btn_buy.set_color(this.carrot.color_highlight);
            Destroy(btn_buy.GetComponent<Button>());
        }
    }

    public void game_del_all_data()
    {
        this.play_sound(1);
        this.carrot.Delete_all_data();
    }

    public void check_and_show_ads()
    {
        this.ads.show_ads_Interstitial();
    }

    public int Get_team_select()
    {
        return this.player_sel_team;
    }

}
