

using static System.Formats.Asn1.AsnWriter;

UI ui = new();
ui.Start();

class UI
{
    string? name;
    int topScore = 0;

    public void Start()
    {

        Console.WriteLine("\nMatematik oyununa hoş geldiniz, lütfen isminizi giriniz:");
        name = Console.ReadLine();
        Console.WriteLine("\nKurallar Şöyle: \n* Karşınıza gelen işlemin sonucunu sayfaya girmelisiniz, her hangi bir zaman limii yok \n* Beş tane hata yapma hakkınız var, Canlarınızı verilen işlemin altında bulunan ♥ sembollerinde görebilirsiniz \n* Oyunu oynarken her an 'Exit' Yazarak oyundan çıkabilirsiniz.\n*Oyun ilerledikçe zorlaşmaya başlıyacak, belli seviyeleri geçince uyarılacaksınız\nEğer kesirli bir cevap vermeniz gerekiyorsa sadece virgülden sonraki ilk sayıyı girin(örn: 2/3 = 0,6). \n\nDevam etmek için her hangi bir tuşa basın");
        Console.ReadKey();
        GameinProgress();
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
            GameinProgress();
        }
    }
    public void GameinProgress()
    {
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
            else if((score == 80) && (level == 1))
            {
                Console.WriteLine("Son Seviyeye Geldiniz!");
                level++;
            }

            int[] currqst = Game.GenerateQuestion(score);
            double answerCorrect = Game.GenerateAnswer(currqst[0], currqst[1], currqst[2]);

            Console.WriteLine(Game.GenerateDisplay(currqst[0], currqst[1], currqst[2], score, livesHud));
            string answer = Console.ReadLine();
            int inputState = Game.CheckAnswer(answer, answerCorrect);
            Console.WriteLine(Game.ResultString(inputState, currqst[3], answerCorrect));
            lives = Game.ResultHealth(inputState, lives);
            livesHud = Game.ResultHealthHud(inputState, lives, livesHud);
            score += Game.ResultScore(inputState, currqst[3]);

        }
            Console.WriteLine(Game.EndString(score, topScore, name));
            topScore = Math.Max(score, topScore);
            End();

       }
}
    



public class Game
{
    public static int[] GenerateQuestion(int score)
    {
        Random rnd = new();

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
    public static string GenerateDisplay(int num1, int num2, int oper, int score, string hud)
    {
        return oper switch
        {
            1 => $"\n\n{num1} - {num2} = \n\nŞu anki Skorunuz: {score}               Can: {hud} \n",
            2 => $"\n\n{num1} X {num2} = \n\nŞu anki Skorunuz: {score}               Can: {hud} \n",
            3 => $"\n\n{num1} / {num2} = \n\nŞu anki Skorunuz: {score}               Can: {hud} \n",
            4 => $"\n\n{num1} % {num2} = \n\nŞu anki Skorunuz: {score}               Can: {hud} \n",
            _ => $"\n\n{num1} + {num2} = \n\nŞu anki Skorunuz: {score}               Can: {hud} \n",
        };
    }
    public static int CheckAnswer(string answer, double correctAnswer)
    {
        answer = answer.Replace('.', ',');
        ; if (double.TryParse(answer, out double answerduo))
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
    public static string ResultString(int state, int addScore, double correctAnswer)
    {
        return state switch
        {
            0 => $"\n\nDoğru Cevap Verdiniz!\n+{addScore} Puan!",
            1 => $"\n\nYanlış cevap veridiniz, Doğru cevap: {correctAnswer}",
            2 => "\n\nOyun durduruluyor, puanınız hesaplanacak...",
            _ => "Yanlış birşey girdiniz... Yeni soru soruluyor",
        };
    }
    public static int ResultHealth(int state, int lives)
    {
        return state switch
        {
            1 => lives - 1,
            2 => 0,
            _ => lives,
        };
    }
    public static int ResultScore(int state, int addScore)
    {
        return state switch
        {
            0 => addScore,
            _ => 0,
        };
    }
    public static string ResultHealthHud(int state, int lives, string liveshud)
    {
        return state switch
        {
            1 => liveshud.Remove(lives),
            _ => liveshud,
        };
    }
    public static string EndString(int score, int topScore, string name)
    {
        if (score <= topScore)
        {
            return $"\n\n{name}, Skorunuz: {score}       En yüksek Skorunuz {topScore}\n\nTekrar oynamak için her hangi bir tuşa tıklayın, çıkmak için Q tuşuna tıklayın.";
        }
        else
        {
            return $"\n\n{name}, Eski En Yüksek Skorunuzu Aştınız!\n\nYeni En Yüksek Skorunuz: {score}       Eski En yüksek skorunuz: {topScore}\n\nTekrar oynamak için her hangi bir tuşa tıklayın, çıkmak için Q tuşuna tıklayın";
        }
    }
}

