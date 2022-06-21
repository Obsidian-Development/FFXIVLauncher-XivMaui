using LibLaunchSupport;

namespace XivLaunch
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs args)
        {
            string gamePath;
            if (File.Exists(Directory.GetCurrentDirectory() + @"\gamepath.txt"))
            {
                TextReader tr = new StreamReader("gamepath.txt");
                string gamePathread = tr.ReadLine();
                gamePath = gamePathread;
                tr.Close();
                Console.WriteLine(gamePath);
            }
            else
            {
                Console.Write("Введите путь до клиента игры - ");
                gamePath = gamepathtextb.Text;
                /* TextWriter tw = new StreamWriter("gamepath.txt");
                 tw.WriteLine(gamePath);
                 tw.Close();*/
            }
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
            int expansionLevel = int.Parse(expchck.Text);
            Console.Write("Укажите регион установленного клиента. Действующие в настоящее время \n 1- Japan , 2 - America , 3 - International: - ");
            int region = int.Parse(regioncheck.Text);
            int langs = int.Parse(lnggcheck.Text);
            try
            {
                var sid = networklogic.GetRealSid(gamePath, username, password, otp, isSteam);
                if (sid.Equals("BAD"))
                    return;

                var ffxivGame = networklogic.LaunchGame(gamePath, sid, langs, dx11, expansionLevel, isSteam, region);



            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            Console.ReadLine();
        }
    }
}