﻿using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace CobwebBot.Commands
{
    public class Utilities : BaseCommandModule
    {
        [Command("greet")]
        public async Task GreetCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("Greetings! I am Cobweb Bot");
        }

        [Command("avatar"),
         Description("Displays the avatar of the command runner or user mentioned with this command")]
        public async Task AvatarCommand(CommandContext ctx, DiscordMember mentionedUser)
        {
            await ctx.RespondAsync(mentionedUser != null
                ? mentionedUser.GetGuildAvatarUrl(ImageFormat.Auto, 512)
                : ctx.Member.GetGuildAvatarUrl(ImageFormat.Auto, 512));
        }

        [Command("avatar"),
         Description("Displays the avatar of the command runner or user mentioned with this command")]
        public async Task AvatarCommand(CommandContext ctx)
        {
            await ctx.RespondAsync(ctx.Member.GetGuildAvatarUrl(ImageFormat.Auto, 512));
        }
        [Command("help"), Description("Shows a help page")]
        public async Task HelpCommand(CommandContext ctx)
        {
            string EmbedDescription =
            @"
            `help`
            Shows this page.

            `avatar` 
            Gets the avatar for a user. 
            Syntax: avatar [user]
    
            `kick` (Admin Only) 
            Kicks a member. 
            Syntax: kick <user> ""<reason>""
    
            `ban` (Admin Only) 
            Bans a member. 
            Syntax: ban <user> ""<reason>"" 
    
            `mute` (Admin Only) 
            Mutes a user. 
            Syntax: mute <user> <duration> ""<reason>"" 
            For duration, the following values are valid: 
            <number>h: Hours 
            <number>m: Minutes 
            <number>s: Seconds
            
            `purge`/`clear` (Admin Only)
            Clear messages.
            Syntax: purge/clear [user] <amount of messages>
            ";
            DiscordEmbed Embed = new DiscordEmbedBuilder().WithDescription(EmbedDescription).WithTitle("Commands").WithColor(DiscordColor.Green);
            await ctx.RespondAsync(Embed);
        }

        [Command("csre")]
        public async Task RoleEmbedCommand(CommandContext ctx)
        {
            if (!ctx.Member.Permissions.HasPermission(Permissions.ManageGuild))
            {
                await ctx.Message.DeleteAsync();
            }
            string EmbedDescription = @"
            ModWeaver:
            Shows the channels for ModWeaver

            CobwebAPI:
            Shows the channels for CobwebAPI
            
            CWLauncher:
            Shows the channels for CWLauncher
            
            Webcrawler:
            Shows the channels for Webcrawler
            ";
            var embed = new DiscordEmbedBuilder().WithTitle("Roles: Show categories").WithDescription(EmbedDescription);
            var builder = new DiscordMessageBuilder().WithEmbed(embed).AddComponents(new DiscordComponent[]
            {
                new DiscordButtonComponent(ButtonStyle.Primary, "modweaver_role_give", "ModWeaver"),
                new DiscordButtonComponent(ButtonStyle.Primary, "cobwebapi_role_give", "CobwebAPI"),
                new DiscordButtonComponent(ButtonStyle.Primary, "cwlauncher_role_give", "CWLauncher"),
                new DiscordButtonComponent(ButtonStyle.Primary, "webcrawler_role_give", "Webcrawler")
            });
            await builder.SendAsync(ctx.Message.Channel);
        }
        [Command("tags")]
        public async Task TagsCommand(CommandContext ctx)
        {
            ulong tagsChannelId = 0;
            if (ctx.Guild.Name == "Cobweb")
            {
                tagsChannelId = 985136865789218866;
            }
            else if (ctx.Guild.Name == "Bot testing")
            {
                tagsChannelId = 990351314707951677;
            }
            //var tagsChannel = ctx.Guild.GetChannel(tagsChannelId);
            DiscordChannel tagsChannel = ctx.Guild.GetChannel(tagsChannelId);
            var Channels = await ctx.Guild.GetChannelsAsync();
            foreach (var channel in Channels.ToList())
            {
                if (channel.Id == tagsChannelId)
                {
                    tagsChannel = channel;
                }
            }
            var messagesList = await tagsChannel.GetMessagesAsync(100);
            var tags = new string[messagesList.Count];
            foreach (var message in messagesList)
            {
                tags = tags.ToArray();
                var splitMessage = message.Content.ToLower().Split("id: ")[1].Split("\n")[0];
                tags.Append(splitMessage);
                await ctx.Channel.SendMessageAsync(splitMessage);
            }

        }
        [Command("tag")]
        public async Task GetTagCommand(CommandContext ctx, string tag)
        {
            ulong tagsChannelId = 0;
            if (ctx.Guild.Name == "Cobweb")
            {
                tagsChannelId = 985136865789218866;
            }
            else if (ctx.Guild.Name == "Bot testing")
            {
                tagsChannelId = 990351314707951677;
            }
            DiscordChannel tagsChannel = ctx.Guild.GetChannel(tagsChannelId);
            var Channels = await ctx.Guild.GetChannelsAsync();
            foreach (var channel in Channels.ToList())
            {
                if (channel.Id == tagsChannelId)
                {
                    tagsChannel = channel;
                }
            }
            var messagesList = await tagsChannel.GetMessagesAsync(100);
            var tags = new string[messagesList.Count];
            var tags_out = new string[messagesList.Count];
            foreach (var message in messagesList)
            {
                var splitMessage = message.Content.Split("id: ")[1].Split("\n")[0];
                tags.Append(splitMessage);
            }

            if (tags.Contains(tag))
            {
                foreach (var message in messagesList)
                {
                    if (message.Content.Contains(tag))
                    {
                        var splitMessage = message.Content.Split($"id: {tag}")[1].Split("out: ")[1];
                        await ctx.Channel.SendMessageAsync(splitMessage);

                    }
                }
            }
            else
            {
                await ctx.Message.RespondAsync("Tag not found!");
                foreach (var tasg in tags)
                {
                    await ctx.Channel.SendMessageAsync(tasg);
                }
            }

        }
    }
}
