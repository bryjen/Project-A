# Project Sentinels

## About
Project Sentinels is a prototype tower defense game inspired by plants vs.zombies. Made using the Unity Game engine and scripted through C#.

The game starts with a pre-game scene where the player gets to pick what sentinels (turrets) they will play. They have four choices: a sentinel that slows the attack and movement speed of the enemy; a short ranged, high damage, and fast fire rate sentinel with peircing capabilities; a sentinel that periodically shoots non peircing bullets that do decent damage; a sentinel that produces collectible energy.


<p align="center">
  <img src="https://user-images.githubusercontent.com/73836176/153020155-b4f219db-7a5d-4b0d-b817-5bb3945919b0.png" alt="Pre-game scene" width="400" height="300"/>
</p>

Like plants vs. zombies, sentinels cost energy to place. Said energy falls down periodically for the sky or is produced trom the energy producing sentinel and can collected by clicking on it.

There are three rounds, each introducing a new enemy with unique mechanics: a small enemy whose 3rd attack peirces; a fast, ranged enemy that deals huge damage; a slow, tanky peircing enemy that combos on its 3rd attack.

<p align="center">
  <img src="https://user-images.githubusercontent.com/73836176/153020284-1aec65eb-0a83-4ce0-96cd-3ba5150df91b.png" alt="Enemies"/>
</p>

At the end of the game, the game displays statistics such as the total elaped time, energy produced and collected, sentinels placed, etc.

Little disclaimer: This project is still a prototype and built as a learning experience.

## Medium
Made in Unity and scripted using C#. All assets were made by me with exceptions to the the [sentinel and enemy sprites](https://penusbmic.itch.io/) and the [tilemap](https://cainos.itch.io/pixel-art-top-down-basic)

## Process
It started with me wanting to recreate a game as a learning experience. Thinking back to my childhood, plants vs. zombies (pvz) played such a pivotal role in my early pre-teenage life, so it would be doing justice to it if I drew inspiration from it. I wanted the game to feel somewhat similar, hoping for an almost similar experience. 

It continued with me looking for sprites for the entities and the tilemap, with me trying to create them myself at first - this was later scrapped after realizing how bad the quality was relative to those that can be found online for free. Development started with me developing the map using Unity's tilemap functionality. I then coded interactions when the player hovers and clicks a title, having them preview and place a sprite respectively.

I then properly coded all sentinel functionality - health, animations, attacks, etc. That was followed by me implementing the pre-game selection scene and the system that tracks the selected sentinel - to find out which sentinel should be placed. I then made a delete functionality that delets the sentinel in a tile when clicked. After that, I used Unity's editor serialization to create easily customizable waves by making wave properties - duration, what and how many ememies spawn, etc. - easily customizable and extendable.

I then used static classes to transfer data between scenes to make the post-game statistics screen

## How to Download
Download the latest release from the [releases](https://github.com/bryjen/Project-A/releases). Download and extract the .rar file in a folder and execute the Project A.exe file.
