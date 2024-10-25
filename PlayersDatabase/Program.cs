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

        public Database()
        {
            _players = new List<Player>();
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
                        AddPlayer();
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

        private int GetIdFromUser()
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                return playerId;
            }

            return -1;
        }

        private void AddPlayer()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите начальный уровень: ");

            if (int.TryParse(Console.ReadLine(), out int startLevel))
            {
                Random random = new();
                int id = random.Next(0, int.MaxValue);

                while (PlayerExist(id))
                {
                    id = random.Next(0, int.MaxValue);
                }

                _players.Add(new(name, startLevel, id));
            }
        }

        private void RemovePlayer(int playerId)
        {
            if (TryGetPlayer(playerId, out Player player))
            {
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

        private bool PlayerExist(int playerId)
        {
            return TryGetPlayer(playerId, out Player _);
        }

        private void ShowAllPlayers()
        {
            foreach (var player in _players)
            {
                player.ShowInfo();   
            }
        }

        private void BanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (player.IsBanned == false)
                {
                    player.Ban();
                }
            }
        }

        private void UnbanPlayer(int playerID)
        {
            if (TryGetPlayer(playerID, out Player player))
            {
                if (player.IsBanned)
                {
                    player.Unban();
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

        public bool IsBanned { get; private set; }

        public void Ban()
        {
            IsBanned = true;
        }

        public void Unban()
        {
            IsBanned = false;
        }

        public void ShowInfo()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            if (IsBanned)
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
