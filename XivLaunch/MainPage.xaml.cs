using Config.Net;
using CoreLibLaunchSupport;


namespace XivLaunch
{
    
    public partial class MainPage : ContentPage
    {
        private static Storage storage;
        int count = 0;
        public interface IMySettings
        {
            
            string UserName { get; set;}

            string Passworders { get; set; }
            string Langnumber { get; set; }
            string GameLocation { get; set; }
        }
        public MainPage()
        {
            storage = new Storage("protocolhandle");
            IMySettings settings = new ConfigurationBuilder<IMySettings>().UseJsonFile(storage.GetFile("launcher.json").FullName)
   .Build();
            InitializeComponent();
            if (settings.UserName != null)
            {
                LoginBox.Text= settings.UserName ;
            }
            if (settings.Passworders != null)
            {
                PASS.Text = settings.Passworders  ;
            }

            if (settings.Langnumber != null)
            {
                lnggcheck.Text = settings.Langnumber;
            }
            if (settings.GameLocation != null)
            {
                gamepathtextb.Text = settings.GameLocation;
            }

        }
        public static string GetExpansionFolder(byte expansionId) =>
            expansionId == 0 ? "ffxiv" : $"ex{expansionId}";
        public static string ReturnXpacNum(ushort expansionId)
        {
            var processxpac = GetExpansionFolder((byte)expansionId);
            return processxpac;
        }
        async private void Button_Clicked(object sender, EventArgs args)
        {
            storage = new Storage("protocolhandle");
            IMySettings settings = new ConfigurationBuilder<IMySettings>().UseJsonFile(storage.GetFile("launcher.json").FullName)
   .Build();
            if(LoginBox.Text != null)
            {
                settings.UserName = LoginBox.Text;
            } 
            if(PASS.Text != null)
            {
                settings.Passworders = PASS.Text;
            }

            if (lnggcheck.Text != null)
            {
                settings.Langnumber = lnggcheck.Text;
            }
            if (gamepathtextb.Text != null)
            {
                settings.GameLocation = gamepathtextb.Text;
            }
            
            
            string gamePath;
            
            Console.Write("Введите путь до клиента игры - ");
            gamePath = gamepathtextb.Text;
                /* TextWriter tw = new StreamWriter("gamepath.txt");
                 tw.WriteLine(gamePath);
                 tw.Close();*/
            
            Console.WriteLine("-------------------------------------");
            bool isSteam = false;

            if (steamcheck.IsChecked == true)
            {
                isSteam = true;
            }
            else
            {
                isSteam = false;
            }
            Console.WriteLine("-------------------------------------");
            Console.Write("Имя пользователя - ");
            string username = LoginBox.Text;
            //Console.WriteLine("Provided username {0}", username);
            Console.Write("Пароль - ");
            string password = PASS.Text;
            //string maskpassword = "";
            //for (int i = 0; i < password.Length; i++) { 
            //maskpassword += "*"; 
            //}


            //Console.Write("Your Password is:" + maskpassword);
            Console.WriteLine();

            Console.Write("Код Двух-Факторной аутентификации - ");
            string otp = otptext.Text;
            bool dx11 = true;

            if (dx11chck.IsChecked == true)
            {
                dx11 = true;
            }
            else
            {
                dx11 = false;
            }
            Console.WriteLine("Пожалуйста, введите уровень доступного вам дополнения - на текущий момент валидными являются следущие \n 0- ARR - 1 - Heavensward - 2 - Stormblood - 3 - Shadowbringers");
            int expansionLevel = 0;
            var xpacPath = "";
            if (ReturnXpacNum(1) == "ex1") { xpacPath = "ex1"; }
            if (ReturnXpacNum(2) == "ex2") { xpacPath = "ex2"; }
            if (ReturnXpacNum(3) == "ex3") { xpacPath = "ex3"; }
            if (ReturnXpacNum(4) == "ex4") { xpacPath = "ex4"; }
            var sqpack = $@"{gamePath}\sqpack\{xpacPath}";

            if (xpacPath == "ex1")
            {
                expansionLevel = 1;
                Console.WriteLine(expansionLevel);
            }
            if (xpacPath == "ex2")
            {
                expansionLevel = 2;
                Console.WriteLine(expansionLevel);
            }
            if (xpacPath == "ex3")
            {
                expansionLevel = 3;
                Console.WriteLine(expansionLevel);
            }
            if (xpacPath == "ex4")
            {
                expansionLevel = 4;
                Console.WriteLine(expansionLevel);
            }
            Console.Write("Укажите регион установленного клиента. Действующие в настоящее время \n 1- Japan , 2 - America , 3 - International: - ");
            int region = 3;
            int langs = int.Parse(lnggcheck.Text);
            try
            {
                var sid = networklogic.GetRealSid(gamePath, username, password, otp, isSteam);
                if (sid.Equals("BAD"))
                    return;

                var ffxivGame = networklogic.LaunchGameAsync(gamePath, sid, langs, dx11, expansionLevel, isSteam, region);



            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.ReadLine();
        }
    }
}