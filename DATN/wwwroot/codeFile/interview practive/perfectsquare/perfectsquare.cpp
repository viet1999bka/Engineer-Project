#include <iostream>
#include <bits/stdc++.h>
#include <stdlib.h>
using namespace std;

bool solution(int n);
int main(int argc, char** argv) {
    int n = atoi(argv[1]);
    if(solution(n)) cout << "true";
    else cout << "false";
}