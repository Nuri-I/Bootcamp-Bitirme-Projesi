using Bitirme_Projesi;
bool playedBefore = false;
Random rnd = new Random();
playerObj game = new playerObj();
QuestionObj qst = new QuestionObj();
bool level2 = false;
bool level3 = false;


//Oyun tekrar tekrar başlatıldığı için ve oyunun farklı yerlerine gidildiği için  oyun tamamen modüller içine yazılmıştır, player.CS içinde kullanılan classları bulabilirsiniz
//Begin() sayfada direk olarak çağıralan tek metod, diğer metodlar ona  bağlı olarak çalışmakta
void begin()
{
//Begin modülünün ilk işi oyun sırasında değişecek olan değişkenleri sıfılamak
    level2 = false;
    level3 = false;
    game.lives = 5;
    game.livesHud = "♥♥♥♥♥";
    Console.WriteLine("\nMatematik oyununa hoş geldiniz, lütfen isminizi giriniz:");
    game.name = Console.ReadLine();
    if (!playedBefore)
    {
        Console.WriteLine("\nKurallar Şöyle: \n* Karşınıza gelen işlemin sonucunu sayfaya girmelisiniz, her hangi bir zaman limii yok \n* Beş tane hata yapma hakkınız var, Canlarınızı verilen işlemin altında bulunan ♥ sembollerinde görebilirsiniz \n* Oyunu oynarken her an 'Exit' Yazarak oyundan çıkabilirsiniz.\n*Oyun ilerledikçe zorlaşmaya başlıyacak, belli seviyeleri geçince uyarılacaksınız\nEğer kesirli bir cevap vermeniz gerekiyorsa sadece virgülden sonraki ilk sayıyı girin(örn: 2/3 = 0,6). Eğer nokta (.) kullanırsanız cevabınız yanlış kabul edilecektir\n\nDevam etmek için her hangi bir tuşa basın");
        playedBefore = true;
    }
    oyun();
}


//Oyunun asıl hesaplarının yapıldığı mmetod
    void oyun()
{
//Switch staementleri kullanmak isterdim bu bölümde ama game.score öğesini ayarlamak için mutlaka if kullanmak zorundaydım
    if (game.lives > 0)
    {
        if (game.score < 30)
        {
            qst.num1 = rnd.Next(1, 100);
            qst.num2 = rnd.Next(1, 100);
            qst.oper = rnd.Next(2);
            qst.bonusScore = 5;
        }
        if (game.score >= 30 && game.score < 80)
        {
            qst.num1 = rnd.Next(1, 1000);
            qst.num2 = rnd.Next(1, 1000);
            qst.oper = rnd.Next(4);
            qst.bonusScore = 10;
//bu yazımın normal olmadığının farkındayım ancak yer küçültüğü için seviye geçişi alarmlarını buraya koydum, okunulabilirlik adına hud için ayrı bölüm açsaydım buradan sadece
//bir güncelleme alıp Hud bölümünün altına yazardım
            if (!level2)
            {
                Console.WriteLine("\n Tebrikler! bir üst seviyeye geçtiniz, işler zorlaşıyor \n");
                level2 = true;
            }
        }
        if (game.score >= 80)
        {
            qst.num1 = rnd.Next(1, 10000);
            qst.num2 = rnd.Next(1, 100000);
            qst.oper = rnd.Next(5);
            qst.bonusScore = 20;
            if (!level3)
            {
                Console.WriteLine("\n Üçüncü seviyeye geçtiniz! Buradan ötesi yok! \n");
                level3 = true;
            }
        }
        switch (qst.oper)
        {
            case 0:
                qst.answer = qst.num1 + qst.num2;
                Console.WriteLine("\n" + qst.num1 + " + " + qst.num2 + " = ");
                break;
            case 1:

                qst.answer = qst.num1 - qst.num2;
                Console.WriteLine("\n" + qst.num1 + " - " + qst.num2 + " = ");
                break;
            case 2:
                qst.answer = qst.num1 * qst.num2;
                Console.WriteLine("\n" + qst.num1 + " X " + qst.num2 + " = ");
                break;
            case 3:

                qst.answer = Math.Round(qst.num1 / qst.num2, 1);
                Console.WriteLine("\n" + qst.num1 + " / " + qst.num2 + " = ");
                break;
            case 4:

                qst.answer = qst.num1 % qst.num2;
                Console.WriteLine("\n" + qst.num1 + " % " + qst.num2 + " = ");
                break;
        }
        Console.WriteLine("\nŞu anki Skorunuz: " + game.score + "          Can: " + game.livesHud + "\n");

            getAnswer();
    }
    else
    {
        score();
    }
}

//Kullanıcıdan cevap alan ve bu cevabın kaşılığını veren metod, yanlış girişlerde kullanıcıyı sıfırdan başlatmak istemediğim için ayrı bir metod olarak kodladım
void getAnswer()
{
    game.answer = Console.ReadLine();
    if (double.TryParse(game.answer, out double answer))
    {
        if (answer == qst.answer)
        {
            Console.WriteLine("\nDoğru Cevap!\n");
            game.score += qst.bonusScore;
            oyun();
        }
        else
        {
            Console.WriteLine("\nCevap Yanlış!\n");
            game.lives -= 1;
            game.livesHud = "";
            for (int i = game.lives; i > 0; i--)
            {
                game.livesHud = game.livesHud.Insert(game.livesHud.Length, "♥");
            }
            oyun();
        }
    }
    else if ((game.answer.IndexOf("Exit", StringComparison.OrdinalIgnoreCase) != -1) && (game.answer.Length == 4))
    {
        score();
    }


    //else if (game.answer.IndexOf("win", StringComparison.OrdinalIgnoreCase) != -1)
    //{
    //    game.score += 20;
    //    oyun();

    //}
    //else if (game.answer.IndexOf("new", StringComparison.OrdinalIgnoreCase) != -1)
    //{
    //    oyun();
    //}
    //yukarıda Kodlama sırasında test için kullandığım kodlar var, uncomment ederek kullanabilirsiniz, ilki otomatik skor yükleme ikincisi can düşürmeden ya da yeni bir skor
    //elde etmeden yeni bir hesap çıkarmak için. Ki ikinci kod random.next sırasında bir hata yaptığımı gösterdi

    else
    {
        Console.WriteLine("\nSayı, ya da Exit komutunu giriniz\n");
        getAnswer();
    }
}

//Skor hesaplarının yapıldığı ve sunulduğu tablo
void score()
{
    if (game.topScore >= game.score)
    {
        Console.WriteLine("\nbu oyundaki skorunuz: " + game.name + " - " + game.score + "\n\nEn yüksek skor: " + game.topName + " - " + game.topScore + "\n\nTekrar oynamak için herhangi bir tuşa basın, Çıkmak için Q tuşuna basın");
        var cki = Console.ReadKey();
        if (cki.Key == ConsoleKey.Q)
        {
            Console.WriteLine("\nOynadığınız için teşekkürler");
            Console.ReadKey();
            return;
        }
        else
        {
            game.score = 0;
            begin();
        }


    }
    else
    {
        Console.WriteLine("\nTebrikler! En yüksek skoru geçtiniz! \n\nYeni Yüksek Skor!:" + game.name + " - " + game.score + "\n\nÖnceki yüksek skor: " + game.topName + " - " + game.topScore + "\n\nTekrar oynamak için herhangi bir tuşa basın, Çıkmak için Q tuşuna basın");
        var cki = Console.ReadKey();
        if (cki.Key == ConsoleKey.Q)
        {
            Console.WriteLine("\nOynadığınız için teşekkürler");
            Console.ReadKey();
            return;
        }
        else
        {
            game.topScore = game.score;
            game.topName = game.name;
            game.score = 0;
            begin();
        }

    }
}

//Bütün metodlar okunduktan sonra begin(); metodunu çağırarak programı çalıştırıyoruz
begin();
