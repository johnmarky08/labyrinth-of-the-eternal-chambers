﻿namespace final_project
{
    // ASCII Art Tokens
    internal class Token
    {
        public static readonly string
            betterLuckNextTime = "\t\t\t      ____           _     _                     _                  _                              _       _     _                      _ \n\t\t\t     | __ )    ___  | |_  | |_    ___   _ __    | |  _   _    ___  | | __    _ __     ___  __  __ | |_    | |_  (_)  _ __ ___     ___  | |\n\t\t\t     |  _ \\   / _ \\ | __| | __|  / _ \\ | '__|   | | | | | |  / __| | |/ /   | '_ \\   / _ \\ \\ \\/ / | __|   | __| | | | '_ ` _ \\   / _ \\ | |\n\t\t\t     | |_) | |  __/ | |_  | |_  |  __/ | |      | | | |_| | | (__  |   <    | | | | |  __/  >  <  | |_    | |_  | | | | | | | | |  __/ |_|\n\t\t\t     |____/   \\___|  \\__|  \\__|  \\___| |_|      |_|  \\__,_|  \\___| |_|\\_\\   |_| |_|  \\___| /_/\\_\\  \\__|    \\__| |_| |_| |_| |_|  \\___| (_)\n\t\t\t                                                                                                                                          ",
            boundary = " _____ \n|     |\n|||||||\n|||||||\n|||||||\n|_____|\n       ",
            closeMenu = "\n                                                                                                                                                   \n                                                                                                                                                   \n                                                                                                                                                   \n                                                                                                                                                   \n                                                                                                                                                   \n                                                __   _____ ____   ____   __     ____ _                                                             \n                                               | _| | ____/ ___| / ___| |_ |   / ___| | ___  ___  ___                                              \n                                               | |  |  _| \\___ \\| |      | |  | |   | |/ _ \\/ __|/ _ \\                                             \n                                               | |  | |___ ___) | |___   | |  | |___| | (_) \\__ |  __/                                             \n                                               | |  |_____|____/ \\____|  | |   \\____|_|\\___/|___/\\___|                                             \n                                               |__|                     |__|                                                                       \n                                                                                                                                                   \n                                                                                                                                                   ",
            door1 = "  ___  \n /   \\ \n|  *  |\n|  1  |\n|     |\n \\___/ \n       ",
            door2 = "  ___  \n /   \\ \n|  *  |\n|  2  |\n|  *  |\n \\___/ \n       ",
            door3 = "  ___  \n /   \\ \n|  *  |\n|* 3 *|\n|     |\n \\___/ \n       ",
            door4 = "  ___  \n /   \\ \n|  *  |\n|* 4 *|\n|  *  |\n \\___/ \n       ",
            endMessage1 = "        _ _    ___                                    _            _          _     _                     _    __   __                 _                            \n       ( | )  / __|  ___   _ _    __ _   _ _   __ _  | |_   _  _  | |  __ _  | |_  (_)  ___   _ _    ___ | |   \\ \\ / /  ___   _  _    | |_    __ _  __ __  ___      \n        V V  | (__  / _ \\ | ' \\  / _` | | '_| / _` | |  _| | || | | | / _` | |  _| | | / _ \\ | ' \\  (_-< |_|    \\ V /  / _ \\ | || |   | ' \\  / _` | \\ V / / -_)     \n              \\___| \\___/ |_||_| \\__, | |_|   \\__,_|  \\__|  \\_,_| |_| \\__,_|  \\__| |_| \\___/ |_||_| /__/ (_)     |_|   \\___/  \\_,_|   |_||_| \\__,_|  \\_/  \\___|     \n                                 |___/                                                                                                                              \n                                  _                 _              _     _     _                                  _                 _                               \n              _ _    __ _  __ __ (_)  __ _   __ _  | |_   ___   __| |   | |_  | |_    ___     _ __    _  _   ___ | |_   ___   _ _  (_)  ___   _  _   ___            \n             | ' \\  / _` | \\ V / | | / _` | / _` | |  _| / -_) / _` |   |  _| | ' \\  / -_)   | '  \\  | || | (_-< |  _| / -_) | '_| | | / _ \\ | || | (_-<            \n             |_||_| \\__,_|  \\_/  |_| \\__, | \\__,_|  \\__| \\___| \\__,_|    \\__| |_||_| \\___|   |_|_|_|  \\_, | /__/  \\__| \\___| |_|   |_| \\___/  \\_,_| /__/            \n                                     |___/                                                            |__/                                                          \n      _              _        _                             _     _                                                 _                                            _  \n     | |_  __ __ __ (_)  ___ | |_   ___    __ _   _ _    __| |   | |_   _  _   _ _   _ _    ___    __ _   _ _    __| |    ___   ___  __   __ _   _ __   ___   __| | \n     |  _| \\ V  V / | | (_-< |  _| (_-<   / _` | | ' \\  / _` |   |  _| | || | | '_| | ' \\  (_-<   / _` | | ' \\  / _` |   / -_) (_-< / _| / _` | | '_ \\ / -_) / _` | \n      \\__|  \\_/\\_/  |_| /__/  \\__| /__/   \\__,_| |_||_| \\__,_|    \\__|  \\_,_| |_|   |_||_| /__/   \\__,_| |_||_| \\__,_|   \\___| /__/ \\__| \\__,_| | .__/ \\___| \\__,_| \n                                                                                                                                                |_|                 \n                _     _              _             _                   _          _     _               __     ___   _                               _              \n               | |_  | |_    ___    | |     __ _  | |__   _  _   _ _  (_)  _ _   | |_  | |_      ___   / _|   | __| | |_   ___   _ _   _ _    __ _  | |             \n               |  _| | ' \\  / -_)   | |__  / _` | | '_ \\ | || | | '_| | | | ' \\  |  _| | ' \\    / _ \\ |  _|   | _|  |  _| / -_) | '_| | ' \\  / _` | | |             \n                \\__| |_||_| \\___|   |____| \\__,_| |_.__/  \\_, | |_|   |_| |_||_|  \\__| |_||_|   \\___/ |_|     |___|  \\__| \\___| |_|   |_||_| \\__,_| |_|             \n                                                          |__/                                                                                                      \n       ___   _                     _                            __   __                         _                                                            _      \n      / __| | |_    __ _   _ __   | |__   ___   _ _   ___       \\ \\ / /  ___   _  _   _ _      (_)  ___   _  _   _ _   _ _    ___   _  _     ___   _ _    __| |  ___\n     | (__  | ' \\  / _` | | '  \\  | '_ \\ / -_) | '_| (_-<  _     \\ V /  / _ \\ | || | | '_|     | | / _ \\ | || | | '_| | ' \\  / -_) | || |   / -_) | ' \\  / _` | (_-<\n      \\___| |_||_| \\__,_| |_|_|_| |_.__/ \\___| |_|   /__/ (_)     |_|   \\___/  \\_,_| |_|      _/ | \\___/  \\_,_| |_|   |_||_| \\___|  \\_, |   \\___| |_||_| \\__,_| /__/\n                                                                                             |__/                                   |__/                            \n        _                              _             _       _     _                                                _                       __     _     _          \n       | |_    ___   _ _   ___        | |__   _  _  | |_    | |_  | |_    ___     _ __    ___   _ __    ___   _ _  (_)  ___   ___    ___   / _|   | |_  | |_    ___ \n       | ' \\  / -_) | '_| / -_)  _    | '_ \\ | || | |  _|   |  _| | ' \\  / -_)   | '  \\  / -_) | '  \\  / _ \\ | '_| | | / -_) (_-<   / _ \\ |  _|   |  _| | ' \\  / -_)\n       |_||_| \\___| |_|   \\___| ( )   |_.__/  \\_,_|  \\__|    \\__| |_||_| \\___|   |_|_|_| \\___| |_|_|_| \\___/ |_|   |_| \\___| /__/   \\___/ |_|      \\__| |_||_| \\___|\n                                |/                                                                                                                                  \n                            _   _                     _             _   _                   _   _   _          _                              _   _     _           \n            ___   _ _    __| | | |  ___   ___  ___   | |_    __ _  | | | |  ___   __ __ __ (_) | | | |    ___ | |_   __ _   _  _    __ __ __ (_) | |_  | |_         \n           / -_) | ' \\  / _` | | | / -_) (_-< (_-<   | ' \\  / _` | | | | | (_-<   \\ V  V / | | | | | |   (_-< |  _| / _` | | || |   \\ V  V / | | |  _| | ' \\        \n           \\___| |_||_| \\__,_| |_| \\___| /__/ /__/   |_||_| \\__,_| |_| |_| /__/    \\_/\\_/  |_| |_| |_|   /__/  \\__| \\__,_|  \\_, |    \\_/\\_/  |_|  \\__| |_||_|       \n                                                                                                                            |__/                                    \n                                                                       __                                           _ _                                             \n                                                _  _   ___   _  _     / _|  ___   _ _   ___  __ __  ___   _ _      ( | )                                            \n                                               | || | / _ \\ | || |   |  _| / _ \\ | '_| / -_) \\ V / / -_) | '_|  _   V V                                             \n                                                \\_, | \\___/  \\_,_|   |_|   \\___/ |_|   \\___|  \\_/  \\___| |_|   (_)                                                  \n                                                |__/                                                                                                                ",
            endMessage2 = "\n\n\n\n\n\n\n\n\n\t                          _____   _                       _               __                            _                   _                   _ \n\t                         |_   _| | |__     __ _   _ __   | | __  ___     / _|   ___    _ __     _ __   | |   __ _   _   _  (_)  _ __     __ _  | |\n\t                           | |   | '_ \\   / _` | | '_ \\  | |/ / / __|   | |_   / _ \\  | '__|   | '_ \\  | |  / _` | | | | | | | | '_ \\   / _` | | |\n\t                           | |   | | | | | (_| | | | | | |   <  \\__ \\   |  _| | (_) | | |      | |_) | | | | (_| | | |_| | | | | | | | | (_| | |_|\n\t                           |_|   |_| |_|  \\__,_| |_| |_| |_|\\_\\ |___/   |_|    \\___/  |_|      | .__/  |_|  \\__,_|  \\__, | |_| |_| |_|  \\__, | (_)\n\t                                                                                               |_|                  |___/               |___/     \n",
            exitKey1 = "\n\t\t\t\t\t__       ____                             ______ _____  ______   __                          _  __  \n\t\t\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ____// ___/ / ____/  / /_ ____     ____ _ __  __ (_)/ /_ \n\t\t\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/  / __/   \\__ \\ / /      / __// __ \\   / __ `// / / // // __/ \n\t\t\t\t\t / /  / ____// /   /  __/(__  )(__  )  / /___  ___/ // /___   / /_ / /_/ /  / /_/ // /_/ // // /_ _ \n\t\t\t\t\t/_/  /_/    /_/    \\___//____//____/  /_____/ /____/ \\____/   \\__/ \\____/   \\__, / \\__,_//_/ \\__/(_)\n\t\t\t\t\t                                                                              /_/                   ",
            exitKey2 = "\t __      ____                                _____   ____     ____     _                             _   _       \n\t \\ \\    |  _ \\   _ __    ___   ___   ___    | ____| / ___|   / ___|   | |_    ___       ___  __  __ (_) | |_     \n\t  \\ \\   | |_) | | '__|  / _ \\ / __| / __|   |  _|   \\___ \\  | |       | __|  / _ \\     / _ \\ \\ \\/ / | | | __|    \n\t  / /   |  __/  | |    |  __/ \\__ \\ \\__ \\   | |___   ___) | | |___    | |_  | (_) |   |  __/  >  <  | | | |_   _ \n\t /_/    |_|     |_|     \\___| |___/ |___/   |_____| |____/   \\____|    \\__|  \\___/     \\___| /_/\\_\\ |_|  \\__| (_)\n\t                                                                                                                 ",
            exitMenu = "                                                                                                             \n                    _                                                                                        \n                   /_\\    _ _   ___     _  _   ___   _  _     ___  _  _   _ _   ___                          \n                  / _ \\  | '_| / -_)   | || | / _ \\ | || |   (_-< | || | | '_| / -_)                         \n                 /_/ \\_\\ |_|   \\___|    \\_, | \\___/  \\_,_|   /__/  \\_,_| |_|   \\___|                         \n                                        |__/                                                                 \n                                                     _       _                         _   _     ___         \n        _  _   ___   _  _    __ __ __  __ _   _ _   | |_    | |_   ___     ___  __ __ (_) | |_  |__ \\        \n       | || | / _ \\ | || |   \\ V  V / / _` | | ' \\  |  _|   |  _| / _ \\   / -_) \\ \\ / | | |  _|   /_/        \n        \\_, | \\___/  \\_,_|    \\_/\\_/  \\__,_| |_||_|  \\__|    \\__| \\___/   \\___| /_\\_\\ |_|  \\__|  (_)         \n        |__/                                                                                                 \n                               _   _                                        _                                \n                              ( \\_/ )                                  _   / )                               \n                               ) _ (                                  ( ) / /                                \n                              (_/ \\_)                                  ()(_/                                 \n                              [ ESC ]                                [ ENTER ]                               \n                                                                                                             ",
            gameOver = "\n\n\n\n\n\n\n\n\n\t\t\t\t      ___           ___           ___           ___                    ___           ___           ___           ___     \n\t\t\t\t     /\\  \\         /\\  \\         /\\__\\         /\\  \\                  /\\  \\         /\\__\\         /\\  \\         /\\  \\    \n\t\t\t\t    /::\\  \\       /::\\  \\       /::|  |       /::\\  \\                /::\\  \\       /:/  /        /::\\  \\       /::\\  \\   \n\t\t\t\t   /:/\\:\\  \\     /:/\\:\\  \\     /:|:|  |      /:/\\:\\  \\              /:/\\:\\  \\     /:/  /        /:/\\:\\  \\     /:/\\:\\  \\  \n\t\t\t\t  /:/  \\:\\  \\   /::\\~\\:\\  \\   /:/|:|__|__   /::\\~\\:\\  \\            /:/  \\:\\  \\   /:/__/  ___   /::\\~\\:\\  \\   /::\\~\\:\\  \\ \n\t\t\t\t /:/__/_\\:\\__\\ /:/\\:\\ \\:\\__\\ /:/ |::::\\__\\ /:/\\:\\ \\:\\__\\          /:/__/ \\:\\__\\  |:|  | /\\__\\ /:/\\:\\ \\:\\__\\ /:/\\:\\ \\:\\__\\\n\t\t\t\t \\:\\  /\\ \\/__/ \\/__\\:\\/:/  / \\/__/~~/:/  / \\:\\~\\:\\ \\/__/          \\:\\  \\ /:/  /  |:|  |/:/  / \\:\\~\\:\\ \\/__/ \\/_|::\\/:/  /\n\t\t\t\t  \\:\\ \\:\\__\\        \\::/  /        /:/  /   \\:\\ \\:\\__\\             \\:\\  /:/  /   |:|__/:/  /   \\:\\ \\:\\__\\      |:|::/  / \n\t\t\t\t   \\:\\/:/  /        /:/  /        /:/  /     \\:\\ \\/__/              \\:\\/:/  /     \\::::/__/     \\:\\ \\/__/      |:|\\/__/  \n\t\t\t\t    \\::/  /        /:/  /        /:/  /       \\:\\__\\                 \\::/  /       ~~~~          \\:\\__\\        |:|  |    \n\t\t\t\t     \\/__/         \\/__/         \\/__/         \\/__/                  \\/__/                       \\/__/         \\|__|    \n\t\t\t\t",
            gameTitle = "\n\n\n\n\n\n\n\n\n\n\n\n\t                           ___             __       _______    ___  ___    _______     __      _____  ___    ___________    __    __           ______      _______                             \n\t                          |\"  |           /\"\"\\     |   _  \"\\  |\"  \\/\"  |  /\"      \\   |\" \\    (\\\"   \\|\"  \\  (\"     _   \")  /\" |  | \"\\         /    \" \\    /\"     \"|                            \n\t                          ||  |          /    \\    (. |_)  :)  \\   \\  /  |:        |  ||  |   |.\\\\   \\    |  )__/  \\\\__/  (:  (__)  :)       // ____  \\  (: ______)                            \n\t                          |:  |         /' /\\  \\   |:     \\/    \\\\  \\/   |_____/   )  |:  |   |: \\.   \\\\  |     \\\\_ /      \\/      \\/       /  /    ) :)  \\/    |                              \n\t                           \\  |___     //  __'  \\  (|  _  \\\\    /   /     //      /   |.  |   |.  \\    \\. |     |.  |      //  __  \\\\      (: (____/ //   // ___)                              \n\t                          ( \\_|:  \\   /   /  \\\\  \\ |: |_)  :)  /   /     |:  __   \\   /\\  |\\  |    \\    \\ |     \\:  |     (:  (  )  :)      \\        /   (:  (                                 \n\t                           \\_______) (___/    \\___)(_______/  |___/      |__|  \\___) (__\\_|_)  \\___|\\____\\)      \\__|      \\__|  |__/        \\\"_____/     \\__/                                 \n\t                                                                                                                                                                                               \n\t    _______   ___________    _______    _______    _____  ___         __       ___             ______     __    __         __       ___      ___  _______     _______    _______     ________  \n\t   /\"     \"| (\"     _   \")  /\"     \"|  /\"      \\  (\\\"   \\|\"  \\       /\"\"\\     |\"  |           /\" _  \"\\   /\" |  | \"\\       /\"\"\\     |\"  \\    /\"  ||   _  \"\\   /\"     \"|  /\"      \\   /\"       ) \n\t  (: ______)  )__/  \\\\__/  (: ______) |:        | |.\\\\   \\    |     /    \\    ||  |          (: ( \\___) (:  (__)  :)     /    \\     \\   \\  //   |(. |_)  :) (: ______) |:        | (:   \\___/  \n\t   \\/    |       \\\\_ /      \\/    |   |_____/   ) |: \\.   \\\\  |    /' /\\  \\   |:  |           \\/ \\       \\/      \\/     /' /\\  \\    /\\\\  \\/.    ||:     \\/   \\/    |   |_____/   )  \\___  \\    \n\t   // ___)_      |.  |      // ___)_   //      /  |.  \\    \\. |   //  __'  \\   \\  |___        //  \\ _    //  __  \\\\    //  __'  \\  |: \\.        |(|  _  \\\\   // ___)_   //      /    __/  \\\\   \n\t  (:      \"|     \\:  |     (:      \"| |:  __   \\  |    \\    \\ |  /   /  \\\\  \\ ( \\_|:  \\      (:   _) \\  (:  (  )  :)  /   /  \\\\  \\ |.  \\    /:  ||: |_)  :) (:      \"| |:  __   \\   /\" \\   :)  \n\t   \\_______)      \\__|      \\_______) |__|  \\___)  \\___|\\____\\) (___/    \\___) \\_______)      \\_______)  \\__|  |__/  (___/    \\___)|___|\\__/|___|(_______/   \\_______) |__|  \\___) (_______/   \n\t  \t                                                                                                                                                                                             ",
            genius = "\t\t\t\t\t    __   __                  _                                                    _                 _ \n\t\t\t\t\t    \\ \\ / /   ___    _   _  ( )  _ __    ___      __ _      __ _    ___   _ __   (_)  _   _   ___  | |\n\t\t\t\t\t     \\ V /   / _ \\  | | | | |/  | '__|  / _ \\    / _` |    / _` |  / _ \\ | '_ \\  | | | | | | / __| | |\n\t\t\t\t\t      | |   | (_) | | |_| |     | |    |  __/   | (_| |   | (_| | |  __/ | | | | | | | |_| | \\__ \\ |_|\n\t\t\t\t\t      |_|    \\___/   \\__,_|     |_|     \\___|    \\__,_|    \\__, |  \\___| |_| |_| |_|  \\__,_| |___/ (_)\n\t\t\t\t\t\t                                                   |___/                                      ",
            good = "\t\t\t\t\t\t\t\t   ____                       _                            _ \n\t\t\t\t\t\t\t\t  / ___|   ___     ___     __| |     ___    _ __     ___  | |\n\t\t\t\t\t\t\t\t | |  _   / _ \\   / _ \\   / _` |    / _ \\  | '_ \\   / _ \\ | |\n\t\t\t\t\t\t\t\t | |_| | | (_) | | (_) | | (_| |   | (_) | | | | | |  __/ |_|\n\t\t\t\t\t\t\t\t  \\____|  \\___/   \\___/   \\__,_|    \\___/  |_| |_|  \\___| (_)\n\t\t\t\t\t\t\t\t                                                             ",
            guide = " .----------------. \n| .--------------. |\n| |    ______    | |\n| |   / _ __ `.  | |\n| |  |_/____) |  | |\n| |    /  ___.'  | |\n| |    |_|       | |\n| |    (_)       | |\n| |              | |\n| '--------------' |\n '----------------' \n       [ F1 ]       ",
            guideMessage1 = "                                                                                                                                                   \n                                                                                                                                                   \n                         __   __                  ____             _   _       _                                                                   \n                         \\ \\ / ___  _   _ _ __   / ___| ___   __ _| | (_)___  | |_ ___     ___ ___  ___ __ _ _ __   ___                            \n                          \\ V / _ \\| | | | '__| | |  _ / _ \\ / _` | | | / __| | __/ _ \\   / _ / __|/ __/ _` | '_ \\ / _ \\                           \n                           | | (_) | |_| | |    | |_| | (_) | (_| | | | \\__ \\ | || (_) | |  __\\__ | (_| (_| | |_) |  __/                           \n                           |_|\\___/ \\__,_|_|     \\____|\\___/ \\__,_|_| |_|___/  \\__\\___/   \\___|___/\\___\\__,_| .__/ \\___|                           \n                                                                                                            |_|                                    \n                  _   _            _          _                _       _   _              __   _____ _                        _                    \n                 | |_| |__   ___  | |    __ _| |__  _   _ _ __(_)_ __ | |_| |__     ___  / _| | ____| |_ ___ _ __ _ __   __ _| |                   \n                 | __| '_ \\ / _ \\ | |   / _` | '_ \\| | | | '__| | '_ \\| __| '_ \\   / _ \\| |_  |  _| | __/ _ | '__| '_ \\ / _` | |                   \n                 | |_| | | |  __/ | |__| (_| | |_) | |_| | |  | | | | | |_| | | | | (_) |  _| | |___| ||  __| |  | | | | (_| | |                   \n                  \\__|_| |_|\\___| |_____\\__,_|_.__/ \\__, |_|  |_|_| |_|\\__|_| |_|  \\___/|_|   |_____|\\__\\___|_|  |_| |_|\\__,_|_|                   \n                                                    |___/                                                                                          \n",
            guideMessage2 = "        ____ _                     _                     _                                      _               _   _           \n       / ___| |__   __ _ _ __ ___ | |__   ___ _ __ ___  | |__  _   _    __ _ _   _  ___ ___ ___(_)_ __   __ _  | |_| |__   ___  \n      | |   | '_ \\ / _` | '_ ` _ \\| '_ \\ / _ | '__/ __| | '_ \\| | | |  / _` | | | |/ _ / __/ __| | '_ \\ / _` | | __| '_ \\ / _ \\ \n      | |___| | | | (_| | | | | | | |_) |  __| |  \\__ \\ | |_) | |_| | | (_| | |_| |  __\\__ \\__ | | | | | (_| | | |_| | | |  __/ \n       \\____|_| |_|\\__,_|_| |_| |_|_.__/ \\___|_|  |___/ |_.__/ \\__, |  \\__, |\\__,_|\\___|___|___|_|_| |_|\\__, |  \\__|_| |_|\\___| \n                                                               |___/   |___/                            |___/                   ",
            guideMessage3 = "\n                         _                       _                            _                 _ _                                   _            \n     _ __ __ _ _ __   __| | ___  _ __ ___     __| | ___   ___  _ __ ___   ___(_)_ __ ___  _   _| | |_ __ _ _ __   ___  ___  _   _ ___| |_   _      \n    | '__/ _` | '_ \\ / _` |/ _ \\| '_ ` _ \\   / _` |/ _ \\ / _ \\| '__/ __| / __| | '_ ` _ \\| | | | | __/ _` | '_ \\ / _ \\/ _ \\| | | / __| | | | |     \n    | | | (_| | | | | (_| | (_) | | | | | | | (_| | (_) | (_) | |  \\__ \\ \\__ | | | | | | | |_| | | || (_| | | | |  __| (_) | |_| \\__ | | |_| |_    \n    |_|  \\__,_|_| |_|\\__,_|\\___/|_| |_| |_|  \\__,_|\\___/ \\___/|_|  |___/ |___|_|_| |_| |_|\\__,_|_|\\__\\__,_|_| |_|\\___|\\___/ \\__,_|___|_|\\__, (_)   \n                                                                                                                                        |___/      ",
            guideControls = "\n                                                                                                                                                   \n                                                                                                                                                   \n                               /\\    _  __        __             __  __                  _   _                                                     \n                              |/\\|  | | \\ \\      / /            |  \\/  | _____   _____  | | | |_ __                                                \n                                    | |  \\ \\ /\\ / /             | |\\/| |/ _ \\ \\ / / _ \\ | | | | '_ \\                                               \n                                    | |   \\ V  V /              | |  | | (_) \\ V |  __/ | |_| | |_) |                                              \n                                    | |    \\_/\\_/               |_|  |_|\\___/ \\_/ \\___|  \\___/| .__/                                               \n                                    |_|                                                       |_|                                                  \n                                     _   ____                    __  __                   ____                                                     \n                            __   __ | | / ___|                  |  \\/  | _____   _____   |  _ \\  _____      ___ __                                 \n                            \\ \\ / / | | \\___ \\                  | |\\/| |/ _ \\ \\ / / _ \\  | | | |/ _ \\ \\ /\\ / | '_ \\                                \n                             \\ V /  | |  ___) |                 | |  | | (_) \\ V |  __/  | |_| | (_) \\ V  V /| | | |                               \n                              \\_/   | | |____/                  |_|  |_|\\___/ \\_/ \\___|  |____/ \\___/ \\_/\\_/ |_| |_|                               \n                                    |_|                                                                                                            \n                              __     _      _                    __  __                   _          __ _                                          \n                             / /    | |    / \\                  |  \\/  | _____   _____   | |    ___ / _| |_                                        \n                            / /     | |   / _ \\                 | |\\/| |/ _ \\ \\ / / _ \\  | |   / _ | |_| __|                                       \n                            \\ \\     | |  / ___ \\                | |  | | (_) \\ V |  __/  | |__|  __|  _| |_                                        \n                             \\_\\    | | /_/   \\_\\               |_|  |_|\\___/ \\_/ \\___|  |_____\\___|_|  \\__|                                       \n                                    |_|                                                                                                            \n                            __       _   ____                    __  __                  ____  _       _     _                                     \n                            \\ \\     | | |  _ \\                  |  \\/  | _____   _____  |  _ \\(_) __ _| |__ | |_                                   \n                             \\ \\    | | | | | |                 | |\\/| |/ _ \\ \\ / / _ \\ | |_) | |/ _` | '_ \\| __|                                  \n                             / /    | | | |_| |                 | |  | | (_) \\ V |  __/ |  _ <| | (_| | | | | |_                                   \n                            /_/     | | |____/                  |_|  |_|\\___/ \\_/ \\___| |_| \\_|_|\\__, |_| |_|\\__|                                  \n                                    |_|                                                          |___/                                             \n                                                                                                                                                   \n                                                                                                                                                   ",
            leftRightWall = "  ___  \n |   | \n |   | \n |   | \n |   | \n |___| \n       ",
            playAgain = "\t                                                                                                                                                                             \n\t                                                                                                                                                                             \n\t                                                                                                                                                                             \n\t                                                                                                                                                                             \n\t                                                                                                                                                                             \n\t                                                                                                                                                                             \n\t __      ____                                ____    ____       _       ____   _____     _                       _                                             _             \n\t \\ \\    |  _ \\   _ __    ___   ___   ___    / ___|  |  _ \\     / \\     / ___| | ____|   | |_    ___      _ __   | |   __ _   _   _      __ _    __ _    __ _  (_)  _ __      \n\t  \\ \\   | |_) | | '__|  / _ \\ / __| / __|   \\___ \\  | |_) |   / _ \\   | |     |  _|     | __|  / _ \\    | '_ \\  | |  / _` | | | | |    / _` |  / _` |  / _` | | | | '_ \\     \n\t  / /   |  __/  | |    |  __/ \\__ \\ \\__ \\    ___) | |  __/   / ___ \\  | |___  | |___    | |_  | (_) |   | |_) | | | | (_| | | |_| |   | (_| | | (_| | | (_| | | | | | | |  _ \n\t /_/    |_|     |_|     \\___| |___/ |___/   |____/  |_|     /_/   \\_\\  \\____| |_____|    \\__|  \\___/    | .__/  |_|  \\__,_|  \\__, |    \\__,_|  \\__, |  \\__,_| |_| |_| |_| (_)\n\t                                                                                                        |_|                  |___/             |___/                         ",
            player = "  ___  \n /. .\\ \n|  `  |\n \\_-_/ \n /| |\\ \n  |_|  \n  / \\  ",
            roomNumber = "  ____                          \n |  _ \\ ___   ___  _ __ ___  _  \n | |_) / _ \\ / _ \\| '_ ` _ \\(_) \n |  _ | (_) | (_) | | | | | |_  \n |_| \\_\\___/ \\___/|_| |_| |_(_) \n                                ",
            smart = "\t\t\t\t __   __                  _                                   _   _                                               _     _ \n\t\t\t\t \\ \\ / /   ___    _   _  ( )  _ __    ___      __ _   _   _  (_) | |_    ___     ___   _ __ ___     __ _   _ __  | |_  | |\n\t\t\t\t  \\ V /   / _ \\  | | | | |/  | '__|  / _ \\    / _` | | | | | | | | __|  / _ \\   / __| | '_ ` _ \\   / _` | | '__| | __| | |\n\t\t\t\t   | |   | (_) | | |_| |     | |    |  __/   | (_| | | |_| | | | | |_  |  __/   \\__ \\ | | | | | | | (_| | | |    | |_  |_|\n\t\t\t\t   |_|    \\___/   \\__,_|     |_|     \\___|    \\__, |  \\__,_| |_|  \\__|  \\___|   |___/ |_| |_| |_|  \\__,_| |_|     \\__| (_)\n\t\t\t\t                                                 |_|                                                                      ",
            startKey = "\n\n\n\n\n\n\t\t\t\t\t__       ____                             _____  ____   ___    ______ ______   __                  __                __  \n\t\t\t\t\t\\ \\     / __ \\ _____ ___   _____ _____   / ___/ / __ \\ /   |  / ____// ____/  / /_ ____     _____ / /_ ____ _ _____ / /_ \n\t\t\t\t\t \\ \\   / /_/ // ___// _ \\ / ___// ___/   \\__ \\ / /_/ // /| | / /    / __/    / __// __ \\   / ___// __// __ `// ___// __/ \n\t\t\t\t\t / /  / ____// /   /  __/(__  )(__  )   ___/ // ____// ___ |/ /___ / /___   / /_ / /_/ /  (__  )/ /_ / /_/ // /   / /_ _ \n\t\t\t\t\t/_/  /_/    /_/    \\___//____//____/   /____//_/    /_/  |_|\\____//_____/   \\__/ \\____/  /____/ \\__/ \\__,_//_/    \\__/(_)",
            topBottomWall = "       \n _____ \n|     |\n|_____|\n       \n       \n       ",
            whitespace = "       \n       \n       \n       \n       \n       \n       ",
            whitespace2 = "   \n   \n   \n   \n   \n   ",
            wrongDoors1 = " __        __                       ____                          \n \\ \\      / / __ ___  _ __   __ _  |  _ \\  ___   ___  _ __ ___ _  \n  \\ \\ /\\ / / '__/ _ \\| '_ \\ / _` | | | | |/ _ \\ / _ \\| '__/ __(_) \n   \\ V  V /| | | (_) | | | | (_| | | |_| | (_) | (_) | |  \\__ \\_  \n    \\_/\\_/ |_|  \\___/|_| |_|\\__, | |____/ \\___/ \\___/|_|  |___(_) \n                            |___/                                 ",
            wrongDoors2 = "\t\t\t\t\t    __        __                                   ____                                     \n\t\t\t\t\t    \\ \\      / /  _ __    ___    _ __     __ _    |  _ \\    ___     ___    _ __   ___   _   \n\t\t\t\t\t     \\ \\ /\\ / /  | '__|  / _ \\  | '_ \\   / _` |   | | | |  / _ \\   / _ \\  | '__| / __| (_)  \n\t\t\t\t\t      \\ V  V /   | |    | (_) | | | | | | (_| |   | |_| | | (_) | | (_) | | |    \\__ \\  _   \n\t\t\t\t\t       \\_/\\_/    |_|     \\___/  |_| |_|  \\__, |   |____/   \\___/   \\___/  |_|    |___/ (_)  \n\t\t\t\t\t                                         |___/                                              ";


        // Number to ASCII Art Number.
        public static string ConvertNumber(string numberToken)
        {
            numberToken = numberToken.Replace("%", "                                                ");
            numberToken = numberToken.Replace("0", "   ___    / _ \\  | | | | | |_| |  \\___/         ");
            numberToken = numberToken.Replace("1", "   __     /  |    |  |    |  |    |__|          ");
            numberToken = numberToken.Replace("2", "  ____   |___ \\    __) |  / __/  |_____|        ");
            numberToken = numberToken.Replace("3", "  _____  |___ /    |_ \\   ___) | |____/         ");
            numberToken = numberToken.Replace("4", " _  _   | || |  | || |_ |__   _|   |_|          ");
            numberToken = numberToken.Replace("5", "  ____   | ___|  |___ \\   ___) | |____/         ");
            numberToken = numberToken.Replace("6", "   __     / /_   | '_ \\  | (_) |  \\___/         ");
            numberToken = numberToken.Replace("7", "  _____  |___  |    / /    / /    /_/           ");
            numberToken = numberToken.Replace("8", "   ___    ( _ )   / _ \\  | (_) |  \\___/         ");
            numberToken = numberToken.Replace("9", "   ___    / _ \\  | (_) |  \\__, |    /_/         ");

            return numberToken;
        }
    }
}
