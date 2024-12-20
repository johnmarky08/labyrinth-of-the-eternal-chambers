﻿namespace labyrinth_of_the_eternal_chambers
{
    // Main Settings
    internal enum Configurations
    {
        WIDTH = 273, // Map width (Divisible by 7), Recommended Size: 273
        HEIGHT = 77, // Map height (Divisible by 7), Recommended Size: 77
        PATTERN_LENGTH = 3, // The number of doors, the player needs to guess to win
        MAX_GUESS = 7 // The number of times the player can guess the door pattern before losing
    }
}