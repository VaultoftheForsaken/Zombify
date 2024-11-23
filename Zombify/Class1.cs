using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Interfaces;
using System;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;


namespace ZombifyPlugin
{
    public class Zombify : Plugin<Config>
    {
        public override string Author { get; } = "gben5692";
        public override string Name { get; } = "Zombify";
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 0);
        public override Version Version { get; } = new Version(1, 7, 2);

        public override void OnEnabled()
        {
            if (Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
                Exiled.Events.Handlers.Player.Hurting += OnPlayerHurting;
                base.OnEnabled();
            }
        }

        private void OnPlayerHurting(HurtingEventArgs ev)
        {
            // Ensure the attacker is SCP-049 and set the damage to 9999 for instant kill
            if (ev.Attacker != null && ev.Attacker.Role == RoleTypeId.Scp049)
            {
                ev.Amount = 200;  // Set damage to maximum for instant kill
            }
        }

        private void OnPlayerDying(DyingEventArgs ev)
        {
            // Ensure the killer is SCP-049 and the player is eligible for zombification
            if (ev.Attacker != null && ev.Attacker.Role == RoleTypeId.Scp049 && Config.IsEnabled)
            {
                if (IsEligibleForZombification(ev.Player))
                {
                    string username = ev.Player.Nickname;

                    // Cancel the dying event to prevent the player from dying
                    ev.IsAllowed = false;

                    // Change the player's role to SCP-049-2 (Zombie)
                    ev.Player.Role.Set(RoleTypeId.Scp0492);

                    // Revive the player as SCP-049-2
                    ev.Player.Health = 600;  // Set health to 100
                    ev.Player.MaxHealth = 100;
                    ev.Player.HumeShield = 100;

                    // Notify SCP-049 about the cure
                    SendMessageToSCP049(username);
                }
            }
        }
        
        private void SendMessageToSCP049(string username)
        {
            // Notify SCP-049 about the cure
            foreach (var player in Player.List)
            {
                if (player.Role == RoleTypeId.Scp049)
                {
                    player.SendConsoleMessage($"The player {username} has been cured.", "yellow");
                }
            }
        }

        private bool IsEligibleForZombification(Player player)
        {
            // Check if the player is eligible for zombification (exclude non-human roles)
            return player.Role == RoleTypeId.ClassD || player.Role == RoleTypeId.Scientist ||
                   player.Role == RoleTypeId.FacilityGuard || player.Role == RoleTypeId.NtfSpecialist ||
                   player.Role == RoleTypeId.NtfPrivate || player.Role == RoleTypeId.NtfCaptain ||
                   player.Role == RoleTypeId.ChaosRifleman || player.Role == RoleTypeId.ChaosMarauder ||
                   player.Role == RoleTypeId.ChaosRepressor || player.Role == RoleTypeId.ChaosConscript ||
                   player.Role == RoleTypeId.NtfSergeant || player.Role == RoleTypeId.Tutorial;

        } 

        public override void OnDisabled()
        {
            if (Config.IsEnabled)
            {
                Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
                Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurting;
            }
            base.OnDisabled();
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true; // Make sure this is set to true if you want the plugin enabled
        public bool Debug { get; set; }
    }
}
