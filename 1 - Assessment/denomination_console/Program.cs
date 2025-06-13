
List<List<int>> divide (int value, int[] available_cartridges)
{
    
    List<string> opt = new List<string>();

    List<List<List<int>>> possible_values = new List<List<List<int>>>();
    List<List<int>> highest_bill = new List<List<int>>();
    
    
    for (int i = 0; i <= value; i++)
    {
        possible_values.Add(new List<List<int>> { });
        highest_bill.Add(new List<int> { });

        for (int j = 0; j < available_cartridges.Length; j++)
        {

            int bill_value = available_cartridges[j];

            if (bill_value > i)
                continue;

            if (bill_value == i)
            {
                possible_values[i].Add(new List<int> { bill_value });
                highest_bill[i].Add(j);
            }


            for (int k = 0; k < possible_values[i - bill_value].Count; k++)
            {
                if (highest_bill[i-bill_value][k] > j) {
                    continue;
                }
                List<int> aux = new List<int>(possible_values[i - bill_value][k]);
                aux.Add(bill_value);
                possible_values[i].Add(aux);
                highest_bill[i].Add(j);
            }
        }
    }

    // foreach (List<int> a in possible_values[value])
    // {
    //     foreach (int aa in a) {
    //         Console.Write(aa + " ");
    //     }
    //     Console.WriteLine();
    // }

    return possible_values[value];
}

Console.WriteLine("Welcome to the Denomination routine!");

int[] available_cartridges = [10, 50, 100];
int[] payouts = [30, 50, 60, 80, 140, 230, 370, 610, 980];

Console.WriteLine("We have available the following bills: ");
foreach (int ac in available_cartridges)
{
    Console.WriteLine(ac + " EUR");
}

foreach (int payout in payouts)
{
    Console.WriteLine("Payout {0} ", payout);
    Console.WriteLine("Options: ");


    List<List<int>> opt = divide(payout, available_cartridges);
    foreach (List<int> a in opt)
    {
        var g = a.GroupBy( i => i );
        string opt_label = "";
        foreach (var grp in g)
        {
            if (opt_label != "")
                opt_label += " + ";

            opt_label += grp.Count() + " x "+ grp.Key +" EUR";
        }

        Console.WriteLine(opt_label);
    }
}




