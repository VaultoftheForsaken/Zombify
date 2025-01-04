using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace Zombify
{
    public class Zombify : Plugin<Config>
    {
        public override string Author { get; } = "gben5692";
        public override string Name { get; } = "Zombify";
        public override Version Version { get; } = new Version(1, 5, 3);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
            base.OnDisabled();
        }

        private void OnPlayerDying(DyingEventArgs ev)
        {
            // Ensure the attacker is SCP-049 and the target is eligible for zombification
            if (ev.Attacker != null && ev.Attacker.Role == RoleTypeId.Scp049 && IsEligibleForZombification(ev.Player))
            {
                // Prevent the player from actually dying
                ev.IsAllowed = false;

                // Convert the player to SCP-049-2
                ev.Player.Role.Set(RoleTypeId.Scp0492);

                // Notify SCP-049 about the zombification
                Notify049(ev.Player.Nickname, ev.Attacker);

                // Set health and Hume Shield for the newly zombified player
                ev.Player.Health = 400;
                ev.Player.HumeShield = 100; 
            }
        }

        private void Notify049(string curedPlayer, Player scp049)
        {
            // Inform SCP-049 about the cured player via console
            scp049.SendConsoleMessage($"The player {curedPlayer} has been cured and turned into SCP-049-2.", "yellow");
        }

        private bool IsEligibleForZombification(Player player)
        {
            // Check if the player's role is in the configured list of eligible roles
            return Config.EligibleRoles.Contains(player.Role);
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true; // Plugin enable/disable setting
        public bool Debug { get; set; } = false;

        
        public List<RoleTypeId> EligibleRoles { get; set; } = new List<RoleTypeId>
        {
            RoleTypeId.ClassD,
            RoleTypeId.Scientist,
            RoleTypeId.FacilityGuard,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfSpecialist,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosConscript,
            RoleTypeId.Tutorial
        };
    }
}
