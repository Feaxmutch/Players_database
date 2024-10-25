using System.Numerics;

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
                        RemovePlayer();
                        break;

                    case CommandBanPlayer:
                        BanPlayer();
                        break;

                    case CommandUnbanPlayer:
                        UnbanPlayer();
                        break;
                }
            }
        }

        private void AddPlayer()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите начальный уровень: ");

            if (int.TryParse(Console.ReadLine(), out int startLevel))
            {
                int id = GenerateId();
                _players.Add(new(name, startLevel, id));
            }
        }

        private int GenerateId()
        {
            string idInString = string.Empty;
            DateTime dateTime = DateTime.Now;

            idInString += dateTime.DayOfYear;
            idInString += dateTime.Hour;
            idInString += dateTime.Minute;
            idInString += dateTime.Second;

            return int.Parse(idInString);
        }

        private void RemovePlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                _players.Remove(player);
            }
        }

        private bool TryGetPlayer(out Player player)
        {
            Console.Clear();
            ShowAllPlayers();
            Console.Write("Введите id игрока: ");

            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                foreach (var currentPlayer in _players)
                {
                    if (currentPlayer.Id == playerId)
                    {
                        player = currentPlayer;
                        return true;
                    }
                }
            }

            player = null;
            return false;
        }

        private void ShowAllPlayers()
        {
            foreach (var player in _players)
            {
                player.ShowInfo();   
            }
        }

        private void BanPlayer()
        {
            if (TryGetPlayer(out Player player))
            {
                if (player.IsBanned == false)
                {
                    player.Ban();
                }
            }
        }

        private void UnbanPlayer()
        {
            if (TryGetPlayer(out Player player))
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
