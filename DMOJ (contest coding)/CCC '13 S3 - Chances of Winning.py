"""Chances of Winning"""
t = int(input())
G = int(input())
scores = [0 for i in range(5)]
done = [[0 for j in range(5)] for k in range(5)]
g = [(1, 2), (1, 3), (1, 4), (2, 3), (2, 4), (3, 4)]
for i in range(G):
    (a, b, sa, sb) = tuple(map(int, input().split()))
    g.remove((a, b))
    done[a][b] = 1
    done[b][a] = 1
    if sa > sb:
        scores[a] += 3
    elif sa < sb:
        scores[b] += 3
    elif sa == sb:
        scores[a] += 1
        scores[b] += 1

ans = 0


def smax(arr, e):
    if arr[e] == max(arr) and arr.count(max(arr)) == 1:
        return True
    else:
        return False


def dfs(i, s):
    global g, t, ans
    (a, b) = g[i]

    if i < len(g) - 1:
        s2 = [x for x in s]
        s2[a] += 3
        dfs(i + 1, s2)
        s2[a] -= 3
        s2[b] += 3
        dfs(i + 1, s2)
        s2[b] -= 2
        s2[a] += 1
        dfs(i + 1, s2)
    else:
        s2 = [x for x in s]
        s2[a] += 3
        if smax(s2, t): ans += 1
        s2[a] -= 3
        s2[b] += 3
        if smax(s2, t): ans += 1
        s2[b] -= 2
        s2[a] += 1
        if smax(s2, t): ans += 1
    return


dfs(0, scores)
print(ans)
