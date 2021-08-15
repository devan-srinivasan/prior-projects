"""Golf"""
from sys import stdin
input = stdin.readline
d = int(input())
n = int(input())
c = []
dp = [5281 for i in range(d + 1)]
for i in range(n):
    num = int(input())
    c.append(num)
    dp[num] = 1

list.sort(c)
for x in range(d + 1):
    for y in range(n):
        if c[y] > x:
            break
        dp[x] = min(dp[x], dp[x - c[y]] + 1)

if dp[d] == 5281:
    print("Roberta acknowledges defeat.")
else:
    print("Roberta wins in " + str(dp[d]) + " strokes.")
