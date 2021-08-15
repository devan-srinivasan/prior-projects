"""Its tough being a teen"""
from sys import stdin
from collections import deque
graph = [[], [4, 7], [1], [4, 5], [], [], [], []]
x = -1
y = -1
while x != 0 and y != 0:  # input
    x = int(stdin.readline())
    y = int(stdin.readline())
    if x == 0 and y == 0: break
    graph[x].append(y)
    graph[x] = list(set(graph[x]))
    graph[x].sort()
impossible = False  # checking if possible first
roots = [True] * 8
roots[0] = False
for i in range(1, 8):
    for e in graph[i]:
        roots[e] = False
    d = deque([i])
    while len(d) != 0 and not impossible:
        n = d.popleft()
        for e in graph[n]:
            d.append(e)
            if e == i: impossible = True
            if impossible: break
        if impossible: break

if not impossible:
    order = [i for i in range(1, 8) if roots[i]]
    i = 0
    while i < len(order):
        for e in graph[order[i]]:
            order.append(e)
        i += 1
    for i in range(1, 8):
        if order.count(i) > 1:
            for j in range(order.count(i) - 1):
                order.remove(i)
    ##    print(order)
    i = 1
    while i < 7:
        if order[i] < order[i - 1] and order[i] not in graph[order[i - 1]]:
            a = order[i]
            b = order[i - 1]
            ##            print(i)
            ##            print("swap "+str(a)+" "+str(b))
            order[i - 1] = a
            order[i] = b
            i -= 2
        ##            print(order)
        i += 1
    print(" ".join([str(x) for x in order]))
else:
    print("Cannot complete these tasks. Going to bed.")
