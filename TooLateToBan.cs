using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Menu;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TooLateToBan.Utils;


namespace TooLateToBan
{


    public class TooLateToBan : BasePlugin, IPluginConfig<PluginConfig>
    {
        public override string ModuleName => "Too Late To Ban";

        public override string ModuleVersion => "0.0.2";

        public override string ModuleAuthor => "GL1TCH1337";

        LimitedDictionary<string, string> PlayerList = new LimitedDictionary<string, string>(10);

        public PluginConfig Config { get; set; }

        public void OnConfigParsed(PluginConfig config)
        {
            Config = config;

        }



        public override void Load(bool hotReload)
        {
            Logger.LogInformation(ModuleName + " initialized");

            RegisterListener<Listeners.OnClientDisconnect>(OnClientDisconnectHandler);


            AddCommand($"css_{Config.CommandName}", "Outputs to list of disconnected players", (player, commandInfo) =>
            {
                if (player == null)
                {
                    player.PrintToChat($" {Localizer["tltb.prefix"]} {Localizer["tltb.onlyingame"]}");
                    return;
                }

                if (!AdminManager.PlayerHasPermissions(player, ["css/generic"]))
                {
                    player.PrintToChat($" {Localizer["tltb.prefix"]} {Localizer["tltb.nopermission"]}");
                    return;
                }

                if (PlayerList.Count() == 0)
                {
                    player.PrintToChat($" {Localizer["tltb.prefix"]} {Localizer["tltb.noplayerstored"]}");
                    return;
                }

                OpenPlayersMenu(player);


            });
        }

        

        private void OnClientDisconnectHandler(int slot)
        {
            var player = new CCSPlayerController(NativeAPI.GetEntityFromIndex(slot + 1));
            if (!player.IsValid || player.IsBot) return;

            if (!PlayerList.ContainsKey(player.AuthorizedSteamID?.SteamId64.ToString()))
            {
                PlayerList.Add(player.AuthorizedSteamID?.SteamId64.ToString(), player.PlayerName);
            }
        }


        public void OpenPlayersMenu(CCSPlayerController admin)
        {
            ChatMenu playerMenu = new ChatMenu($"{ Localizer["tltb.mainmenutitle"] }");
            foreach (var item in PlayerList)
            {
                playerMenu.AddMenuOption(item.Value, (_, _) =>
                {
                    if (Config.Debug)
                    {
                        Logger.LogDebug($"Selected Player: {item.Key} - {item.Value}");
                    }
                    OpenOperationMenu(admin, item.Key, item.Value);
                });
            }

            MenuManager.OpenChatMenu(admin, playerMenu);
        }

        public void OpenOperationMenu(CCSPlayerController admin, string playerSteamId, string playerName)
        {
            ChatMenu operationMenu = new ChatMenu($"{Localizer["tltb.operationtitle"]}");
            bool hasBan = AdminManager.PlayerHasPermissions(admin, "@css/ban");
            bool hasChat = AdminManager.PlayerHasPermissions(admin, "@css/chat");

            if (hasBan)
            {
                operationMenu.AddMenuOption("Ban", (_, _) =>
                {
                    OpenDurationMenu(admin, playerSteamId, playerName, "ban");
                });
            }
            if (hasChat)
            {
                operationMenu.AddMenuOption("Gag", (_, _) =>
                {
                    OpenDurationMenu(admin, playerSteamId, playerName, "gag");

                });

                operationMenu.AddMenuOption("Mute", (_, _) =>
                {
                    OpenDurationMenu(admin, playerSteamId, playerName, "mute");

                });

                operationMenu.AddMenuOption("Silence", (_, _) =>
                {
                    OpenDurationMenu(admin, playerSteamId, playerName, "silence");

                });
            }

            MenuManager.OpenChatMenu(admin, operationMenu);
        }


        public void OpenDurationMenu(CCSPlayerController admin, string playerSteamId, string playerName, string operation)
        {
            if (Config.Debug)
            {
                Logger.LogDebug($"Selected Operation: {playerSteamId} - {playerName} - {operation}");
            }

            Tuple<string, int>[] _durations = new[]
            {
                new Tuple<string, int>("1 minute", 1),
                new Tuple<string, int>("5 minutes", 5),
                new Tuple<string, int>("15 minutes", 15),
                new Tuple<string, int>("1 hour", 60),
                new Tuple<string, int>("1 day", 60 * 24),
                new Tuple<string, int>("Permanent", 0)
            };

            ChatMenu durationMenu = new ChatMenu("Duration");
            foreach (Tuple<string, int> duration in _durations)
            {
                string optionName = duration.Item1;
                durationMenu.AddMenuOption(optionName, (_, durationOption) => 
                {
                    OpenReasonMenu(admin, playerSteamId, playerName, operation, duration.Item2);
                });
            }

            MenuManager.OpenChatMenu(admin, durationMenu);
        }

        public void OpenReasonMenu(CCSPlayerController admin, string playerSteamId, string playerName, string operation, int duration)
        {
            if (Config.Debug)
            {
                Logger.LogDebug($"Selected Duration: {playerSteamId} - {playerName} - {operation} - {duration}");
            }

            List<string> options = Config.Reasons;

            ChatMenu reasonMenu = new ChatMenu("Reason");
            foreach (string option in options)
            {
                reasonMenu.AddMenuOption(option, (_, _) =>
                {
                    if (Config.Debug)
                    {
                        Logger.LogDebug($"Selected Reason: {playerSteamId} - {playerName} - {operation} - {duration} - {option}");
                    }


                    switch (operation)
                    {
                        case "ban":
                            admin.ExecuteClientCommandFromServer($"{Config.BanCommand} {playerSteamId} {duration} {option}");
                            break;
                        case "gag":
                            admin.ExecuteClientCommandFromServer($"{Config.GagCommand} {playerSteamId} {duration} {option}");
                            break;
                        case "mute":
                            admin.ExecuteClientCommandFromServer($"{Config.MuteCommand} {playerSteamId} {duration} {option}");
                            break;
                        case "silence":
                            admin.ExecuteClientCommandFromServer($"{Config.SilenceCommand} {playerSteamId} {duration} {option}");
                            break;
                    }
                });
            }

            MenuManager.OpenChatMenu(admin, reasonMenu);

        }

    }

}
