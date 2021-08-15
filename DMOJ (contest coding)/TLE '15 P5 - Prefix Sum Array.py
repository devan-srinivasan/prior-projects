"""Prefix Sum Array"""
from sys import stdin, stdout
n = int(input())
lin = [int(l) for l in stdin.readline().split(' ')]
c = int(input())
c -= 1
strout = str(lin[0]) + " "
coeff = [1]
MOD = 1000000007


def binpow(b, p) -> int:
    """Binary exponentiation"""
    ans = 1
    while p > 0:
        if p % 2 == 1:
            p -= 1
            ans *= b
        p = p // 2
        b = (b * b) % MOD
    return ans


for i in range(1, n):
    numer = (coeff[i - 1] * (i + c))
    fakedenom = binpow(i, 1000000005)
    coeff.append((numer % MOD) * (fakedenom % MOD))
    nn = 0
    for z in range(1, i + 2):
        nn += (coeff[z - 1] % MOD) * (lin[i - z + 1] % MOD)
    strout += str(nn % MOD) + " "
stdout.write(strout)
