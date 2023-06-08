
Helper helper = new Helper();
Game game = new Game();
helper.Start();

class Helper
{


    public void Start()
    {
        Console.WriteLine("\nMatematik oyununa hoş geldiniz, lütfen isminizi giriniz:");
        string? name = Console.ReadLine();
        Console.WriteLine("\nKurallar Şöyle: \n* Karşınıza gelen işlemin sonucunu sayfaya girmelisiniz, her hangi bir zaman limii yok \n* Beş tane hata yapma hakkınız var, Canlarınızı verilen işlemin altında bulunan ♥ sembollerinde görebilirsiniz \n* Oyunu oynarken her an 'Exit' Yazarak oyundan çıkabilirsiniz.\n*Oyun ilerledikçe zorlaşmaya başlıyacak, belli seviyeleri geçince uyarılacaksınız\nEğer kesirli bir cevap vermeniz gerekiyorsa sadece virgülden sonraki ilk sayıyı girin(örn: 2/3 = 0,6). Eğer nokta (.) kullanırsanız cevabınız yanlış kabul edilecektir\n\nDevam etmek için her hangi bir tuşa basın");
        Console.ReadKey();
        Display();
    }
    public void Display()
    {
        Game game = new Game();
        int score = 0;
        int level = 0;
        int lives = 5;
        string livesHud = "♥♥♥♥♥";
        int InputState = 0;
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
            double answerCorrect = game.GenerateAnswer(currqst[0], currqst[1], currqst[2]);

            Console.WriteLine($"\n{game.GenerateDisplay(currqst[0], currqst[1], currqst[2])}\n" +
                $"\nŞu anki Skorunuz: {score}               Can: {livesHud} \n");
            string answer = Console.ReadLine();
            InputState = game.CheckAnswer(answer, answerCorrect);
            switch (InputState)
            {
                case 0:
                    Console.WriteLine($"\n\nDoğru Cevap Verdiniz!\n+{currqst[3]} Puan!");
                    score = score + currqst[3];
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
                    break;


            }
            {

            }
        }
    }
}


public class Game
    {
        //, answer;
        public int score, lives;
        Random rnd = new Random();
        Helper helper = new Helper();
        public bool level2, level3;
        public string? livesHud;
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
        public double GenerateAnswer(int num1, int num2, int oper)
        {
        double a = num1;
        double b = num2;
            switch (oper)
            {
                case 1:
                    return num1 - num2;
                case 2:
                    return num1 * num2;
                case 3:
                return Math.Floor(a / b * 10)/10;
                case 4:
                    return num1 % num2;
                default:
                    return num1 + num2;
            }

        }
        public string GenerateDisplay(int num1, int num2, int oper)
        {
            switch (oper)
            {
                case 1:
                    return $"\n{num1} - {num2} = ";
                case 2:
                    return $"\n{num1} X {num2} = ";
                case 3:
                    return $"\n{num1} / {num2} = ";
                case 4:
                    return $"\n{num1} % {num2} = ";
                default:
                    return $"\n{num1} + {num2} = ";
            }

        }
        public int CheckAnswer(string answer, double correctAnswer)
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

