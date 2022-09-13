#include <bits/stdc++.h>
using namespace std;

float solution(float param1, float param2, float param3);
int main(int argc, char** argv) {
    string f1(argv[1]);
    string f2(argv[2]);
    string f3(argv[3]);
    float param1 = stof(f1);
    float param2 = stof(f2);
    float param3 = stof(f3);
    cout << solution(param1, param2, param3);
}

