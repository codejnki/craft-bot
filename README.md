# craft-bot

[![.NET](https://github.com/codejnki/craft-bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/codejnki/craft-bot/actions/workflows/dotnet.yml)

This is a Discord bot that is pretty dumb.  

I'm using this mostly to learn the Discord API and make my wife laugh in the family server.

But I'm going to try and document what I learn, along the way so maybe some day someone might find some use out of it.

For now...it doesn't do much.

## Running

CraftBot is currently designed to be ran by you hosted on your own infrastructure.  This could be a RaspberryPi, a micro VM on GCP, or anywhere else that `dotnet core` can run.  So right now these instructions assume you know what you are doing.

Eventually if I keep going with this I will figure out how to make this a lot smoother.

- Create an application in the [Discord Developer Portal](https://discord.com/developers/applications)
- Add your new app to your server
- Clone repository
- Set an environment variable `DISCORD_TOKEN`
- `cd CraftBot.App`
- `dotnet run`

## History

I originally wrote `craft-bot` using the Python Discord client.  But Python isn't my strongest skill set, and I was getting annoyed by dev workflow related things.  So I rewrote it using C#.
