namespace PlayersDatabase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new();
            database.ShowMenu();
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

        public void ShowMenu()
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
                        AddPlayer(GetNameFromUser(), GetStarLevelFromUser());
                        break;

                    case CommandRemovePlayer:
                        RemovePlayer(GetIdFromUser());
                        break;

                    case CommandBanPlayer:
                        BanPlayer(GetIdFromUser());
                        break;

                    case CommandUnbanPlayer:
                        UnbanPlayer(GetIdFromUser());
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

        private int GetIdFromUser()
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                return playerId;
            }

            return -1;
        }

        private string GetNameFromUser()
        {
            Console.Write("Введите имя: ");
            return Console.ReadLine();
        }

        private int GetStarLevelFromUser()
        {
            Console.Write("Введите начальный уровень: ");

            if (int.TryParse(Console.ReadLine(), out int startLevel))
            {
                return startLevel;
            }

            return -1;
        }

        private void AddPlayer(string name, int startLevel)
        {
            Random random = new();
            int id = random.Next(0, int.MaxValue);

            while (PlayerExist(id))
            {
                id = random.Next(0, int.MaxValue);
            }

            _players.Add(new(name, startLevel, id));
        }

        private void RemovePlayer(int playerId)
        {
            if (TryGetPlayer(playerId, out Player player))
            {
                if (IsBanned(player.Id))
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
            ConsoleColor defaultColor = Console.ForegroundColor;

            foreach (var player in _players)
            {
                if (IsBanned(player.Id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                player.ShowInfo();   
            }
        }

        private void BanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (IsBanned(player.Id) == false)
                {
                    _bannedPlayers.Add(player);
                }
            }
        }

        private void UnbanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (IsBanned(player.Id))
                {
                    _bannedPlayers.Remove(player);
                }
            }
        }
    }

    public class Player
    {
        public Player(string name, int startLevel, int id)
        {
            Name = name;
            Level = startLevel;
            Id = id;
        }

        public int Id { get; }

        public string Name { get; private set; }

        public int Level { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{Id} | {Name} | {Level} ");
        }
    }
}
