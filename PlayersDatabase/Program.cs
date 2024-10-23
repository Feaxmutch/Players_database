namespace PlayersDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new();
            database.ShowMainMenu();
        }
    }

    public class Database
    {
        private List<Player> _players;
        private List<Player> _bannedPlayers;

        public Database()
        {
            _players = new List<Player>();
            _bannedPlayers = new List<Player>();
        }

        public void ShowMainMenu()
        {
            const string CommandAddPlayer = "1";
            const string CommandRemovePlayer = "2";
            const string CommandBanPlayer = "3";
            const string CommandUnbanPlayer = "4";

            bool isActive = true;

            while (isActive)
            {
                Console.Clear();
                Console.WriteLine($"{CommandAddPlayer}) Добавить игрока" +
                                $"\n{CommandRemovePlayer}) Удалить игрока" +
                                $"\n{CommandBanPlayer}) Забанить игрока" +
                                $"\n{CommandUnbanPlayer}) Разбанить игрока");
                string userCommand = Console.ReadLine();

                switch (userCommand)
                {
                    case CommandAddPlayer:
                        ShowAddPlayerMenu();
                        break;

                    case CommandRemovePlayer:
                        ShowRemovePlayerMenu();
                        break;

                    case CommandBanPlayer:
                        ShowBanMenu();
                        break;

                    case CommandUnbanPlayer:
                        ShowUnbanMenu();
                        break;
                }
            }
        }

        public bool IsBanned(int playerId)
        {
            foreach (var player in _bannedPlayers)
            {
                if (player.Id == playerId)
                {
                    return true;
                }
            }

            return false;
        }

        private void ShowAddPlayerMenu()
        {
            Console.Clear();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите начальный уровень: ");

            if (int.TryParse(Console.ReadLine(), out int startLevel))
            {
                AddPlayer(name, startLevel);
            }
        }

        private void ShowRemovePlayerMenu()
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                RemovePlayer(playerId);
            }
        }

        private void AddPlayer(string name, int startLevel)
        {
            Random random = new();
            int id = random.Next(0, int.MaxValue);

            while (PlayerExist(id))
            {
                id = random.Next(0, int.MaxValue);
            }

            _players.Add(new(name, startLevel, id, this));
        }

        private void RemovePlayer(int playerId)
        {
            if (TryGetPlayer(playerId, out Player player))
            {
                if (player.IsBanned())
                {
                    UnbanPlayer(player.Id);
                }

                _players.Remove(player);
            }
        }

        private bool TryGetPlayer(int playerId, out Player player)
        {
            foreach (var currentPlayer in _players)
            {
                if (currentPlayer.Id == playerId)
                {
                    player = currentPlayer;
                    return true;
                }
            }

            player = null;
            return false;
        }

        private bool PlayerExist(int playerID)
        {
            return TryGetPlayer(playerID, out Player _);
        }

        private void ShowAllPlayers()
        {
            foreach (var player in _players)
            {
                player.ShowInfo();   
            }
        }

        private void ShowBanMenu()
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока для бана: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                if (PlayerExist(playerId))
                {
                    BanPlayer(playerId);
                }
            }
        }

        private void ShowUnbanMenu()
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока для разбана: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                if (PlayerExist(playerId))
                {
                    UnbanPlayer(playerId);
                }
            }
        }

        private void BanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (player.IsBanned() == false)
                {
                    _bannedPlayers.Add(player);
                }
            }
        }

        private void UnbanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (player.IsBanned())
                {
                    _bannedPlayers.Remove(player);
                }
            }
        }
    }

    public class Player
    {
        Database _database;

        public Player(string name, int startLevel, int id, Database database)
        {
            Name = name;
            Level = startLevel;
            Id = id;
            _database = database;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public int Level { get; private set; }

        public bool IsBanned()
        {
            return _database.IsBanned(Id);
        }

        public void ShowInfo()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            if (IsBanned())
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine($"{Id} | {Name} | {Level} ");
            Console.ForegroundColor = defaultColor;
        }
    }
}
