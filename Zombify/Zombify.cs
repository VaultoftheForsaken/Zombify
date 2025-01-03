using System;
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
        public override Version RequiredExiledVersion { get; } = new Version(2, 0, 0);
        public override Version Version { get; } = new Version(1, 3, 0);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
            Exiled.Events.Handlers.Player.Hurting += OnPlayerHurting;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
            Exiled.Events.Handlers.Player.Hurting -= OnPlayerHurting;
            base.OnDisabled();
        }

        private void OnPlayerHurting(HurtingEventArgs ev)
        {
            // Ensure the attacker is SCP-049
            if (ev.Attacker != null && ev.Attacker.Role == RoleTypeId.Scp049)
            {
                ev.Amount = 200;  // Apply maximum damage for instant "death"
            }
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
            }
        }

        private void Notify049(string curedPlayer, Player scp049)
        {
            // Inform SCP-049 about the cured player via console
            scp049.SendConsoleMessage($"The player {curedPlayer} has been cured and turned into SCP-049-2.", "yellow");
        }

        private bool IsEligibleForZombification(Player player)
        {
            // List of roles eligible for zombification
            return player.Role == RoleTypeId.ClassD || player.Role == RoleTypeId.Scientist ||
                   player.Role == RoleTypeId.FacilityGuard || player.Role == RoleTypeId.NtfSpecialist ||
                   player.Role == RoleTypeId.NtfPrivate || player.Role == RoleTypeId.NtfCaptain ||
                   player.Role == RoleTypeId.NtfSergeant || player.Role == RoleTypeId.ChaosRifleman ||
                   player.Role == RoleTypeId.ChaosMarauder || player.Role == RoleTypeId.ChaosRepressor ||
                   player.Role == RoleTypeId.ChaosConscript || player.Role == RoleTypeId.Tutorial;
        }
    }

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true; // Plugin enable/disable setting
        public bool Debug { get; set; }
    }
}