
Helper helper = new();
helper.Start();

class Helper
{
    string? name;
    int topScore = 0;

    public void Start()
    {

        Console.WriteLine("\nMatematik oyununa hoş geldiniz, lütfen isminizi giriniz:");
        name = Console.ReadLine();
        Console.WriteLine("\nKurallar Şöyle: \n* Karşınıza gelen işlemin sonucunu sayfaya girmelisiniz, her hangi bir zaman limii yok \n* Beş tane hata yapma hakkınız var, Canlarınızı verilen işlemin altında bulunan ♥ sembollerinde görebilirsiniz \n* Oyunu oynarken her an 'Exit' Yazarak oyundan çıkabilirsiniz.\n*Oyun ilerledikçe zorlaşmaya başlıyacak, belli seviyeleri geçince uyarılacaksınız\nEğer kesirli bir cevap vermeniz gerekiyorsa sadece virgülden sonraki ilk sayıyı girin(örn: 2/3 = 0,6). Eğer nokta (.) kullanırsanız cevabınız yanlış kabul edilecektir\n\nDevam etmek için her hangi bir tuşa basın");
        Console.ReadKey();
        Display();
    }

    public void End()
    {
        var readakey = Console.ReadKey();
        if (readakey.Key == ConsoleKey.Q)
        {
            Console.WriteLine("Oynadığınız için teşekkürler");
            return;
        }
        else
        {
            Display();
        }
    }
    public void Display()
    {
        Game game = new();
        int score = 0;
        int level = 0;
        int lives = 5;
        string livesHud = "♥♥♥♥♥";
        while (lives > 0)
        {
            if ((score == 30) && (level == 0))
            {
                Console.WriteLine("Sonraki seviyeye Geçtiniz!");
                level++;
            }
            if ((score == 80) && (level == 1))
            {
                Console.WriteLine("Son Seviyeye Geldiniz!");
                level++;
            }

            int[] currqst = game.GenerateQuestion(score);
            double answerCorrect = Game.GenerateAnswer(currqst[0], currqst[1], currqst[2]);

            Console.WriteLine($"\n{Game.GenerateDisplay(currqst[0], currqst[1], currqst[2])}\n" +
                $"\nŞu anki Skorunuz: {score}               Can: {livesHud} \n");
            string answer = Console.ReadLine();
            int InputState = Game.CheckAnswer(answer, answerCorrect);
            switch (InputState)
            {
                case 0:
                    Console.WriteLine($"\n\nDoğru Cevap Verdiniz!\n+{currqst[3]} Puan!");
                    score += currqst[3];
                    break;
                case 1:
                    Console.WriteLine($"\n\nYanlış cevap veridiniz, Doğru cevap: {answerCorrect}");
                    lives--;
                    livesHud = livesHud.Remove(lives);
                    break;
                case 2:
                    Console.WriteLine("\n\nOyun durduruluyor, puanınız hesaplanacak...");
                    lives = 0;
                    break;
                default:
                    Console.WriteLine("Yanlış birşey girdiniz... Yeni soru soruluyor");
                    //  score += 30;
                    //only uncomment above for testing
                    break;


            }

        }
        if (score <= topScore)
        {
            Console.WriteLine($"\n\n{name}, Skorunuz: {score}       En yüksek Skorunuz {topScore}\n\n" +
                $"Tekrar oynamak için her hangi bir tuşa tıklayın, çıkmak için Q tuşuna tıklayın.");
            End();
        }
        else
        {
            Console.WriteLine($"\n\n{name}, Eski En Yüksek Skorunuzu Aştınız!\n\n" +
                $"Yeni En Yüksek Skorunuz: {score}       Eski En yüksek skorunuz: {topScore}\n\n" +
                $"Tekrar oynamak için her hangi bir tuşa tıklayın, çıkmak için Q tuşuna tıklayın");
            topScore = score;
            End();
            {

        }
        }
    }
}


public class Game
    {
    readonly Random rnd = new();
        public int[] GenerateQuestion(int score)
        {
            int[] Qst = new int[4];
            if (score < 30)
            {
                Qst[0] = rnd.Next(1, 100);
                Qst[1] = rnd.Next(1, 100);
                Qst[2] = rnd.Next(2);
                Qst[3] = 5;
            }
            else if (score >= 30 && score < 80)
            {
                Qst[0] = rnd.Next(1, 1000);
                Qst[1] = rnd.Next(1, 1000);
                Qst[2] = rnd.Next(4);
                Qst[3] = 10;
            }
            else //score >= 80
            {
                Qst[0] = rnd.Next(1, 10000);
                Qst[1] = rnd.Next(1, 100000);
                Qst[2] = rnd.Next(5);
                Qst[3] = 20;
            }
            return Qst;
        }
        public static double GenerateAnswer(int num1, int num2, int oper)
        {
        double a = num1;
        double b = num2;
        return oper switch
        {
            1 => num1 - num2,
            2 => num1 * num2,
            3 => Math.Floor(a / b * 10) / 10,
            4 => num1 % num2,
            _ => num1 + num2,
        };
    }
        public static string GenerateDisplay(int num1, int num2, int oper)
        {
        return oper switch
        {
            1 => $"\n{num1} - {num2} = ",
            2 => $"\n{num1} X {num2} = ",
            3 => $"\n{num1} / {num2} = ",
            4 => $"\n{num1} % {num2} = ",
            _ => $"\n{num1} + {num2} = ",
        };
    }
        public static int   CheckAnswer(string answer, double correctAnswer)
        {
            answer = answer.Replace('.', ',');
;            if (double.TryParse(answer, out double answerduo))
            {
                if (Math.Equals(answerduo, correctAnswer))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }

            }
            else if ((answer.IndexOf("Exit", StringComparison.OrdinalIgnoreCase) != -1) && (answer.Length == 4))
            {
                return 2;
            }
            else
            {
                return 3;
            }

        }
    }

