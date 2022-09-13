#include <iostream>
#include <stdlib.h>
using namespace std;

int solution(int size, int* arr);
int main(int argc, char** argv) {
    int size = atoi(argv[1]);
    int *arr = new int [size];
    for(int i = 0; i < size; i++){
        arr[i] = atoi(argv[i+2]);
    }
    cout << solution(size, arr);
}