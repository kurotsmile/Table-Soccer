using Carrot;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Play : MonoBehaviour
{
    [Header("Obj Play")]
    public Game g;
    public Control_Player[] player_1;
    public Control_Player[] player_2;
    public GameObject Panel_change_player;
    public Animator ani;
    public Image img_change_team1;
    public Image img_change_team2;
    public Sprite[] icon_change_team;
    public bool is_edit_team = false;
    private bool is_mode_one_play = false;
    private bool is_unlock_all_player = false;
    public Ball ball;
    int player_sel_team = 0;
    int index_edit_team = 0;
    public int scores_total;
    public GameObject button_game_back_home;
    public GameObject button_game_setting;

    [Header("Info Player Change")]
    public GameObject panel_player_none;
    public GameObject panel_player_loading;
    public GameObject panel_player_in;
    public Text info_change_name;
    public Image info_change_avatar;
    public Text info_change_name_draft;
    public Image info_change_avatar_draft;
    public Transform area_body_list_player;
    public GameObject item_player_prefab;
    public Slider slider_ball_force_in;
    public Slider slider_ball_control_in;
    public Slider slider_ball_cutting_in;
    public Slider slider_ball_force_out;
    public Slider slider_ball_control_out;
    public Slider slider_ball_cutting_out;
    private Football_Player fplayer_in;
    private Football_Player fplayer_out;
    public GameObject button_change_buy;
    public GameObject button_change_done;

    [Header("Info Player ball")]
    public GameObject panel_info_player;
    public Image player_info_avatar;
    public Text Player_info_name;
    public Text txt_info_ball_force;
    public Text txt_info_ball_control;
    public Text txt_info_ball_cutting;

    [Header("Goalkeeper")]
    public GameObject Panel_goalkeeper;
    public GameObject Panel_goalkeeper_1player;
    private int scores_p1 = 0;
    private int scores_p2 = 0;
    public Sprite icon_football_player_goals;
    public Text Goalkeeper_txt_scores_p1;
    public Text Goalkeeper_txt_scores_p2;
    public Text Goalkeeper_txt_name_p1;
    public Text Goalkeeper_txt_name_p2;
    public Text football_txt_scores_p1;
    public Text football_txt_scores_p2;
    public Image football_img_player_goals;

    [Header("Goalkeeper win 2 player")]
    public GameObject Panel_goalkeeper_2player;
    public Text Goalkeeper_win2_left_scores_p1;
    public Text Goalkeeper_win2_left_scores_p2;
    public Text Goalkeeper_win2_right_scores_p1;
    public Text Goalkeeper_win2_right_scores_p2;

    private Carrot_Window_Input box_input = null;

    public void on_start()
    {
        this.panel_info_player.SetActive(false);
        this.Panel_change_player.SetActive(false);
        this.Panel_goalkeeper.SetActive(false);
    }

    private void load_game_before_play()
    {
        this.gameObject.SetActive(true);
        this.ball.on_Start_Game();
        this.scores_p1 = 0;
        this.scores_p2 = 0;
        this.football_txt_scores_p1.text = "0";
        this.football_txt_scores_p2.text = "0";
        this.ani.Play("Game_Play_Load");
        this.g.play_sound(1);
        this.g.panel_menu.SetActive(false);
    }

    public void game_one_player(int your_team)
    {
        this.is_mode_one_play = true;
        this.load_game_before_play();
        this.player_sel_team = your_team;
        for (int i = 0; i < player_1.Length; i++)
        {
            this.player_1[i].name = "p_1";
            this.player_2[i].name = "p_2";
            if (your_team == 2)
            {
                this.player_2[i].on_Strart_Play(true, 2);
                this.player_1[i].on_Strart_Play(false, 1);
            }
            else
            {
                this.player_2[i].on_Strart_Play(false, 1);
                this.player_1[i].on_Strart_Play(true, 2);
            }
            this.player_2[i].on_player_one_mode();
        }
    }

    public void game_two_player()
    {
        this.is_mode_one_play = false;
        this.load_game_before_play();
        for (int i = 0; i < player_1.Length; i++)
        {
            this.player_1[i].on_Strart_Play(false, 1);
            this.player_1[i].name = "p_1";
            this.player_2[i].on_Strart_Play(false, 2);
            this.player_2[i].name = "p_2";
            this.player_2[i].on_player_two_mode();
        }
    }

    public void game_reset_player()
    {
        this.g.check_and_show_ads();
        this.ani.Play("Game_Play_nomal");
        for (int i = 0; i < player_1.Length; i++)
        {
            this.player_1[i].on_Reset_Play();
            this.player_2[i].on_Reset_Play();
        }
        this.ball.on_Restart_play();
        this.Panel_goalkeeper.SetActive(false);
    }

    public void btn_change_mode_team_player(int index_team)
    {
        if (this.is_edit_team)
            this.close_team_football_player();
        else
            this.edit_team_football_player(index_team);
    }

    public void edit_team_football_player(int index_team)
    {
        if (PlayerPrefs.GetInt("is_buy_player", 0) == 0)
            this.is_unlock_all_player = false;
        else
            this.is_unlock_all_player = true;

        for (int i = 0; i < player_1.Length; i++)
        {
            this.player_1[i].on_edit_team(index_team);
            this.player_2[i].on_edit_team(index_team);
        }
        this.ball.on_hide();
        this.button_game_back_home.SetActive(false);
        this.button_game_setting.SetActive(false);
        this.is_edit_team = true;
        this.index_edit_team = index_team;
        this.check_icon_change_team();
        this.panel_info_player.SetActive(false);
        if (index_team == 2)
        {
            if (this.is_mode_one_play)
            {
                if (this.player_sel_team == 2) this.ani.Play("Game_Play_Edit_P2"); else this.ani.Play("Game_Play_Edit_P1");
            }
            else
                this.ani.Play("Game_Play_Edit_P2");

            if (!this.is_mode_one_play) this.Panel_change_player.transform.rotation = Quaternion.Euler(0, 0, 180f);
            this.img_change_team1.gameObject.SetActive(false);
        }
        else
        {
            if (this.is_mode_one_play)
            {
                if (this.player_sel_team == 2) this.ani.Play("Game_Play_Edit_P1"); else this.ani.Play("Game_Play_Edit_P2");
            }
            else
                this.ani.Play("Game_Play_Edit_P1");
            this.Panel_change_player.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.img_change_team2.gameObject.SetActive(false);
        }
        this.button_change_buy.SetActive(false);
        this.button_change_done.SetActive(true);
    }

    public void close_team_football_player()
    {
        this.ani.Play("Game_Play_nomal");
        this.hide_edit_team_football_player();
        for (int i = 0; i < player_1.Length; i++)
        {
            this.player_1[i].on_edit_close();
            this.player_2[i].on_edit_close();
        }
        this.ball.on_show();
        this.is_edit_team = false;
        this.button_game_back_home.SetActive(true);
        this.button_game_setting.SetActive(true);
        this.img_change_team1.gameObject.SetActive(true);
        this.img_change_team2.gameObject.SetActive(true);
        this.check_icon_change_team();
    }

    public void on_game()
    {
        this.ani.enabled = false;
    }

    public void hide_edit_team_football_player()
    {
        this.g.play_sound(1);
        this.Panel_change_player.SetActive(false);
    }

    private void check_icon_change_team()
    {
        if (this.is_edit_team)
        {
            this.img_change_team1.sprite = this.icon_change_team[1];
            this.img_change_team2.sprite = this.icon_change_team[1];
        }
        else
        {
            this.img_change_team1.sprite = this.icon_change_team[0];
            this.img_change_team2.sprite = this.icon_change_team[0];
        }
    }

    public void show_goalkeeper()
    {
        this.g.play_sound(6);
        string user_name_player = "";

        if (this.g.carrot.user.get_id_user_login() != "") user_name_player= this.g.carrot.user.get_data_user_login("name");

        this.football_txt_scores_p1.text = this.scores_p1.ToString();
        this.football_txt_scores_p2.text = this.scores_p2.ToString();

        if (this.player_sel_team != this.ball.get_index_goal_after())
        {
            Football_Player fp_goal = this.ball.get_football_player_ball();
            this.football_img_player_goals.sprite = fp_goal.img_avatar.sprite;
        }
        else
        {
            this.football_img_player_goals.sprite = this.icon_football_player_goals;
        }

        this.Panel_goalkeeper_1player.SetActive(false);
        this.Panel_goalkeeper_2player.SetActive(false);

        if (this.is_mode_one_play)
        {
            this.ani.Play("Game_Win");
            this.Goalkeeper_txt_scores_p1.text = this.scores_p1.ToString();
            this.Goalkeeper_txt_scores_p2.text = this.scores_p2.ToString();
            this.upload_scores();
            this.Panel_goalkeeper_1player.SetActive(true);

            if (user_name_player != "")
            {
                if (this.player_sel_team == 1)
                {
                    this.Goalkeeper_txt_name_p2.text = this.g.carrot.user.get_data_user_login("name");
                    this.Goalkeeper_txt_name_p1.text = this.g.carrot.L("robot", "Robot");
                }
                else
                {
                    this.Goalkeeper_txt_name_p2.text = this.g.carrot.L("robot", "Robot");
                    this.Goalkeeper_txt_name_p1.text = this.g.carrot.user.get_data_user_login("name");
                }
            }
            else
            {
                if (this.player_sel_team == 1)
                {
                    this.Goalkeeper_txt_name_p2.text = this.g.carrot.L("player", "Player");
                    this.Goalkeeper_txt_name_p1.text = this.g.carrot.L("robot", "Robot");
                }
                else
                {
                    this.Goalkeeper_txt_name_p2.text = this.g.carrot.L("robot", "Robot");
                    this.Goalkeeper_txt_name_p1.text = this.g.carrot.L("player", "Player");
                }
            }
        }
        else
        {
            this.Goalkeeper_win2_left_scores_p1.text = this.scores_p1.ToString();
            this.Goalkeeper_win2_left_scores_p2.text = this.scores_p2.ToString();
            this.Goalkeeper_win2_right_scores_p1.text = this.scores_p1.ToString();
            this.Goalkeeper_win2_right_scores_p2.text = this.scores_p2.ToString();
            this.Goalkeeper_txt_name_p2.text = this.g.carrot.L("player", "Player")+" 2";
            this.Goalkeeper_txt_name_p1.text = this.g.carrot.L("player", "Player")+" 1";
            this.ani.Play("Game_Win_2");
            this.Panel_goalkeeper_2player.SetActive(true);
        }
        this.Panel_goalkeeper.SetActive(true);
    }


    public void add_scores_player(int index_player_goalkeeper)
    {
        if (index_player_goalkeeper == 1)
            this.scores_p1++;
        else
            this.scores_p2++;

        if (index_player_goalkeeper != this.player_sel_team)
        {
            this.scores_total++;
            PlayerPrefs.SetInt("scores_total", this.scores_total);
        }
    }

    private void upload_scores()
    {
        this.g.carrot.game.update_scores_player(this.scores_total);
    }

    private void get_list_player()
    {
        this.panel_player_loading.SetActive(true);
        this.g.carrot.clear_contain(this.area_body_list_player);
        StructuredQuery q = new("football");
        q.Add_where("playing_position", Query_OP.EQUAL, this.fplayer_out.playing_position.ToString());
        this.g.carrot.server.Get_doc(q.ToJson(), this.Act_get_list_player);
    }

    private void Act_get_list_player(string s_data)
    {
        this.panel_player_loading.SetActive(false);
        Fire_Collection fc = new(s_data);
        if (!fc.is_null)
        {
            this.g.carrot.clear_contain(this.area_body_list_player);
            for (int i = 0; i < fc.fire_document.Length; i++)
            {
                IDictionary data_player =fc.fire_document[i].Get_IDictionary();
                GameObject Item_player = Instantiate(this.item_player_prefab);
                Item_player.transform.SetParent(this.area_body_list_player);
                Item_player.transform.localScale = new Vector3(1f, 1f, 1f);
                Item_player.GetComponent<Football_Player>().txt_name.text = data_player["name"].ToString();
                Item_player.GetComponent<Football_Player>().ball_force = int.Parse(data_player["ball_force"].ToString());
                Item_player.GetComponent<Football_Player>().ball_control = int.Parse(data_player["ball_control"].ToString());
                Item_player.GetComponent<Football_Player>().ball_cutting = int.Parse(data_player["ball_cutting"].ToString());

                string s_index_position = data_player["playing_position"].ToString();
                string s_tip = "";
                if (s_index_position == "0")
                    s_tip = g.carrot.L("playing_position_" + s_index_position, "Striker");
                if (s_index_position == "1")
                    s_tip = g.carrot.L("playing_position_" + s_index_position, "Midfielder");
                if (s_index_position == "2")
                    s_tip = g.carrot.L("playing_position_" + s_index_position, "Defender");
                if (s_index_position == "3")
                    s_tip = g.carrot.L("playing_position_" + s_index_position, "Goalie");

                Item_player.GetComponent<Football_Player>().txt_tip.text = s_tip;

                if (this.is_unlock_all_player)
                {
                    Item_player.GetComponent<Football_Player>().is_free = true;
                    Item_player.GetComponent<Football_Player>().button_buy.SetActive(false);
                }
                else
                {
                    if (data_player["buy"] !=null)
                    {
                        if (data_player["buy"].ToString() == "0")
                        {
                            Item_player.GetComponent<Football_Player>().is_free = true;
                            Item_player.GetComponent<Football_Player>().button_buy.SetActive(false);
                        }
                        else
                        {
                            Item_player.GetComponent<Football_Player>().is_free = false;
                            Item_player.GetComponent<Football_Player>().button_buy.SetActive(true);
                        }
                    }
                    else
                    {
                        Item_player.GetComponent<Football_Player>().is_free = false;
                        Item_player.GetComponent<Football_Player>().button_buy.SetActive(true);
                    }
                }

                if (this.index_edit_team == 2)
                {
                    if (!this.is_mode_one_play) Item_player.transform.rotation = Quaternion.Euler(0, 0, 180f);
                }
                else
                    Item_player.transform.rotation = Quaternion.Euler(0, 0, 0);

                this.g.carrot.get_img(data_player["icon"].ToString(), Item_player.GetComponent<Football_Player>().img_avatar);
            }
        }
        else
        {
            this.g.carrot.Show_msg(this.g.carrot.L("change_player", "Change football player"), this.g.carrot.L("list_none", "No se encontraron elementos en la lista"));
        }
    }

    public void show_player_info(Football_Player f_player)
    {
        this.txt_info_ball_control.text = f_player.ball_control.ToString();
        this.txt_info_ball_cutting.text = f_player.ball_cutting.ToString();
        this.txt_info_ball_force.text = f_player.ball_force.ToString();
        this.Player_info_name.text = f_player.s_name;
        this.player_info_avatar.sprite = f_player.img_avatar.sprite;
        this.g.play_sound(2);
        this.panel_info_player.SetActive(true);
    }

    public void show_change_player(Football_Player fp)
    {
        this.fplayer_out = fp;
        this.panel_info_player.SetActive(false);
        this.panel_player_none.SetActive(true);
        this.panel_player_in.SetActive(false);
        this.info_change_avatar.sprite = fp.img_avatar.sprite;
        this.info_change_name.text = fp.s_name;
        this.slider_ball_control_out.value = fp.ball_control;
        this.slider_ball_cutting_out.value = fp.ball_cutting;
        this.slider_ball_force_out.value = fp.ball_force;
        this.g.play_sound(1);
        this.Panel_change_player.SetActive(true);
        this.get_list_player();
    }

    public void show_change_player_in(Football_Player fp)
    {
        if (fp.is_free)
        {
            this.button_change_buy.SetActive(false);
            this.button_change_done.SetActive(true);
        }
        else
        {
            this.button_change_buy.SetActive(true);
            this.button_change_done.SetActive(false);
        }
        this.panel_player_none.SetActive(false);
        this.panel_player_in.SetActive(true);
        this.info_change_name_draft.text = fp.txt_name.text;
        this.info_change_avatar_draft.sprite = fp.img_avatar.sprite;
        this.slider_ball_control_in.value = fp.ball_force;
        this.slider_ball_cutting_in.value = fp.ball_control;
        this.slider_ball_force_in.value = fp.ball_cutting;
        this.fplayer_in = fp;
    }

    public void done_change_player()
    {
        if (this.fplayer_in == null)
        {
            this.g.carrot.Show_msg(this.g.carrot.L("change_player", "Change football player"), this.g.carrot.L("change_player_tip", "Please choose a player from the list next to replace"), Carrot.Msg_Icon.Alert);
        }
        else
        {
            this.fplayer_out.s_name = this.info_change_name_draft.text;
            this.fplayer_out.img_avatar.sprite = this.info_change_avatar_draft.sprite;
            this.fplayer_out.ball_control = this.fplayer_in.ball_control;
            this.fplayer_out.ball_cutting = this.fplayer_in.ball_cutting;
            this.fplayer_out.ball_force = this.fplayer_in.ball_force;
            this.hide_edit_team_football_player();
        }
    }

    public void show_search_player()
    {
        this.box_input=this.g.carrot.show_search(Act_search_done, this.g.carrot.L("search_player_tip", "Enter the name of the football player you want to search for for a change of person"));
    }

    private void Act_search_done(string s_val)
    {
        if (this.box_input != null) this.box_input.close();
        this.panel_player_loading.SetActive(true);
        this.g.carrot.clear_contain(this.area_body_list_player);
        StructuredQuery q = new("football");
        q.Add_where("name", Query_OP.EQUAL, s_val);
        this.g.carrot.server.Get_doc(q.ToJson(), this.Act_get_list_player);
    }

    public void unlock_player()
    {
        this.button_change_buy.SetActive(false);
        this.button_change_done.SetActive(true);
    }

}
