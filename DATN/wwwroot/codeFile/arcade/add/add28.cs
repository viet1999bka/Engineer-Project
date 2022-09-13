using System;
public class add28
{
    /* Write solution here */
public static int solution(int param1, int param2)
{
    return param1 + param2;
}

    public static void Main(string[] args)
    {
        int arA, arB, arOutput;
        arA = Int32.Parse(args[0]);
        arB = Int32.Parse(args[1]);
        Console.Write(solution(arA, arB));
    }
}
