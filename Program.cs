// Vojta Grym
Console.WriteLine("Vitej v ukladaci loginu");
string filePath = "loginy.txt";
string masterPath = "master.txt";
string masterPassword;

if (!File.Exists(masterPath))
{
    Console.Write("Nastav sve hlavni heslo: ");
    masterPassword = Console.ReadLine();
    File.WriteAllText(masterPath, masterPassword);
    Console.WriteLine("Heslo ulozeno. Spust program znovu.");
    return;
}
else
{
    Console.Write("Zadej hlavni heslo: ");
    string zadane = Console.ReadLine();
    masterPassword = File.ReadAllText(masterPath);

    if (zadane != masterPassword)
    {
        Console.WriteLine("Spatne heslo. Pristup zamitnut.");
        return;
    }
}

while (true)
{
    Console.Clear();
    Console.WriteLine("1. Pridat login");
    Console.WriteLine("2. Zobrazit seznam sluzeb a detaily");
    Console.WriteLine("3. Zmenit heslo pro login");
    Console.WriteLine("4. Zmenit hlavni heslo");
    Console.WriteLine("5. Konec");
    Console.Write("Zvol moznost: ");

    string volba = Console.ReadLine();

    if (volba == "1")
    {
        Console.Write("Sluzba: ");
        string service = Console.ReadLine();
        Console.Write("Uzivatel: ");
        string username = Console.ReadLine();
        Console.Write("Heslo: ");
        string password = Console.ReadLine();

        bool existuje = false;
        bool duplikat = false;

        if (File.Exists(filePath))
        {
            string[] zaznamy = File.ReadAllLines(filePath);
            int index = 0;
            while (index < zaznamy.Length)
            {
                string[] casti = zaznamy[index].Split('|');
                if (casti.Length == 3 && casti[0] == service && casti[1] == username)
                {
                    existuje = true;
                    if (casti[2] == password)
                    {
                        duplikat = true;
                    }
                    break;
                }
                index++;
            }
        }

        if (duplikat)
        {
            Console.WriteLine("Udaje jsou duplikaty jiz existujicich.");
        }
        else if (existuje)
        {
            Console.WriteLine("Udaje pro tuto sluzbu a uzivatele jiz existuji.");
        }
        else
        {
            File.AppendAllText(filePath, $"{service}|{username}|{password}\n");
            Console.WriteLine("Login ulozen.");
        }
    }
    else if (volba == "2")
    {
        if (File.Exists(filePath))
        {
            var sluzby = new System.Collections.Generic.List<string>();
            string[] zaznamy = File.ReadAllLines(filePath);

            int i = 0;
            while (i < zaznamy.Length)
            {
                string[] casti = zaznamy[i].Split('|');
                if (casti.Length == 3)
                {
                    if (!sluzby.Contains(casti[0]))
                    {
                        sluzby.Add(casti[0]);
                    }
                }
                i++;
            }

            if (sluzby.Count == 0)
            {
                Console.WriteLine("Zadne sluzby nejsou ulozene.");
            }
            else
            {
                Console.WriteLine("\nSeznam sluzeb:");

                int j = 0;
                while (j < sluzby.Count)
                {
                    Console.WriteLine("- " + sluzby[j]);
                    j++;
                }

                Console.Write("\nChces zobrazit detaily nektere sluzby? (a/n): ");
                string odpoved = Console.ReadLine().ToLower();

                if (odpoved == "a")
                {
                    Console.Write("Zadej nazev sluzby: ");
                    string hledana = Console.ReadLine();
                    bool nalezeno = false;

                    int k = 0;
                    while (k < zaznamy.Length)
                    {
                        string[] casti = zaznamy[k].Split('|');
                        if (casti.Length == 3 && casti[0].ToLower() == hledana.ToLower())
                        {
                            Console.WriteLine("Uzivatel: " + casti[1] + " | Heslo: " + casti[2]);
                            nalezeno = true;
                        }
                        k++;
                    }

                    if (!nalezeno)
                    {
                        Console.WriteLine("Sluzba nenalezena.");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Zadne loginy nejsou ulozene.");
        }
    }
    else if (volba == "3")
    {
        Console.Write("Sluzba: ");
        string service = Console.ReadLine();
        Console.Write("Uzivatel: ");
        string username = Console.ReadLine();
        Console.Write("Nove heslo: ");
        string noveHeslo = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Zaznamy neexistuji.");
        }
        else
        {
            string[] zaznamy = File.ReadAllLines(filePath);
            bool nalezen = false;

            int m = 0;
            while (m < zaznamy.Length)
            {
                string[] casti = zaznamy[m].Split('|');
                if (casti.Length == 3 && casti[0] == service && casti[1] == username)
                {
                    zaznamy[m] = $"{service}|{username}|{noveHeslo}";
                    nalezen = true;
                    break;
                }
                m++;
            }

            if (nalezen)
            {
                File.WriteAllLines(filePath, zaznamy);
                Console.WriteLine("Heslo uspesne zmeneno.");
            }
            else
            {
                Console.WriteLine("Zaznam nebyl nalezen.");
            }
        }
    }
    else if (volba == "4")
    {
        Console.Write("Zadej aktualni hlavni heslo: ");
        string zadaneHeslo = Console.ReadLine();

        if (zadaneHeslo == masterPassword)
        {
            Console.Write("Zadej nove hlavni heslo: ");
            string noveHeslo = Console.ReadLine();
            File.WriteAllText(masterPath, noveHeslo);
            masterPassword = noveHeslo;
            Console.WriteLine("Hlavni heslo uspesne zmeneno.");
        }
        else
        {
            Console.WriteLine("Spatne hlavni heslo. Zmena zamitnuta.");
        }
    }
    else if (volba == "5")
    {
        break;
    }
    else
    {
        Console.WriteLine("Neplatna volba.");
    }

    Console.WriteLine("Stiskni libovolnou klavesu pro pokracovani...");
    Console.ReadKey();
}
