#include <iostream>
#include <stdlib.h>
using namespace std;

int solution(int year);
int main(int argc, char** argv) {
    int year = atoi(argv[1]);
    cout << solution(year);
}