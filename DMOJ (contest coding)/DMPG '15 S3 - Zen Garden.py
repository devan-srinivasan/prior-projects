"""Zen Garden"""
N = int(input())
flowers = list(map(int, input().split()))
M = int(input())

for i in range(M):
    (a, b) = tuple(map(int, input().split(" ")))
    best = max(flowers[a], flowers[a - 1], (flowers[a] + flowers[a - 1] - b))
    if best == flowers[a - 1]:
        flowers[a] = 0
    elif best == flowers[a]:
        flowers[a - 1] = 0
    else:
        flowers[a - 1] -= b

print(sum(flowers))
