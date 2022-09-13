import java.util.ArrayList;
public class countevent {
    /* Write solution here */
    public static void main(String[] args) {
        int size = Integer.parseInt(args[0]);
        int[] arr = new int[size];
        for(int i = 0; i < size; i++){
            arr[i] = (Integer.parseInt(args[i+1]));
        }
        System.out.print(solution(size, arr));  
    }
}